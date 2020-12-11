using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using ShwasherSys.Authorization;
using ShwasherSys.Authorization.Users;
using ShwasherSys.Models.Account;
using IwbZero.Auditing;
using IwbZero.Authorization;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.Controllers
{
    [AllowAnonymous]
    [AuditLog("用户账号")]
    public class AccountController : ShwasherControllerBase
    {
        private readonly UserManager _userManager;
        private readonly LogInManager _logInManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IAuditingHelper _auditingHelper;
        private readonly IRepository<SysLog,long> _auditLogRepository;
        private readonly IAuditInfoProvider _auditInfoProvider;

        public AccountController(
            UserManager userManager,
            LogInManager logInManager,
            IMultiTenancyConfig multiTenancyConfig,
            IAuthenticationManager authenticationManager,
            IIwbSettingManager settingManager, IAuditingHelper auditingHelper, IAuditInfoProvider auditInfoProvider, IRepository<SysLog, long> auditLogRepository)
        {
            _userManager = userManager;
            _logInManager = logInManager;
            _multiTenancyConfig = multiTenancyConfig;
            _authenticationManager = authenticationManager;
            _auditingHelper = auditingHelper;
            SettingManager = settingManager;
            _auditInfoProvider = auditInfoProvider;
            _auditLogRepository = auditLogRepository;
        }

        #region Login / Logout
        [DisableAuditing]
        [AuditLog("登陆")]
        public async Task<ActionResult> Login(string returnUrl = "", string e = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }
            ViewBag.SystemName = await SettingManager.GetSettingValueAsync(SettingNames.AdminSystemName);
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            //_authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //_authenticationManager.SignOut();
            var model = new LoginFormViewModel
            {
                ReturnUrl = returnUrl,
                IsMultiTenancyEnabled = false,
                IsSelfRegistrationAllowed = false,
                MultiTenancySide = MultiTenancySides.Host,
                ErrorMsg = e
            };
            return View(model);
        }

        [HttpPost]
        [AuditLog("登陆")]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();
            var actionStopwatch = Stopwatch.StartNew();
            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password
                );

            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);
            //loginModel.Password = "";
            //await _auditingHelper.SaveAsync(_auditingHelper.CreateUserLoginAuditInfo(Request, loginResult.User.TenantId, loginResult.User.Id,GetType(),new Dictionary<string, object>(){["loginModel"]= loginModel }));

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }
            actionStopwatch.Stop();
            await WriteLoginLog(loginResult, actionStopwatch);
            return AbpJson(new AjaxResponse { TargetUrl = returnUrl });
        }

        private async Task<IwbLoginResult<SysUser>> GetLoginResultAsync(string usernameOrEmailAddress, string password)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password);
            if (loginResult.User == null)
            {
                throw new UserFriendlyException(L("LoginFailed"), "用户名无效，请检查后再登录！");
            }
            this.LogInfo("用户【" + loginResult.User.UserName + "】登录-" + loginResult.Result);
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress);
            }
        }

        private async Task SignInAsync(SysUser user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            identity.AddClaim(new Claim(IwbClaimTypes.RememberMe, rememberMe.ToString(CultureInfo.InvariantCulture)));
            identity.AddClaim(new Claim(ShwasherConsts.UserDepartmentIdClaimType, user.DepartmentID??""));
            //_authenticationManager.SignOut();
            _authenticationManager.SignOut(ShwasherConsts.AuthenticationTypes);
            // Many browsers do not clean up session cookies when you close them. So the rule of thumb must be:
            // For having a consistent behaviour across all browsers, don't rely solely on browser behaviour for proper clean-up
            // of session cookies. It is safer to use non-session cookies (IsPersistent == true) in bundle with an expiration date.
            // See http://blog.petersondave.com/cookies/Session-Cookies-in-Chrome-Firefox-and-Sitecore/

            if (!rememberMe)
            {
                var expiresUtc = DateTimeOffset.UtcNow.AddMinutes(int.Parse(
                    System.Configuration.ConfigurationManager.AppSettings[
                        "AuthSession.ExpireTimeInMinutes"] ?? "90"));
                identity.AddClaim(new Claim(IwbClaimTypes.ExpireTime, expiresUtc.ToString(CultureInfo.InvariantCulture)));
            }
            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName = "")
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "UserEmailIsNotConfirmedAndCanNotLogin");
                case AbpLoginResultType.LockedOut:
                    return new UserFriendlyException(L("LoginFailed"), L("UserLockedOutMessage"));
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }
        private async Task WriteLoginLog(IwbLoginResult<SysUser> loginResult, Stopwatch actionStopwatch)
        {
            try
            {
                var auditInfo = new AuditInfo
                {
                    TenantId = null,
                    UserId = loginResult.User.Id,
                    ImpersonatorUserId = AbpSession.ImpersonatorUserId,
                    ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                    ServiceName = "用户账号",
                    MethodName = "登陆",
                    Parameters = "",
                    ExecutionTime = Clock.Now,
                    ExecutionDuration = Convert.ToInt32(actionStopwatch.Elapsed.TotalMilliseconds)
                };
                _auditInfoProvider.Fill(auditInfo);
                //await _auditingHelper.SaveAsync(auditInfo);
               await _auditLogRepository.InsertAsync(SysLog.CreateFromAuditInfo(auditInfo, loginResult.User.UserName, 1));
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
        }
        [AuditLog("注销")]
        public ActionResult Logout()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [AuditLog("修改密码")]
        public async Task<JsonResult> UpdatePassword(UpdatePwdViewModel input)
        {
            CheckModelState();
            var loginResult = await GetUpdatePwdResultAsync(
                input.LoginName,
                input.LoginPassword
            );
            if (loginResult.Result == AbpLoginResultType.Success)
            {
                await _userManager.ChangePasswordAsync(loginResult.User, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return AbpJson(new AjaxResponse { Success = true });
        }

        private async Task<IwbLoginResult<SysUser>> GetUpdatePwdResultAsync(string usernameOrEmailAddress, string password)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, false, true);
            this.LogInfo("用户【" + loginResult.User.UserName + "】修改密码-" + loginResult.Result);
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedUpdatePwdAttempt(loginResult.Result, usernameOrEmailAddress);
            }
        }

        private Exception CreateExceptionForFailedUpdatePwdAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName = "")
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException("密码修改失败", L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException("密码修改失败", L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException("密码修改失败", L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException("密码修改失败", L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException("密码修改失败", "UserEmailIsNotConfirmedAndCanNotLogin");
                case AbpLoginResultType.LockedOut:
                    return new UserFriendlyException("密码修改失败", L("UserLockedOutMessage"));
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException("密码修改失败");
            }
        }

        #endregion

    }
}

using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Models;
using IwbZero.Authorization.Permissions;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;

namespace ShwasherSys.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class ShwasherControllerBase : AbpController
    {
        public new IIwbSession AbpSession { get; set; }
        public new IIwbPermissionManager PermissionManager { get; set; }
        protected new IIwbSettingManager SettingManager { get; set; }
        protected ICacheManager CacheManager { get; set; }
        protected IStatesAppService StatesAppService { get; set; }
        protected ShwasherControllerBase()
        {
            LocalizationSourceName = ShwasherConsts.LocalizationSourceName;
        }
        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected CurrentUserViewModel GetCurrentUser()
        {
            CurrentUserViewModel currentUserInfo = new CurrentUserViewModel();
            if (AbpSession?.UserId != null)
            {
                var name = AbpSession.GetClaimValue(IwbClaimTypes.EmployeeName);
                if (AbpSession?.UserId != null) 
                    currentUserInfo.UserId = (long) AbpSession?.UserId;
                currentUserInfo.UserName = AbpSession?.UserName;
                currentUserInfo.RealName =  string.IsNullOrEmpty(name)? AbpSession?.RealName??AbpSession?.UserName:name;
                currentUserInfo.UserType = AbpSession?.UserType ?? default(int);
                currentUserInfo.EmailAddress = AbpSession?.EmailAddress;
            }
            return currentUserInfo;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
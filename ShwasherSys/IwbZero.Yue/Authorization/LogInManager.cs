using System;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.Authorization.Roles;
using IwbZero.Authorization.Users;
using IwbZero.Configuration;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization
{
    public abstract class IwbLogInManager<TRole, TUser> : ITransientDependency
        where TRole : IwbSysRole<TUser>, new()
        where TUser : IwbSysUser<TUser>, new()
    {
        public IClientInfoProvider ClientInfoProvider { get; set; }

        //protected IMultiTenancyConfig MultiTenancyConfig { get; }
        protected IRepository<UserLoginAttempt, long> UserLoginAttemptRepository { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IwbUserManager<TRole, TUser> UserManager { get; }
        protected IIwbSettingManager SettingManager { get; }
        protected IIwbUserManagementConfig IwbUserManagementConfig { get; }
        protected IIocResolver IocResolver { get; }
        protected IwbRoleManager<TRole, TUser> RoleManager { get; }

        public IwbLogInManager(
            IwbUserManager<TRole, TUser> userManager,
            //IMultiTenancyConfig multiTenancyConfig,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IIwbSettingManager settingManager,
            IIwbUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IwbRoleManager<TRole, TUser> roleManager)
        {
            //MultiTenancyConfig = multiTenancyConfig;
            UserLoginAttemptRepository = userLoginAttemptRepository;
            UnitOfWorkManager = unitOfWorkManager;
            SettingManager = settingManager;
            IwbUserManagementConfig = userManagementConfig;
            IocResolver = iocResolver;
            RoleManager = roleManager;
            UserManager = userManager;

            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        [UnitOfWork]
        public virtual async Task<IwbLoginResult<TUser>> LoginAsync(UserLoginInfo login)
        {
            var result = await LoginAsyncInternal(login);
            await SaveLoginAttempt(result, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        protected virtual async Task<IwbLoginResult<TUser>> LoginAsyncInternal(UserLoginInfo login)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }


            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var user = await UserManager.UserStore.FindAsync(null, login);
                if (user == null)
                {
                    return new IwbLoginResult<TUser>(AbpLoginResultType.UnknownExternalLogin);
                }

                return await CreateLoginResultAsync(user);
            }
        }

        [UnitOfWork]
        public virtual async Task<IwbLoginResult<TUser>> LoginAsync(string userNameOrEmailAddress, string plainPassword, bool shouldLockout = true, bool isUpdatePwd = false)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, shouldLockout);
            if (!isUpdatePwd)
            {
                await SaveLoginAttempt(result, userNameOrEmailAddress);
            }
            return result;
        }

        protected virtual async Task<IwbLoginResult<TUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            //TTenant tenant = null;
            //using (UnitOfWorkManager.Current.SetTenantId(null))
            //{
            //    if (!MultiTenancyConfig.IsEnabled)
            //    {
            //        tenant = await GetDefaultTenantAsync();
            //    }
            //    else if (!string.IsNullOrWhiteSpace(tenancyName))
            //    {
            //        tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
            //        if (tenant == null)
            //        {
            //            return new AbpLoginResult(AbpLoginResultType.InvalidTenancyName);
            //        }

            //        if (!tenant.IsActive)
            //        {
            //            return new AbpLoginResult(AbpLoginResultType.TenantIsNotActive, tenant);
            //        }
            //    }
            //}

            //var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                //TryLoginFromExternalAuthenticationSources method may create the user, that's why we are calling it before AbpStore.FindByNameOrEmailAsync
                // var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

                var user = await UserManager.UserStore.FindByNameOrEmailAsync(null, userNameOrEmailAddress);
                if (user == null)
                {
                    return new IwbLoginResult<TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress);
                }

                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return new IwbLoginResult<TUser>(AbpLoginResultType.LockedOut, user);
                }

                UserManager.InitializeLockoutSettings();
                var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return await GetFailedPasswordValidationAsLoginResultAsync(user, shouldLockout);
                }

                if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return await GetSuccessRehashNeededAsLoginResultAsync(user);
                }

                await UserManager.ResetAccessFailedCountAsync(user.Id);
                return await CreateLoginResultAsync(user);
            }
        }

        protected virtual async Task<IwbLoginResult<TUser>> GetFailedPasswordValidationAsLoginResultAsync(TUser user, bool shouldLockout = false)
        {
            if (shouldLockout)
            {
                if (await TryLockOutAsync(null, user.Id))
                {
                    return new IwbLoginResult<TUser>(AbpLoginResultType.LockedOut, user);
                }
            }

            return new IwbLoginResult<TUser>(AbpLoginResultType.InvalidPassword, user);
        }

        protected virtual async Task<IwbLoginResult<TUser>> GetSuccessRehashNeededAsLoginResultAsync(TUser user, bool shouldLockout = false)
        {
            return await GetFailedPasswordValidationAsLoginResultAsync(user, shouldLockout);
        }

        protected virtual async Task<IwbLoginResult<TUser>> CreateLoginResultAsync(TUser user)
        {
            if (!user.IsActive)
            {
                return new IwbLoginResult<TUser>(AbpLoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync() && !user.IsEmailConfirmed)
            {
                return new IwbLoginResult<TUser>(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await UserManager.UserStore.UpdateAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return new IwbLoginResult<TUser>(user,
                await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)
            );
        }


        protected virtual async Task SaveLoginAttempt(IwbLoginResult<TUser> loginResult, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(null))
                {
                    var loginAttempt = new UserLoginAttempt
                    {

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,
                        Result = loginResult.Result,
                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    (await UserManager.AccessFailedAsync(userId)).CheckErrors();

                    var isLockOut = await UserManager.IsLockedOutAsync(userId);

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }

        //protected virtual async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        //{
        //    if (!UserManagementConfig.ExternalAuthenticationSources.Any())
        //    {
        //        return false;
        //    }

        //    foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
        //    {
        //        using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource>(sourceType))
        //        {
        //            if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
        //            {
        //                var tenantId = tenant == null ? (int?)null : tenant.Id;
        //                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
        //                {
        //                    var user = await UserManager.AbpStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
        //                    if (user == null)
        //                    {
        //                        user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

        //                        user.TenantId = tenantId;
        //                        user.AuthenticationSource = source.Object.Name;
        //                        user.Password = UserManager.PasswordHasher.HashPassword(Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

        //                        if (user.Roles == null)
        //                        {
        //                            user.Roles = new List<UserRole>();
        //                            foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
        //                            {
        //                                user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
        //                            }
        //                        }

        //                        await UserManager.AbpStore.CreateAsync(user);
        //                    }
        //                    else
        //                    {
        //                        await source.Object.UpdateUserAsync(user, tenant);

        //                        user.AuthenticationSource = source.Object.Name;

        //                        await UserManager.AbpStore.UpdateAsync(user);
        //                    }

        //                    await UnitOfWorkManager.Current.SaveChangesAsync();

        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        //protected virtual async Task<TTenant> GetDefaultTenantAsync()
        //{
        //    var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == AbpTenant<SysUser>.DefaultTenantName);
        //    if (tenant == null)
        //    {
        //        throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
        //    }

        //    return tenant;
        //}

        protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync()
        {

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(IwbAdminSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }
    }
}

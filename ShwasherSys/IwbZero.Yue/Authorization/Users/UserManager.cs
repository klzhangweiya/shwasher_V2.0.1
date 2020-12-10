using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Roles;
using IwbZero.Caching;
using IwbZero.Configuration;
using IwbZero.IdentityFramework;
using IwbZero.Session;
using IwbZero.Setting;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Users
{
    public abstract class IwbUserManager<TRole, TUser> : UserManager<TUser, long>, IDomainService
        where TRole : IwbSysRole<TUser>, new()
        where TUser : IwbSysUser<TUser>, new()
    {
        protected IIwbUserPermissionStore<TUser> IwbUserPermissionStore
        {
            get
            {
                if (!(Store is IIwbUserPermissionStore<TUser>))
                {
                    throw new AbpException("Store is not IUserPermissionStore");
                }

                return Store as IIwbUserPermissionStore<TUser>;
            }
        }

        public ILocalizationManager LocalizationManager { get; }

        public IAbpSession AbpSession { get; set; }

        public FeatureDependencyContext FeatureDependencyContext { get; set; }

        protected IwbRoleManager<TRole, TUser> RoleManager { get; }

        public IwbUserStore<TRole, TUser> UserStore { get; }

        public IMultiTenancyConfig MultiTenancy { get; set; }

        private readonly IIwbPermissionManager _permissionManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICacheManager _cacheManager;
        //private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        //private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        //private readonly IOrganizationUnitSettings _organizationUnitSettings;
        private readonly IIwbSettingManager _settingManager;


        protected IwbUserManager(
            IwbUserStore<TRole, TUser> userStore,
            IwbRoleManager<TRole, TUser> roleManager,
            IIwbPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            //IRepository<OrganizationUnit, long> organizationUnitRepository,
            //IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            //IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            IwbIdentityEmailMessageService emailService,
            IIwbSettingManager settingManager,
            IIwbUserTokenProviderAccessor iwbUserTokenProviderAccessor,
            string localizationSourceName=null)
            : base(userStore)
        {
            UserStore = userStore;
            RoleManager = roleManager;
            LocalizationManager = localizationManager;
            _settingManager = settingManager;

            _permissionManager = permissionManager;
            _unitOfWorkManager = unitOfWorkManager;
            _cacheManager = cacheManager;
            //_organizationUnitRepository = organizationUnitRepository;
            //_userOrganizationUnitRepository = userOrganizationUnitRepository;
            //_organizationUnitSettings = organizationUnitSettings;

            AbpSession = NullAbpSession.Instance;

            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            EmailService = emailService;

            UserTokenProvider = iwbUserTokenProviderAccessor.GetUserTokenProviderOrNull<TUser>();
            LocalizationSourceName = localizationSourceName ?? IwbZeroConsts.IwbZeroLocalizationSourceName;
        }
        protected string LocalizationSourceName { get; set; }

        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            //var tenantId = GetCurrentTenantId();
            //if (tenantId.HasValue && !user.TenantId.HasValue)
            //{
            //    user.TenantId = tenantId.Value;
            //}

            var isLockoutEnabled = user.IsLockoutEnabled;

            var identityResult = await base.CreateAsync(user);

            if (identityResult.Succeeded)
            {
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await SetLockoutEnabledAsync(user.Id, isLockoutEnabled);
            }

            return identityResult;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await IsGrantedAsync(
                userId,
                _permissionManager.GetPermission(permissionName)
                );
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public virtual async Task<bool> IsGrantedOnlyUserAsync(long userId, string permissionName)
        {
            return await IsGrantedOnlyUserAsync(
                userId,
                _permissionManager.GetPermission(permissionName)
            );
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual Task<bool> IsGrantedAsync(TUser user, Permission permission)
        {
            return IsGrantedAsync(user.Id, permission);
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permission">Permission</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, Permission permission)
        {
            //Check for multi-tenancy side
            if (!permission.MultiTenancySides.HasFlag(GetCurrentMultiTenancySide()))
            {
                return false;
            }

            //Check for depended features
            if (permission.FeatureDependency != null && GetCurrentMultiTenancySide() == MultiTenancySides.Tenant)
            {
                FeatureDependencyContext.TenantId = GetCurrentTenantId();

                if (!await permission.FeatureDependency.IsSatisfiedAsync(FeatureDependencyContext))
                {
                    return false;
                }
            }

            //Get cached user permissions
            var cacheItem = await GetUserPermissionCacheItemAsync(userId);
            if (cacheItem == null)
            {
                return false;
            }

            //Check for user-specific value
            if (cacheItem.GrantedPermissions.Contains(permission.Name))
            {
                return true;
            }

            if (cacheItem.ProhibitedPermissions.Contains(permission.Name))
            {
                return false;
            }

            //Check for roles
            foreach (var roleId in cacheItem.RoleIds)
            {
                if (await RoleManager.IsGrantedAsync(roleId, permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permission">Permission</param>
        public async Task<bool> IsGrantedOnlyUserAsync(long userId, Permission permission)
        {
            //Check for multi-tenancy side
            if (!permission.MultiTenancySides.HasFlag(GetCurrentMultiTenancySide()))
            {
                return false;
            }

            //Check for depended features
            if (permission.FeatureDependency != null && GetCurrentMultiTenancySide() == MultiTenancySides.Tenant)
            {
                FeatureDependencyContext.TenantId = GetCurrentTenantId();

                if (!await permission.FeatureDependency.IsSatisfiedAsync(FeatureDependencyContext))
                {
                    return false;
                }
            }

            //Get cached user permissions
            var cacheItem = await GetUserPermissionCacheItemAsync(userId);
            if (cacheItem == null)
            {
                return false;
            }

            //Check for user-specific value
            if (cacheItem.GrantedPermissions.Contains(permission.Name))
            {
                return true;
            }

            if (cacheItem.ProhibitedPermissions.Contains(permission.Name))
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets granted permissions for a user(user and role).
        /// </summary>
        /// <param name="user">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TUser user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(user.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Gets granted permissions for a user(only user).
        /// </summary>
        /// <param name="user">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedOnlyUserPermissionsAsync(TUser user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedOnlyUserAsync(user.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a user at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetUserGrantedPermissionsAsync(TUser user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedOnlyUserPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await RemovePermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Sets all granted permissions of a user at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(TUser user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Prohibits all permissions for a user.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ProhibitAllPermissionsAsync(TUser user)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a user.
        /// It removes all permission settings for the user.
        /// User will have permissions according to his roles.
        /// This method does not prohibit all permissions.
        /// For that, use <see cref="ProhibitAllPermissionsAsync"/>.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ResetAllPermissionsAsync(TUser user)
        {
            await IwbUserPermissionStore.RemoveAllPermissionSettingsAsync(user);
        }

        /// <summary>
        /// Grants a permission for a user if not already granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task GrantPermissionAsync(TUser user, Permission permission)
        {
            await IwbUserPermissionStore.RemovePermissionAsync(user, new IwbPermissionGrantInfo(permission.Name, false));

            if (await IsGrantedOnlyUserAsync(user.Id, permission))
            {
                return;
            }

            await IwbUserPermissionStore.AddPermissionAsync(user, new IwbPermissionGrantInfo(permission.Name, true));
            await SetUserGrantedPermissionCacheItem(user.Id, permission.Name);
        }

        /// <summary>
        /// Prohibits a permission for a user if it's granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task ProhibitPermissionAsync(TUser user, Permission permission)
        {
            await IwbUserPermissionStore.RemovePermissionAsync(user, new IwbPermissionGrantInfo(permission.Name, true));

            if (!await IsGrantedOnlyUserAsync(user.Id, permission))
            {
                return;
            }

            await IwbUserPermissionStore.AddPermissionAsync(user, new IwbPermissionGrantInfo(permission.Name, false));
            await SetUserProhibitedPermissionCacheItem(user.Id, permission.Name);
        }
        /// <summary>
        /// Prohibits a permission for a user if it's granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task RemovePermissionAsync(TUser user, Permission permission)
        {
            await IwbUserPermissionStore.RemovePermissionAsync(user, new IwbPermissionGrantInfo(permission.Name, true));
            await SetUserGrantedPermissionCacheItem(user.Id, permission.Name, false);
        }

        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await UserStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public virtual Task<List<TUser>> FindAllAsync(UserLoginInfo login)
        {
            return UserStore.FindAllAsync(login);
        }

        /// <summary>
        /// Gets a user by given id.
        /// Throws exception if no user found with given id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        /// <exception cref="AbpException">Throws exception if no user found with given id</exception>
        public virtual async Task<TUser> GetUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + userId);
            }

            return user;
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            identity.AddClaim(new Claim(IwbClaimTypes.UserName, user.UserName));
            identity.AddClaim(new Claim(IwbClaimTypes.RealName, user.RealName));
            identity.AddClaim(new Claim(IwbClaimTypes.UserType, user.UserType.ToString()));
            identity.AddClaim(new Claim(IwbClaimTypes.EmailAddress, user.EmailAddress));
            var roleList = await GetRolesAsync(user.Id);
            string userRoles = roleList.Any() ? string.Join(",", roleList.ToArray()) : "";
            identity.AddClaim(new Claim(IwbClaimTypes.UserRoles, userRoles));//IwbClaimTypes.EmployeeNo
            //if (user.TenantId.HasValue)
            //{
            //    identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            //}
            return identity;
        }

        public override async Task<IdentityResult> UpdateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            //Admin user's username can not be changed!
            if (user.UserName != UserBase.AdminUserName)
            {
                if ((await GetOldUserNameAsync(user.Id)) == UserBase.AdminUserName)
                {
                    return IwbIdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), UserBase.AdminUserName));
                }
            }
            else if (user.UserName != UserBase.SystemUserName)
            {
                if ((await GetOldUserNameAsync(user.Id)) == UserBase.SystemUserName)
                {
                    return IwbIdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), UserBase.AdminUserName));
                }
            }

            return await base.UpdateAsync(user);
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user)
        {
            if (user.UserName == UserBase.AdminUserName)
            {
                return IwbIdentityResult.Failed(string.Format(L("CanNotDeleteAdminUser"), UserBase.AdminUserName));
            }

            return await base.DeleteAsync(user);
        }

        //public override async Task<TUser> FindByIdAsync(long useId)
        //{
        //    return await AbpStore.FindByIdAsync(useId);
        //}

        public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await UserStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return IwbIdentityResult.Failed(string.Format(L("Identity.DuplicateUserName"), userName));
            }

            user = (await FindByEmailAsync(emailAddress));
            if (user != null && user.Id != expectedUserId)
            {
                return IwbIdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), emailAddress));
            }

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> SetRoles(TUser user, string[] roleNames)
        {
            //Remove from removed roles
            if (user.Roles.Any())
            {
                foreach (var userRole in user.Roles.ToList())
                {
                    var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                    if (roleNames != null && roleNames.All(roleName => role.Name == roleName))
                        continue;
                    await RemoveFromRoleAsync(user.Id, role.Name);
                }
            }

            //Add to added roles
            if (roleNames != null)
            {
                foreach (var roleName in roleNames)
                {
                    var role = await RoleManager.GetRoleByNameAsync(roleName);
                    if (user.Roles.All(ur => ur.RoleId != role.Id))
                    {
                        var result = await AddToRoleAsync(user.Id, roleName);
                        if (!result.Succeeded)
                        {
                            return result;
                        }
                    }
                }
            }

            return IdentityResult.Success;
        }
        #region Organization

        //public virtual async Task<bool> IsInOrganizationUnitAsync(long userId, long ouId)
        //{
        //    return await IsInOrganizationUnitAsync(
        //        await GetUserByIdAsync(userId),
        //        await _organizationUnitRepository.GetAsync(ouId)
        //        );
        //}

        //public virtual async Task<bool> IsInOrganizationUnitAsync(SysUser user, OrganizationUnit ou)
        //{
        //    return await _userOrganizationUnitRepository.CountAsync(uou =>
        //        uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id
        //        ) > 0;
        //}

        //public virtual async Task AddToOrganizationUnitAsync(long userId, long ouId)
        //{
        //    await AddToOrganizationUnitAsync(
        //        await GetUserByIdAsync(userId),
        //        await _organizationUnitRepository.GetAsync(ouId)
        //        );
        //}

        //public virtual async Task AddToOrganizationUnitAsync(SysUser user, OrganizationUnit ou)
        //{
        //    var currentOus = await GetOrganizationUnitsAsync(user);

        //    if (currentOus.Any(cou => cou.Id == ou.Id))
        //    {
        //        return;
        //    }

        //    await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, currentOus.Count + 1);

        //    await _userOrganizationUnitRepository.InsertAsync(new UserOrganizationUnit(user.TenantId, user.Id, ou.Id));
        //}

        //public virtual async Task RemoveFromOrganizationUnitAsync(long userId, long ouId)
        //{
        //    await RemoveFromOrganizationUnitAsync(
        //        await GetUserByIdAsync(userId),
        //        await _organizationUnitRepository.GetAsync(ouId)
        //        );
        //}

        //public virtual async Task RemoveFromOrganizationUnitAsync(SysUser user, OrganizationUnit ou)
        //{
        //    await _userOrganizationUnitRepository.DeleteAsync(uou => uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id);
        //}

        //public virtual async Task SetOrganizationUnitsAsync(long userId, params long[] organizationUnitIds)
        //{
        //    await SetOrganizationUnitsAsync(
        //        await GetUserByIdAsync(userId),
        //        organizationUnitIds
        //        );
        //}

        //private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int? tenantId, int requestedCount)
        //{
        //    var maxCount = await _organizationUnitSettings.GetMaxUserMembershipCountAsync(tenantId);
        //    if (requestedCount > maxCount)
        //    {
        //        throw new AbpException(string.Format("Can not set more than {0} organization unit for a user!", maxCount));
        //    }
        //}

        //public virtual async Task SetOrganizationUnitsAsync(SysUser user, params long[] organizationUnitIds)
        //{
        //    if (organizationUnitIds == null)
        //    {
        //        organizationUnitIds = new long[0];
        //    }

        //    await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, organizationUnitIds.Length);

        //    var currentOus = await GetOrganizationUnitsAsync(user);

        //    //Remove from removed OUs
        //    foreach (var currentOu in currentOus)
        //    {
        //        if (!organizationUnitIds.Contains(currentOu.Id))
        //        {
        //            await RemoveFromOrganizationUnitAsync(user, currentOu);
        //        }
        //    }

        //    //Add to added OUs
        //    foreach (var organizationUnitId in organizationUnitIds)
        //    {
        //        if (currentOus.All(ou => ou.Id != organizationUnitId))
        //        {
        //            await AddToOrganizationUnitAsync(
        //                user,
        //                await _organizationUnitRepository.GetAsync(organizationUnitId)
        //                );
        //        }
        //    }
        //}

        //[UnitOfWork]
        //public virtual Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(SysUser user)
        //{
        //    var query = from uou in _userOrganizationUnitRepository.GetAll()
        //                join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
        //                where uou.UserId == user.Id
        //                select ou;

        //    return Task.FromResult(query.ToList());
        //}

        //[UnitOfWork]
        //public virtual Task<List<SysUser>> GetUsersInOrganizationUnit(OrganizationUnit organizationUnit, bool includeChildren = false)
        //{
        //    if (!includeChildren)
        //    {
        //        var query = from uou in _userOrganizationUnitRepository.GetAll()
        //                    join user in AbpStore.Users on uou.UserId equals user.Id
        //                    where uou.OrganizationUnitId == organizationUnit.Id
        //                    select user;

        //        return Task.FromResult(query.ToList());
        //    }
        //    else
        //    {
        //        var query = from uou in _userOrganizationUnitRepository.GetAll()
        //                    join user in AbpStore.Users on uou.UserId equals user.Id
        //                    join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
        //                    where ou.Code.StartsWith(organizationUnit.Code)
        //                    select user;

        //        return Task.FromResult(query.ToList());
        //    }
        //} 
        #endregion

        public virtual void RegisterTwoFactorProviders(int? tenantId = null)
        {
            TwoFactorProviders.Clear();

            if (!IsTrue(IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEnabled))
            {
                return;
            }

            if (EmailService != null &&
                IsTrue(IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled))
            {
                RegisterTwoFactorProvider(
                    L("Email"),
                    new EmailTokenProvider<TUser, long>
                    {
                        Subject = L("EmailSecurityCodeSubject"),
                        BodyFormat = L("EmailSecurityCodeBody")
                    }
                );
            }

            if (SmsService != null &&
                IsTrue(IwbAdminSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled))
            {
                RegisterTwoFactorProvider(
                    L("Sms"),
                    new PhoneNumberTokenProvider<TUser, long>
                    {
                        MessageFormat = L("SmsSecurityCodeMessage")
                    }
                );
            }
        }

        public virtual void InitializeLockoutSettings(int? tenantId = null)
        {
            UserLockoutEnabledByDefault = IsTrue(IwbAdminSettingNames.UserManagement.UserLockOut.IsEnabled);
            DefaultAccountLockoutTimeSpan = TimeSpan.FromSeconds(GetSettingValue<int>(IwbAdminSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds));
            MaxFailedAccessAttemptsBeforeLockout = GetSettingValue<int>(IwbAdminSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout);
        }

        public override async Task<IList<string>> GetValidTwoFactorProvidersAsync(long userId)
        {
            //var user = await GetUserByIdAsync(userId);

            //RegisterTwoFactorProviders(user.TenantId);
            RegisterTwoFactorProviders();

            return await base.GetValidTwoFactorProvidersAsync(userId);
        }

        public override async Task<IdentityResult> NotifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            //var user = await GetUserByIdAsync(userId);
            //RegisterTwoFactorProviders(user.TenantId);
            RegisterTwoFactorProviders();

            return await base.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        public override async Task<string> GenerateTwoFactorTokenAsync(long userId, string twoFactorProvider)
        {
            //var user = await GetUserByIdAsync(userId);
            //RegisterTwoFactorProviders(user.TenantId);
            RegisterTwoFactorProviders();

            return await base.GenerateTwoFactorTokenAsync(userId, twoFactorProvider);
        }

        public override async Task<bool> VerifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            //var user = await GetUserByIdAsync(userId);
            //RegisterTwoFactorProviders(user.TenantId);
            RegisterTwoFactorProviders();

            return await base.VerifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        protected virtual Task<string> GetOldUserNameAsync(long userId)
        {
            return UserStore.GetUserNameFromDatabaseAsync(userId);
        }

        public Task<TUser> GetOldUserAsync(long userId)
        {
            return UserStore.GetUserFromDatabaseAsync(userId);
        }

        private async Task<IwbUserPermissionCacheItem> GetUserPermissionCacheItemAsync(long userId)
        {
            var cacheKey = userId + "@" + (GetCurrentTenantId() ?? 0);
            return await _cacheManager.GetUserPermissionCache().GetAsync(cacheKey, async () =>
            {
                var user = await FindByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                var newCacheItem = new IwbUserPermissionCacheItem(userId);

                foreach (var roleName in await GetRolesAsync(userId))
                {
                    newCacheItem.RoleIds.Add((await RoleManager.GetRoleByNameAsync(roleName)).Id);
                }

                foreach (var permissionInfo in await IwbUserPermissionStore.GetPermissionsAsync(userId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.Add(permissionInfo.Name);
                    }
                    else
                    {
                        newCacheItem.ProhibitedPermissions.Add(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }

        private Task SetUserGrantedPermissionCacheItem(long userId, string permissionName, bool isAdded = true)
        {
            var cacheKey = userId + "@" + (GetCurrentTenantId() ?? 0);
            IwbUserPermissionCacheItem cacheItem = _cacheManager.GetUserPermissionCache().Get(cacheKey, () => null) ?? new IwbUserPermissionCacheItem(userId);
            if (isAdded)
            {
                cacheItem.GrantedPermissions.AddIfNotContains(permissionName);
            }
            else
            {
                cacheItem.GrantedPermissions.Remove(permissionName);
            }
            return _cacheManager.GetUserPermissionCache().SetAsync(cacheKey, cacheItem);
        }
        private Task SetUserProhibitedPermissionCacheItem(long userId, string permissionName, bool isAdded = true)
        {
            var cacheKey = userId + "@" + (GetCurrentTenantId() ?? 0);
            var cacheItem = _cacheManager.GetUserPermissionCache().Get(cacheKey, () => null) ?? new IwbUserPermissionCacheItem(userId);
            if (isAdded)
            {
                cacheItem.ProhibitedPermissions.AddIfNotContains(permissionName);
            }
            else
            {
                cacheItem.ProhibitedPermissions.Remove(permissionName);
            }
            return _cacheManager.GetUserPermissionCache().SetAsync(cacheKey, cacheItem);
        }
        private bool IsTrue(string settingName)
        {
            return GetSettingValue<bool>(settingName);
        }

        private T GetSettingValue<T>(string settingName) where T : struct
        {
            return _settingManager.GetSettingValueForApplication<T>(settingName);
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(LocalizationSourceName, name);
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

        private MultiTenancySides GetCurrentMultiTenancySide()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return MultiTenancy.IsEnabled && !_unitOfWorkManager.Current.GetTenantId().HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }

            return AbpSession.MultiTenancySide;
        }
    }
}

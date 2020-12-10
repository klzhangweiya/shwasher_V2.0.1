using System.Collections.Generic;
using System.Linq;
using IwbZero.IdentityFramework;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;
using IwbZero.Caching;
using IwbZero.Configuration;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Roles
{
    public abstract class IwbRoleManager<TRole, TUser> : RoleManager<TRole, int>,
        IDomainService
        where TRole : IwbSysRole<TUser>,new()
        where TUser : IwbSysUser<TUser>
    {
        public ILocalizationManager LocalizationManager { get; set; }

        public IAbpSession AbpSession { get; set; }

        public IIwbRoleManagementConfig IwbRoleManagementConfig { get; private set; }

        public FeatureDependencyContext FeatureDependencyContext { get; set; }

        private IIwbRolePermissionStore<TRole> IwbRolePermissionStore
        {
            get
            {
                if (!(Store is IIwbRolePermissionStore<TRole>))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }

                return Store as IIwbRolePermissionStore<TRole>;
            }
        }

        protected  string LocalizationSourceName { get; set; }


        protected IwbRoleStore<TRole, TUser> AbpStore { get; private set; }

        protected IIwbPermissionManager PermissionManager { get; }

        protected ICacheManager CacheManager { get; }

        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IwbRoleManager(
            IwbRoleStore<TRole, TUser> store,
            IIwbPermissionManager permissionManager,
            IIwbRoleManagementConfig iwbRoleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            string localizationSourceName = null)
            : base(store)
        {
            PermissionManager = permissionManager;
            CacheManager = cacheManager;
            UnitOfWorkManager = unitOfWorkManager;

            IwbRoleManagementConfig = iwbRoleManagementConfig;
            AbpStore = store;
            AbpSession = NullAbpSession.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
            LocalizationSourceName = localizationSourceName ?? IwbZeroConsts.IwbZeroLocalizationSourceName;
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> IsGrantedAsync(string roleName, string permissionName)
        {
            return await IsGrantedAsync((await GetRoleByNameAsync(roleName)).Id, PermissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> IsGrantedAsync(int roleId, string permissionName)
        {
            return await IsGrantedAsync(roleId, PermissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public Task<bool> IsGrantedAsync(TRole role, Permission permission)
        {
            return IsGrantedAsync(role.Id, permission);
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> IsGrantedAsync(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByNameAsync(roleName));
        }

        /// <summary>
        /// Gets granted permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TRole role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in PermissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(role.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p, IwbPermissionEqualityComparer.Instance)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p, IwbPermissionEqualityComparer.Instance)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Grants a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task GrantPermissionAsync(TRole role, Permission permission)
        {
            if (await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await IwbRolePermissionStore.RemovePermissionAsync(role, new IwbPermissionGrantInfo(permission.Name, false));
            await IwbRolePermissionStore.AddPermissionAsync(role, new IwbPermissionGrantInfo(permission.Name, true));
            await SetRoleGrantedPermissionCacheItem(role.Id, permission.Name);
        }

        /// <summary>
        /// Prohibits a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task ProhibitPermissionAsync(TRole role, Permission permission)
        {
            if (!await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await IwbRolePermissionStore.RemovePermissionAsync(role, new IwbPermissionGrantInfo(permission.Name, true));
            await IwbRolePermissionStore.AddPermissionAsync(role, new IwbPermissionGrantInfo(permission.Name, false));
            await SetRoleGrantedPermissionCacheItem(role.Id, permission.Name, false);
        }


        private Task SetRoleGrantedPermissionCacheItem(int roleId, string permissionName, bool isAdded = true)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
            IwbRolePermissionCacheItem cacheItem = CacheManager.GetRolePermissionCache().Get(cacheKey, () => null) ?? new IwbRolePermissionCacheItem(roleId);
            if (isAdded)
            {
                cacheItem.GrantedPermissions.AddIfNotContains(permissionName);
            }
            else
            {
                cacheItem.GrantedPermissions.Remove(permissionName);
            }
            return CacheManager.GetRolePermissionCache().SetAsync(cacheKey, cacheItem);
        }
        //private Task SetRoleProhibitedPermissionCacheItem(int roleId, string permissionName, bool isAdded = true)
        //{
        //    var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
        //    RolePermissionCacheItem cacheItem = CacheManager.GetRolePermissionCache().Get(cacheKey, () => null) ?? new RolePermissionCacheItem(roleId);
        //    if (isAdded)
        //    {
        //        cacheItem.ProhibitedPermissions.Add(permissionName);
        //    }
        //    else
        //    {
        //        cacheItem.ProhibitedPermissions.Remove(permissionName);
        //    }
        //    return CacheManager.GetRolePermissionCache().SetAsync(cacheKey, cacheItem);
        //}

        /// <summary>
        /// Prohibits all permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ProhibitAllPermissionsAsync(TRole role)
        {
            foreach (var permission in PermissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a role.
        /// It removes all permission settings for the role.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ResetAllPermissionsAsync(TRole role)
        {
            await IwbRolePermissionStore.RemoveAllPermissionSettingsAsync(role);
        }

        /// <summary>
        /// Creates a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override async Task<IdentityResult> CreateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.RoleDisplayName);
            if (!result.Succeeded)
            {
                return result;
            }

            //var tenantId = GetCurrentTenantId();
            //if (tenantId.HasValue && !role.TenantId.HasValue)
            //{
            //    role.TenantId = tenantId.Value;
            //}

            return await base.CreateAsync(role);
        }

        public override async Task<IdentityResult> UpdateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.RoleDisplayName);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateAsync(role);
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override async Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role.IsStatic)
            {
                return IwbIdentityResult.Failed(string.Format(L("CanNotDeleteStaticRole"), role.Name));
            }

            return await base.DeleteAsync(role);
        }

        public override async Task<TRole> FindByIdAsync(int roleId)
        {
            return await AbpStore.FindByIdAsync(roleId);
        }

        /// <summary>
        /// Gets a role by given id.
        /// Throws exception if no role with given id.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given id</exception>
        public virtual async Task<TRole> GetRoleByIdAsync(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id: " + roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets a role by given name.
        /// Throws exception if no role with given roleName.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given roleName</exception>
        public virtual async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name: " + roleName);
            }

            return role;
        }

        public async Task GrantAllPermissionsAsync(TRole role)
        {
            //FeatureDependencyContext.TenantId = role.TenantId;

            var permissions = PermissionManager.GetAllPermissions()
                                                .Where(permission =>
                                                    permission.FeatureDependency == null ||
                                                    permission.FeatureDependency.IsSatisfied(FeatureDependencyContext)
                                                );

            await SetGrantedPermissionsAsync(role, permissions);
        }

        [UnitOfWork]
        public virtual async Task<IdentityResult> CreateStaticRoles(int tenantId)
        {
            var staticRoleDefinitions = IwbRoleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant);

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                foreach (var staticRoleDefinition in staticRoleDefinitions)
                {
                    var role = new TRole
                    {
                        //TenantId = tenantId,
                        Name = staticRoleDefinition.RoleName,
                        RoleDisplayName = staticRoleDefinition.RoleName,
                        IsStatic = true
                    };

                    var identityResult = await CreateAsync(role);
                    if (!identityResult.Succeeded)
                    {
                        return identityResult;
                    }
                }
            }

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateRoleNameAsync(int? expectedRoleId, string name, string displayName)
        {
            var role = await FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                return IwbIdentityResult.Failed(string.Format(L("RoleNameIsAlreadyTaken"), name));
            }

            role = await FindByDisplayNameAsync(displayName);
            if (role != null && role.Id != expectedRoleId)
            {
                return IwbIdentityResult.Failed(string.Format(L("RoleDisplayNameIsAlreadyTaken"), displayName));
            }

            return IdentityResult.Success;
        }

        private Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return AbpStore.FindByDisplayNameAsync(displayName);
        }

        private async Task<IwbRolePermissionCacheItem> GetRolePermissionCacheItemAsync(int roleId)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);

            return await CacheManager.GetRolePermissionCache().GetAsync(cacheKey, async () =>
            {
                var newCacheItem = new IwbRolePermissionCacheItem(roleId);

                var role = await Store.FindByIdAsync(roleId);
                if (role == null)
                {
                    throw new AbpException("There is no role with given id: " + roleId);
                }

                var staticRoleDefinition = IwbRoleManagementConfig.StaticRoles.FirstOrDefault(r =>
                    r.RoleName == role.Name);
                if (staticRoleDefinition != null)
                {
                    foreach (var permission in PermissionManager.GetAllPermissions())
                    {
                        if (staticRoleDefinition.IsGrantedByDefault(permission))
                        {
                            newCacheItem.GrantedPermissions.Add(permission.Name);
                        }
                    }
                }

                foreach (var permissionInfo in await IwbRolePermissionStore.GetPermissionsAsync(roleId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                    }
                    else
                    {
                        newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(LocalizationSourceName, name);
        }

        private int? GetCurrentTenantId()
        {
            if (UnitOfWorkManager.Current != null)
            {
                return UnitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}

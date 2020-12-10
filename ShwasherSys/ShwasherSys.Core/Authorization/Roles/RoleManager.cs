using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Roles;
using IwbZero.Configuration;

namespace ShwasherSys.Authorization.Roles
{
    public class RoleManager : IwbRoleManager<SysRole, SysUser>
    {
        
        public RoleManager(
            RoleStore store,
            IIwbPermissionManager permissionManager,
            IIwbRoleManagementConfig iwbRoleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(store,permissionManager,iwbRoleManagementConfig,cacheManager,unitOfWorkManager, ShwasherConsts.LocalizationSourceName)
        {
        }

    }
}

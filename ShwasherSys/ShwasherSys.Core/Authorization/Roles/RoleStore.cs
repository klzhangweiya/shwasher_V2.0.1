using Abp.Domain.Repositories;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Roles;

namespace ShwasherSys.Authorization.Roles
{
    public class RoleStore : IwbRoleStore<SysRole, SysUser>

    {
        public RoleStore(
            IRepository<SysRole> roleRepository,
            IRepository<SysUserRole, long> userRoleRepository,
            IRepository<SysPermission, long> rolePermissionSettingRepository):base(roleRepository,userRoleRepository,rolePermissionSettingRepository)

        {
        }

    }
}

using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ShwasherSys.Authorization.Roles;
using IwbZero.Authorization.Permissions;
using IwbZero.Authorization.Users;

namespace ShwasherSys.Authorization.Users
{
    public class UserStore : IwbUserStore<SysRole,SysUser>
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserStore(
            IRepository<SysUser, long> userRepository,
            IRepository<SysUserRole, long> userRoleRepository,
            IRepository<SysRole, int> roleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<SysPermission, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager):
            base(userRepository,userRoleRepository,roleRepository,userLoginRepository,userPermissionSettingRepository,unitOfWorkManager)
        {
            
        }

    }
}

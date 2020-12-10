using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization.Permissions;

namespace ShwasherSys.Authorization.Permissions
{
    public class ShwasherPermissionChecker : IwbPermissionChecker<SysRole, SysUser>
    {
        public ShwasherPermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}

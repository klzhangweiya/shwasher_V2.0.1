using System.Security.Claims;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization;

namespace ShwasherSys.Authorization
{
    public class LoginResult: IwbLoginResult<SysUser>
    {
        public LoginResult(AbpLoginResultType result, SysUser user = null) : base(result, user)
        {
        }

        public LoginResult(SysUser user, ClaimsIdentity identity) : base(user, identity)
        {
        }
    }
}

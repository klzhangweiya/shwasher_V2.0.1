using System.Security.Claims;
using IwbZero.Authorization.Users;

namespace IwbZero.Authorization
{
    public class IwbLoginResult<TUser>
        where TUser : IwbSysUser<TUser>
    {
        public AbpLoginResultType Result { get; private set; }

        public TUser User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public IwbLoginResult(AbpLoginResultType result, TUser user = null)
        {
            Result = result;
            User = user;
        }

        public IwbLoginResult(TUser user, ClaimsIdentity identity)
            : this(AbpLoginResultType.Success)
        {
            User = user;
            Identity = identity;
        }
    }
    public enum AbpLoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,

        InvalidPassword,

        UserIsNotActive,

        InvalidTenancyName,

        TenantIsNotActive,

        UserEmailIsNotConfirmed,

        UnknownExternalLogin,

        LockedOut,

        UserPhoneNumberIsNotConfirmed,
    }
}

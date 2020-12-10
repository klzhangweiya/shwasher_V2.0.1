using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Users
{
    public interface IIwbUserTokenProviderAccessor
    {
        IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>()
            where TUser : IwbSysUser<TUser>;
    }
}

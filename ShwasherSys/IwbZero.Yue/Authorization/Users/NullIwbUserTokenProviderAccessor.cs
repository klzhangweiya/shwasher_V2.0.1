using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace IwbZero.Authorization.Users
{
    public class NullIwbUserTokenProviderAccessor : IIwbUserTokenProviderAccessor, ISingletonDependency
    {
        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>() where TUser : IwbSysUser<TUser>
        {
            return null;
        }
    }
}

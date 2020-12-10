using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using IwbZero.Authorization.Users;
using IwbZero.BaseSysInfo;

namespace IwbZero.Navigation
{
    public interface IIwbNavigationManager<TFun, TUser> : ITransientDependency
        where TUser : IwbSysUser<TUser>
        where TFun : IwbSysFunction<TUser>
    {
        Task<IwbUserMenu> GetMenuAsync(UserIdentifier user);
        Task<IwbUserMenu> GetMenuAsync(UserIdentifier user, List<TFun> funs);
    }
    public interface IIwbNavigationManager: ITransientDependency
    {
        Task<IwbUserMenu> GetMenuAsync(UserIdentifier user);
    }
}

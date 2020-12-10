using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero;
using IwbZero.Navigation;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.Navigation
{
    public class NavigationManager : IwbNavigationManager<SysFunction, SysUser>
    {
        public NavigationManager(IRepository<SysFunction, int> sysFunctionRepository, ICacheManager cacheManager, IIocResolver iocResolver) : base(sysFunctionRepository, cacheManager, iocResolver)
        {
        }

        public override async Task<IwbUserMenu> GetMenuAsync(UserIdentifier user)
        {
            if (!TryGetFuns(out var funs))
            {
                funs = (await SysFunctionRepository.GetAllListAsync()).OrderBy(a=>a.Sort).ToList();
                await CacheManager.GetCache(IwbZeroConsts.SysFunctionCache).SetAsync("SysFun", funs);
            }
            var menus = await GetMenuAsync(user, funs);
            return menus;
        }

        private bool TryGetFuns(out List<SysFunction> funs)
        {
            funs = (List<SysFunction>)CacheManager.GetCache(IwbZeroConsts.SysFunctionCache).GetOrDefault("SysFun") ??
                   new List<SysFunction>();
            return funs.Any();
        }
    }
}

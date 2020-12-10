using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using IwbZero.Authorization.Users;
using IwbZero.BaseSysInfo;
using IwbZero.Session;

namespace IwbZero.Navigation
{
    //public class IwbNavigationManager<TFun, TUser> : IIwbNavigationManager<TFun, TUser>, ISingletonDependency
    public class IwbNavigationManager<TFun, TUser> : IIwbNavigationManager, ISingletonDependency
        where TUser : IwbSysUser<TUser>,new()
        where TFun : IwbSysFunction<TUser>
    {
        public IwbSession AbpSession { get; set; }

        protected readonly ICacheManager CacheManager;
        //private readonly ILocalizationContext _localizationContext;
        protected readonly IIocResolver IocResolver;
        protected readonly IRepository<TFun, int> SysFunctionRepository;

        public IwbNavigationManager(IRepository<TFun, int> sysFunctionRepository,
            //ILocalizationContext localizationContext,
            ICacheManager cacheManager,
            IIocResolver iocResolver)
        {
            SysFunctionRepository = sysFunctionRepository;
            CacheManager = cacheManager;
            //_localizationContext = localizationContext;
            IocResolver = iocResolver;
        }

        public virtual async  Task<IwbUserMenu> GetMenuAsync(UserIdentifier user)
        {
            if (!TryGetFuns(out var funs))
            {
                funs = await SysFunctionRepository.GetAllListAsync();
                await CacheManager.GetCache(IwbZeroConsts.SysFunctionCache).SetAsync("SysFun", funs);
            }
            var menus = await GetMenuAsync(user, funs);
            return menus;
        }
        public async Task<IwbUserMenu> GetMenuAsync(UserIdentifier user, List<TFun> funs)
        {
            var menuDefinition = GetMenuDefinition(funs);
            var userMenu = new IwbUserMenu(menuDefinition);
            await FillUserMenuItems(user, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }
        private bool TryGetFuns(out List<TFun> funs)
        {
            funs = (List<TFun>)CacheManager.GetCache(IwbZeroConsts.SysFunctionCache).GetOrDefault("SysFun") ??
                   new List<TFun>();
            return funs.Any();
        }
        private async Task<int> FillUserMenuItems(UserIdentifier user, IList<IwbMenuItemDefinition> menuItemDefinitions, IList<IwbUserMenuItem> userMenuItems)
        {
            //TODO: Can be optimized by re-using FeatureDependencyContext.

            var addedMenuItemCount = 0;

            using (var scope = IocResolver.CreateScope())
            {
                var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
                permissionDependencyContext.User = user;

                //var featureDependencyContext = scope.Resolve<FeatureDependencyContext>();
                //featureDependencyContext.TenantId = user == null ? null : user.TenantId;

                foreach (var menuItemDefinition in menuItemDefinitions)
                {
                    if (AbpSession.UserName != UserBase.AdminUserName)
                    {
                        if (menuItemDefinition.RequiresAuthentication && user == null)
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(menuItemDefinition.RequiredPermissionName))
                        {
                            var permissionDependency = new SimplePermissionDependency(menuItemDefinition.RequiredPermissionName);
                            if (user == null || !(await permissionDependency.IsSatisfiedAsync(permissionDependencyContext)))
                            {
                                continue;
                            }
                        }

                        if (menuItemDefinition.PermissionDependency != null &&
                            (user == null || !(await menuItemDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext))))
                        {
                            continue;
                        }

                        //if (menuItemDefinition.FeatureDependency != null &&
                        //    (AbpSession.MultiTenancySide == MultiTenancySides.Tenant || (user != null && user.TenantId != null)) &&
                        //    !(await menuItemDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext)))
                        //{
                        //    continue;
                        //}
                    }

                    var userMenuItem = new IwbUserMenuItem(menuItemDefinition);
                    if (menuItemDefinition.IsLeaf || (await FillUserMenuItems(user, menuItemDefinition.Items, userMenuItem.Items)) > 0)
                    {
                        userMenuItems.Add(userMenuItem);
                        ++addedMenuItemCount;
                    }
                }
            }

            return addedMenuItemCount;
        }
        private IwbMenuDefinition GetMenuDefinition(List<TFun> funs)
        {
            var menuDefinition = new IwbMenuDefinition("IwbAdmin", "IwbAdminNavMenu");
            //await _sysFunctionRepository.GetAllListAsync();
            //var funs = await _cacheManager.GetCache(IwbConsts.SysFunctionCache).GetAsync("SysFun", () => _sysFunctionRepository.GetAllListAsync());
            var topFunNo = System.Configuration.ConfigurationManager.AppSettings["SystemFunction.Top.FunctionNo"] ?? "HTSystem";
            var topFun = funs.FirstOrDefault(a => a.FunctionNo == topFunNo);
            if (topFun != null)
            {
                menuDefinition.AddItem(
                    new IwbMenuItemDefinition(topFun.PermissionName, "主页", topFun.Icon, "/", true,customData:topFun.FunctionType));
            }

            var childfuns = funs.Where(a => a.ParentNo == topFunNo);
            foreach (var fun in childfuns)
            {
                var menuItem = new IwbMenuItemDefinition(fun.PermissionName, fun.FunctionName, fun.Icon,
                    fun.Url, false, fun.PermissionName,customData:fun.FunctionType);
                AddMenuItemDefinition(menuItem, funs, fun.FunctionNo);
                menuDefinition.AddItem(menuItem);
            }
            return menuDefinition;
        }

        private void AddMenuItemDefinition(IwbMenuItemDefinition menuItem, List<TFun> funs, string parentFunNo)
        {
            var childFuns = funs.Where(a => a.ParentNo == parentFunNo);
            foreach (var fun in childFuns)
            {
                var childMenuItem = new IwbMenuItemDefinition(fun.PermissionName, fun.FunctionName, fun.Icon,
                    fun.Url, false, fun.PermissionName,customData:fun.FunctionType);
                menuItem.AddItem(childMenuItem);
                AddMenuItemDefinition(childMenuItem, funs, fun.FunctionNo);
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Threading;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Models.Layout;
using IwbZero;
using IwbZero.Authorization.Permissions;
using IwbZero.Navigation;
using IwbZero.Setting;
using ShwasherSys.Navigation;
using ShwasherSys.NotificationInfo;
using ShwasherSys.Views.Shared.New.SearchForm;

namespace ShwasherSys.Controllers
{
    [DisableAuditing, AllowAnonymous]
    public class LayoutController : ShwasherControllerBase
    {
       // private readonly IIwbNavigationManager<SysFunction,SysUser> _navigationManager;
        private readonly NavigationManager _navigationManager;
        private readonly ILanguageManager _languageManager;

        private readonly IRepository<SysFunction, int> _sysFunctionRepository;
        public IRepository<BulletinInfo> BulletinInfoRepository { get; }

        public LayoutController(
            //IIwbNavigationManager<SysFunction, SysUser> navigationManager,
            NavigationManager navigationManager,
            //ILocalizationManager localizationManager,
            IRepository<SysFunction, int> sysFunctionRepository,
            ICacheManager cacheManager,
            ILanguageManager languageManager,
            IIwbPermissionManager permissionManager,
            IIwbSettingManager settingManager, IRepository<BulletinInfo> bulletinInfoRepository)
        {
            _navigationManager = navigationManager;
            _languageManager = languageManager;
            BulletinInfoRepository = bulletinInfoRepository;
            _sysFunctionRepository = sysFunctionRepository;
            CacheManager = cacheManager;
            PermissionManager = permissionManager;
            SettingManager = settingManager;
        }

        /// <summary>
        /// 顶部导航栏
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult MainHeader()
        {
            ViewBag.SystemName = SettingManager.GetSettingValue(SettingNames.AdminSystemName);
            var model = new MainHeaderViewModel { UserInfos = GetCurrentUser() };
            var bulletinInfos = BulletinInfoRepository.GetAllList(i=>i.ExpirationDate>=DateTime.Now).OrderByDescending(i=>i.PromulgatTime).ToList();
            ViewBag.BulletinInfos = bulletinInfos;
            return PartialView("_MainHeader", model);
        }

        /// <summary>
        /// 左侧导航栏
        /// </summary>
        /// <param name="activeMenuName"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult SideBarNav(string activeMenuName = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _navigationManager.GetMenuAsync(AbpSession.ToUserIdentifier())),
                ActiveNames = new List<string>() { "Pages" },
                PageTitle = "<li><a href=\"/\" class=\"active\"><i class=\"iconfont icon-home\"></i>主页</a></li>"
            };

            if (!string.IsNullOrEmpty(activeMenuName))
            {
                var activeMenu = PermissionManager.GetPermission(activeMenuName);
                var nameList = new List<string>();
                model.PageTitle = GetIconAndName(activeMenu, ref nameList);
                model.ActiveNames = nameList;

            }

            return PartialView("_SideBarNav", model);
        }

        private string GetIconAndName(Permission permission, ref List<string> nameList, bool isFirst = true)
        {
            string iconName = "";
            if (permission != null)
            {
                var fun = CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache).Get(permission.Name,
                    () => _sysFunctionRepository.FirstOrDefault(a => a.PermissionName == permission.Name));
                string icon = permission.Name == PermissionNames.Pages ? "icon-home" : fun.Icon;
                string name = permission.Name == PermissionNames.Pages ? "主页" : fun.FunctionName;
                string active = isFirst ? "active" : "";
                string href = permission.Name == "Pages" ? "/" : "JavaScript:void(0)";
                string icn = $"<li><a href=\"{href}\" class=\"{active}\"><i class=\"iconfont {icon}\"></i> {name}</a></li>";
                if (permission.Name != "Pages")
                {
                    nameList.Add(permission.Name);
                }
                iconName = GetIconAndName(permission.Parent, ref nameList, false) + icn;
            }
            return iconName;
        }


        /// <summary>
        /// 单条件搜索
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="searchForm"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult ToolMenu(string pageName, object searchForm)
        {
            try
            {
                ViewBag.SearchFrom = (SearchFormViewModal)searchForm;
            }
            catch
            {
                try
                {
                    ViewBag.SearchFrom = (SearchFormViewModel)searchForm;
                }
                catch
                {
                    ViewBag.SearchFrom = null;
                }
               
            }
            if (string.IsNullOrEmpty(pageName))
            {
                return  PartialView("_ToolMenu");
            }
            var permission = PermissionManager.GetPermission(pageName);
            List<PermissionButtonViewModel> model = GetChildBtnPerms(permission);
            return  PartialView("_ToolMenu",model);
        }

        /// <summary>
        /// 多条件搜索
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="searchForm"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult ToolMenuWithMs(string pageName, object searchForm)
        {
            try
            {
                var mulitForm = (SearchFormViewModal)searchForm;
                mulitForm.IsSingle = false;
                ViewBag.SearchFrom = mulitForm;
            }
            catch
            {
                try
                {
                    ViewBag.SearchFrom = (SearchFormViewModel)searchForm;
                }
                catch
                {
                    ViewBag.SearchFrom = null;
                }
            }
            if (string.IsNullOrEmpty(pageName))
            {
                return PartialView("_ToolMenuWithMs");
            }
            var permission = PermissionManager.GetPermission(pageName);
            List<PermissionButtonViewModel> model = GetChildBtnPerms(permission);
            return PartialView("_ToolMenuWithMs", model);
        }

        /// <summary>
        /// 获取用户操作按钮
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        private List<PermissionButtonViewModel> GetChildBtnPerms(Permission permission)
        {
            List<PermissionButtonViewModel> permissions = new List<PermissionButtonViewModel>();
            if (permission != null && permission.Children.Count > 0)
            {
                foreach (var p in permission.Children)
                {
                    if (AsyncHelper.RunSync(() => PermissionChecker.IsGrantedAsync(p.Name)))
                    {
                        var sysFun = CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache).Get(p.Name,
                            () => _sysFunctionRepository.FirstOrDefault(a => a.PermissionName == p.Name));
                        permissions.Add(new PermissionButtonViewModel(sysFun));
                        //if (p.Children.Count > 0)
                        //{
                        //    permissions.AddRange(GetChildBtnPerms(p));
                        //}
                    }
                }
            }
            return permissions;
        }

        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages()
            };

            return PartialView("_LanguageSelection", model);
        }

    }
}
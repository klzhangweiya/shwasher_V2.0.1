using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Timing;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Security.AntiForgery;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.AuditLog;
using ShwasherSys.BaseSysInfo.AuditLog.Dto;
using ShwasherSys.BaseSysInfo.Functions;
using ShwasherSys.BaseSysInfo.Roles;
using ShwasherSys.BaseSysInfo.Roles.Dto;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BaseSysInfo.Users;
using IwbZero;
using IwbZero.Auditing;
using IwbZero.Authorization.Users;
using IwbZero.Setting;
using Abp.Extensions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BasicInfo.Departments;
using ShwasherSys.BasicInfo.Dutys;
using ShwasherSys.BasicInfo.Factory;

namespace ShwasherSys.Controllers
{
    [AuditLog("系统基础信息")]
    public class SystemController : ShwasherControllerBase
    {
        private readonly IUsersAppService _usersAppService;
        private readonly IRolesAppService _rolesAppService;
        private readonly IFunctionsAppService _funsAppService;
        private readonly IAuditLogsAppService _logsAppService;
        private readonly IFactoriesAppService _factoriesAppService;
        private readonly IDepartmentsAppService _departmentsAppService;
        private readonly IDutysAppService _dutysAppService;
        private readonly IRepository<SysHelp> _sysHelpRepository;

        public SystemController(
            IUsersAppService usersAppService,
            IRolesAppService rolesAppService,
            IFunctionsAppService funsAppService,
            IAuditLogsAppService logsAppService,
            ICacheManager cacheManager,
            IStatesAppService statesAppService, IFactoriesAppService factoriesAppService, IDepartmentsAppService departmentsAppService, IDutysAppService dutysAppService, IRepository<SysHelp> sysHelpRepository)
        {
            _usersAppService = usersAppService;
            _rolesAppService = rolesAppService;
            _funsAppService = funsAppService;
            _logsAppService = logsAppService;
            _factoriesAppService = factoriesAppService;
            _departmentsAppService = departmentsAppService;
            _dutysAppService = dutysAppService;
            _sysHelpRepository = sysHelpRepository;
            CacheManager = cacheManager;
            StatesAppService = statesAppService;
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemUsers), AuditLog("用户管理页面")]
        public ActionResult SysUsers()
        {
            var user = GetCurrentUser();
            ViewBag.UserType = _usersAppService.GetUserTypeSelect();
            ViewBag.IsActive = StatesAppService.GetSelectLists("SysUser", "IsActive");
            ViewBag.CurrentUser = user;
            ViewBag.Roles = _usersAppService.GetRoleSelects();
            ViewBag.Factories = _factoriesAppService.GetFactoriesSelects();
            ViewBag.Departments = _departmentsAppService.GetDepartmentsSelects();
            ViewBag.Duties = _dutysAppService.GetDutysSelects();
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemRoles), AuditLog("角色管理页面")]
        public ActionResult SysRoles()
        {
            ViewBag.RoleType = _rolesAppService.GetRoleTypeSelect();
            return View();
        }


        #region Auth

        /// <summary>
        /// 用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost, DisableAuditing, AbpAuthorize(PermissionNames.PagesSystemUsersAuth)]
        public async Task<ActionResult> GetUserPermission(long userId)
        {
            var permissions = (await _usersAppService.GetAllPermissions()).Items;
            List<PermissionDto> currentPerms = new List<PermissionDto>();
            if (AbpSession.UserName == UserBase.AdminUserName)
            {
                currentPerms.AddRange(permissions);
            }
            else
            {
                foreach (var perm in permissions)
                {
                    if (await PermissionChecker.IsGrantedAsync(perm.Name))
                        currentPerms.Add(perm);
                }
            }
            var permission = permissions.FirstOrDefault(a => a.Name == PermissionNames.Pages);
            var model = new PermissionViewModel();
            if (permission != null)
            {
                var fun = await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache)
                    .GetAsync(permission.Name, () => _funsAppService.GetFunByPermissionName(permission.Name));
                model.Name = permission.Name;
                model.IsAuth = await _usersAppService.IsGrantedOnlyUserAsync(userId, permission.Name);
                model.PermDisplayName = fun.FunctionName;
                model.Sort = fun.Sort;
                model.Icon = fun.Icon;
                model.IsOpen = fun.Depth < 2;
                model.Children = await GetPermissionTree(permission.Name, currentPerms, userId);
            }
            return AbpJson(model);
        }

        /// <summary>
        /// 获取用户权限树
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="permissions"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<PermissionViewModel>> GetPermissionTree(string parentName, List<PermissionDto> permissions, long userId)
        {
            var parentPerms = permissions.Where(a => a.Parent?.Name == parentName).OrderBy(a => a.Sort).ToList();
            var list = new List<PermissionViewModel>();
            if (parentPerms.Any())
            {
                foreach (var p in parentPerms)
                {
                    var fun = await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache)
                        .GetAsync(p.Name, () => _funsAppService.GetFunByPermissionName(p.Name));
                    var model = new PermissionViewModel
                    {
                        Name = p.Name,
                        IsAuth = await _usersAppService.IsGrantedOnlyUserAsync(userId, p.Name),
                        PermDisplayName = fun.FunctionName,
                        Sort = fun.Sort,
                        Icon = fun.Icon,
                        IsOpen = fun.Depth < 2,
                        Children = await GetPermissionTree(p.Name, permissions, userId)
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost, DisableAuditing, AbpAuthorize(PermissionNames.PagesSystemUsersAuth)]
        public async Task<ActionResult> GetRolePermission(int roleId)
        {
            var permissions = (await _rolesAppService.GetAllPermissions()).Items;
            List<PermissionDto> currentPerms = new List<PermissionDto>();
            if (AbpSession.UserName == UserBase.AdminUserName)
            {
                currentPerms.AddRange(permissions);
            }
            else
            {
                foreach (var perm in permissions)
                {
                    if (await PermissionChecker.IsGrantedAsync(perm.Name))
                        currentPerms.Add(perm);
                }
            }
            var permission = permissions.FirstOrDefault(a => a.Name == PermissionNames.Pages);
            var model = new PermissionViewModel();
            if (permission != null)
            {
                var fun = await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache)
                    .GetAsync(permission.Name, () => _funsAppService.GetFunByPermissionName(permission.Name));
                model.Name = permission.Name;
                model.IsAuth = await _rolesAppService.IsGrantedAsync(roleId, permission.Name);
                model.PermDisplayName = fun.FunctionName;
                model.Sort = fun.Sort;
                model.Icon = fun.Icon;
                model.IsOpen = fun.Depth < 2;
                model.Children = await GetPermissionTree(permission.Name, currentPerms, roleId);
            }
            return AbpJson(model);
        }

        /// <summary>
        /// 获取角色权限树
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="permissions"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<PermissionViewModel>> GetPermissionTree(string parentName, List<PermissionDto> permissions, int userId)
        {
            var parentPerms = permissions.Where(a => a.Parent?.Name == parentName).OrderBy(a => a.Sort).ToList();
            var list = new List<PermissionViewModel>();
            if (parentPerms.Any())
            {
                foreach (var p in parentPerms)
                {
                    var fun = await CacheManager.GetCache(IwbZeroConsts.SysFunctionItemCache)
                        .GetAsync(p.Name, () => _funsAppService.GetFunByPermissionName(p.Name));
                    var model = new PermissionViewModel
                    {
                        Name = p.Name,
                        IsAuth = await _rolesAppService.IsGrantedAsync(userId, p.Name),
                        PermDisplayName = fun.FunctionName,
                        Sort = fun.Sort,
                        Icon = fun.Icon,
                        IsOpen = fun.Depth < 2,
                        Children = await GetPermissionTree(p.Name, permissions, userId)
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        public class PermissionViewModel
        {
            public string Name { get; set; }
            public string PermDisplayName { get; set; }
            public int Sort { get; set; }
            public string Icon { get; set; }
            public bool IsOpen { get; set; }
            public bool IsAuth { get; set; }
            public List<PermissionViewModel> Children { get; set; }
        }
        #endregion


        [AbpMvcAuthorize(PermissionNames.PagesSystemSysFunction), AuditLog("功能菜单页面")]
        public ActionResult SysFunctions()
        {
            ViewBag.FunctionType = StatesAppService.GetSelectLists("SysFunction", "FunctionType");
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemSysState), AuditLog("系统字典页面")]
        public ActionResult SysStates()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemSysSetting), AuditLog("系统配置页面")]
        public ActionResult SysSettings()
        {
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemSysLog), AuditLog("操作日志页面")]
        public async Task<ActionResult> SysLogs()
        { 
            ViewBag.ServiceNames = await _logsAppService.GetLogServiceSelectListStrs();
            ViewBag.MethodNames = await _logsAppService.GetLogMethodSelectListStrs(new QueryMethodName());
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSystemSysHelp), AuditLog("系统帮助维护页面")]
        public ActionResult SysHelps()
        {
            ViewBag.Classification = StatesAppService.GetSelectLists("SysHelp", "Classification");
            return View();
        }

        public ActionResult SysHelpPreview()
        {
            var helps = _sysHelpRepository.GetAllList();
            return View(helps);
        }
        [DisableAbpAntiForgeryTokenValidation]
        [DontWrapResult]
        public ActionResult KindEditorUploadFile()
        {
            Hashtable hash;
            try
           {
                int maxSize = 1024*1024*10;
                HttpPostedFileBase file = Request.Files["imgFile"];
                if (file == null)
                {
                    hash = new Hashtable
                    {
                        ["error"] = 1,
                        ["url"] = "未上传文件！"
                    };
                    return Json(hash, "text/html;charset=UTF-8");
                }
                if (file.ContentLength > maxSize)
                {
                    hash = new Hashtable
                    {
                        ["error"] = 1,
                        ["url"] = "上传文件大于10M！"
                    };
                    return Json(hash, "text/html;charset=UTF-8");
                }
                var fileName = Clock.Now.ToString("yyyyMMddHHmmss") + file.FileName;
                string lcPath = SettingManager.GetValue(SettingNames.DownloadPath) + "/KindEditorUploadFile";
                string dir = Request["dir"];
                if (!dir.IsNullOrEmpty())
                {
                    lcPath = Path.Combine(lcPath, dir);
                }
                var filePath = Server.MapPath($"~/{lcPath}");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                file.SaveAs(Path.Combine(filePath, fileName));
                hash = new Hashtable
                {
                    ["error"] = 0,
                    ["url"] = Path.Combine(lcPath, fileName)
                };
                return Json(hash, "text/html;charset=UTF-8");
            }
            catch (Exception e)
            {
                this.LogError(e);
                hash = new Hashtable
                {
                    ["error"] = 1,
                    ["url"] = "附件上传失败！"
                };
                return Json(hash, "text/html;charset=UTF-8");

            }


        }
    }
}
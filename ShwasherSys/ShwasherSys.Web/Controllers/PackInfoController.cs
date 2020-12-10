using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.CompanyInfo;
using ShwasherSys.CompanyInfo.EmployeeInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("半成品包装信息")]
    public class PackInfoController : ShwasherControllerBase
    {
        public IEmployeeAppService EmployeeAppService { get; }
		public PackInfoController(IStatesAppService statesAppService,
			ICacheManager cacheManager,
			IIwbSettingManager settingManager, IEmployeeAppService employeeAppService)
        {
            EmployeeAppService = employeeAppService;
            StatesAppService = statesAppService;
			SettingManager = settingManager;
			CacheManager = cacheManager;
        }

        [AbpMvcAuthorize(PermissionNames.PagesPackInfoPackInfoMg), AuditLog("半成品包装维护页面")]
        public async Task<ActionResult> Index()
        {
            ViewBag.ProductApplyStatus = StatesAppService.GetSelectLists("FinshedEnterStore", "ApplyStatus");
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("PackInfoApply", "ApplyStatus");
            var packageMan =await SettingManager.GetSettingValueAsync(ShwasherSettingNames.CKBZRY);
            ViewBag.HsRate = 10;
            var pmList = packageMan.Split(',');
          
            ViewBag.Employee =await EmployeeAppService.GetSelectStr2((e) => pmList.Contains(e.No));
            return View();
        }

        [AbpMvcAuthorize]
        [AbpMvcAuthorize(PermissionNames.PagesPackInfoPackageApplyInfo), AuditLog("半成品包装申请查询页面")]
        public ActionResult PackageApplyInfo()
        {
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("PackInfoApply", "ApplyStatus");
            return View();
        }
    }
}
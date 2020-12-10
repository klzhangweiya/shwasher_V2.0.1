using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BasicInfo;
using ShwasherSys.BasicInfo.Region;
using ShwasherSys.Common;

namespace ShwasherSys.Controllers
{
    [AuditLog("基础信息维护")]
    public class BasicInfoController : ShwasherControllerBase
    {
        private IRegionsAppService _regionsAppService;
        private IQueryAppService QueryAppService;
        private IRepository<ExpressServiceProvider> ExpressServiceProviderRepository;
        public BasicInfoController(IRegionsAppService regionsAppService, IStatesAppService statesAppService, IQueryAppService queryAppService, IRepository<ExpressServiceProvider> expressServiceProviderRepository)
        {
            _regionsAppService = regionsAppService;
            QueryAppService = queryAppService;
            ExpressServiceProviderRepository = expressServiceProviderRepository;
            StatesAppService = statesAppService;
        }
        // GET: BasicInfo
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoDepartments), AuditLog("部门管理页面")]
        public ActionResult Departments()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoDutys), AuditLog("职务管理页面")]
        public ActionResult Dutys()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoFactories), AuditLog("办公场所管理页面")]
        public async Task<ActionResult> Factories()
        {
            ViewBag.RegionInfo =await _regionsAppService.GetRegionSelectStrs();
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoRegions), AuditLog("区域管理页面")]
        public ActionResult Regions()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoStoreHouses), AuditLog("仓库信息管理")]
        public ActionResult StoreHouses()
        {
            ViewBag.StoreHouseType = StatesAppService.GetSelectLists("StoreHouse", "StoreHouseType");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoStoreHouseLocations), AuditLog("仓库位置信息管理")]
        public ActionResult StoreHouseLocations()
        {
            ViewBag.StoreHouse = QueryAppService.QueryStoreHouseSelect();
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoStoreHouseLocations), AuditLog("仓库位置信息管理")]
        public ActionResult OutFactory()
        {
            //ViewBag.StoreHouse = QueryAppService.QueryStoreHouseSelect(1);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoCurrency), AuditLog("货币汇率信息管理")]
        public async Task<ActionResult> Currency()
        {
            //ViewBag.StoreHouse = QueryAppService.QueryStoreHouseSelect(1);
            ViewBag.Currency =await QueryAppService.QueryAllCurrency();
            return View();
        }
        
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoLicenseType), AuditLog("证照组信息管理")]
        public ActionResult LicenseType()
        {
            ViewBag.LicenseGroup = StatesAppService.GetSelectLists("LicenseType", "Type");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoLicenseType), AuditLog("证照组信息管理")]
        public ActionResult QualityIssueLabel()
        {
            ViewBag.LicenseGroup = StatesAppService.GetSelectLists("LicenseType", "Type");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoLicenseType), AuditLog("证照组信息管理")]
        public ActionResult ScrapType()
        {
            ViewBag.LicenseGroup = StatesAppService.GetSelectLists("LicenseType", "Type");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoLicenseType), AuditLog("证照组信息管理")]
        public ActionResult FixedAssetType()
        {
            ViewBag.LicenseGroup = StatesAppService.GetSelectLists("LicenseType", "Type");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesBasicInfoExpress), AuditLog("快递公司信息管理")]
        public async Task<ActionResult> Express()
        {
            //var providers = await ExpressServiceProviderRepository.GetAllListAsync();
            //List<SelectListItem> proListItems = HtmlHelpers.TranSelectItems<ExpressServiceProvider>(providers, "ExpressName", "Id");
            //ViewBag.Providers = proListItems;
            ViewBag.Providers = await ExpressServiceProviderRepository.GetAllListAsync();
            return View();
        }
    }
}
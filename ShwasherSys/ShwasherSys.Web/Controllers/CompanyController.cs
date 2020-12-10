using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Abp.Runtime.Caching;
using IwbZero.Auditing;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BasicInfo.Departments;
using ShwasherSys.BasicInfo.Dutys;
using ShwasherSys.BasicInfo.FixedAssetTypeInfo;
using ShwasherSys.CompanyInfo.FixedAssetInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("公司管理信息")]
    public class CompanyController : ShwasherControllerBase
    {

		public CompanyController(IStatesAppService statesAppService, ICacheManager cacheManager, IDutysAppService dutiesAppService, IDepartmentsAppService departmentsAppService, IFixedAssetTypeAppService fixedAssetTypeAppService, IFixedAssetAppService fixedAssetAppService)
        {
            DutiesAppService = dutiesAppService;
            DepartmentsAppService = departmentsAppService;
            FixedAssetTypeAppService = fixedAssetTypeAppService;
            FixedAssetAppService = fixedAssetAppService;
            CacheManager = cacheManager;
            StatesAppService = statesAppService;
        }

        protected IDutysAppService DutiesAppService { get; }
        protected IDepartmentsAppService DepartmentsAppService { get; }
        protected IFixedAssetTypeAppService FixedAssetTypeAppService { get; }
        protected IFixedAssetAppService FixedAssetAppService { get; }


        [AbpMvcAuthorize]
        public ActionResult Employee()
        {
            ViewBag.Duty = DutiesAppService.GetDutysSelects();
            ViewBag.Department = DepartmentsAppService.GetDepartmentsSelects();
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult Performance()
        {
            ViewBag.WorkType = StatesAppService.GetSelectLists("Performance", "WorkType");
            return View();
        }
        [AbpMvcAuthorize]
        public async Task<ActionResult> FixedAsset()
        {
            ViewBag.FixedAssetType = await FixedAssetTypeAppService.GetSelectList();
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult LicenseDocument()
        {
            ViewBag.LicenseGroup = StatesAppService.GetSelectLists("LicenseType", "Type");
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult Mold()
        {
            return View();
        }
        [AbpMvcAuthorize]
        public async Task<ActionResult> DeviceMgPlan()
        {
            ViewBag.FixedAsset =await FixedAssetAppService.GetSelectListName();
            ViewBag.PlanType = StatesAppService.GetSelectLists("Maintain", "Type");
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult MaintainRecord()
        {
            ViewBag.MaintainType = StatesAppService.GetSelectLists("Maintain", "Type");
            ViewBag.MaintainState = StatesAppService.GetSelectLists("Maintain", "State");
            return View();
        }
    }

}
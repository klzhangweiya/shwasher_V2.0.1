using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Runtime.Caching;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.BaseSysInfo.States;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("数据统计信息")]
    public class StatisticController : ShwasherControllerBase
    {
        public StatisticController(IStatesAppService statesAppService, ICacheManager cacheManager)
        {
            StatesAppService = statesAppService;
            CacheManager = cacheManager;
        }
        [AbpMvcAuthorize]
        public ActionResult PackageDaily()
        {
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult ProductionReport()
        {
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult OutsourcingReport()
        {
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult InventoryCheckReport()
        {
            ViewBag.CheckStateListStr = StatesAppService.GetSelectListStrs("InventoryCheck", "CheckState");
            return View();
        }
        [AbpMvcAuthorize]
        public ActionResult StatementBillReport()
        {
            
            return View();
        }
        //[AbpMvcAuthorize]
        //public ActionResult SalesReport()
        //{
        //    return View();
        //}

    }
}
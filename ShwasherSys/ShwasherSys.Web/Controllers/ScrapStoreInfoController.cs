using ShwasherSys.BaseSysInfo.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Common;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("废品出入库维护")]
    public class ScrapStoreInfoController : ShwasherControllerBase
    {
        protected IQueryAppService QueryAppService { get; }
        public ScrapStoreInfoController(IStatesAppService statesAppService, IQueryAppService queryAppService)
        {
            QueryAppService = queryAppService;
            StatesAppService = statesAppService;
        }
       
        [AbpMvcAuthorize(PermissionNames.PagesScrapStoreScrapStoreEnterMg), AuditLog("废品入库页面")]
        public ActionResult ScrapStoreEnterMg()
        {
            ViewBag.ProductType = StatesAppService.GetSelectLists("ScrapEnterStore", "ProductType");
            ViewBag.ScrapSource = StatesAppService.GetSelectLists("ScrapEnterStore", "ScrapSource");
            return View();
        }

      

      

        //[AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreApplyMg), AuditLog("半成品出库申请审核页面")]
        //public ActionResult SemiOutStoreApplyMg()
        //{
        //    string s1 = OutStoreApplyStatusEnum.Applying.ToInt() + "",
        //        s2 = OutStoreApplyStatusEnum.Audited.ToInt() + "",
        //        s3 = OutStoreApplyStatusEnum.Refused.ToInt() + "",
        //        s4 = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
        //    ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus", a =>
        //        a.CodeValue == s1 ||
        //        a.CodeValue == s2 ||
        //        a.CodeValue == s3 ||
        //        a.CodeValue == s4);
        //    ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
        //    return View();
        //}

        //[AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreMg), AuditLog("半成品出库信息页面")]
        //public ActionResult SemiOutStoreMg()
        //{
        //    ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus");
        //    ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
        //    return View();
        //}
    }
}
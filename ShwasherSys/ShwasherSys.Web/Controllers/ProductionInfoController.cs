using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo.EmployeeInfo;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("半成品排产出入库维护")]
    public class ProductionInfoController : ShwasherControllerBase
    {
        protected IQueryAppService QueryAppService { get; }
        protected ICommonAppService CommonAppService{ get; }
    
        public IEmployeeAppService EmployeeAppService { get; }
        public ProductionInfoController(IStatesAppService statesAppService, IQueryAppService queryAppService, IEmployeeAppService employeeAppService, ICommonAppService commonAppService)
        {
            QueryAppService = queryAppService;
            EmployeeAppService = employeeAppService;
            CommonAppService = commonAppService;
            StatesAppService = statesAppService;
        }
        // GET: ProductionInfo
        [AbpMvcAuthorize(PermissionNames.PagesProductionInfoProductionOrderMg), AuditLog("半成品排产页面")]
        public  async Task<ActionResult> ProductionOrderMg()
        {
            ViewBag.ProductionOrderStatus = StatesAppService.GetSelectLists("ProductionOrders", "ProductionOrderStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");
            ViewBag.Employee =await EmployeeAppService.GetSelectList();
            await CommonAppService.CloseProductOrder();
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesProductionInfoProductionEnterStoreApplyMg), AuditLog("半成品排产入库申请页面")]
        public ActionResult ProductionEnterStoreApplyMg()
        {
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesProductionInfoProductionOutStoreApplyMg), AuditLog("半成品外协出库申请页面")]
        public ActionResult ProductionOutStoreApplyMg()
        {
            string exclude = ProductionOrderProcessTypeEnum.CarMachining.ToInt() + "";
            ViewBag.ProcessTypeItems = StatesAppService.GetSelectLists("ProductionOrders", "ProcessingType",
                i => i.CodeValue != exclude);
            ViewBag.ApplyStatus= StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesProductionInfoOutProductionOrderMg), AuditLog("半成品外协排产页面")]
        public  async Task<ActionResult> OutProductionOrderMg()
        {
            ViewBag.ProductionOrderStatus = StatesAppService.GetSelectLists("ProductionOrders", "ProductionOrderStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");
            ViewBag.Employee =await EmployeeAppService.GetSelectList();
            return View();
        }

        //[AbpMvcAuthorize(PermissionNames.PagesProductionInfoOutProductionOrderMg), AuditLog("半成品外协排产页面")]
        public  ActionResult QueryAll()
        {
            ViewBag.ProductionOrderStatus = StatesAppService.GetSelectLists("ProductionOrders", "ProductionOrderStatus");
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");
            ViewBag.PackageApplyStatus = StatesAppService.GetSelectLists("PackInfoApply", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesProductionInfoRePlatingOutStoreApplyMg), AuditLog("改镀出库申请")]
        public ActionResult RePlatingOutStoreApplyMg()
        {
            string exclude = ProductionOrderProcessTypeEnum.CarMachining.ToInt() + "";
            ViewBag.ProcessTypeItems = StatesAppService.GetSelectLists("ProductionOrders", "ProcessingType",
                i => i.CodeValue != exclude);
            ViewBag.ApplyStatus = StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }


    }
}
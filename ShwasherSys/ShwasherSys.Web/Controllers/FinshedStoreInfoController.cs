using ShwasherSys.BaseSysInfo.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BasicInfo;
using ShwasherSys.Common;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.StoreQuery;
using ShwasherSys.StoreQuery.Dto;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize,AuditLog("成品仓库维护")]
    public class FinshedStoreInfoController : ShwasherControllerBase
    {
        protected IInventoryCheckAppService InventoryCheckAppService{ get; }
        protected IStoreStatisticsApplicationService StoreStatisticsApplicationService { get; }
        protected IQueryAppService QueryAppService { get; }
        protected IRepository<InventoryCheckInfo,string> InventoryCheckInfoRepository { get; }
        protected ISqlExecuter SqlExcuter { get; }
        public FinshedStoreInfoController(IStatesAppService statesAppService, IStoreStatisticsApplicationService storeStatisticsApplicationService, IQueryAppService queryAppService, ISqlExecuter sqlExcuter, IRepository<InventoryCheckInfo, string> inventoryCheckInfoRepository, IInventoryCheckAppService inventoryCheckAppService)
        {
            StoreStatisticsApplicationService = storeStatisticsApplicationService;
            QueryAppService = queryAppService;
            SqlExcuter = sqlExcuter;
            InventoryCheckInfoRepository = inventoryCheckInfoRepository;
            InventoryCheckAppService = inventoryCheckAppService;
            StatesAppService = statesAppService;
        }
        // GET: FinshedProductStoreInfo
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreApplyMg),AuditLog("成品入库申请页面")]
        public ActionResult FinshedEnterStoreApplyMg()
        {
            string s1 = FinshedEnterStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 = FinshedEnterStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = FinshedEnterStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = FinshedEnterStoreApplyStatusEnum.EnterStored.ToInt() + "";
            ViewBag.FinshedApplyStatus = StatesAppService.GetSelectLists("FinshedEnterStore", "ApplyStatus", a =>
                a.CodeValue == s1 ||
                a.CodeValue == s2 ||
                a.CodeValue == s3 ||
                a.CodeValue == s4);
            ViewBag.StoreHouse = QueryAppService.QueryStoreHouseSelect();
            ViewBag.CreateSourceType = StatesAppService.GetSelectLists("FinshedEnterStore", "CreateSourceType");
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedEnterStoreMg),AuditLog("成品入库信息页面")]
        public ActionResult FinshedEnterStoreMg()
        {
            ViewBag.FinshedApplyStatus = StatesAppService.GetSelectLists("FinshedEnterStore", "ApplyStatus");
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreApplyMg),AuditLog("成品出库申请页面")]
        public ActionResult FinshedOutStoreApplyMg()
        {
            string s1 = OutStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 = OutStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = OutStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            ViewBag.FinshedApplyStatus = StatesAppService.GetSelectLists("FinshedOutStore", "ApplyStatus", a =>
                a.CodeValue == s1 ||
                a.CodeValue == s2 ||
                a.CodeValue == s3 ||
                a.CodeValue == s4);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoFinshedOutStoreMg),AuditLog("成品出库信息页面")]
        public ActionResult FinshedOutStoreMg()
        {
            ViewBag.FinshedApplyStatus = StatesAppService.GetSelectLists("FinshedOutStore", "ApplyStatus");
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentFinshedStoreHouseMg),AuditLog("成品仓库实时信息页面")]
        public ActionResult CurrentFinshedStoreHouseMg()
        {
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(1);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMg), AuditLog("库存数量查询页面")]
        public ActionResult CurrentStoreHouseQueryMg()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMgPrint), AuditLog("库存清单明细")]
        public ActionResult StoreListReport()
        {
            string lcSql =
                @"select vp.*,e.AllEnterQuantity,o.AllOutQuantity from v_ProductStoreInfo vp left join (select ProductNo,SUM(Quantity) as AllEnterQuantity from (select ProductNo,StoreHouseId,Quantity,AuditDate from FinshedEnterStore where ApplyStatus=5 and EnterStoreDate > (CONVERT(varchar(7),GETDATE(),120)+'-01')) gt group by ProductNo
) as e on vp.ProductNo=e.ProductNo left join (select ProductNo,SUM(ActualQuantity) as AllOutQuantity from (select ProductNo,StoreHouseId,ActualQuantity,AuditDate from ProductOutStore where  ApplyStatus=5 and OutStoreDate > (CONVERT(varchar(7),GETDATE(),120)+'-01')) gt group by ProductNo) as o
on vp.ProductNo=o.ProductNo";
            var entity = SqlExcuter.SqlQuery<ProductStoreCount>(lcSql).ToList();
            ViewBag.QueryRecord = entity;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMgQueryEnterOut), AuditLog("进出库信息查看")]
        public async Task<ActionResult> ProductStoreDetail(string id)
        {
            id = string.IsNullOrEmpty(id) ? Request["id"] : id;
            var enterOutList = await StoreStatisticsApplicationService.QueryProductEnterOutRecord(id);
            var productStore = await StoreStatisticsApplicationService.QueryStoreTotalByProduct(id);
            if (enterOutList.Any())
            {
                ViewBag.EnterOutList = enterOutList;
            }
            ViewBag.ProductStore = productStore;
            var storeHouseList = QueryAppService.QueryStoreHouse();
            ViewBag.StoreHouseList = storeHouseList;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoEnterOutStoreHouseQueryMg), AuditLog("进出库数量查询页面")]
        public ActionResult EnterOutStoreHouseQueryMg()
        {
            var storeHouseList = QueryAppService.QueryStoreHouse();
            ViewBag.StoreHouseList = HtmlHelpers.TranSelectItems<StoreHouse>(storeHouseList, "StoreHouseName", "Id");
            return View();
        }

        public ActionResult InventoryCheckCreate()
        {
            var storeHouseList = QueryAppService.QueryStoreHouse().Where(i=>i.StoreHouseTypeId!=3).ToList();

            ViewBag.StoreHouseList = storeHouseList; /*HtmlHelpers.TranSelectItems<StoreHouse>(storeHouseList, "StoreHouseName", "Id");*/
            ViewBag.CheckState = StatesAppService.GetSelectLists("InventoryCheck", "CheckState");
            return View();
        }

        public ActionResult InventoryCheck()
        {
            var storeHouseList = QueryAppService.QueryStoreHouse().Where(i => i.StoreHouseTypeId != 3).ToList();

            ViewBag.StoreHouseList = HtmlHelpers.TranSelectItems<StoreHouse>(storeHouseList, "StoreHouseName", "Id");
            ViewBag.CheckState = StatesAppService.GetSelectLists("InventoryCheck", "CheckState");
            return View();
        }

        public async Task<ActionResult>  InventoryCheckRecord(string id)
        {
            ViewBag.InventoryCheck = ObjectMapper.Map<InventoryCheckDto>(InventoryCheckInfoRepository.Get(id));
            var storeHouseList = QueryAppService.QueryStoreHouse().Where(i => i.StoreHouseTypeId != 3).ToList();
            ViewBag.StoreHouseList = HtmlHelpers.TranSelectItems<StoreHouse>(storeHouseList, "StoreHouseName", "Id");
            ViewBag.CheckState = StatesAppService.GetSelectLists("InventoryCheck", "CheckState");
            return View();
        }
        public async Task<ActionResult> InventoryCheckRecordSemi(string id)
        {
            ViewBag.InventoryCheck = ObjectMapper.Map<InventoryCheckDto>(InventoryCheckInfoRepository.Get(id));
            var storeHouseList = QueryAppService.QueryStoreHouse().Where(i => i.StoreHouseTypeId != 3).ToList();
            ViewBag.StoreHouseList = HtmlHelpers.TranSelectItems<StoreHouse>(storeHouseList, "StoreHouseName", "Id");
            ViewBag.CheckState = StatesAppService.GetSelectLists("InventoryCheck", "CheckState");
            return View();
        }


    }
}
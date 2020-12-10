using ShwasherSys.BaseSysInfo.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Common;
using ShwasherSys.EntityFramework;
using ShwasherSys.StoreQuery;
using ShwasherSys.StoreQuery.Dto;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("半成品排产出入库维护")]
    public class SemiProductStoreInfoController : ShwasherControllerBase
    {
        protected IQueryAppService QueryAppService { get; }
        protected ISqlExecuter SqlExcuter { get; }
        protected IStoreStatisticsApplicationService StoreStatisticsApplicationService { get; }
       
        public SemiProductStoreInfoController(IStatesAppService statesAppService, IQueryAppService queryAppService, ISqlExecuter sqlExcuter, IStoreStatisticsApplicationService storeStatisticsApplicationService)
        {
            QueryAppService = queryAppService;
            SqlExcuter = sqlExcuter;
            StoreStatisticsApplicationService = storeStatisticsApplicationService;
            StatesAppService = statesAppService;
        }
        // GET: SemiProductStoreInfo
        [AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiEnterStoreApplyMg), AuditLog("半成品入库申请审核页面")]
        public ActionResult SemiEnterStoreApplyMg()
        {
            string s1 = EnterStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 = EnterStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = EnterStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = EnterStoreApplyStatusEnum.EnterStored.ToInt() + "";
            ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus", a =>
                a.CodeValue == s1 ||
                a.CodeValue == s2 ||
                a.CodeValue == s3||
                a.CodeValue == s4);
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiEnterStoreMg), AuditLog("半成品入库信息页面")]
        public ActionResult SemiEnterStoreMg()
        {
            ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoCurrentSemiStoreHouseMg), AuditLog("半成品仓库实时信息页面")]
        public ActionResult CurrentSemiStoreHouseMg()
        {
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreApplyMg), AuditLog("半成品出库申请审核页面")]
        public ActionResult SemiOutStoreApplyMg()
        {
            string s1 = OutStoreApplyStatusEnum.Applying.ToInt() + "",
                s2 = OutStoreApplyStatusEnum.Audited.ToInt() + "",
                s3 = OutStoreApplyStatusEnum.Refused.ToInt() + "",
                s4 = OutStoreApplyStatusEnum.OutStored.ToInt() + "";
            ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus", a =>
                a.CodeValue == s1 ||
                a.CodeValue == s2 ||
                a.CodeValue == s3 ||
                a.CodeValue == s4);
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesSemiProductStoreInfoSemiOutStoreMg), AuditLog("半成品出库信息页面")]
        public ActionResult SemiOutStoreMg()
        {
            ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiOutStore", "ApplyStatus");
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            return View();
        }

        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMg), AuditLog("半成品库存数量查询页面")]
        public ActionResult CurrentSemiStoreHouseQueryMg()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMgPrint), AuditLog("库存清单明细")]
        public ActionResult StoreListReport()
        {
            string lcSql =
                @"select vp.*,vp.Id as SemiProductNo,e.AllEnterQuantity,o.AllOutQuantity from v_SemiProductStoreInfo vp left join 
    (select SemiProductNo,SUM(Quantity) as AllEnterQuantity from 
	     (select SemiProductNo,StoreHouseId,Quantity,AuditDate from SemiEnterStore where ApplyStatus=5 and EnterStoreDate > (CONVERT(varchar(7),GETDATE(),120)+'-01')) gt 
		      group by SemiProductNo) as e on vp.Id=e.SemiProductNo left join 
(select SemiProductNo,SUM(ActualQuantity) as AllOutQuantity from 
   (select SemiProductNo,StoreHouseId,ActualQuantity,AuditDate from SemiOutStores where  ApplyStatus=5 and OutStoreDate > (CONVERT(varchar(7),GETDATE(),120)+'-01')) gt 
   group by SemiProductNo) as o
on vp.Id=o.SemiProductNo";
            var entity = SqlExcuter.SqlQuery<SemiProductStoreCount>(lcSql).ToList();
            ViewBag.QueryRecord = entity;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesFinshedStoreInfoCurrentStoreHouseQueryMgQueryEnterOut), AuditLog("进出库信息查看")]
        public async Task<ActionResult> ProductStoreDetail(string id)
        {
            id = string.IsNullOrEmpty(id) ? Request["id"] : id;
            var enterOutList = await StoreStatisticsApplicationService.QuerySemiEnterOutRecord(id);
            var productStore = await StoreStatisticsApplicationService.QuerySemiCurrentStoreTotalByProduct(id);
            if (enterOutList.Any())
            {
                ViewBag.EnterOutList = enterOutList;
            }
            ViewBag.ProductStore = productStore;
            var storeHouseList = QueryAppService.QueryStoreHouse(houseType: 2);
            ViewBag.StoreHouseList = storeHouseList;
            return View();
        }

    }
}
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize,DisableAuditing]
    public class ProductInspectController : ShwasherControllerBase
    {
        protected IProductsAppService ProductsAppService{ get; }
        protected IStandardsAppService StandardsAppService{ get; }
        protected IQueryAppService QueryAppService { get; }

        public ProductInspectController(IStatesAppService statesAppService,IProductsAppService productsAppService, IStandardsAppService standardsAppService, IQueryAppService queryAppService)
        {
            ProductsAppService = productsAppService;
            StandardsAppService = standardsAppService;
            QueryAppService = queryAppService;
            StatesAppService = statesAppService;
        }

      
        [AbpMvcAuthorize(PermissionNames.PagesProductInspectProductItemInspectMg)]
        public ActionResult ProductItem()
        {
            ViewBag.UserName = AbpSession.UserName?.ToLower();
            ViewBag.StoreHouses = QueryAppService.QueryStoreHouseSelect(2);
            ViewBag.SemiApplyStatus = StatesAppService.GetSelectLists("SemiEnterStore", "ApplyStatus");

            return View();
        }
      
        [AbpMvcAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public ActionResult DisqualifiedProduct()
        {
            ViewBag.UserName = AbpSession.UserName?.ToLower();
            ViewBag.HandleType = StatesAppService.GetSelectLists("DisProduct", "HandleType");
            ViewBag.StoreHouse1 = QueryAppService.QueryStoreHouseSelectStr(2);
            ViewBag.StoreHouse2 = QueryAppService.QueryStoreHouseSelectStr(1);

            return View();
        }
       
        [AbpMvcAuthorize(PermissionNames.PagesProductInspectProductInspectMg)]
        public ActionResult Index()
        {
            ViewBag.UserName = AbpSession.UserName?.ToLower();
            return View();
        }
      
        [AbpMvcAuthorize(PermissionNames.PagesProductInspectInspectReport)]
        public ActionResult Report()
        {
            
            return View();
        }
      
    }
}
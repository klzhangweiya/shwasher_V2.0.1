using IwbZero.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.Controllers
{
    [AuditLog("产品信息维护")]
    public class ProductInfoController : ShwasherControllerBase
    {
        protected IProductsAppService ProductsAppService;
        protected IStandardsAppService StandardsAppService;

        public ProductInfoController(IProductsAppService productsAppService, IStandardsAppService standardsAppService)
        {
            ProductsAppService = productsAppService;
            StandardsAppService = standardsAppService;
        }

        // GET: ProductInfo
        public ActionResult Products()
        {
            ViewBag.MaterialSelect = ProductsAppService.GetProductPropertyList("Material");
            ViewBag.SurfaceColorSelect = ProductsAppService.GetProductPropertyList("SurfaceColor");
            ViewBag.RigiditySelect = ProductsAppService.GetProductPropertyList("Rigidity");
            ViewBag.StandardIdSelect = StandardsAppService.GetStandardsList();
            return View();
        }
        // GET: ProductInfo
        public ActionResult SemiProducts()
        {
          
            return View();
        }
        public ActionResult Standards()
        {
            return View();
        }
        public ActionResult RmProduct()
        {
            return View();
        }

       
    }
}
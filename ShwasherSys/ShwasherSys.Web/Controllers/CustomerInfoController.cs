using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.UI;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.Controllers
{
    public class CustomerInfoController : ShwasherControllerBase
    {
        protected CustomersAppService CustomersAppService;

        public CustomerInfoController(CustomersAppService customersAppService,IStatesAppService statesAppService)
        {
            CustomersAppService = customersAppService;
            StatesAppService = statesAppService;
        }

        // GET: CustomerInfo
        public ActionResult Customers()
        {
            ViewBag.SaleType = StatesAppService.GetSelectLists("Customer", "SaleType");
            return View();
        }
        public async Task<ActionResult> CustomerDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("未找到对应的客户！");
            }
            var loCustomer =await CustomersAppService.Get(new EntityDto<string>() {Id = id});

            return View(loCustomer);
        }
    }
}
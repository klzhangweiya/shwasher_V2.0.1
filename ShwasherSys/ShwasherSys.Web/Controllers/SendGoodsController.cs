using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using IwbZero.Auditing;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BasicInfo;
using ShwasherSys.Common;
using ShwasherSys.CustomerInfo;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize,AuditLog("发货管理")]
    public class SendGoodsController : ShwasherControllerBase
    {
        protected IOrderSendBillAppService OrderSendBillAppService;
        protected IRepository<OrderSend> OrderSendRepository;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        protected IRepository<Customer,string> CustomerRepository;
        protected IQueryAppService QueryAppService;
        private IRepository<ExpressLogistics> ExpressLogisticsRepository;
        private IRepository<ExpressProviderMapper> ExpressProviderMapperRepository;
        private IRepository<ExpressServiceProvider> ExpressServiceProviderRepository;
        public SendGoodsController(IOrderSendBillAppService orderSendBillAppService, IRepository<OrderSend> orderSendRepository, IRepository<Customer, string> customerRepository, IRepository<ViewOrderSend> viewOrderSendRepository, IQueryAppService queryAppService, IStatesAppService statesAppService, IRepository<ExpressLogistics> expressLogisticsRepository, IRepository<ExpressProviderMapper> expressProviderMapperRepository, IRepository<ExpressServiceProvider> expressServiceProviderRepository)
        {
            OrderSendBillAppService = orderSendBillAppService;
            OrderSendRepository = orderSendRepository;
            CustomerRepository = customerRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            QueryAppService = queryAppService;
            ExpressLogisticsRepository = expressLogisticsRepository;
            ExpressProviderMapperRepository = expressProviderMapperRepository;
            ExpressServiceProviderRepository = expressServiceProviderRepository;
            StatesAppService = statesAppService;
        }
        [AbpMvcAuthorize(PermissionNames.PagesSendGoodsOrderSendBillCreate),AuditLog("发货单创建页面")]
        // GET: SendGoods
        public ActionResult OrderSendBillCreate()
        {
            ViewBag.ExpressList = ExpressLogisticsRepository.GetAll().OrderByDescending(a=>a.Sort).ToList();
            ViewBag.CustomerList = OrderSendBillAppService.GetHasSendOrderCustomer();
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesSendGoodsOrderSendBillMg), AuditLog("发货单管理页面")]
        public ActionResult OrderSendBillMg()
        {
            
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesSendGoodsOrderSendBillMgShowSendBill), AuditLog("查看发货单详情页面")]
        public async Task<ActionResult> OrderSendBillDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("未传入对应编号！");
            }
            ViewBag.SendBillNo = id;
            var bill = await OrderSendBillAppService.Get(new EntityDto<string>(id));
            ViewBag.SendBill = bill;
            ViewBag.OrderSends = ViewOrderSendRepository.GetAllList(i => i.OrderSendBillNo == id);
            ViewBag.CustomerInfo = CustomerRepository.Get(bill.CustomerId);
            var templateInfo = await QueryAppService.QueryTemplate(bill.CustomerId, 2);
            ViewBag.TemplateInfo = templateInfo;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesSendGoodsOrderSendQueryMg), AuditLog("客户发货统计页面")]
        public ActionResult OrderSendQueryMg()
        {
            ViewBag.OrderSaleType = StatesAppService.GetSelectLists("OrderHeader", "SaleType");
            return View();
        }
        public ActionResult ShowExpressProcess()
        {
            int expressId = Convert.ToInt32(Request["expressId"]);
            string expressBillNo = Request["expressBillNo"];

            //暂时测试使用快递100
            var providerMapper = ExpressProviderMapperRepository.GetAllIncluding(i => i.ExpressServiceProvider)
                .FirstOrDefault(i => i.ExpressId == expressId && i.ActiveStatus == 1);
            var url = string.Format(providerMapper?.ExpressServiceProvider?.QueryApiUrl ?? "", providerMapper?.MapperCode,
                expressBillNo);
            return Redirect(url);
        }
        
        public ActionResult ReturnGood()
        {
            ViewBag.ReturnState = StatesAppService.GetSelectLists("ReturnGood", "StateType");
            return View();
        }
    }
}
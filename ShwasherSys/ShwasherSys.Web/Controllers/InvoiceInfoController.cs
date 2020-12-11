using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using ShwasherSys.CustomerInfo;
using ShwasherSys.Invoice;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.OrderSendInfo.Dto;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize, AuditLog("发票管理")]
    public class InvoiceInfoController : ShwasherControllerBase
    {
        protected IOrderSendBillAppService OrderSendBillAppService;
        public IRepository<OrderSendBill,string> OrderSendBillRepository { get; set; }
        protected IRepository<OrderSend> OrderSendRepository;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        protected IRepository<ViewOrderSendStickBill> ViewOrderSendStickBillRepository;
        protected IRepository<Customer,string> CustomerRepository;
        protected IStatementBillAppService StatementBillAppService;
        protected IRepository<StatementBill> StatementBillRepository;
        protected IRepository<OrderStickBill,string> OrderStickBillRepository;
        public InvoiceInfoController(IStatesAppService statesAppService, IOrderSendBillAppService orderSendBillAppService, IRepository<OrderSend> orderSendRepository, IRepository<Customer, string> customerRepository, IRepository<ViewOrderSend> viewOrderSendRepository, IRepository<ViewOrderSendStickBill> viewOrderSendStickBillRepository, IRepository<OrderSendBill, string> orderSendBillRepository, IStatementBillAppService statementBillAppService, IRepository<StatementBill> statementBillRepository,IRepository<OrderStickBill, string> orderStickBillRepository)
        {
            OrderSendBillAppService = orderSendBillAppService;
            OrderSendRepository = orderSendRepository;
            CustomerRepository = customerRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            ViewOrderSendStickBillRepository = viewOrderSendStickBillRepository;
            OrderSendBillRepository = orderSendBillRepository;
            StatementBillAppService = statementBillAppService;
            StatesAppService = statesAppService;
            StatementBillRepository = statementBillRepository;
            OrderStickBillRepository = orderStickBillRepository;
        }
        [AbpMvcAuthorize(PermissionNames.PagesInvoiceInfoInvoiceCreate), AuditLog("发票创建页面")]
        public ActionResult InvoiceCreate()
        {
            ViewBag.InitStickMan = AbpSession.UserName;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesInvoiceInfoInvoiceMgShowStickBill), AuditLog("发票详情页面")]
        public async Task<ActionResult> InvoiceDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("未传入对应编号！");
            }
            ViewBag.SendStickBillNo = id;
            var viewOrderSendStickBills =await ViewOrderSendStickBillRepository.GetAll().Where(i => i.OrderStickBillNo == id).OrderBy(i=>i.SendDate).ToListAsync();
            ViewBag.OrderSends = viewOrderSendStickBills;
            //var bill = await OrderSendBillAppService.Get(new EntityDto<string>(viewOrderSendStickBills.First().OrderSendBillNo));
            if (viewOrderSendStickBills.Any())
            {
                var bill = ObjectMapper.Map<OrderSendBillDto>(
                    OrderSendBillRepository.Get(viewOrderSendStickBills.First().OrderSendBillNo));
                ViewBag.SendBill = bill;
                ViewBag.CustomerInfo = CustomerRepository.Get(bill.CustomerId);
            }
            else
            {
                ViewBag.CustomerInfo = CustomerRepository.Get(OrderStickBillRepository.FirstOrDefault(i => i.Id == id)?.CustomerId);
            }
           
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesInvoiceInfoInvoiceMg), AuditLog("发票管理页面")]
        public ActionResult InvoiceMg()
        {
            ViewBag.InvoiceState = StatesAppService.GetSelectLists("Invoice", "State");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesInvoiceInfoStatementBill), AuditLog("对账单管理页面")]
        public ActionResult StatementBill()
        {
            ViewBag.CustomerList = StatementBillAppService.GetHasSendOrderCustomer();
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesInvoiceInfoStatementBillShowStickBill), AuditLog("对账单详情页面")]
        public async Task<ActionResult> StatementBillDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("未传入对应编号！");
            }
            ViewBag.SendStickBillNo = id;
            var viewOrderSendStickBills =await ViewOrderSendStickBillRepository.GetAll().Where(i => i.StatementBillNo == id).OrderByDescending(i => i.SendDate).ThenBy(i=>i.ProductName).ToListAsync();
            ViewBag.OrderSends = viewOrderSendStickBills;
            //var bill = await OrderSendBillAppService.Get(new EntityDto<string>(viewOrderSendStickBills.First().OrderSendBillNo));
            if (viewOrderSendStickBills.Any())
            {
                var bill = ObjectMapper.Map<OrderSendBillDto>(
                    OrderSendBillRepository.Get(viewOrderSendStickBills.First().OrderSendBillNo));
                ViewBag.SendBill = bill;
                ViewBag.CustomerInfo = CustomerRepository.Get(bill.CustomerId);
            }
            else
            {
                ViewBag.CustomerInfo = CustomerRepository.Get(StatementBillRepository.FirstOrDefault(i=>i.StatementBillNo==id)?.CustomerId);
            }
            
            
            return View();
        }
    }
}
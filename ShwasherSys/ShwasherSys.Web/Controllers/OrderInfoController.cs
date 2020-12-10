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
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Invoice;
using ShwasherSys.Models.OrderInfo;
using ShwasherSys.Order;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.Controllers
{
    [AbpMvcAuthorize,AuditLog("订单管理")]
    public class OrderInfoController : ShwasherControllerBase
    {
        protected IOrderHeadersAppService OrderHeadersAppService;
        protected IProductsAppService ProductsAppService;
        protected IQueryAppService QueryAppService;
        protected ICustomerDefaultProductAppService CustomerDefaultProductAppService;
        protected IRepository<ViewOrderItems> ViewOrderItemsRepository;
        protected IRepository<ViewOrderSendStickBill> ViewOrderSendStickBillRepository;
        public OrderInfoController(IStatesAppService statesAppService, IOrderHeadersAppService orderHeadersAppService,IProductsAppService productsAppService, ICustomerDefaultProductAppService customerDefaultProductAppService,  IRepository<ViewOrderItems> viewOrderItemsRepository, IRepository<ViewOrderSendStickBill> viewOrderSendStickBillRepository, IQueryAppService queryAppService, IIwbSettingManager settingManager)
        {
            OrderHeadersAppService = orderHeadersAppService;
            StatesAppService = statesAppService;
            ProductsAppService = productsAppService;
            CustomerDefaultProductAppService = customerDefaultProductAppService;
            ViewOrderItemsRepository = viewOrderItemsRepository;
            ViewOrderSendStickBillRepository = viewOrderSendStickBillRepository;
            QueryAppService = queryAppService;
            SettingManager = settingManager;
        }
        // GET: OrderInfo
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderStatusMg), AuditLog("订单状态管理页面")]
        public ActionResult OrderStatusMg()
        {
            ViewBag.OrderStatus = StatesAppService.GetSelectLists("OrderHeader", "OrderStatusId");
            ViewBag.OrderItemStatus = StatesAppService.GetSelectLists("OrderItems", "OrderItemStatusId");
            ViewBag.OrderStatusStr = StatesAppService.GetSelectListStrs("OrderHeader", "OrderStatusId");
            ViewBag.ViewPriceRole = SettingManager.GetSettingValue(ShwasherSettingNames.CanShowOrderItemPrice);
            ViewBag.EmergencyLevel = StatesAppService.GetSelectLists("OrderItems", "EmergencyLevel");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderMg), AuditLog("订单管理页面")]
        public async Task<ActionResult> OrderMg()
        {
            ViewBag.OrderStatus = StatesAppService.GetSelectLists("OrderHeader", "OrderStatusId");
            ViewBag.OrderItemStatus = StatesAppService.GetSelectLists("OrderItems", "OrderItemStatusId");
            ViewBag.OrderStatusStr = StatesAppService.GetSelectListStrs("OrderHeader", "OrderStatusId");
            ViewBag.OrderSaleType = StatesAppService.GetSelectLists("OrderHeader", "SaleType");
            ViewBag.TagRate = SettingManager.GetSettingValue(ShwasherSettingNames.OrderItemPriceTaxRate);
            ViewBag.FromCurrenyId =await QueryAppService.QueryAllCurrency();
            ViewBag.ToCNYCurreny = await QueryAppService.QueryCurrencyRate("", "CNY");
            ViewBag.EmergencyLevel = StatesAppService.GetSelectLists("OrderItems", "EmergencyLevel");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgPrint), AuditLog("订单打印页面")]
        public async Task<ActionResult> OrderPrint(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("无效参数！");
            }
            var orderHeadDto = await OrderHeadersAppService.Get(new EntityDto<string>(id));
            var customer = QueryAppService.GetCustomerInfo(new EntityDto<string>(orderHeadDto.CustomerId));
            var customerSend = QueryAppService.GetCustomerSendInfo(new EntityDto<int>(orderHeadDto.CustomerSendId));
            var orderItems = ViewOrderItemsRepository.GetAll().Where(i => i.OrderNo == id).ToList();
            ViewBag.OrderHeadDto = orderHeadDto;
            ViewBag.Customer = customer;
            ViewBag.CustomerSend = customerSend;
            ViewBag.OrderItems = orderItems;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderMgShowDetail,PermissionNames.PagesOrderInfoOrderStatusMgQuery), AuditLog("订单详情查看页面")]
        public async Task<ActionResult> OrderDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("无效参数！");
            }
            var orderHeadDto =await OrderHeadersAppService.Get(new EntityDto<string>(id));
            var customer = QueryAppService.GetCustomerInfo(new EntityDto<string>(orderHeadDto.CustomerId));
            if (customer == null)
            {
                CheckErrors(new IwbIdentityResult("客户信息不存在！"));
            }
            var customerSend = QueryAppService.GetCustomerSendInfo(new EntityDto<int>(orderHeadDto.CustomerSendId));
            if (customerSend == null)
            {
                CheckErrors(new IwbIdentityResult("客户发货信息不存在！"));
            }
            ViewBag.OrderHeadDto = orderHeadDto;
            ViewBag.Customer = customer;
            ViewBag.CustomerSend = customerSend;
            var orderItems = ViewOrderItemsRepository.GetAll().Where(i => i.OrderNo == id).ToList();
            List<OrderItemSendDto> orderItemSendDtos = new List<OrderItemSendDto>();
            if (orderItems.Any())
            {
                orderItemSendDtos = ObjectMapper.Map<List<OrderItemSendDto>>(orderItems);
                foreach (var itemSendDto in orderItemSendDtos)
                {
                    List<ViewOrderSendStickBill> list = ViewOrderSendStickBillRepository.GetAllList(i => i.OrderItemId == itemSendDto.Id).OrderByDescending(i => i.SendDate).ToList();
                    itemSendDto.ViewOrderSendStickBills = list;
                }
            }
            ViewBag.OrderItemSendDtos = orderItemSendDtos;
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderMgUpdate), AuditLog("订单详情明细信息维护页面")]
        public async Task<ActionResult> OrderItemDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new UserFriendlyException("未找到对应的订单！");
            }
            var loOrderHeaderDto = await OrderHeadersAppService.Get(new EntityDto<string>() { Id = id });
            ViewBag.MaterialSelect = QueryAppService.GetProductPropertyList("Material");
            ViewBag.SurfaceColorSelect = QueryAppService.GetProductPropertyList("SurfaceColor");
            ViewBag.RigiditySelect = QueryAppService.GetProductPropertyList("Rigidity");

            ViewBag.CustomerDefaultProducts = QueryAppService.GetDefualtProductByOrderNo(id);
            return View(loOrderHeaderDto);
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderQueryMg), AuditLog("订单明细查询页面")]
        public ActionResult OrderQueryMg()
        {
            ViewBag.OrderStatus = StatesAppService.GetSelectLists("OrderHeader", "OrderStatusId");
            ViewBag.OrderItemStatus = StatesAppService.GetSelectLists("OrderItems", "OrderItemStatusId");
            ViewBag.OrderStatusStr = StatesAppService.GetSelectListStrs("OrderHeader", "OrderStatusId");
            ViewBag.OrderSaleType = StatesAppService.GetSelectLists("OrderHeader", "SaleType");
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesOrderInfoNotCompleteOrderItem), AuditLog("未完成订单查看页面")]
        public ActionResult NotCompleteOrderItem()
        {
            ViewBag.OrderStatus = StatesAppService.GetSelectLists("OrderHeader", "OrderStatusId");
            ViewBag.OrderItemStatus = StatesAppService.GetSelectLists("OrderItems", "OrderItemStatusId",i=>i.CodeValue!="11");
            //ViewBag.OrderStatusStr = StatesAppService.GetSelectListStrs("OrderHeader", "OrderStatusId");
            ViewBag.OrderSaleType = StatesAppService.GetSelectLists("OrderHeader", "SaleType");
            return View();
        }
       [AbpMvcAuthorize(PermissionNames.PagesOrderInfoOrderItemStatistics), AuditLog("订单明细统计")]
        public ActionResult OrderItemStatistics()
        {
            //ViewBag.OrderStatus = StatesAppService.GetSelectLists("OrderHeader", "OrderStatusId");
            //ViewBag.OrderItemStatus = StatesAppService.GetSelectLists("OrderItems", "OrderItemStatusId", i => i.CodeValue != "11");
            //ViewBag.OrderStatusStr = StatesAppService.GetSelectListStrs("OrderHeader", "OrderStatusId");
            ViewBag.OrderSaleType = StatesAppService.GetSelectLists("OrderHeader", "SaleType");
            return View();
        }
    }

    
}
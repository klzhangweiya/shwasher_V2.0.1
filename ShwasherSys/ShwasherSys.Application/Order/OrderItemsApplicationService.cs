using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using Abp.Timing;
using Abp.UI;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.Common.Dto;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Lambda;
using ShwasherSys.Order.Dto;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.Order
{
    [AbpAuthorize]
    public class OrderItemsAppService : ShwasherAsyncCrudAppService<OrderItem, OrderItemDto, int, PagedRequestDto, OrderItemCreateDto, OrderItemUpdateDto >, IOrderItemsAppService
    {
        protected IRepository<CustomerDefaultProduct> CustomerDefaultProductRepository;
        protected IRepository<OrderHeader, string> OrderHeaderRepository;
        protected IRepository<ViewOrderItems> ViewOrderItemsRepository;
        protected IRepository<OrderSend> OrderSendRepository;      
        protected IStatesAppService StatesAppService;
        protected IRepository<ProductOutStore> ProductOutStoreRepository;
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository;
        protected IRepository<ViewBookedProductNum, string> ViewBookedProductNumRepository;
        protected IRepository<ViewCanProductStore, string> ViewCanProductStoreRepository;
        protected IRepository<BusinessLog> BusinessLogRepository;
        protected IRepository<OrderSendExceed> OrderSendExceedRepository;
        protected ICommonAppService CommonAppService { get; }
        protected ISqlExecuter SqlExecuter { get; }
        public OrderItemsAppService(IRepository<OrderItem, int> repository, IRepository<CustomerDefaultProduct> customerDefaultProductRepository, IRepository<OrderHeader, string> orderHeaderRepository, IRepository<ViewOrderItems> viewOrderItemsRepository, IRepository<OrderSend> orderSendRepository, IIwbSettingManager settingManager, IRepository<ProductOutStore> productOutStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<ViewBookedProductNum, string> viewBookedProductNumRepository, IRepository<ViewCanProductStore, string> viewCanProductStoreRepository, IRepository<BusinessLog> businessLogRepository, IStatesAppService statesAppService, ICommonAppService commonAppService, IRepository<OrderSendExceed> orderSendExceedRepository, ISqlExecuter sqlExecuter) : base(repository)
        {
            CustomerDefaultProductRepository = customerDefaultProductRepository;
            OrderHeaderRepository = orderHeaderRepository;
            ViewOrderItemsRepository = viewOrderItemsRepository;
            OrderSendRepository = orderSendRepository;
            ProductOutStoreRepository = productOutStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            ViewBookedProductNumRepository = viewBookedProductNumRepository;
            ViewCanProductStoreRepository = viewCanProductStoreRepository;
            BusinessLogRepository = businessLogRepository;
            StatesAppService = statesAppService;
            SettingManager = settingManager;
            CommonAppService = commonAppService;
            OrderSendExceedRepository = orderSendExceedRepository;
            SqlExecuter = sqlExecuter;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMg;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMg;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgCreateOrderItem;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgUpdateOrderItem;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgDeleteOrderItem;
        public override async Task<PagedResultDto<OrderItemDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            // CacheManager.GetCache(ShwasherConsts.EmployeeCache).GetAsync("",()=>ViewEmployeeRepository.GetAllList())

            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {

                    if (o.KeyWords.IsNullOrEmpty())
                        continue;

                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<OrderItem>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<OrderItemDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoNotCompleteOrderItem, PermissionNames.PagesOrderInfoOrderQueryMg)]
        public  async Task<PagedResultDto<ViewOrderItems>> GetViewAll(PagedRequestDto input)
        {
            var query = ViewOrderItemsRepository.GetAll();
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType,
                    });
                  
                }
                var exp = objList.GetExp<ViewOrderItems>();
                query = query.Where(exp);
            }

            query = query.Where(i => i.IsLock != "Y");
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.OrderNo);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewOrderItems>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoNotCompleteOrderItem, PermissionNames.PagesOrderInfoOrderQueryMg)]
        public async Task<PagedResultDto<ViewOrderItems>> GetViewAllNot(PagedRequestDto input)
        {
            var query = ViewOrderItemsRepository.GetAll();
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType,
                    });

                }
                var exp = objList.GetExp<ViewOrderItems>();
                query = query.Where(exp);
            }

            query = query.Where(i => i.IsLock != "Y");
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(i => i.SendDate).ThenBy(i=>i.ProductNo);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<ViewOrderItems>(
                totalCount,
                entities
            );
            return dtos;
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoNotCompleteOrderItem, PermissionNames.PagesOrderInfoOrderQueryMg)]
        public async Task<string> ExportExcel(List<MultiSearchDtoExt> input)
        {
            var query = ViewOrderItemsRepository.GetAll();
            if (input != null && input.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    var logicType = LogicType.And;
                    if (o.LogicType == 1)
                    {
                        logicType = LogicType.Or;
                    }
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType,
                        LogicType = logicType
                    });
                }
                var exp = objList.GetExp<ViewOrderItems>();
                query = query.Where(exp);
            }
            query = query.Where(i => i.IsLock != "Y");
            query = query.OrderByDescending(i => i.OrderNo);
           
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var r = entities.Select(i => new
            {
                i.OrderNo,
                OrderItemStatusName = StatesAppService.GetDisplayValue("OrderItems", "OrderItemStatusId", i.OrderItemStatusId.ToString()),
                i.CustomerName,
                i.Model,
                i.Rigidity,
                i.SurfaceColor,
                i.Material,
                i.PartNo,
                OrderDate = i.OrderDate?.ToString("yyy-MM-dd"),
                i.Quantity,
                IsPartSend = i.IsPartSend == "Y" ? "是" : "否",
                IsReport = i.IsReport == "Y" ? "需要" : "不需要",
                i.Price,
                i.AfterTaxPrice,
                i.TotalPrice,
                i.AfterTaxTotalPrice,
                i.IsSendQuantity,
                i.RemainingQuantity,
                i.OrderUnitName,
                i.StockNo,
                i.ProductNo,
                i.ProductName,
                SendDate = i.SendDate.ToString("yyy-MM-dd"),
                i.OrderItemDesc,
                i.SaleType,
                SaleTypeName= StatesAppService.GetDisplayValue("OrderHeader", "SaleType", i.SaleType.ToString()),
                i.CurrencyId
            }).ToList();
            string downloadUrl =await SettingManager.GetSettingValueAsync("SYSTEMDOWNLOADPATH");
            string lcFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\" +
                                downloadUrl;
            var exportEntity = new Dictionary<string, string>()
            {
                {"OrderNo", "订单流水号"},
                {"StockNo", "客户订单号"},
                {"CustomerName", "客户"},
                { "OrderDate","订单日期"},
                {"PartNo", "零件号"},
                {"ProductName", "产品名称"},
                {"Model", "型号"},
                {"SurfaceColor", "表色"},
                {"Material", "材质"},
                {"Rigidity", "硬度"},
                {"Quantity", "数量"},
                {"IsSendQuantity", "已发数量"},
                {"RemainingQuantity", "剩余数"},
                {"SendDate", "送货日期"},
                {"SaleTypeName", "内销/外销"},
                {"IsReport", "检验报告"},
                {"IsPartSend", "部分送货"},
                {"OrderItemStatusName", "状态"},
                {"ProductNo", "产品编号"},
                {"OrderUnitName", "单位"},
                {"CurrencyId", "货币"},
                {"Price", "含税价"},
                {"AfterTaxPrice", "不含税价"},
                {"TotalPrice", "总价"},
                {"AfterTaxTotalPrice", "不含税总价"},
                {"OrderItemDesc", "备注"}
            };
            if (!PermissionChecker.IsGranted(PermissionNames.PagesOrderInfoOrderMgQueryOrderPrice))
            {
                exportEntity.Remove("TotalPrice");
            }

            string lcResultFileName = ExcelHelper.EntityListToExcel2003(exportEntity, r, "sheet", lcFilePath);
            return Path.Combine(downloadUrl, lcResultFileName);
        }



        public override async Task<OrderItemDto> Create(OrderItemCreateDto input)
        {
            CheckCreatePermission();
            var orderHeader = OrderHeaderRepository.Get(input.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var customerDefaultProduct = CustomerDefaultProductRepository.GetAll().Where(i =>
                i.ProductNo == input.ProductNo && i.CustomerId == orderHeader.CustomerId)?.FirstOrDefault();
            if (customerDefaultProduct == null)
            {
                customerDefaultProduct = new CustomerDefaultProduct()
                {
                    CustomerId = orderHeader.CustomerId,
                    ProductNo = input.ProductNo,
                    Sequence = 1,
                    TimeLastMod = Clock.Now
                };
            }
            else
            {
                customerDefaultProduct.Sequence = customerDefaultProduct.Sequence + 1;
                customerDefaultProduct.TimeLastMod = Clock.Now;
            }

            CustomerDefaultProductRepository.InsertOrUpdate(customerDefaultProduct);
            input.OrderItemStatusId = OrderItemStatusEnum.NewCreate.ToInt();
            if (input.SendDate==null)
            {
                input.SendDate=new DateTime(1900,1,1);;
            }
            var dto= await CreateEntity(input);
            //await CommonAppService.WriteShortMessage(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANXGMSG), "有新订单明细录入", $"新的订单明细录入成功，订单明细流水号为:{dto.Id},请注意查看，并及时跟踪！订单录入明细人为{AbpSession.UserName}");
            return dto;
        }

        public override async Task<OrderItemDto> Update(OrderItemUpdateDto input)
        {
            CheckCreatePermission();
            
            var orderHeader = OrderHeaderRepository.Get(input.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var isHasSend = await CommonAppService.CheckOrderHasSend(orderHeader.Id);
          var itemEntity=  await CheckOrderItem(input.Id);
            if (isHasSend)
            {
                CheckErrors(IwbIdentityResult.Failed("订单已存在发货记录，不可变更信息！详情请联系系统管理员！"));
            }
            if (input.SendDate==null)
            {
                input.SendDate= new DateTime(1900,1,1);
            }
            var customerDefaultProduct = CustomerDefaultProductRepository.GetAll().Where(i =>
                i.ProductNo == input.ProductNo && i.CustomerId == orderHeader.CustomerId)?.FirstOrDefault();
            if (customerDefaultProduct == null)
            {
                customerDefaultProduct = new CustomerDefaultProduct()
                {
                    CustomerId = orderHeader.CustomerId,
                    ProductNo = input.ProductNo,
                    Sequence = 1,
                    TimeLastMod = Clock.Now
                };
            }
            else
            {
                customerDefaultProduct.Sequence = customerDefaultProduct.Sequence + 1;
                customerDefaultProduct.TimeLastMod = Clock.Now;
            }
            CustomerDefaultProductRepository.InsertOrUpdate(customerDefaultProduct);
            var prePrice = itemEntity.Price;
            var preQuantity = itemEntity.Quantity;
            var dto= await UpdateEntity(input);
            //await CommonAppService.WriteShortMessage(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANXGMSG), "订单明细信息修改", $"订单明细流水号为:{dto.Id},订单信息已变更,请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            string sendMsgType = ShwasherSettingNames.DINGDANXGTOD;
            string changeInfo = "";
            if (preQuantity != input.Quantity)
            {
                sendMsgType = ShwasherSettingNames.DINGDANSLXGTOD;
                changeInfo = $"明细数量由{preQuantity}K -> {input.Quantity}K |";
            }

            if (prePrice != input.Price)
            {
                sendMsgType = ShwasherSettingNames.DINGDANJEXGTOD;
                changeInfo += $"明细单价由{prePrice} -> { input.Price}K |";
            }
            await CommonAppService.WriteShortMessageByDep(AbpSession.UserName, SettingManager.GetSettingValue(sendMsgType), "订单明细信息修改", $"订单(${orderHeader.Id})明细流水号为:{dto.Id},订单信息已变更({changeInfo}),请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            return dto;
        }

       
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            var entity =  await CheckOrderItem(input.Id);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == entity.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var isHasSend = await CommonAppService.CheckOrderHasSend(entity.OrderNo);
            if (isHasSend)
            {
                CheckErrors(IwbIdentityResult.Failed("订单已存在发货记录，不可删除！详情请联系系统管理员！"));
            }
           
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细删除", $"订单明细审核通过！orderNo:{entity.OrderNo},itemId:{input}");
            await Repository.DeleteAsync(input.Id);
        }

        #region 订单明细状态信息变更

        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgAuditItem), AuditLog("审核订单明细")]
        public async Task<OrderItem> Audit(EntityDto<int> input)
        {
            var orderItem = Repository.Get(input.Id);
            var orderHeader =await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            orderItem.OrderItemStatusId = OrderItemStatusEnum.Audited.ToInt();
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细审核", $"订单明细审核通过！{orderItem.Id}");
            return await Repository.UpdateAsync(orderItem);
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgAuditItem), AuditLog("审核所有订单明细")]
        public async Task<List<OrderItem>> AuditAllItems(EntityDto<string> input)
        {
            string[] arrIds = input.Id.Split(',');
            if (arrIds.Length > 0)
            {
                var orderFirstItem = Repository.Get(Convert.ToInt32(arrIds[0]));
                var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderFirstItem.OrderNo);
                if (orderHeader.IsLock == "Y")
                {
                    CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
                }
            }
           
            List<OrderItem> alList = new List<OrderItem>();
            foreach (var arrId in arrIds)
            {
                var orderItem = Repository.Get(Convert.ToInt32(arrId));
               
                alList.Add(orderItem);
                orderItem.OrderItemStatusId = OrderItemStatusEnum.Audited.ToInt();
                orderItem.TimeLastMod = Clock.Now;
                orderItem.UserIDLastMod = AbpSession.UserName;
                BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细审核", $"订单明细审核通过！{orderItem.Id}");
                await Repository.UpdateAsync(orderItem);
            }
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细审核", $"订单明细审核通过！{alList.ToJsonString()}");
            return alList;
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgEndItem), AuditLog("结束订单明细")]
        public async Task<OrderItemEndCall> End(EntityDto<int> input)
        {
            var orderItem = Repository.Get(input.Id);
            orderItem.OrderItemStatusId = OrderItemStatusEnum.End.ToInt();
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细结束", $"订单明细结束！{orderItem.ToJsonString()}");
            //int endstatus = OrderItemStatusEnum.End.ToInt();
            string lcOrderNo = orderItem.OrderNo;
            //var notEndItems = Repository.GetAllList(i =>
            //i.OrderNo == lcOrderNo && (i.OrderItemStatusId != endstatus||i.OrderItemStatusId ==null)&&i.Id != input.Id);
            OrderItemEndCall result = new OrderItemEndCall();
            var isHasExOrderItem = IsExOrderItem(lcOrderNo, input.Id);//是否有其它订单明细
            if (!isHasExOrderItem)
            {
                var order = OrderHeaderRepository.Get(lcOrderNo);
                order.OrderStatusId = OrderStatusEnum.Completed.ToInt();
                await OrderHeaderRepository.UpdateAsync(order);
                result.IsAllEnd = true;
            }
            else
            {
                if (IsExNotEndItem(lcOrderNo, input.Id)) //判断其它订单明细状态是否都是完成
                {
                    var order = OrderHeaderRepository.Get(lcOrderNo);
                    order.OrderStatusId = OrderStatusEnum.Completed.ToInt();
                    await OrderHeaderRepository.UpdateAsync(order);
                    result.IsAllEnd = true;
                }
            }
            result.OrderItem = await Repository.UpdateAsync(orderItem);
            return result;
        }
        /// <summary>
        /// 判断当前订单中是否有其它订单（true:有其它订单）
        /// </summary>
        /// <param name="pcOrderNo">订单号</param>
        /// <param name="piExOrderItemId">排除订单明细Id（如果值不为0，则排除传入订单明细Id，只判断当前订单中其它明细状态是否完成）</param>
        /// <param name="poExOrderItems">排除订单明细Id（如果值不为Null，则排除传入订单明细集合，只判断当前订单中其它明细状态是否完成）</param>
        /// <returns>true:有其它订单</returns>
        private bool IsExOrderItem(string pcOrderNo, int piExOrderItemId = 0, List<OrderItem> poExOrderItems = null)
        {
            bool lbRetval = false;
            var notItems = Repository.GetAllList(i =>
                i.OrderNo == pcOrderNo);
            if (piExOrderItemId != 0)
            {
                notItems = notItems.Where(i => i.Id != piExOrderItemId).ToList();
            }

            if (poExOrderItems != null && poExOrderItems.Any())
            {
                List<int> lcExOrderItemsId = poExOrderItems.Select(i => i.Id).ToList();
                notItems = notItems.Where(i => !lcExOrderItemsId.Contains(i.Id)).ToList();
            }
            if (notItems.Any())
            {
                lbRetval = true;
            }
            return lbRetval;
        }
        
        /// <summary>
        /// 判断当前订单中订单明细是否为都结束状态（true:其它订单明细都结束了）
        /// </summary>
        /// <param name="pcOrderNo">订单号</param>
        /// <param name="piExOrderItemId">排除订单明细Id（如果值不为0，则排除传入订单明细Id，只判断当前订单中其它明细状态是否完成）</param>
        /// <param name="poExOrderItems">排除订单明细Id（如果值不为Null，则排除传入订单明细集合，只判断当前订单中其它明细状态是否完成）</param>
        /// <returns>true:订单明细都结束了</returns>
        private bool IsExNotEndItem(string pcOrderNo, int piExOrderItemId = 0, List<OrderItem> poExOrderItems = null)
        {
            bool lbRetval = false;
            int endstatus = OrderItemStatusEnum.End.ToInt();
            int completeStatus = OrderItemStatusEnum.NegotiationComplete.ToInt();
            //过滤掉结束和协商完成的
            var notEndItems = Repository.GetAllList(i =>
                i.OrderNo == pcOrderNo && i.OrderItemStatusId != endstatus&&i.OrderItemStatusId!= completeStatus);
            if (piExOrderItemId != 0)
            {
                notEndItems = notEndItems.Where(i => i.Id != piExOrderItemId).ToList();
            }

            if (poExOrderItems != null && poExOrderItems.Any())
            {
                List<int> lcExOrderItemsId = poExOrderItems.Select(i => i.Id).ToList();
                notEndItems = notEndItems.Where(i => !lcExOrderItemsId.Contains(i.Id)).ToList();
            }
            if (!notEndItems.Any())
            {
                lbRetval = true;
            }
            return lbRetval;
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgChangeStatus), AuditLog("变更订单明细状态")]
        public async Task<List<OrderItem>> ChangeOrderItemStatus(ChangeOrderItemStatusDto input)
        {
            string ids = input.Id;
            if (string.IsNullOrEmpty(ids))
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到修改的订单明细！"));
                return null;
            }

            var arr = ids.Split(',');
            StringBuilder sb = new StringBuilder();
            List<OrderItem> items = new List<OrderItem>();
            foreach (var id in arr)
            {
                var item = Repository.Get(Convert.ToInt32(id));
                sb.Append($"[ItemId:{item.Id},Status:{item.OrderItemStatusId}]");
                item.OrderItemStatusId = input.OrderItemStatusId;
                item.TimeLastMod = Clock.Now;
                item.UserIDLastMod = AbpSession.UserName;
                items.Add(item);
                await Repository.UpdateAsync(item);
            }
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细状态修改", $"订单明细状态修改操作,订单明细：{sb.ToString()},变更状态为:{input.OrderItemStatusId}");
            return items;
        }
        /// <summary>
        /// 变更同一个订单的订单明细状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgChangeStatus), AuditLog("变更订单明细状态")]
        public async Task<OrderItemsCallAndEnd> ChangeOrderItemStatusOnHeader(ChangeOrderItemStatusDto input)
        {
            string ids = input.Id;
            if (string.IsNullOrEmpty(ids))
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到修改的订单明细！"));
                return null;
            }

            var arr = ids.Split(',');
            StringBuilder sb = new StringBuilder();
            List<OrderItem> items = new List<OrderItem>();

            foreach (var id in arr)
            {
                var item = Repository.Get(Convert.ToInt32(id));
                sb.Append($"[ItemId:{item.Id},Status:{item.OrderItemStatusId}]");
                item.OrderItemStatusId = input.OrderItemStatusId;
                item.TimeLastMod = Clock.Now;
                item.UserIDLastMod = AbpSession.UserName;
                items.Add(item);
                await Repository.UpdateAsync(item);
            }
            OrderItemsCallAndEnd loResult = new OrderItemsCallAndEnd();
            if (items.Any() && input.OrderItemStatusId == OrderItemStatusEnum.End.ToInt())
            {
                string lcOrderNo = items[0].OrderNo;
                var isHasExItems = IsExOrderItem(lcOrderNo, poExOrderItems: items);//是否有其它订单明细
                if (isHasExItems && IsExNotEndItem(items[0].OrderNo, poExOrderItems: items))//其它订单明细是否都结束了
                {
                    var orderHeader = OrderHeaderRepository.Get(items[0].OrderNo);
                    orderHeader.OrderStatusId = OrderStatusEnum.Completed.ToInt();
                    loResult.IsAllEnd = true;
                    await OrderHeaderRepository.UpdateAsync(orderHeader);
                }
                else if (!isHasExItems)
                {
                    var orderHeader = OrderHeaderRepository.Get(items[0].OrderNo);
                    orderHeader.OrderStatusId = OrderStatusEnum.Completed.ToInt();
                    loResult.IsAllEnd = true;
                    await OrderHeaderRepository.UpdateAsync(orderHeader);
                }
            }

            loResult.OrderItems = items;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细状态修改", $"订单明细状态修改操作,订单明细：{sb.ToString()},变更状态为:{input.OrderItemStatusId}");
            return loResult;
        }
        #endregion

        
       

        #region 订单明细信息变更

        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderMgUpdateOrderItem),AuditLog("修改订单明细价格")]
        public async Task<OrderItem> ChangePrice(ChangeOrderItemInfoDto input)
        {

            if (!decimal.TryParse((await SettingManager.GetSettingValueAsync(ShwasherSettingNames.OrderItemPriceTaxRate)),
                out var rate))
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到税率！"));
            }
            var orderItem = await CheckOrderItem(input.OrderItemNo);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            decimal oldPrice = orderItem.Price;
            orderItem.Price = input.NewPrice;
            orderItem.AfterTaxPrice = Math.Round(input.NewPrice / (1 + rate / 100), 3);
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细修改", $"修改订单明细价格,原价：{oldPrice}！{orderItem.ToJsonString()}");
            await CommonAppService.WriteShortMessageByDep(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANJEXGTOD), "订单明细价格变更", $"订单(${orderHeader.Id})明细流水号为:{orderItem.Id},订单税前价格由 {oldPrice} 变更为 {input.NewPrice} ,请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            return await Repository.UpdateAsync(orderItem);
            //throw new System.NotImplementedException();
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderMgUpdateOrderItem), AuditLog("修改订单明细税后价格")]
        public async Task<OrderItem> ChangeAfterTaxPrice(ChangeOrderItemInfoDto input)
        {
            if (!decimal.TryParse((await SettingManager.GetSettingValueAsync(ShwasherSettingNames.OrderItemPriceTaxRate)),
                out var rate))
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到税率！"));
            }
            var orderItem =  await CheckOrderItem(input.OrderItemNo);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            decimal oldPrice = orderItem.AfterTaxPrice;
            orderItem.AfterTaxPrice = input.NewAfterTaxPrice;
            orderItem.Price = Math.Round(input.NewAfterTaxPrice * (1 + rate / 100), 3);
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细修改", $"修改订单明细税后价格,原价：{oldPrice}！{orderItem.ToJsonString()}");
            await CommonAppService.WriteShortMessageByDep(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANJEXGTOD), "订单明细价格变更", $"订单(${orderHeader.Id})明细流水号为:{orderItem.Id},订单税后价格由 {oldPrice} 变更为 {input.NewAfterTaxPrice} ,请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            return await Repository.UpdateAsync(orderItem);
            //throw new System.NotImplementedException();
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderMgUpdateOrderItem), AuditLog("修改订单明细数量")]
        public async Task<OrderItem> ChangeQuantity(ChangeOrderItemInfoDto input)
        {
            var orderItem =  await CheckOrderItem(input.OrderItemNo);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var oldQuantity = orderItem.Quantity;
            orderItem.Quantity = input.NewQuantity;
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细修改(数量变更)", $"修改订单明细数量,原数量：{oldQuantity}！{orderItem.ToJsonString()}");
            await CommonAppService.WriteShortMessageByDep(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANSLXGTOD), "订单明细修改", $"订单(${orderHeader.Id})明细流水号为:{orderItem.Id},订单明细数量由 {oldQuantity} 变更为 {input.NewQuantity},请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            return await Repository.UpdateAsync(orderItem);
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderMgUpdateOrderItem), AuditLog("修改订单明细发货日期")]
        public async Task<OrderItem> ChangeSendDate(ChangeOrderItemInfoDto input)
        {
            var orderItem = await CheckOrderItem(input.OrderItemNo);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var oldSendDate = orderItem.SendDate;
            orderItem.SendDate = input.NewSendDate;
            orderItem.TimeLastMod = Clock.Now;
            orderItem.UserIDLastMod = AbpSession.UserName;
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细修改(发货日期变更)", $"修改订单明细发货日期,原日期：{oldSendDate:yyyy-MM-dd}！{orderItem.ToJsonString()}");
            await CommonAppService.WriteShortMessageByDep(AbpSession.UserName, SettingManager.GetSettingValue(ShwasherSettingNames.DINGDANXGTOD), "订单明细修改", $"订单(${orderHeader.Id})明细流水号为:{orderItem.Id},订单发货日期由 {oldSendDate:yyyy-MM-dd} 变更为 {input.NewSendDate:yyyy-MM-dd},请注意查看，并及时跟踪！订单变更人为{AbpSession.UserName}");
            return await Repository.UpdateAsync(orderItem);
        }

        private async Task<OrderItem> CheckOrderItem(int orderItemNo)
        {
            var orderItem = await Repository.FirstOrDefaultAsync(a => a.Id == orderItemNo);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            if (orderItem == null)
            {
                CheckErrors(IwbIdentityResult.Failed( "未查询到订单明细。"));
                return null;
            }
            if (orderItem.OrderItemStatusId==OrderItemStatusEnum.Send.ToInt() )
            {
                CheckErrors(IwbIdentityResult.Failed( "已发货，不能操作。"));
                return null;
            }
            if (orderItem.OrderItemStatusId==OrderItemStatusEnum.End.ToInt() ||orderItem.OrderItemStatusId==OrderItemStatusEnum.NegotiationComplete.ToInt() ||orderItem.OrderItemStatusId==OrderItemStatusEnum.Delete.ToInt() )
            {
                CheckErrors(IwbIdentityResult.Failed( "已结束，不能操作。"));
                return null;
            }

            return orderItem;
        }
       
        #endregion

        #region 订单发货
        /// <summary>
        /// 订单明细发货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgSendItem), AuditLog("订单明细发货")]
        public async Task<OrderItem> SendOrderAction(SendOrderInfoDto input)
        {
            //OrderSendRepository
            var allSendQuantity = GetItemSend(input.Id);
            var orderItem = Repository.Get(input.Id);
            var orderHeader = await OrderHeaderRepository.FirstOrDefaultAsync(i => i.Id == orderItem.OrderNo);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已是删除状态，不可变更信息！详情请联系系统管理员！"));
            }
            var readySendAll = allSendQuantity + input.SendAllQuantity;
            if (readySendAll > orderItem.Quantity)
            {
                var exceedRate = ((readySendAll - orderItem.Quantity) / orderItem.Quantity) * 100;
                if (exceedRate > 5)
                {
                    throw new UserFriendlyException("发货数量不能超过 订单明细总数量的5%！");
                }
                //throw new UserFriendlyException("发货数量不能超过 待发货总数量！");
                OrderSendExceedRepository.Insert(new OrderSendExceed()
                {
                    OrderItemId = input.Id, ExceedQuantity = (readySendAll - orderItem.Quantity),
                    OperatorMan = AbpSession.UserName,
                    ProductNo = input.ProductNo,
                    OrderNo = orderItem.OrderNo
                });
            }

            if ((allSendQuantity + input.SendAllQuantity) >= orderItem.Quantity)
            {
                orderItem.OrderItemStatusId = OrderItemStatusEnum.Send.ToInt();
            }
            
            foreach (var orderSendItemDto in input.SendItems)
            {
                if (await CommonAppService.CheckProductCanSendToCustomer(orderSendItemDto.ProductBatchNum,input.CustomerNo))
                {
                    CheckErrors(IwbIdentityResult.Failed($"批次({orderSendItemDto.ProductBatchNum})不能发货给客户({input.CustomerNo}),请检查后再试..."));
                }
                var sendQuantiy = orderSendItemDto.SendQuantity;
                    

                var avgQuantity = orderSendItemDto.AvgQuantity==0? sendQuantiy: orderSendItemDto.AvgQuantity;
                decimal packDecimal = 0;
                if (sendQuantiy <= avgQuantity)
                {
                    packDecimal = 1;
                    avgQuantity = sendQuantiy;
                }
                else
                {
                    packDecimal = sendQuantiy / avgQuantity;
                }
                var currentProductStore = CurrentProductStoreHouseRepository.FirstOrDefault(i =>
                    i.CurrentProductStoreHouseNo == orderSendItemDto.CurrentProductStoreHouseNo);
                //CurrentProductStoreHouse loOldCurrentProductStoreHouse = currentProductStore;//原库存记录
                decimal isCanSendQuantity = currentProductStore.Quantity - currentProductStore.FreezeQuantity;
                if (sendQuantiy > isCanSendQuantity)
                {
                    CheckErrors(IwbIdentityResult.Failed("发货批次("+ currentProductStore.ProductionOrderNo+")库存数量不足，最多可发:"+ isCanSendQuantity+"千件"));
                }
                currentProductStore.FreezeQuantity += orderSendItemDto.SendQuantity;
                currentProductStore.TimeLastMod = Clock.Now;
                currentProductStore.UserIDLastMod = AbpSession.UserName;
                //currentProductStores.Add(currentProductStore);
                await CurrentProductStoreHouseRepository.UpdateAsync(currentProductStore);//更新库存的冻结数量

               
                OrderSend orderSend = new OrderSend()
                {
                    OrderItemId = input.Id,
                    OrderUnitId = orderItem.OrderUnitId,
                    SendDate = Clock.Now,
                    SendQuantity = orderSendItemDto.SendQuantity,
                    UserIDLastMod = AbpSession.UserName,
                    TimeLastMod = Clock.Now,
                    TimeCreated = Clock.Now,
                    QuantityPerPack = avgQuantity,
                    PackageCount = packDecimal,
                    ProductBatchNum = orderSendItemDto.ProductBatchNum,
                    StoreLocationNo = orderSendItemDto.StoreLocationNo,
                    CurrentProductStoreHouseNo = orderSendItemDto.CurrentProductStoreHouseNo,
                    CreatorUserId = AbpSession.UserName
                };
                if (orderItem.ProductNo != input.ProductNo)
                {
                    orderSend.Remark = $"替换发货,用编号:{input.ProductNo}替换{orderItem.ProductNo}";
                }
                this.LogError(orderSend.ToJsonString());
                var sendId = OrderSendRepository.InsertAndGetId(orderSend); //添加发货记录

                //库存变更逻辑，添加仓库出库申请
                ProductOutStore loProductOutStore = new ProductOutStore
                {
                    ApplyOutDate = Clock.Now,
                    ApplyStatus = OutStoreApplyStatusEnum.Applying.ToInt(),
                    StoreHouseId = 1,
                    IsClose = false,
                    CreatorUserId = AbpSession.UserName,
                    CurrentProductStoreHouseNo = orderSendItemDto.CurrentProductStoreHouseNo,
                    IsConfirm = false,
                    Quantity = orderSendItemDto.SendQuantity,
                    ProductionOrderNo = orderSendItemDto.ProductBatchNum ?? "",
                    ProductNo = input.ProductNo,
                    TimeLastMod = Clock.Now,
                    TimeCreated = Clock.Now,
                    UserIDLastMod = AbpSession.UserName,
                    OrderSendId = sendId,
                    ApplyOutStoreSourceType = OutStoreApplyTypeEnum.SendGood.ToInt()
                };
                await ProductOutStoreRepository.InsertAsync(loProductOutStore);
                this.LogInfo($"订单明细发货操作,发货数量{sendQuantiy}！订单明细：{orderItem.ToJsonString()},库存记录变更：{currentProductStore.ToJsonString()}");
            }
            /*BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细发货", $"订单明细发货操作,发货数量{sendQuantiy}！订单明细：{orderItem.ToJsonString()}原库存记录{loOldCurrentProductStoreHouse.ToJsonString()},库存记录变更：{currentProductStore.ToJsonString()}");*/
            /*for (int i=0;i<  currentProductStores.Count;i++)
            {
                BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单明细发货", $"订单明细发货操作,发货数量{sendQuantiys[i]}！订单明细：{orderItem.ToJsonString()},库存记录变更：{currentProductStores[i].ToJsonString()}");
            }*/
            return await Repository.UpdateAsync(orderItem);//更新订单明细状态
        }
        #endregion

        /// <summary>
        /// 根据订单编号获取当前订单的订单明细
        /// </summary>
        /// <param name="pcOrderNo"></param>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<GetOrderItemDto> GetOrderItemsByOrderNo(string pcOrderNo)
        {
            var query = ViewOrderItemsRepository.GetAll().Where(i => i.OrderNo == pcOrderNo);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            GetOrderItemDto lOrderItemDto = new GetOrderItemDto();
            lOrderItemDto.IsAllSend = IsAllItemEnd(pcOrderNo);
            lOrderItemDto.OrderItems = entities;
            return lOrderItemDto;
        }
        /// <summary>
        /// 判断一个订单所有订单明细是否都处于结束状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [DisableAuditing]
        public bool IsAllItemEnd(string orderNo)
        {
            bool lbRetval = true;
            int endstatus = OrderItemStatusEnum.End.ToInt();
            int negotiationComplete = OrderItemStatusEnum.NegotiationComplete.ToInt();
            var result = Repository.GetAllList(i =>
                i.OrderNo == orderNo && i.OrderItemStatusId != endstatus&&i.OrderItemStatusId!= negotiationComplete);
            if (result.Any())
            {
                lbRetval = false;
            }
            /*else
            {
                var order = OrderHeaderRepository.Get(orderNo);
                order.OrderStatusId = OrderStatusEnum.Completed.ToInt();
                OrderHeaderRepository.UpdateAsync(order);
            }*/
            return lbRetval;
        }
        /// <summary>
        /// 获取单个订单明细已发数量
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [DisableAuditing]
        public decimal GetItemSend(int orderItemId)
        {
            var sendQuantity = OrderSendRepository.GetAll().Where(i => i.OrderItemId == orderItemId).Sum(i => (decimal?)i.SendQuantity) ?? 0;
            return sendQuantity;
        }

        /// <summary>
        /// 查询当前产品库存和被定数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ViewQueryCurrentProductNum QueryCurrentProductNum(EntityDto<string> input)
        {
            ViewBookedProductNum entityBook = ViewBookedProductNumRepository.FirstOrDefault(input.Id);
            ViewCanProductStore entityCan = ViewCanProductStoreRepository.FirstOrDefault(input.Id);
           
            ViewQueryCurrentProductNum entity = new ViewQueryCurrentProductNum();
            if (entityBook != null)
            {
                entity.Id = entityBook.Id;
                entity.BookedQuantity = entityBook.BookedQuantity;
            }

            if (entityCan != null)
            {
                entity.Id = entityCan.Id;
                entity.CanUserQuantity = entityCan.CanUserQuantity;
            }
            return entity;
        }
       
        public List<StatisticsItem> StatisticsItem(PagedRequestDto input)
        {
            string conn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Default"].ToString();
            List<StatisticsItem> resultList = new List<StatisticsItem>();
            using (var loSqlConn = new SqlConnection(conn))
            {
                using (loSqlConn.CreateCommand())
                {
                    loSqlConn.Open();
                   
                    string queryType = "", dataWhereSql = "where 1=1 and ( t.SaleType=1 or t.SaleType=2 )",sType="", currencyId="CNY";
                    if (input.SearchList != null && input.SearchList.Count > 0)
                    {
                        foreach (var searchDto in input.SearchList)
                        {
                            if (searchDto.KeyField == "stype")
                            {
                                sType = searchDto.KeyWords;
                                switch (searchDto.KeyWords)
                                {
                                    case "1":
                                        queryType = "t.CreateOrderDate";
                                        break;
                                    case "2":
                                        queryType = "datepart(week,CreateOrderDate)";
                                        break;
                                    case "3":
                                        queryType = "datepart(mm,CreateOrderDate)";
                                        break;
                                    case "4":
                                        queryType = "datepart(qq,CreateOrderDate)";
                                        break;
                                    case "5":
                                        queryType = "datepart(yyyy,CreateOrderDate)";
                                        break;
                                }
                            }

                          
                            if (searchDto.KeyField == "startDate" && !searchDto.KeyWords.IsNullOrEmpty())
                            {
                                dataWhereSql += " and t.CreateOrderDate>='" + searchDto.KeyWords + "' ";
                            }

                            if (searchDto.KeyField == "endDate" && !searchDto.KeyWords.IsNullOrEmpty())
                            {
                                dataWhereSql += " and t.CreateOrderDate<='" + searchDto.KeyWords + "' ";
                            }

                            if (searchDto.KeyField == "saleType" && !searchDto.KeyWords.IsNullOrEmpty())
                            {
                                if (Int16.Parse(searchDto.KeyWords) == 2)
                                {
                                    currencyId = "USD";
                                    dataWhereSql += " and t.CurrencyId ='USD' ";
                                }
                                else if (Int16.Parse(searchDto.KeyWords) == 1)
                                {
                                    dataWhereSql += " and t.CurrencyId ='CNY' ";
                                }
                                
                            }
                            if (searchDto.KeyField == "saleMan" && !searchDto.KeyWords.IsNullOrEmpty())
                            {
                                dataWhereSql += " and t.SaleMan ='"+ searchDto.KeyWords +"' ";
                                
                            }
                        }
                        if (queryType.IsNullOrEmpty())
                        {
                            return resultList;
                        }
                    }

                    string lcSql =
                        $"select count(*) as OrderCount,sum(t.Price*t.Quantity) as TotalPrice,{queryType} as QueryValue from (select Convert(varchar(10),OrderDate,120) as CreateOrderDate,* from N_ViewOrderItems) as t {dataWhereSql} group by {queryType} order by {queryType}";
                    SqlDataAdapter loDataAdapter = new SqlDataAdapter(lcSql, loSqlConn);
                    DataSet loDataSet = new DataSet(); // 创建DataSet
                    loDataAdapter.Fill(loDataSet, "OrderInfo");
                    DataTable loTable = loDataSet.Tables["OrderInfo"];
                    foreach (DataRow row in loTable.Rows)
                    {
                        StatisticsItem lDailyOrderInfo = new StatisticsItem();
                        /*lDailyOrderInfo.CustomerName = row["CustomerName"] + "";
                        lDailyOrderInfo.ProductName = row["ProductName"] + "";
                        lDailyOrderInfo.ProductNo = row["ProductNo"] + "";*/
                        lDailyOrderInfo.OrderCount = (int)row["OrderCount"];
                        lDailyOrderInfo.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);
                        lDailyOrderInfo.QueryValue = row["QueryValue"];
                        lDailyOrderInfo.QueryUnit = sType;
                        lDailyOrderInfo.CurrencyId = currencyId;
                        resultList.Add(lDailyOrderInfo);
                    }
                }
            }

            return resultList;
        }

        public async Task<LockOrderProductQuantity> GetCurrentProductLock(string productNo,string orderNo)
        {
            //int auditStatus = OrderItemStatusEnum.Audited.ToInt();
            //int createStatus = OrderItemStatusEnum.NewCreate.ToInt();
            //var orderItems =await Repository.GetAllListAsync(i =>
            //    i.ProductNo == productNo && (i.OrderItemStatusId == null || i.OrderItemStatusId == auditStatus ||
            //                                 i.OrderItemStatusId == createStatus)&&i.OrderNo == orderNo);
            //var itemIds = orderItems.Select(i => i.Id).ToList();
            //var sends = OrderSendRepository.GetAllList(i => itemIds.Contains(i.OrderItemId));
            //LockOrderProductQuantity lockOrderProductQuantity = new LockOrderProductQuantity()
            //{
            //    ProductNo = productNo,
            //    Quantity = orderItems.Sum(i=>i.Quantity) - sends.Sum(i=>i.SendQuantity)
            //};
            //return lockOrderProductQuantity;
            int auditStatus = OrderItemStatusEnum.Audited.ToInt();
            int createStatus = OrderItemStatusEnum.NewCreate.ToInt();
            var orderItems = await Repository.GetAllListAsync(i =>
                i.ProductNo == productNo && (i.OrderItemStatusId == null || i.OrderItemStatusId == auditStatus ||
                                             i.OrderItemStatusId == createStatus));
            LockOrderProductQuantity lockOrderProductQuantity = new LockOrderProductQuantity()
            {
                ProductNo = productNo,
                Quantity = orderItems.Sum(i => i.Quantity)
            };
            return lockOrderProductQuantity;
        }

     
    }


}

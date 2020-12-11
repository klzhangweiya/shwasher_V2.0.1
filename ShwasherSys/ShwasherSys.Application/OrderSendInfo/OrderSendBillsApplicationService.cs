using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using Abp.Timing;
using Castle.MicroKernel.Registration.Interceptor;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using Microsoft.AspNet.Identity;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Lambda;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo.Dto;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.OrderSendInfo
{
    [AbpAuthorize]
    public class OrderSendBillAppService : ShwasherAsyncCrudAppService<OrderSendBill, OrderSendBillDto, string, PagedRequestDto, OrderSendBillCreateDto, OrderSendBillUpdateDto >, IOrderSendBillAppService
    {
        protected IRepository<OrderSend> OrderSendRepository;
        protected IRepository<OrderItem> OrderItemRepository;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        protected IRepository<ViewOrderSendBill,string> ViewOrderSendBillRepository;
        protected IRepository<FinshedEnterStore> FinshedEnterStoreRepository;
        protected IRepository<CurrentProductStoreHouse> CurrentProductStoreHouseRepository;
        protected IRepository<ProductOutStore> ProductOutStoreRepository;
        protected IQueryAppService QueryAppService { get; }
        protected ICommonAppService CommonAppService { get; }
        protected IRepository<BusinessLog> BusinessLogRepository { get; }
        protected IRepository<Customer,string> CustomerRepository { get; }
        protected ISqlExecuter SqlExecuter;
        public OrderSendBillAppService(IRepository<OrderSendBill, string> repository, IRepository<OrderSend> orderSendRepository, ISqlExecuter sqlExecuter, IRepository<OrderItem> orderItemRepository, IRepository<ViewOrderSend> viewOrderSendRepository, IRepository<ViewOrderSendBill,string> viewOrderSendBillRepository, IRepository<FinshedEnterStore> finshedEnterStoreRepository, IRepository<CurrentProductStoreHouse> currentProductStoreHouseRepository, IRepository<ProductOutStore> productOutStoreRepository, IRepository<BusinessLog> businessLogRepository, IRepository<Customer, string> customerRepository, IQueryAppService queryAppService, ICommonAppService commonAppService) : base(repository)
        {
            OrderSendRepository = orderSendRepository;
            SqlExecuter = sqlExecuter;
            OrderItemRepository = orderItemRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            ViewOrderSendBillRepository = viewOrderSendBillRepository;
            FinshedEnterStoreRepository = finshedEnterStoreRepository;
            CurrentProductStoreHouseRepository = currentProductStoreHouseRepository;
            ProductOutStoreRepository = productOutStoreRepository;
            BusinessLogRepository = businessLogRepository;
            CustomerRepository = customerRepository;
            QueryAppService = queryAppService;
            CommonAppService = commonAppService;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesSendGoodsOrderSendBillCreate;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSendGoodsOrderSendBillCreate;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSendGoodsOrderSendBillCreateCreate;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSendGoodsOrderSendBillCreateDelete;


        #region 发货管理
        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillCreateDelete),AuditLog("撤销发货")]
        public bool CancelOrderSend(EntityDto<string> input)
        {
            bool lbRetval = false;
            string orderSendIds = input.Id;
            try
            {
                var arrOrderSendIds = orderSendIds.Split(',');
                foreach (var orderSendId in arrOrderSendIds)
                {
                    OrderSend loSendInfo = OrderSendRepository.Get(Convert.ToInt32(orderSendId));
                    OrderItem loOrderItem = OrderItemRepository.Get(loSendInfo.OrderItemId);
                    //-------------- 添加订单发货撤销后入库申请逻辑 ---------------
                    var productOutStore = ProductOutStoreRepository.FirstOrDefault(i => i.OrderSendId == loSendInfo.Id);
                    if (productOutStore == null)
                    {
                        CheckErrors(IwbIdentityResult.Failed("未发现出库记录!"));
                        return false;
                    }
                    if (productOutStore.ApplyStatus==FinshedOutStoreApplyStatusEnum.OutStored.ToInt())
                    {
                        CheckErrors(IwbIdentityResult.Failed("发货记录:"+loSendInfo.Id+"的出库申请已确认出库,发货明细不可撤销!"));
                        return false;
                    }
                    productOutStore.IsClose = true;//关闭出库申请记录
                    ProductOutStoreRepository.UpdateAsync(productOutStore);

                    CurrentProductStoreHouse currentStore = CurrentProductStoreHouseRepository.FirstOrDefault(i =>
                        i.CurrentProductStoreHouseNo == loSendInfo.CurrentProductStoreHouseNo);
                    if (currentStore == null)
                    {
                        CheckErrors(IwbIdentityResult.Failed("未发现库存!"));
                        return false;
                    }
                    currentStore.FreezeQuantity -= loSendInfo.SendQuantity;//恢复冻结库存
                    currentStore.TimeLastMod = Clock.Now;
                    CurrentProductStoreHouseRepository.UpdateAsync(currentStore);

                    loOrderItem.OrderItemStatusId = OrderItemStatusEnum.Audited.ToInt();//更新订单明细状态
                    OrderSendRepository.DeleteAsync(loSendInfo);//删除发货明细
                    loOrderItem.TimeLastMod = Clock.Now;
                    loOrderItem.UserIDLastMod = AbpSession.UserName;
                    OrderItemRepository.UpdateAsync(loOrderItem);
                    BusinessLogTypeEnum.OrderSend.WriteLog(BusinessLogRepository,"撤销发货",$"发货单{loSendInfo.ToJsonString()}取消发货,库存编码为:{currentStore.CurrentProductStoreHouseNo}的库存冻结数量减少{loSendInfo.SendQuantity}！订单明细状态变更为发货中..{loOrderItem.ToJsonString()}");
                }
                this.LogInfo("撤销发货明细:" + input.Id + " 成功!");
                lbRetval = true;
            }
            catch (Exception e)
            {
                this.LogError("撤销发货明细:" + input.Id + " 失败!" + e);
                throw;
            }

            return lbRetval;
        }

        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillCreate),AuditLog("根据客户编号,查询出待生成发货单的发货明细")]
        public async Task<List<ViewOrderSend>> GetOrderSendByCustomerId(QuerySendDto input)
        {
            var queryAllList = ViewOrderSendRepository.GetAll().Where(i =>
                i.CustomerId == input.CustomerId && (i.OrderSendBillNo == null || i.OrderSendBillNo == "") &&
                i.CustomerSendId == input.CustomerSendId);
            return await AsyncQueryableExecuter.ToListAsync(queryAllList);
        }
        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillCreate),AuditLog("查询出待生成发货单的客户列表记录")]
        public List<SelectListItem> GetHasSendOrderCustomer()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            var queryAllList = ViewOrderSendRepository.GetAll().Where(i => (i.OrderSendBillNo == null || i.OrderSendBillNo == "") && i.CustomerId != null).Select(i => i.CustomerId).Distinct();
            foreach (var send in queryAllList)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = send,
                    Value = send
                });
            }
            return listItems;
        }
        #endregion

        #region 发货单管理
        /// <summary>
        /// 生成发货单号
        /// </summary>
        /// <returns></returns>
        public string GetNewOrderSendBillNo()
        {
            string lcRetVal = "";
            DateTime loTiem = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            loTiem = loTiem.AddSeconds(-1);
            var bills = Repository.GetAll().Where(i => i.TimeCreated > loTiem).OrderByDescending(i => i.Id);
            if (bills.Any())
            {
                int liTempNo = 0;
                string lcTempNo = bills.First().Id;
                lcTempNo = lcTempNo.Remove(0, 1);
                liTempNo = Convert.ToInt32(lcTempNo);
                liTempNo++;
                lcRetVal = "B" + liTempNo;
                if (lcRetVal.Length < 10)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                DateTime loDate = DateTime.Today;
                string month = loDate.Date.Month < 10 ? "0" + loDate.Date.Month : loDate.Date.Month.ToString();
                lcRetVal = "B" + loDate.Date.Year + month + "0001";
            }
            return lcRetVal;
        }
        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillCreateCreate),AuditLog("发货单生成")]
        public override async Task<OrderSendBillDto> Create(OrderSendBillCreateDto input)
        {
            string orderSendIds = input.OrderSendIds;
            input.Id = GetNewOrderSendBillNo();
            input.SendDate = Clock.Now;
            input.TimeCreated = input.SendDate;
            input.TimeLastMod = input.SendDate;
            input.UserIDLastMod = AbpSession.UserName;
            input.IsDoBill = "N";
            string lcSql = "update OrderSend set OrderSendBillNo='" + input.Id + "' ";
            lcSql += " where OrderSendId in(" + orderSendIds + ")";
            var sCount = SqlExecuter.Execute(lcSql);
            
            var dto= await CreateEntity(input);
            var customer = await CustomerRepository.FirstOrDefaultAsync(a => a.Id == dto.CustomerId);
            if (string.IsNullOrEmpty(customer.Email))
            {
                return dto;
            }

            var orders = await ViewOrderSendRepository.GetAllListAsync(a => (orderSendIds + ",").Contains(a.Id + ","));
            if (orders.Any())
            {
                var expressName =await QueryAppService.GetExpressNameById(input.ExpressId ?? 0);
                string msg = $"发货快递：{expressName },快递单号：{input.ExpressBillNo}\r\n\r\n";
                foreach (var os in orders)
                {
                    msg += $"订单号:{os.StockNo},产品名称：{os.ProductName},发货数量：{os.SendQuantity}\r\n";
                }
                CommonAppService.SendEmail(customer.Email,"您有订单已发货",msg,false);
            }

            return dto;
        }
        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillMg),AuditLog("发货单管理查询")]
        public async Task<PagedResultDto<ViewOrderSendBill>> GetAllView(PagedRequestDto input)
        {
            var actionStopwatch = Stopwatch.StartNew();
            var query = ViewOrderSendBillRepository.GetAll();
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    if (o.KeyField == "sendDate" && (LambdaExpType)o.ExpType == LambdaExpType.LessOrEqual)
                    {
                        keyWords = Convert.ToDateTime(keyWords).AddDays(1);
                    }
                    if (o.KeyField == "isbill")
                    {
                       
                        if (keyWords.ToString() == "1")
                        {
                            query = query.Where(i=> i.OrderSendCount>=0&&i.StatementCount==0);
                        }else if (keyWords.ToString() == "2")
                        {
                            query = query.Where(i => i.OrderSendCount >0&& i.StatementCount == i.OrderSendCount);
                        }
                        else if(keyWords.ToString() == "3")
                        {
                            query = query.Where(i => i.StatementCount > 0&& i.StatementCount < i.OrderSendCount);
                        }
                        continue;
                    }
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<ViewOrderSendBill>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(a => a.SendDate);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<ViewOrderSendBill>(
                totalCount,
                entities
            );
            actionStopwatch.Stop();
            Debug.WriteLine(actionStopwatch.ElapsedMilliseconds);
            return dtos;
        }

        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendBillMgDelete),AuditLog("发货单撤销")]
        public override  Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var entity =Repository.Get(input.Id);
            var orderSends = OrderSendRepository.GetAllList(i => i.OrderSendBillNo == entity.Id);
            foreach (var send in orderSends)
            {
                send.OrderSendBillNo = "";
                send.OrderStickBillNo = "";
                OrderSendRepository.UpdateAsync(send);
            }
            BusinessLogTypeEnum.OrderSend.WriteLog(BusinessLogRepository, "撤销发货单", $"发货单{entity.ToJsonString()}撤销");
            //this.Logger.Info("撤销发货单!"+entity.ToString());
            return  Repository.DeleteAsync(input.Id);
        }

        public async Task<string> ExportOrderSend(EntityDto<string> input)
        {
            var bill = await Repository.FirstOrDefaultAsync(input.Id);
            var orderSends =(await ViewOrderSendRepository.GetAllListAsync(i => i.OrderSendBillNo == input.Id)).OrderByDescending(i => i.SendDate).ToList();
            var customerInfo =await CustomerRepository.FirstOrDefaultAsync(bill.CustomerId);
            var templateInfo = await QueryAppService.QueryTemplate(bill.CustomerId, 2);
            string[] classPath = templateInfo.ClassPath.Split("@@",StringSplitOptions.RemoveEmptyEntries);
            if (!classPath.Any())
            {
                CheckErrors(IdentityResult.Failed("未查到对应的classPath"));
            }
            string className = classPath[0];
            string methodName = classPath[1];

            var tpType = Type.GetType(className);
            object obj = Activator.CreateInstance(tpType ?? throw new InvalidOperationException());
            Object[] paras = { bill, orderSends, customerInfo, templateInfo };
            MethodInfo method = tpType.GetMethod(methodName);
            var result =  method?.Invoke(obj, paras);
            return result?.ToString();
        }

        public async Task<string> ExportOrderSendCommon(EntityDto<string> input)
        {
            var bill = await Repository.FirstOrDefaultAsync(input.Id);
            //var orderSends = (await ViewOrderSendRepository.GetAllListAsync(i => i.OrderSendBillNo == input.Id)).OrderBy(i=>i.SurfaceColor).ThenBy(i=>i.Rigidity).ToList();
            var orderSends =
                ViewOrderSendRepository.GetAllList(i => i.OrderSendBillNo == input.Id).OrderByDescending(i=>i.SendDate).ToList();
            var customerInfo = await CustomerRepository.FirstOrDefaultAsync(bill.CustomerId);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/OrderSendTemplate/送货单模板.xlsx";
            var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(2, 1).SetCellValue("单号："+bill.Id);
            sheet1.GenerateCell(5, 2).SetCellValue("客户：" + customerInfo.CustomerName);
            sheet1.GenerateCell(5, 10).SetCellValue("地址：" + bill.SendAddress);
            sheet1.GenerateCell(6, 2).SetCellValue("联系电话：" + bill.ContactTels);
            sheet1.GenerateCell(6, 10).SetCellValue("联系人：" + bill.ContactMan);
            sheet1.GenerateCell(7, 1).SetCellValue("日期:" + DateTime.Now.ToString("yyyy年MM月dd日"));
            sheet1.InsertRows(10, orderSends.Count);
            int index = 0;
            decimal allPackageCount = 0;
            foreach (var send in orderSends)
            {
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = send.QuantityPerPack ?? 0;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                allPackageCount += packageCount+(sysl>0?1:0);
                sheet1.GenerateCell(10 + index, 1).SetValue<int>(index + 1);
                sheet1.GenerateCell(10 + index, 2).SetValue(send.StockNo?? send.OrderNo);
                sheet1.GenerateCell(10 + index, 3).SetValue(send.PartNo ?? "");
                sheet1.GenerateCell(10 + index, 4).SetValue(send.ProductName??"");
                sheet1.GenerateCell(10 + index, 5).SetValue(send.Model ?? "");
                sheet1.GenerateCell(10 + index, 6).SetValue(send.SurfaceColor ?? "");
                sheet1.GenerateCell(10 + index, 7).SetValue(send.Material ?? "");
                sheet1.GenerateCell(10 + index, 8).SetValue(send.Rigidity ?? "");
                sheet1.GenerateCell(10 + index, 9).SetValue("千件");
                sheet1.GenerateCell(10 + index, 10).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(10 + index, 11).SetValue<decimal>(packageCount);
                sheet1.GenerateCell(10 + index, 12).SetValue<decimal>(quantityPerPack);
                sheet1.GenerateCell(10 + index, 13).SetValue<decimal>(sysl);
                sheet1.GenerateCell(10 + index, 14).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(10 + index, 15).SetValue(send.ProductBatchNum??"");
                index++;
            }
            index++;
            sheet1.GenerateCell(10 + index, 8).SetValue( "合计： 托盘、"+ allPackageCount + "箱、待进仓");
            index+=3;
            sheet1.GenerateCell(10 + index, 12).SetValue( "送货日期："+DateTime.Now.ToString("yyyy-MM-dd"));
            var fileName = $"送货单-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
        #endregion
    }
}

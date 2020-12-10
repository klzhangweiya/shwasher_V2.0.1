using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Lambda;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo.Dto;
namespace ShwasherSys.OrderSendInfo
{
    [AbpAuthorize]
    public class OrderSendAppService : ShwasherAsyncCrudAppService<OrderSend, OrderSendDto, int, PagedRequestDto, OrderSendCreateDto, OrderSendUpdateDto >, IOrderSendAppService
    {
        protected IRepository<ViewOrderItems> ViewOrderItemsRepository;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        public OrderSendAppService(IRepository<OrderSend, int> repository, IRepository<ViewOrderItems> viewOrderItemsRepository,IIwbSettingManager settingManager, IRepository<ViewOrderSend> viewOrderSendRepository) : base(repository)
        {
            ViewOrderItemsRepository = viewOrderItemsRepository;
            ViewOrderSendRepository = viewOrderSendRepository;
            SettingManager = settingManager;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesOrderSend;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesOrderSend;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesOrderSendCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesOrderSendUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesOrderSendDelete;

        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendQueryMgExportExcel),AuditLog("发货明细导入excel")]
        public async Task<string> ExportExcel(List<MultiSearchDto> input)
        {
            var query = ViewOrderSendRepository.GetAll();
            if (input != null && input.Count > 0)
            {
                
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                    {
                        if (o.KeyField == "customerId")
                        {
                            CheckErrors(IwbIdentityResult.Failed("请在搜索项上填上【客户编号】"));
                        }
                        continue;

                    }
                    object keyWords = o.KeyWords;
                    
                    if (o.KeyWords == "0")
                    {
                        query = query.Where(i => i.OrderStickBillNo == "" || i.OrderStickBillNo == null);
                        continue;
                    }
                    if (o.KeyWords == "1")
                    {
                        query = query.Where(i => i.OrderStickBillNo != "" && i.OrderStickBillNo != null);
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
                var exp = objList.GetExp<ViewOrderSend>();
                query = query.Where(exp);
            }
            query = query.OrderByDescending(i => i.OrderDate);
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            if (!list.Any())
            {
                CheckErrors(IwbIdentityResult.Failed("未查询到记录！"));
                return null;
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/OrderSendTemplate/客户发货单明细统计.xlsx";
            var savePath = "Download/Excel/SendBills";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet = work.GetSheet("Worksheet");
            sheet.InsertRows(2,list.Count-1);
            var index = 2;
            foreach (var l in list)
            {
                sheet.GenerateCell(index,1).SetValue(l.CustomerId);
                sheet.GenerateCell(index,2).SetValue<DateTime>(l.OrderDate);
                sheet.GenerateCell(index,3).SetValue(l.StockNo);
                sheet.GenerateCell(index,4).SetValue(l.OrderNo);
                sheet.GenerateCell(index,5).SetValue(l.PartNo);
                sheet.GenerateCell(index,6).SetValue<DateTime>(l.SendDate);
                sheet.GenerateCell(index,7).SetValue(l.ProductNo);
                sheet.GenerateCell(index,8).SetValue(l.ProductName);
                sheet.GenerateCell(index,9).SetValue<decimal>(l.SendQuantity);
                sheet.GenerateCell(index,10).SetValue(l.OrderUnitName);
                sheet.GenerateCell(index,11).SetValue(l.OrderStickBillNo);
                if (await IsGrantedAsync(PermissionNames.PagesSendGoodsOrderSendQueryMgQueryPrice))
                {
                    sheet.GenerateCell(index,12).SetValue<decimal>(l.TotalPrice);
                }
                index++;
            }

            decimal totalSendQuantity = list.Sum(a => a.SendQuantity),totalPrice= list.Sum(a=>a.TotalPrice);
            sheet.GenerateCell(index,3).SetValue<int>(list.Count);
            sheet.GenerateCell(index,9).SetValue<decimal>(totalSendQuantity);
            if (await IsGrantedAsync(PermissionNames.PagesSendGoodsOrderSendQueryMgQueryPrice))
            {
                sheet.GenerateCell(index,12).SetValue<decimal>(totalPrice);
            }

            var fileName = $"发货明细-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
        /// <summary>
        /// 查询的完成订单明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesSendGoodsOrderSendQueryMg),AuditLog("发货统计查询")]
        public async Task<List<ViewOrderSend>> GetViewOrderItemAll(PagedRequestDto input)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码
            var query = ViewOrderSendRepository.GetAll();

            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;

                    if (o.KeyField == "isDoBill")
                    {
                        if (o.KeyWords == "0")
                        {
                            query= query.Where(i => i.OrderStickBillNo == "" || i.OrderStickBillNo == null);
                            continue;
                        }
                        if (o.KeyWords == "1")
                        {
                            query = query.Where(i => i.OrderStickBillNo != "" && i.OrderStickBillNo != null);
                            continue;
                        }
                    }
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<ViewOrderSend>();
                query = query.Where(exp);
            }
            query = query.OrderByDescending(i => i.SendDate);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            stopwatch.Stop(); //  停止监视
            long timeSpan = stopwatch.ElapsedMilliseconds;
            this.LogInfo(timeSpan.ToString());
            return entities;
        }
    }
}

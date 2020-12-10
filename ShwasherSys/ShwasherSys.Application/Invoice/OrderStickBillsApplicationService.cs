using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using Abp.Timing;
using Abp.UI;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;
using IwbZero.IdentityFramework;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Invoice.Dto;
using ShwasherSys.Lambda;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.OrderSendInfo.Dto;

namespace ShwasherSys.Invoice
{
    [AbpAuthorize]
    public class OrderStickBillAppService : ShwasherAsyncCrudAppService<OrderStickBill, OrderStickBillDto, string, PagedRequestDto, OrderStickBillCreateDto, OrderStickBillUpdateDto >, IOrderStickBillAppService
    {
        protected IRepository<ViewCustomerStick> ViewCustomerStickRepository;
        protected IRepository<ViewOrderSendStickBill> ViewOrderSendStickBillRepository;
        public IRepository<OrderSendBill, string> OrderSendBillRepository;
        protected IRepository<Customer,string> CustomerRepository;
        protected IRepository<OrderSend> OrderSendRepository;
        protected ISqlExecuter SqlExecute;
        protected IRepository<BusinessLog> BusinessLogRepository { get; }
        protected IRepository<ViewStickBill,string> ViewStickBillRepository { get; }
        protected IRepository<ViewStatementBill> ViewStatementBillRepository { get; }
        public OrderStickBillAppService(IRepository<OrderStickBill, string> repository, IRepository<ViewCustomerStick> viewCustomerStickRepository, ISqlExecuter sqlExecute, IRepository<Customer, string> customerRepository, IRepository<OrderSend> orderSendRepository, IRepository<BusinessLog> businessLogRepository, IRepository<ViewStickBill, string> viewStickBillRepository,IRepository<ViewOrderSendStickBill> viewOrderSendStickBillRepository, IRepository<OrderSendBill, string> orderSendBillRepository, IRepository<ViewStatementBill> viewStatementBillRepository) : base(repository)
        {
            ViewCustomerStickRepository = viewCustomerStickRepository;
            SqlExecute = sqlExecute;
            CustomerRepository = customerRepository;
            OrderSendRepository = orderSendRepository;
            BusinessLogRepository = businessLogRepository;
            ViewStickBillRepository = viewStickBillRepository;
            ViewOrderSendStickBillRepository = viewOrderSendStickBillRepository;
            OrderSendBillRepository = orderSendBillRepository;
            ViewStatementBillRepository = viewStatementBillRepository;
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesInvoiceInfoInvoiceMg;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesInvoiceInfoInvoiceMg;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesInvoiceInfoInvoiceCreateCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesInvoiceInfoInvoiceMgUpdate;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesInvoiceInfoInvoiceMgDelete;
       

        public  async Task<PagedResultDto<ViewStickBill>> GetViewAll(PagedRequestDto input)
        {
            CheckGetAllPermission();
            var query = ViewStickBillRepository.GetAll();
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
                var exp = objList.GetExp<ViewStickBill>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i => i.CreatDate);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<ViewStickBill>(
                totalCount,
                entities
            );
            return dtos;
        }

        //[AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceCreateQueryPage)]
        //public async Task<List<ViewCustomerStick>> GetAllCreateView(PagedRequestDto input)
        //{
        //    var query = ViewCustomerStickRepository.GetAll();
        //    if (input.SearchList != null && input.SearchList.Count > 0)
        //    {
        //        List<LambdaObject> objList = new List<LambdaObject>();
        //        foreach (var o in input.SearchList)
        //        {
        //            if (o.KeyWords.IsNullOrEmpty())
        //                continue;
        //            object keyWords = o.KeyWords;
        //            /*if (o.KeyField == "sendDate" && (LambdaExpType)o.ExpType == LambdaExpType.LessOrEqual)
        //            ]
        //                keyWords = Convert.ToDateTime(keyWords).AddDays(1);
        //            }*/
        //            objList.Add(new LambdaObject
        //            {
        //                FieldType = (LambdaFieldType)o.FieldType,
        //                FieldName = o.KeyField,
        //                FieldValue = keyWords,
        //                ExpType = (LambdaExpType)o.ExpType
        //            });
        //        }
        //        var exp = objList.GetExp<ViewCustomerStick>();
        //        query = query.Where(exp).OrderByDescending(i=>i.SendDate);
        //    }
        //    var entities = await AsyncQueryableExecuter.ToListAsync(query);
        //    return entities;

        //}
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceCreateQueryPage)]
        public async Task<List<ViewStatementBill>> GetAllCreateView(PagedRequestDto input)
        {
            var query = ViewStatementBillRepository.GetAll();
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    /*if (o.KeyField == "sendDate" && (LambdaExpType)o.ExpType == LambdaExpType.LessOrEqual)
                    ]
                        keyWords = Convert.ToDateTime(keyWords).AddDays(1);
                    }*/
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<ViewStatementBill>();
                query = query.Where(exp).OrderByDescending(i => i.CreationTime);
            }

            query = query.Where(i => i.OrderStickBillNo == null || i.OrderStickBillNo == "");
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities;

        }
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceCreateCreate),AuditLog("创建发票")]
        public override async Task<OrderStickBillDto> Create(OrderStickBillCreateDto input)
        {
            //CheckCreatePermission();
            if (input.OrderSendIds.IsNullOrEmpty())
            {
                throw new UserFriendlyException("无效参数！");
            }

            var entity = Repository.FirstOrDefault(input.Id);
            if (entity != null)
            {
                throw new UserFriendlyException("该发票号已存在，请勿重复创建相同发票号！");
            }

            string sql = $"update StatementBill set StatementState=1,OrderStickBillNo='{input.StickNum}' where Id in ({input.OrderSendIds});";
            sql += $"update OrderSend set OrderStickBillNo='{input.StickNum}' where StatementBillNo in (select StatementBillNo from StatementBill where Id in ({input.OrderSendIds}));";
            //SqlExecute.Execute("update OrderSend set OrderStickBillNo='"+input.StickNum+"' where OrderSendId in ("+ input.OrderSendIds + ") ");
            SqlExecute.Execute(sql);
            //input.Amount = 0;
            input.InvoiceState = InvoiceState.NotPay.ToInt();
            input.InvoiceType = InvoiceTypeDefinition.Normal;
            return await CreateEntity(input);
        }
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceMgCreateRed),AuditLog("创建红冲发票")]
        public  async Task<OrderStickBillDto> CreateRed(RedOrderStickBillCreateDto input)
        {
            var entity =await Repository.FirstOrDefaultAsync(input.Id);
            if (entity != null)
            {
                throw new UserFriendlyException("该发票号已存在，请勿重复创建相同发票号！");
            }
            
            var oi = await Repository.FirstOrDefaultAsync(a => a.StickNum == input.OriginalStickNum);
            if (oi == null)
            {
                throw new UserFriendlyException("原发票号不存在，请检查后再试！");

            }
            if ( input.InvoiceType != InvoiceTypeDefinition.RedLess && input.Amount> oi.Amount)
            {
                throw new UserFriendlyException($"退还金额不能大于原发票金额【{oi.Amount}】。");
            }
            entity = MapToEntity(input);
            entity.StickNum = entity.Id;
            entity.InvoiceState = InvoiceState.NotPay.ToInt();
            entity.CustomerId = oi.CustomerId;
            if (entity.InvoiceType== InvoiceTypeDefinition.RedOver || entity.InvoiceType== InvoiceTypeDefinition.RedReturn)
            {
                entity.Amount = entity.Amount > 0 ? 0 - entity.Amount : entity.Amount;
            }
            if (string.IsNullOrEmpty(entity.StickMan))
            {
                entity.StickMan = AbpSession.UserName;
            }
            //entity.InvoiceType = InvoiceTypeDefinition.RedReturn;
            entity.OriginalStickNum = input.OriginalStickNum;
            entity.OrderNo = input.OrderNo;
            entity.ReturnOrderNo = input.ReturnOrderNo;
            entity = await Repository.InsertAsync(entity);
            return MapToEntityDto(entity);
        }

        [AuditLog("作废发票")]
        public override async Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var entity = Repository.Get(input.Id);
            var orderSends = OrderSendRepository.GetAllList(i => i.OrderStickBillNo == entity.Id);
            foreach (var send in orderSends)
            {
               send.OrderStickBillNo = "";
               send.TimeLastMod = Clock.Now;
               send.UserIDLastMod = AbpSession.UserName;
               await OrderSendRepository.UpdateAsync(send);
            }
            string sql = $"update StatementBill set StatementState=0,OrderStickBillNo='' where OrderStickBillNo ='{entity.Id}';";
            await SqlExecute.ExecuteAsync(sql);
            BusinessLogTypeEnum.OrderSend.WriteLog(BusinessLogRepository, "撤销发票", $"发票{entity.ToJsonString()}撤销");
            //this.Logger.Info("撤销发票!" + entity.ToString());
            await Repository.DeleteAsync(input.Id);
        }
        [AuditLog("修改发票号")]
        public async Task<OrderStickBill> UpdateStickNum(OrderStickBillUpdateDto input)
        {
            CheckUpdatePermission();
            if (Repository.FirstOrDefault(i => i.StickNum == input.StickNum) != null)
            {
                throw new UserFriendlyException("该发票号已存在，请勿重复创建相同发票号！");
            }
            var entity = Repository.Get(input.Id);
            entity.StickNum = input.StickNum;
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            this.Logger.Info("修改发票号!" + entity.ToString());
            return await Repository.UpdateAsync(entity);

        }
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceMgUpdateState), AuditLog("修改发票状态")]
        public async Task<OrderStickBill> UpdateState(OrderStickBillUpdateDto input)
        {
            var entity = Repository.Get(input.Id);
            entity.Amount = input.Amount;
            if (entity.InvoiceType== InvoiceTypeDefinition.RedOver || entity.InvoiceType== InvoiceTypeDefinition.RedReturn)
            {
                entity.Amount = entity.Amount > 0 ? 0 - entity.Amount : entity.Amount;
            }
            entity.InvoiceState = InvoiceState.HasPay.ToInt();
            entity.TimeLastMod = Clock.Now;
            entity.UserIDLastMod = AbpSession.UserName;
            this.Logger.Info("修改发票状态!" + entity.ToJsonString());
            return await Repository.UpdateAsync(entity);
        }
        [AuditLog("导出发票")]
        public async Task<string> ExportInvoice(EntityDto<string> input)
        {
           
            var orderSends =
                ViewOrderSendStickBillRepository.GetAll().Where(i => i.OrderStickBillNo == input.Id).OrderBy(i => i.SendDate).ToList();

            var bill = ObjectMapper.Map<OrderSendBillDto>(
                OrderSendBillRepository.Get(orderSends.First().OrderSendBillNo));
            var customerInfo = await CustomerRepository.FirstOrDefaultAsync(bill.CustomerId);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/InvoiceTemplate/发票导出模板.xlsx";
            var savePath = "Download/Excel/InvoiceBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Worksheet");
            sheet1.GenerateCell(4, 4).SetCellValue(customerInfo.CustomerName);
            sheet1.GenerateCell(4, 11).SetCellValue(bill.SendAddress);
            sheet1.GenerateCell(5, 4).SetCellValue(bill.ContactTels);
            sheet1.GenerateCell(5, 11).SetCellValue(bill.ContactMan);
            sheet1.GenerateCell(6, 4).SetCellValue(input.Id);
            sheet1.GenerateCell(6, 8).SetCellValue("日期:" + DateTime.Now.ToString("yyyy年MM月dd日"));
            if (orderSends.Count>1)
            {
                sheet1.InsertRows(8, orderSends.Count-1);
            }
          
            int index = 0;
            decimal ldAccontTotal = 0;
            decimal ldNoTaxTotal = 0;
            string currencyId = "CNY";
            bool isCanViewPrice = PermissionChecker.IsGranted(PermissionNames.PagesOrderInfoOrderMgQueryOrderPrice);
            foreach (var send in orderSends)
            {
                var sendQuantity = $"{send.SendQuantity ?? 0:N3}";
                var price = $"{send.Price:N3}";
                var noTaxprice = $"{send.AfterTaxPrice:N3}";
                var totalprice = $"{send.totalprice:N3}";
                var totalNoTaxprice = $"{(send.SendQuantity ?? 0) * send.AfterTaxPrice:N3}";
                var sendDate = $"{send.SendDate:yyyy-MM-dd}";
                currencyId = send.CurrencyId;
                sheet1.GenerateCell(8 + index, 1).SetValue<int>(index + 1);
                sheet1.GenerateCell(8 + index, 2).SetValue(send.StockNo ?? send.OrderNo);
                sheet1.GenerateCell(8 + index, 3).SetValue(send.OrderSendBillNo ?? "");
                sheet1.GenerateCell(8 + index, 4).SetValue(sendDate);
                sheet1.GenerateCell(8 + index, 5).SetValue(send.PartNo ?? "");
                sheet1.GenerateCell(8 + index, 6).SetValue(send.Model ?? "");
                sheet1.GenerateCell(8 + index, 7).SetValue(send.ProductName ?? "");
                sheet1.GenerateCell(8 + index, 8).SetValue(send.Rigidity ?? "");
                sheet1.GenerateCell(8 + index, 9).SetValue(send.SurfaceColor ?? "");
                sheet1.GenerateCell(8 + index, 10).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(8 + index, 11).SetValue(send.OrderUnitName??"");
                if (isCanViewPrice)
                {
                    sheet1.GenerateCell(8 + index, 12).SetValue<decimal>(price);
                    sheet1.GenerateCell(8 + index, 13).SetValue<decimal>(noTaxprice);
                    sheet1.GenerateCell(8 + index, 14).SetValue<decimal>(totalprice);
                    sheet1.GenerateCell(8 + index, 15).SetValue<decimal>(totalNoTaxprice);
                }
                ldAccontTotal += send.totalprice;
                ldNoTaxTotal += (send.SendQuantity ?? 0) * send.AfterTaxPrice;
                index++;
            }
            sheet1.GenerateCell(8 + index, 13).SetValue($"总金额({currencyId})");
            sheet1.GenerateCell(8 + index, 14).SetValue<decimal>(ldAccontTotal);
            sheet1.GenerateCell(8 + index, 15).SetValue<decimal>(ldNoTaxTotal);
            var fileName = $"发票单-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
        [AuditLog("导出发票"),AbpAuthorize(PermissionNames.PagesInvoiceInfoInvoiceMgExportInvoices)]
        public async Task<string> ExportInvoices(PagedRequestDto input)
        {
            var query = ViewStickBillRepository.GetAll();
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
                var exp = objList.GetExp<ViewStickBill>();
                query = query.Where(exp);
            }
            var list =await query.OrderByDescending(i => i.CreatDate).ToListAsync();
           if (!list.Any())
           {
                CheckErrors(IwbIdentityResult.Failed("未查询到记录！"));
                return null;
           }
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/InvoiceTemplate/多发票导出模板.xlsx";
            var savePath = "Download/Excel/InvoiceBills";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet = work.GetSheet("Worksheet");
            sheet.InsertRows(2,list.Count-1);
            var index = 2;
            foreach (var l in list)
            {
                sheet.GenerateCell(index,1).SetValue(l.Id);
                sheet.GenerateCell(index,2).SetValue(l.CustomerId);
                sheet.GenerateCell(index,3).SetValue(l.CustomerName);
                sheet.GenerateCell(index,4).SetValue<DateTime>(l.CreatDate??DateTime.MinValue);
                sheet.GenerateCell(index,5).SetValue(l.StickMan);
                sheet.GenerateCell(index,6).SetValue<decimal>(l.TotalPrice??0);
                sheet.GenerateCell(index,7).SetValue<decimal>(l.AfterTaxTotalPrice??0);
                sheet.GenerateCell(index,8).SetValue(l.InvoiceState==1?"未收款":"已收款");
                sheet.GenerateCell(index,9).SetValue<decimal>(l.Amount??0);
                sheet.GenerateCell(index,10).SetValue(l.CurrencyId);
                index++;
            }
            var fileName = $"发票单-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
        [AuditLog("导出对账单")]
        public async Task<string> ExportStatementBill(EntityDto<string> input)
        {
           
            var orderSends =
                ViewOrderSendStickBillRepository.GetAll().Where(i => i.StatementBillNo == input.Id).OrderBy(i => i.SendDate).ToList();

            var bill = ObjectMapper.Map<OrderSendBillDto>(
                OrderSendBillRepository.Get(orderSends.First().OrderSendBillNo));
            var customerInfo = await CustomerRepository.FirstOrDefaultAsync(bill.CustomerId);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/InvoiceTemplate/对账单导出模板.xlsx";
            var savePath = "Download/Excel/InvoiceBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Worksheet");
            sheet1.GenerateCell(4, 4).SetCellValue(customerInfo.CustomerName);
            sheet1.GenerateCell(4, 11).SetCellValue(bill.SendAddress);
            sheet1.GenerateCell(5, 4).SetCellValue(bill.ContactTels);
            sheet1.GenerateCell(5, 11).SetCellValue(bill.ContactMan);
            sheet1.GenerateCell(6, 4).SetCellValue(input.Id);
            sheet1.GenerateCell(6, 8).SetCellValue("日期:" + DateTime.Now.ToString("yyyy年MM月dd日"));
            if (orderSends.Count>1)
            {
                sheet1.InsertRows(8, orderSends.Count-1);
            }
          
            int index = 0;
            decimal ldAccontTotal = 0;
            decimal ldNoTaxTotal = 0;
            string currencyId = "CNY";
            bool isCanViewPrice = PermissionChecker.IsGranted(PermissionNames.PagesOrderInfoOrderMgQueryOrderPrice);
            foreach (var send in orderSends)
            {
                var sendQuantity = $"{send.SendQuantity ?? 0:N3}";
                var price = $"{send.Price:N3}";
                var noTaxprice = $"{send.AfterTaxPrice:N3}";
                var totalprice = $"{send.totalprice:N3}";
                var totalNoTaxprice = $"{(send.SendQuantity ?? 0) * send.AfterTaxPrice:N3}";
                var sendDate = $"{send.SendDate:yyyy-MM-dd}";
                currencyId = send.CurrencyId;
                sheet1.GenerateCell(8 + index, 1).SetValue<int>(index + 1);
                sheet1.GenerateCell(8 + index, 2).SetValue(send.StockNo ?? send.OrderNo);
                sheet1.GenerateCell(8 + index, 3).SetValue(send.OrderSendBillNo ?? "");
                sheet1.GenerateCell(8 + index, 4).SetValue(sendDate);
                sheet1.GenerateCell(8 + index, 5).SetValue(send.PartNo ?? "");
                sheet1.GenerateCell(8 + index, 6).SetValue(send.Model ?? "");
                sheet1.GenerateCell(8 + index, 7).SetValue(send.ProductName ?? "");
                sheet1.GenerateCell(8 + index, 8).SetValue(send.Rigidity ?? "");
                sheet1.GenerateCell(8 + index, 9).SetValue(send.SurfaceColor ?? "");
                sheet1.GenerateCell(8 + index, 10).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(8 + index, 11).SetValue(send.OrderUnitName??"");
                if (isCanViewPrice)
                {
                    sheet1.GenerateCell(8 + index, 12).SetValue<decimal>(price);
                    sheet1.GenerateCell(8 + index, 13).SetValue<decimal>(noTaxprice);
                    sheet1.GenerateCell(8 + index, 14).SetValue<decimal>(totalprice);
                    sheet1.GenerateCell(8 + index, 15).SetValue<decimal>(totalNoTaxprice);
                }
                ldAccontTotal += send.totalprice;
                ldNoTaxTotal += (send.SendQuantity ?? 0) * send.AfterTaxPrice;
                index++;
            }
            sheet1.GenerateCell(8 + index, 13).SetValue($"总金额({currencyId})");
            sheet1.GenerateCell(8 + index, 14).SetValue<decimal>(ldAccontTotal);
            sheet1.GenerateCell(8 + index, 15).SetValue<decimal>(ldNoTaxTotal);
            var fileName = $"对账单-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using NPOI.HSSF.UserModel;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Inspection;
using ShwasherSys.Order;
using ShwasherSys.ProductInfo;
using ShwasherSys.ReturnGoods.Dto;
using Abp.Extensions;

namespace ShwasherSys.ReturnGoods
{
    [AbpAuthorize]
    public class ReturnGoodOrderAppService : IwbZeroAsyncCrudAppService<ReturnGoodOrder, ReturnGoodOrderDto, int, IwbPagedRequestDto, ReturnGoodOrderCreateDto, ReturnGoodOrderUpdateDto >, IReturnGoodOrderAppService
    {
        public ReturnGoodOrderAppService(
			ICacheManager cacheManager,
			IRepository<ReturnGoodOrder, int> repository, IRepository<Product, string> productRepository, IRepository<DisqualifiedProduct> disqualifiedProductRepository, IQueryAppService queryAppService, IRepository<BusinessLog> logRepository, IRepository<Employee> employeeRepository, IRepository<Customer, string> customerRepository, IRepository<OrderHeader, string> orderHeaderRepository, IRepository<OrderItem> orderItemRepository) : base(repository, "Id")
        {
            ProductRepository = productRepository;
            DisqualifiedProductRepository = disqualifiedProductRepository;
            QueryAppService = queryAppService;
            LogRepository = logRepository;
            EmployeeRepository = employeeRepository;
            CustomerRepository = customerRepository;
            OrderHeaderRepository = orderHeaderRepository;
            OrderItemRepository = orderItemRepository;
            CacheManager = cacheManager;
        }
        protected  IRepository<OrderHeader, string> OrderHeaderRepository { get; }
        protected  IRepository<OrderItem> OrderItemRepository { get; }
        protected IRepository<Product,string> ProductRepository { get; }
        protected IRepository<Employee> EmployeeRepository { get; }
        protected IRepository<Customer,string> CustomerRepository { get; }

        protected IRepository<DisqualifiedProduct> DisqualifiedProductRepository { get; }
        public IQueryAppService QueryAppService { get; }
        public IRepository<BusinessLog> LogRepository { get; }
        protected override bool KeyIsAuto { get; set; } = true;

        #region GetSelect

        [DisableAuditing]
        public override async Task<List<SelectListItem>> GetSelectList()
        {
            var list = await Repository.GetAllListAsync();
            var sList = new List<SelectListItem> {new SelectListItem {Text = @"请选择...", Value = "", Selected = true}};
            foreach (var l in list)
            {
                //sList.Add(new SelectListItem { Value = l.Id, Text = l. });
            }
            return sList;
        }
        [DisableAuditing]
        public override async Task<string> GetSelectStr()
        {
            var list = await Repository.GetAllListAsync();
            string str = "<option value=\"\" selected>请选择...</option>";
            foreach (var l in list)
            {
                //str += $"<option value=\"{l.Id}\">{l.}</option>";
            }
            return str;
        }

        #endregion

        #region CURD

       [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgCreate)]
        public override async Task Create(ReturnGoodOrderCreateDto input)
        {
            input.ReturnOrderNo = "R" + Clock.Now.ToString("yyMMddHHmmssfff");
            input.ReturnState = ReturnGoodStateDefinition.New;
            if (!int.TryParse(input.OrderItemNo,out var  oid))
            {
                CheckErrors("未查询到订单明细！");
            }
            var orderItem = await OrderItemRepository.FirstOrDefaultAsync(a => a.Id == oid);
            if (orderItem==null)
            {
                CheckErrors("未查询到订单明细！");
                return;
            }

            if (input.ReturnType == ReturnGoodType.Return)
            {
                var preQuantity = Repository.GetAll().Where(i => i.OrderItemNo == orderItem.Id + "")
                    .Sum(i => i.Quantity);
                if (preQuantity != null && (preQuantity + input.Quantity) > orderItem.Quantity)
                {
                    CheckErrors("当前订单明细退货数量已经超过最大限制！");
                    return;
                }
            } 
            await CreateEntity(input);
            if (input.ReturnType == ReturnGoodType.Change)
            {
                var date = DateTime.Now;
                if (input.OrderDate == null)
                {
                    CheckErrors("发货日期不能为空，请检查后再试！");
                    return;
                }
                

                if (input.CustomerSendId == null)
                {
                    CheckErrors("发货地址不能为空，请检查后再试！");
                    return;
                }
                if (input.SendDate == null)
                {
                   input.SendDate= new DateTime(1900,1,1);
                }
                var oh = new OrderHeader()
                {
                    Id=await OrderTypeDefinition.GetNewOrderNo(OrderHeaderRepository),
                    CustomerId = input.CustomerId,
                    OrderStatusId = OrderStatusEnum.NewCreate.ToInt(),
                    OrderDate = Convert.ToDateTime(input.OrderDate),
                    CustomerSendId = input.CustomerSendId ?? 0,
                    LinkName = input.LinkName,
                    Telephone = input.Telephone,
                    Fax = input.Fax,
                    StockNo = input.StockNo,
                    SaleType = OrderTypeDefinition.Exchange,
                    TimeCreated = date,
                    UserIDLastMod = AbpSession.UserName,
                    TimeLastMod = date,
                };
                oh =  await OrderHeaderRepository.InsertAsync(oh);
                var oi = new OrderItem()
                {
                    OrderNo = oh.Id,
                    ProductNo = input.ProductNo,
                    IsPartSend = "Y",
                    IsReport = "Y",
                    SendDate = Convert.ToDateTime(input.SendDate),
                    Quantity = input.Quantity ?? 0,
                    CurrencyId = "CNY",
                    Price = 0,
                    AfterTaxPrice = 0,
                    OrderItemStatusId = OrderItemStatusEnum.NewCreate.ToInt(),
                    OrderItemDesc = $"换货[{input.ReturnOrderNo}]-[{input.Reason}]",
                    WareHouse = input.WareHouse,
                };
                await OrderItemRepository.InsertAsync(oi);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        

        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgUpdate)]
        public override async Task Update(ReturnGoodOrderUpdateDto input)
        {
            await UpdateEntity(input);
        }

        //[AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgDelete)]
        public override Task Delete(EntityDto<int> input)
        {
            return Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMg)]
        public override async Task<PagedResultDto<ReturnGoodOrderDto>> GetAll(IwbPagedRequestDto input)
        {
          var   query1 = ApplyFilter(Repository.GetAll(), input);
          var query = from a in query1
              join p in ProductRepository.GetAll() on a.ProductNo equals p.Id
                  into rp
              from s in rp.DefaultIfEmpty()
              join ep in EmployeeRepository.GetAll() on a.HandleUser equals ep.No
                  into ee
              from e in ee.DefaultIfEmpty()
              join cc in CustomerRepository.GetAll() on a.CustomerId equals cc.Id into ccc
              from c in ccc.DefaultIfEmpty()
              select new ReturnGoodOrderDto()
              {
                  Id = a.Id,
                  HandleUser = a.HandleUser,
                  SendOrderNo = a.SendOrderNo,
                  HandleUserName = e.Name,
                  ProductionOrderNo = a.ProductionOrderNo,
                  OrderNo = a.OrderNo,
                  ProductNo = a.ProductNo,
                  ReturnState = a.ReturnState,
                  Quantity = a.Quantity,
                  ReturnOrderNo = a.ReturnOrderNo,
                  Material = s.Material,
                  Model = s.Model,
                  Rigidity = s.Rigidity,
                  SurfaceColor = s.SurfaceColor,
                  ReturnDate = a.ReturnDate,
                  Amount = a.Amount,
                  AuditAmount = a.AuditAmount,
                  Reason = a.Reason,
                  CustomerId = a.CustomerId,
                  CustomerName = c.CustomerName,
                  ReturnType = a.ReturnType,
                  ApplyUser = a.ApplyUser,
                  ConfirmUser = a.ConfirmUser,
                  ApplyDate = a.ApplyDate,
                  ConfirmDate = a.ConfirmDate,
                  LinkName = a.LinkName
              };
                
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(a=>a.ReturnOrderNo);
            query = _ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ReturnGoodOrderDto>(totalCount, entities);
            return dtoList;
        }
        public  async Task<PagedResultDto<ReturnGoodOrderDto>> GetAllRefund(IwbPagedRequestDto input)
        {
            
          var  query1 = ApplyFilter(Repository.GetAll().Where(a=>a.ReturnState!=ReturnGoodStateDefinition.New&& a.ReturnState!=ReturnGoodStateDefinition.Check), input);
            var query = from a in query1
                join p in ProductRepository.GetAll() on a.ProductNo equals p.Id
                    into rp
                from s in rp.DefaultIfEmpty()
                join ep in EmployeeRepository.GetAll() on a.HandleUser equals ep.No
                    into ee
                from e in ee.DefaultIfEmpty()
                join cc in CustomerRepository.GetAll() on a.CustomerId equals cc.Id  into ccc
                from c in ccc.DefaultIfEmpty()
                select new ReturnGoodOrderDto()
                {
                    Id = a.Id,
                    HandleUser = a.HandleUser,
                    SendOrderNo = a.SendOrderNo,
                    HandleUserName = e.Name,
                    ProductionOrderNo = a.ProductionOrderNo,
                    OrderNo = a.OrderNo,
                    ProductNo = a.ProductNo,
                    ReturnState = a.ReturnState,
                    Quantity = a.Quantity,
                    ReturnOrderNo = a.ReturnOrderNo,
                    Material = s.Material,
                    Model = s.Model,
                    Rigidity = s.Rigidity,
                    SurfaceColor = s.SurfaceColor,
                    ReturnDate = a.ReturnDate,
                    Amount = a.Amount,
                    AuditAmount = a.AuditAmount,
                    Reason = a.Reason,
                    CustomerId = a.CustomerId,
                    CustomerName = c.CustomerName,
                    ReturnType = a.ReturnType
                };
                
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderBy(a=>a.ReturnState);
            query = _ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtoList = new PagedResultDto<ReturnGoodOrderDto>(totalCount, entities);
            return dtoList;
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgChangeState)]
        public async Task<ReturnGoodOrder> ChangeState(EntityDto<int> input)
        {
            var entity = await Repository.GetAsync(input.Id);
            entity.ReturnState = ReturnGoodStateDefinition.Check;
            
            await DisqualifiedProductRepository.InsertAsync(new DisqualifiedProduct()
            {
                
                DisqualifiedNo = await ProductTypeDefinition.GetDisProductNo(DisqualifiedProductRepository, ProductTypeDefinition.Finish),
                ProductOrderNo = entity.ProductionOrderNo,
                ReturnOrderNo = entity.ReturnOrderNo,
                ProductNo = entity.ProductNo,
                ProductName = await QueryAppService.GetProductName(entity.ProductNo),
                ProductType = ProductTypeDefinition.Finish,
                QuantityWeight = 0,
                KgWeight = 0,
                QuantityPcs = entity.Quantity??0,
                CheckUser = AbpSession.UserName,
                CheckDate = Clock.Now,
                HandleType = DisProductStateDefinition.NoHandle,
                HandleDate = Clock.Now,
                HandleUser = AbpSession.UserName,
                DisqualifiedType = DisqualifiedType.ReturnCheck
            });
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "退货",
                $"退货检验申请成功，退货号[{entity.ReturnOrderNo}]", $"申请人：{entity.CreatorUserId}",
                entity.ProductionOrderNo);
            return await Repository.UpdateAsync(entity);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgRefundApply)]
        public async Task<ReturnGoodOrder> RefundApply(RefundAmountDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            if (entity.ReturnType == ReturnGoodType.Change)
            {
                CheckErrors("换货不能退款。");
            }

            if (entity.ReturnState != ReturnGoodStateDefinition.HasChecked)
            {
                CheckErrors("退货单未检验，或已处理，请检查后再试。");
            }

            if (!int.TryParse(entity.OrderItemNo,out var  oid))
            {
                CheckErrors("未查询到订单明细！");
            }
            var orderItem = await OrderItemRepository.FirstOrDefaultAsync(a => a.Id == oid);
            if (orderItem==null)
            {
                CheckErrors("未查询到订单明细！");
                return null;
            }

            var maxPrice = orderItem.Price * orderItem.Quantity ;
            if (input.Amount>maxPrice)
            {
                CheckErrors($"退款金额不能超过订单明细的价格【{maxPrice} {orderItem.CurrencyId}】！");
            }
            entity.ReturnState = ReturnGoodStateDefinition.RefundApply;
            entity.Amount = input.Amount;
            entity.ApplyUser = AbpSession.UserName;
            entity.ApplyDate =DateTime.Now;
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "退货",
                $"退款申请，退货号[{entity.ReturnOrderNo}]", $"申请人：{entity.CreatorUserId}",
                entity.ProductionOrderNo);
            return await Repository.UpdateAsync(entity);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgRefundConfirm)]
        public async Task<ReturnGoodOrder> RefundConfirm(RefundAmountDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            if (entity.ReturnType == ReturnGoodType.Change)
            {
                CheckErrors("换货不能退款。");
            }
            
            if (entity.ReturnState != ReturnGoodStateDefinition.HasChecked && entity.ReturnState != ReturnGoodStateDefinition.RefundApply)
            {
                CheckErrors("退货单未检验，或已处理，请检查后再试。");
            }
            if (!int.TryParse(entity.OrderItemNo,out var  oid))
            {
                CheckErrors("未查询到订单明细！");
            }
            var orderItem = await OrderItemRepository.FirstOrDefaultAsync(a => a.Id == oid);
            if (orderItem==null)
            {
                CheckErrors("未查询到订单明细！");
                return null;
            }

            var maxPrice = orderItem.Price * orderItem.Quantity ;
            if (input.Amount>maxPrice)
            {
                CheckErrors($"退款金额不能超过订单明细的价格【{maxPrice} {orderItem.CurrencyId}】！");
            }
            entity.ReturnState = ReturnGoodStateDefinition.RefundConfirm;
            entity.AuditAmount = input.AuditAmount;
            entity.ConfirmUser = AbpSession.UserName;
            entity.ConfirmDate =DateTime.Now;
            BusinessLogTypeEnum.SStore.WriteLog(LogRepository, "退货",
                $"退款申请，退货号[{entity.ReturnOrderNo}]", $"申请人：{entity.CreatorUserId}",
                entity.ProductionOrderNo);
            return await Repository.UpdateAsync(entity);
        }
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesSendGoodsReturnGoodMgExportReturn)]
        public string ExportReturn(EntityDto<int> input)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources/ReturnOrder/上海垫圈厂客户退货申请单.xls";
            var entity = Repository.Get(input.Id);
            if (entity.ReturnType == ReturnGoodType.Change)
            {
                CheckErrors("换货不能导出退货申请单！");
            }
            var productEntity = ProductRepository.Get(entity.ProductNo);
            var customer = CustomerRepository.Get(entity.CustomerId);
            //string fileDir = "ReturnGood" + DateTime.Now.ToString("yyMMddHHmmss") + "-" + new Random().Next(1000);
            var savePath = "Download/Excel/ReturnGoodOrder";
            HSSFWorkbook work = ExcelHelper.CreateWorkBook03(path);
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(2, 2).SetCellValue(customer?.CustomerName);
            sheet1.GenerateCell(2, 6).SetCellValue(entity.Reason);
            sheet1.GenerateCell(3, 2).SetCellValue(entity?.LinkName);
            sheet1.GenerateCell(3, 6).SetCellValue(entity?.OrderNo);
            sheet1.GenerateCell(4, 6).SetCellValue(entity?.ProductionOrderNo);
            sheet1.GenerateCell(6, 1).SetCellValue(productEntity?.ProductName);
            sheet1.GenerateCell(6, 2).SetCellValue(productEntity?.Model);
            sheet1.GenerateCell(6, 3).SetCellValue(productEntity?.SurfaceColor);
            sheet1.GenerateCell(6, 4).SetValue((entity.Quantity??0).ToString("N"));
            sheet1.GenerateCell(6, 5).SetValue(productEntity?.PartNo);
            sheet1.GenerateCell(6, 6).SetValue(productEntity?.Rigidity);
            sheet1.GenerateCell(6, 7).SetValue(entity?.ReturnDate?.ToString("yyyy-MM-dd"));
            sheet1.GenerateCell(7, 2).SetValue(entity.SurveyReason);
            sheet1.GenerateCell(8, 2).SetValue(entity.SurveyDetail);
            sheet1.GenerateCell(9, 2).SetValue(entity.Solution);
            var fileName = $"客户退货申请单-{Clock.Now:HHmmss}.xls";
            //string lcPath = $"{AppDomain.CurrentDomain.BaseDirectory}{savePath}" + "\\" + fileName;
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }

            return $"{savePath}/{fileName}";
        }


        #region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrderDto> GetDto(EntityDto<int> input)
        {
            var entity = await GetEntity(input);
            return MapToEntityDto(entity);
        }

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrderDto> GetDtoById(int id)
        {
            var entity = await GetEntityById(id);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体Dto（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrderDto> GetDtoByNo(string no)
        {
            var entity = await GetEntityByNo(no);
            return MapToEntityDto(entity);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrder> GetEntity(EntityDto<int> input)
        {
            var entity = await GetEntityById(input.Id);
            return entity;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrder> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
       // [AbpAuthorize(PermissionNames.PagesMgReturnGoodOrderMgQuery)]
        public override async Task<ReturnGoodOrder> GetEntityByNo(string no)
        {
            //CheckGetPermission();
            if (string.IsNullOrEmpty(KeyFiledName))
            {
                ThrowError("NoKeyFieldName");
            }
            return await base.GetEntityByNo(no);
        }

    
        #endregion

        #region Hide

        ///// <summary>
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{ReturnGoodOrder}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<ReturnGoodOrder> CreateFilteredQuery(IwbPagedRequestDto input)
        //{
        //    var query = Repository.GetAll();
        //    var pagedInput = input as IIwbPagedRequest;
        //    if (pagedInput == null)
        //    {
        //        return query;
        //    }
        //    if (!string.IsNullOrEmpty(pagedInput.KeyWords))
        //    {
        //        object keyWords = pagedInput.KeyWords;
        //        LambdaObject obj = new LambdaObject()
        //        {
        //            FieldType = (LambdaFieldType)pagedInput.FieldType,
        //            FieldName = pagedInput.KeyField,
        //            FieldValue = keyWords,
        //            ExpType = (LambdaExpType)pagedInput.ExpType
        //        };
        //        var exp = obj.GetExp<ReturnGoodOrder>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    if (pagedInput.SearchList != null && pagedInput.SearchList.Count > 0)
        //    {
        //        List<LambdaObject> objList = new List<LambdaObject>();
        //       foreach (var o in pagedInput.SearchList)
        //        {
        //            if (string.IsNullOrEmpty(o.KeyWords))
        //                continue;
        //           object keyWords = o.KeyWords;
        //            objList.Add(new LambdaObject
        //            {
        //                FieldType = (LambdaFieldType)o.FieldType,
        //                FieldName = o.KeyField,
        //                FieldValue = keyWords,
        //                ExpType = (LambdaExpType)o.ExpType
        //            });
        //        }
        //        var exp = objList.GetExp<ReturnGoodOrder>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<ReturnGoodOrder> ApplySorting(IQueryable<ReturnGoodOrder> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<ReturnGoodOrder> ApplyPaging(IQueryable<ReturnGoodOrder> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion

    }
}

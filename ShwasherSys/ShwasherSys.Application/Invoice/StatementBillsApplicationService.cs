using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.Auditing;
using IwbZero.AppServiceBase;
using IwbZero.Helper;
using JetBrains.Annotations;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.Common;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Invoice.Dto;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.OrderSendInfo.Dto;
using ShwasherSys.EntityFramework;
using ShwasherSys.ProductStoreInfo.Dto;

namespace ShwasherSys.Invoice
{
    [AbpAuthorize]
    public class StatementBillAppService : IwbZeroAsyncCrudAppService<StatementBill, StatementBillDto, int, IwbPagedRequestDto, StatementBillCreateDto, StatementBillUpdateDto >, IStatementBillAppService
    {
        public StatementBillAppService(
			ICacheManager cacheManager,
			IRepository<StatementBill, int> repository, IRepository<ViewOrderSend> viewOrderSendRepository, ISqlExecuter sqlExecuter, IRepository<OrderSend> orderSendRepository,IRepository<ViewStatementBill> viewStatementBillRepository, IRepository<Customer, string> customerRepository) : base(repository, "StatementBillNo")
        {
            ViewOrderSendRepository = viewOrderSendRepository;
            CacheManager = cacheManager;
            SqlExecuter = sqlExecuter;
            _viewStatementBillRepository = viewStatementBillRepository;
            CustomerRepository = customerRepository;
            OrderSendRepository = orderSendRepository;
        }
        protected ISqlExecuter SqlExecuter;
        private readonly IRepository<ViewStatementBill> _viewStatementBillRepository;
        protected override bool KeyIsAuto { get; set; } = false;
        protected IRepository<ViewOrderSend> ViewOrderSendRepository { get; }
        protected IRepository<OrderSend> OrderSendRepository { get; }
        protected IRepository<Customer,string> CustomerRepository { get; }

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

        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBillCreate)]
        public override async Task Create(StatementBillCreateDto input)
        {
            string orderSendIds = input.OrderSendIds;
            string lcSql = "update OrderSend set StatementBillNo='" + input.StatementBillNo + "' ";
            lcSql += " where OrderSendId in(" + orderSendIds + ")";
            var sCount = SqlExecuter.Execute(lcSql);
            input.BillMan = AbpSession.UserName;
            input.StatementState = 0;
            await CreateEntity(input);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task Update(StatementBillUpdateDto input)
        {
            await UpdateEntity(input);
        }

        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBillDelete)]
        public override async Task Delete(EntityDto<int> input)
        {
            var entity = Repository.Get(input.Id);
            var orderSends = OrderSendRepository.GetAllList(i => i.StatementBillNo == entity.StatementBillNo);
            foreach (var send in orderSends)
            {
                send.StatementBillNo = "";
                send.TimeLastMod = Clock.Now;
                send.UserIDLastMod = AbpSession.UserName;
                OrderSendRepository.Update(send);
            }
            //BusinessLogTypeEnum.OrderSend.WriteLog(BusinessLogRepository, "撤销对账单", $"对账单{entity.ToJsonString()}撤销");
            //this.Logger.Info("撤销发票!" + entity.ToString());
            await Repository.DeleteAsync(input.Id);
        }

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<PagedResultDto<StatementBillDto>> GetAll(IwbPagedRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            var customers = CustomerRepository.GetAll();
            var result = query.Join(customers, s => s.CustomerId, c => c.Id, (s, c) => new StatementBillDto(){CustomerId=s.CustomerId,CustomerName=c.CustomerName,BillMan = s.BillMan,Description=s.Description,Id=s.Id, OrderStickBillNo=s.OrderStickBillNo,StatementBillNo=s.StatementBillNo, StatementState  = s.StatementState,
                CreationTime=s.CreationTime
            });
            result = ApplyFilter(result, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(result);
            result = result.OrderByDescending(i => i.CreationTime);
            result = result.Skip(input.SkipCount).Take(input.MaxResultCount);
            var dtoList = new PagedResultDto<StatementBillDto>(totalCount, result.ToList());
            return dtoList;
        }

		#region GetEntity/Dto

        /// <summary>
        ///  查询实体Dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBillDto> GetDto(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBillDto> GetDtoById(int id)
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
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBillDto> GetDtoByNo(string no)
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
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBill> GetEntity(EntityDto<int> input)
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
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBill> GetEntityById(int id)
        {
            return await Repository.FirstOrDefaultAsync(a=>a.Id==id);
        }

        /// <summary>
        /// 查询实体（需指明自定义字段）
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill)]
        public override async Task<StatementBill> GetEntityByNo(string no)
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
        ///// 根据给定的<see cref="IwbPagedRequestDto"/>创建 <see cref="IQueryable{StatementBill}"/>过滤查询.
        ///// </summary>
        ///// <param name="input">The input.</param>
        //protected override IQueryable<StatementBill> CreateFilteredQuery(IwbPagedRequestDto input)
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
        //        var exp = obj.GetExp<StatementBill>();
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
        //        var exp = objList.GetExp<StatementBill>();
        //        query = exp != null ? query.Where(exp) : query;
        //    }
        //    return query;
        //}



        //protected override IQueryable<StatementBill> ApplySorting(IQueryable<StatementBill> query, IwbPagedRequestDto input)
        //{
        //    return query.OrderBy(a => a.No);
        //}

        //protected override IQueryable<StatementBill> ApplyPaging(IQueryable<StatementBill> query, IwbPagedRequestDto input)
        //{
        //    if (input is IPagedResultRequest pagedInput)
        //    {
        //        return query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
        //    }
        //    return query;
        //}

        #endregion

        #endregion


        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill), AuditLog("查询出待生成对账单的客户列表记录")]
        public List<SelectListItem> GetHasSendOrderCustomer()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            var queryAllList = ViewOrderSendRepository.GetAll().Where(i => (i.OrderSendBillNo != null && i.OrderSendBillNo != "") && i.CustomerId != null&& (i.StatementBillNo == null || i.StatementBillNo == "")).Select(i => i.CustomerId).Distinct();
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


        [AbpAuthorize(PermissionNames.PagesInvoiceInfoStatementBill), AuditLog("根据客户编号,查询出待生成对账单的发货明细")]
        public async Task<List<ViewOrderSend>> GetOrderSendByCustomerId(QuerySendDto input)
        {
            var queryAllList = ViewOrderSendRepository.GetAll().Where(i =>
                i.CustomerId == input.CustomerId && (i.OrderSendBillNo != null && i.OrderSendBillNo != "") &&
                (i.StatementBillNo == ""|| i.StatementBillNo == null)).OrderByDescending(i=>i.OrderSendBillNo);
            return await AsyncQueryableExecuter.ToListAsync(queryAllList);
        }
        public async Task<List<ViewStatementBill>> QueryStatisticStatementBillItems(QueryStatementBillReportDto input)
        {
            var startDate = input.Year.GetDateByType(input.Month, out var endDate, out var dateStr);

            //查询外购的排产单进行统计(不是新建状态的)
            var query = _viewStatementBillRepository.GetAll().Where(a => a.CreationTime >= startDate && a.CreationTime < endDate);
            

            if (!string.IsNullOrEmpty(input.CustomerId))
            {
                query = query.Where(i =>i.CustomerId==input.CustomerId);
            }
          
            var items = await query.OrderByDescending(i => i.CreationTime).ToListAsync();

            return items;
        }
    }
}

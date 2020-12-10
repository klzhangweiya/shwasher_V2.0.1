using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using Abp.Runtime.Caching;
using Abp.Timing;
using IwbZero.AppServiceBase;
using IwbZero.IdentityFramework;
using IwbZero.Setting;
using NPOI.OpenXmlFormats.Dml;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Common;
using ShwasherSys.CompanyInfo;
using ShwasherSys.CustomerInfo;
using ShwasherSys.EntityFramework;
using ShwasherSys.Lambda;
using ShwasherSys.NotificationInfo;
using ShwasherSys.NotificationInfo.Dto;
using ShwasherSys.Order.Dto;
namespace ShwasherSys.Order
{
    [AbpAuthorize]
    public class OrderHeadersAppService : ShwasherAsyncCrudAppService<OrderHeader, OrderHeaderDto, string, PagedRequestDto, OrderHeaderCreateDto, OrderHeaderUpdateDto >, IOrderHeadersAppService
    {
        protected IShortMessagesAppService ShortMessagesAppService;

        protected IStatesAppService StatesAppService;
       // protected IIwbSettingManager SettingManager;
        protected IRepository<CustomerSend> CustomerSendRepository;
        protected IRepository<BusinessLog> BusinessLogRepository;
        protected IRepository<OrderItem> OrderItemRepository;
        protected ICommonAppService CommonAppService { get; }
        protected IRepository<Employee> EmployeeRepository { get; }
        protected ISqlExecuter SqlExecuter { get; }
        public OrderHeadersAppService(ICacheManager cacheManager, IRepository<OrderHeader, string> repository, IShortMessagesAppService shortMessagesAppService, IIwbSettingManager settingManager, IStatesAppService statesAppService, IRepository<CustomerSend> customerSendRepository, IRepository<BusinessLog> businessLogRepository,  ICommonAppService commonAppService, IRepository<OrderItem> orderItemRepository, IRepository<Employee> employeeRepository, ISqlExecuter sqlExecuter) : base(repository)
        {
            ShortMessagesAppService = shortMessagesAppService;
            StatesAppService = statesAppService;
            CustomerSendRepository = customerSendRepository;
            SettingManager = settingManager;
            BusinessLogRepository = businessLogRepository;
            CommonAppService = commonAppService;
            OrderItemRepository = orderItemRepository;
            EmployeeRepository = employeeRepository;
            SqlExecuter = sqlExecuter;
            CacheManager = cacheManager;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMg;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMg;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesOrderInfoOrderMgDelete;

        public override async Task<OrderHeaderDto> Get(EntityDto<string> input)
        {
            CheckGetPermission();
            if (!KeyFiledName.IsNullOrEmpty() && input.Id == null)
            {
                return await GetDtoByNoAsync(input.GetFiledValue<string>(KeyFiledName));
            }
            var entity = await GetEntityByIdAsync(input.Id);
            var customerSend = CustomerSendRepository.Get(entity.CustomerSendId);

            var dto = MapToEntityDto(entity);
            dto.CustomerSendName = customerSend.CustomerSendName;
            dto.OrderStatusName =
                StatesAppService.GetDisplayValue("OrderHeader", "OrderStatusId", dto.OrderStatusId + "");
            dto.SendAdress = customerSend.SendAdress;
            return dto;
        }
        public override async Task<PagedResultDto<OrderHeaderDto>> GetAll(PagedRequestDto input)
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
                var exp = objList.GetExp<OrderHeader>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderByDescending(i=>i.TimeCreated);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var employeeList = EmployeeRepository.GetAllList();
            
            var dtos = new PagedResultDto<OrderHeaderDto>(
                totalCount,
                entities.Select(i=>new OrderHeaderDto()
                {
                    CustomerId = i.CustomerId,
                    CustomerSendId = i.CustomerSendId,
                    Fax = i.Fax,
                    LinkName = i.LinkName,
                    Id = i.Id,
                    OrderDate = i.OrderDate,
                    OrderStatusId = i.OrderStatusId,
                    OrderStatusName = StatesAppService.GetDisplayValue("OrderHeader", "OrderStatusId", i.OrderStatusId+""),
                    StockNo = i.StockNo,
                    Telephone = i.Telephone,TimeLastMod = i.TimeLastMod,UserIDLastMod = i.UserIDLastMod,TimeCreated = i.TimeCreated,
                    SaleType = i.SaleType,
                    SaleTypeName = StatesAppService.GetDisplayValue("OrderHeader", "SaleType", i.SaleType + ""),
                    SaleMan= i.SaleMan,
                    SaleManName = employeeList.FirstOrDefault(a=>a.No == i.SaleMan)?.Name,
                    IsLock = i.IsLock
                }).ToList()
            );
            return dtos;
        }

        /// <summary>
        /// 获取新建订单的编号
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderMgCreate),DisableAuditing]
        public string GetNewOrderNo()
        {
            string lcRetVal = "";
            DateTime loTiem = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01");
            loTiem = loTiem.AddSeconds(-1);
            var orders = Repository.GetAll().Where(i => i.TimeCreated > loTiem).OrderByDescending(i => i.Id);
            if (orders.Any())
            {
                var loFirstOrder = orders.FirstOrDefault();
                int liTempNo = 0;
                liTempNo = Convert.ToInt32(loFirstOrder?.Id);
                liTempNo++;
                lcRetVal = liTempNo.ToString();
                if (lcRetVal.Length < 10)
                {
                    lcRetVal = "0" + lcRetVal;
                }
            }
            else
            {
                DateTime loDate = DateTime.Today;
                int liMonth = loDate.Date.Month;
                string lcMonth = liMonth < 10 ? "0" + liMonth : liMonth.ToString();
                lcRetVal = loDate.Date.Year + lcMonth + "0001";
            }
            return lcRetVal;
        }


        public override async Task<OrderHeaderDto> Create(OrderHeaderCreateDto input)
        {
            CheckCreatePermission();
            var result = await CreateEntity(input);
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单创建", $"订单创建,{result.ToJsonString()}");
           //await  CommonAppService.WriteShortMessage(AbpSession.UserName,
           //     SettingManager.GetSettingValue("DINGDANLRMSG"), "有新订单录入",
           //     $"新的订单录入成功，订单流水号为:{result.Id},请注意查看，并及时跟踪！订单录入人为{AbpSession.UserName}");
           //await CommonAppService.WriteShortMessage(AbpSession.UserName, SettingManager.GetSettingValue("DINGDANLRMSG"), "有新订单录入", $"新的订单录入成功，订单流水号为:{result.Id},请注意查看，并及时跟踪！订单录入人为{AbpSession.UserName}");
            return result;
        }

        public override async Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var orderHeader = Repository.Get(input.Id);
            if (orderHeader.IsLock == "Y")
            {
                CheckErrors(IwbIdentityResult.Failed("订单已删除，不可删除！详情请联系系统管理员！"));
            }
            var isHasSend = await CommonAppService.CheckOrderHasSend(orderHeader.Id);
            if (isHasSend)
            {
                CheckErrors(IwbIdentityResult.Failed("订单已存在发货记录，不可删除！详情请联系系统管理员！"));
            }

            if (orderHeader.OrderStatusId != OrderStatusEnum.NewCreate.ToInt())//非新建状态的订单使用软删除
            {
                await SqlExecuter.ExecuteAsync($"update OrderItems set IsLock='Y' where OrderNo='{orderHeader.Id}';");
                await SqlExecuter.ExecuteAsync($"update OrderHeader set IsLock='Y' where OrderNo='{orderHeader.Id}';");
            }
            else
            {
                await OrderItemRepository.DeleteAsync(i => i.OrderNo == input.Id);
                await Repository.DeleteAsync(input.Id);
            }
            
            //await CommonAppService.WriteShortMessage(AbpSession.UserName, SettingManager.GetSettingValue("DINGDANXGMSG"), "订单" + input.Id + "已经删除", $"请注意订单:{input.Id},已经审核！订单的基本信息为：{orderHeader.ToString()} ,订单删除人为{AbpSession.UserName}");
           
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单删除", $"修改订单,删除的订单为{orderHeader.ToJsonString()}");
        }
        [AbpAuthorize(PermissionNames.PagesOrderInfoOrderStatusMgAudit)]
        public async Task<OrderHeader> Audit(EntityDto<string> input)
        {
            var orderHeader = Repository.Get(input.Id);
            orderHeader.OrderStatusId = OrderStatusEnum.Audited.ToInt();
            orderHeader.TimeLastMod = Clock.Now;
            orderHeader.UserIDLastMod = AbpSession.UserName;
            //await CommonAppService.WriteShortMessage(AbpSession.UserName, SettingManager.GetSettingValue("DINGDANXGMSG"), "订单" + input.Id + "已经审核", $"请注意订单:{input.Id},已经审核！订单的基本信息为：{orderHeader.ToString()},订单审核人为{AbpSession.UserName}");
           
            BusinessLogTypeEnum.OrderLog.WriteLog(BusinessLogRepository, "订单审核", $"审核订单,订单信息为{orderHeader.ToJsonString()}");
            return await Repository.UpdateAsync(orderHeader);
           
        }
    }
}

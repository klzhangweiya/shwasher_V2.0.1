using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CustomerInfo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using IwbZero.IdentityFramework;
using ShwasherSys.Dto;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.CustomerInfo
{
    [AbpAuthorize]
    public class CustomerSendsAppService : ShwasherAsyncCrudAppService<CustomerSend, CustomerSendDto, int, PagedRequestDto, CustomerSendCreateDto, CustomerSendUpdateDto>, ICustomerSendsAppService
    {
        protected IRepository<ViewOrderSend> ViewOrderSendRepository;
        public CustomerSendsAppService(IRepository<CustomerSend, int> repository, IRepository<ViewOrderSend> viewOrderSendRepository) : base(repository)
        {
            ViewOrderSendRepository = viewOrderSendRepository;
        }

        protected override string GetPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersCreateSend;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersUpdateSend;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersDeleteSend;

        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();
            var entity = await GetEntityByIdAsync(input.Id);
            var queryAllList = ViewOrderSendRepository.GetAll().Where(i =>
                i.CustomerId == entity.CustomerId && (i.OrderSendBillNo == null || i.OrderSendBillNo == "") &&
                i.CustomerSendId == input.Id);
            if (queryAllList.Any())
            {
                CheckErrors(new IwbIdentityResult("该客户发货地址存在未生成发货单的发货记录，不可删除！"));
            }
            #region shwasher temp
            AddCommonPropertyValue("TimeLastMod", DateTime.Now, ref entity);
            AddCommonPropertyValue("UserIDLastMod", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("IsLock", "Y", ref entity);
            #endregion
            await CurrentUnitOfWork.SaveChangesAsync();
            //return MapToEntityDto(entity);
        }
        [DisableAuditing]
        public List<CustomerSendDto> GetCustomerSendDtoByCustomerId(CustomerSendDto customerId)
        {

            var entities = Repository.GetAll().Where(i => i.CustomerId == customerId.CustomerId && i.IsLock == "N");

            return ObjectMapper.Map<List<CustomerSendDto>>(entities.ToList());
        }
        [DisableAuditing]
        public async Task<CustomerSendDto> GetEntityById(CommonDto<int> input)
        {
            CheckGetPermission();
            if (input == null || input.Key == 0)
            {
                return null;
            }
            var entity = await GetEntityByIdAsync(input.Key);
            return MapToEntityDto(entity);
        }
    }
}

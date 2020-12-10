using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.CustomerInfo.Dto;
namespace ShwasherSys.CustomerInfo
{
    [AbpAuthorize]
    public class CustomersAppService : ShwasherAsyncCrudAppService<Customer, CustomerDto, string, PagedRequestDto, CustomerCreateDto, CustomerUpdateDto >, ICustomersAppService
    {
        public CustomersAppService(IRepository<Customer, string> repository) : base(repository)
        {
            KeyIsAuto = false;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomers;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersUpdate;
		protected override string DeletePermissionName { get; set; } = PermissionNames.PagesCustomerInfoCustomersDelete;


        public override async  Task Delete(EntityDto<string> input)
        {
            CheckDeletePermission();
            var entity = await GetEntityByIdAsync(input.Id);

            #region shwasher temp
            AddCommonPropertyValue("TimeLastMod", DateTime.Now, ref entity);
            AddCommonPropertyValue("UserIDLastMod", AbpSession.UserName, ref entity);
            AddCommonPropertyValue("IsLock", "Y", ref entity);
            #endregion
            await CurrentUnitOfWork.SaveChangesAsync();
            //return MapToEntityDto(entity);
        }

    }
}

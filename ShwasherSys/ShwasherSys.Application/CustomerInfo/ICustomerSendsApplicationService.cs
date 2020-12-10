using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CustomerInfo.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShwasherSys.Dto;

namespace ShwasherSys.CustomerInfo
{
    public interface ICustomerSendsAppService : IIwbAsyncCrudAppService<CustomerSendDto, int, PagedRequestDto, CustomerSendCreateDto, CustomerSendUpdateDto >
    {
        List<CustomerSendDto> GetCustomerSendDtoByCustomerId(CustomerSendDto customerId);
        Task<CustomerSendDto> GetEntityById(CommonDto<int> input);
    }
}

using IwbZero.AppServiceBase;
using ShwasherSys.CustomerInfo.Dto;

namespace ShwasherSys.CustomerInfo
{
    public interface ICustomersAppService : IIwbAsyncCrudAppService<CustomerDto, string, PagedRequestDto, CustomerCreateDto, CustomerUpdateDto >
    {
    }
}

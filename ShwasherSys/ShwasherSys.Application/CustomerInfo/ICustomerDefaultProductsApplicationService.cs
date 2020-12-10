using IwbZero.AppServiceBase;

using ShwasherSys.CustomerInfo.Dto;

namespace ShwasherSys.CustomerInfo
{
    public interface ICustomerDefaultProductAppService : IIwbAsyncCrudAppService<CustomerDefaultProductDto, int, PagedRequestDto, CustomerDefaultProductCreateDto, CustomerDefaultProductUpdateDto >
    {
        /*string GetDefualtProductByOrderItemNo(int orderItemNo);
        string GetDefualtProductByOrderNo(string orderNo);
        string GetDefualtProductByCustomerId(string customerId);*/


    }
}

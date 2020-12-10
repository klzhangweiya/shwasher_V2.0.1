using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using Microsoft.Extensions.Primitives;
using ShwasherSys.Order.Dto;

namespace ShwasherSys.Order
{
    public interface IOrderHeadersAppService : IIwbAsyncCrudAppService<OrderHeaderDto, string, PagedRequestDto, OrderHeaderCreateDto, OrderHeaderUpdateDto >
    {
        string GetNewOrderNo();

        Task<OrderHeader> Audit(EntityDto<string> input);
    }
}

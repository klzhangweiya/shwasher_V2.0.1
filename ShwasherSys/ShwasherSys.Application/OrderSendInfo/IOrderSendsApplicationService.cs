using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo.Dto;

namespace ShwasherSys.OrderSendInfo
{
    public interface IOrderSendAppService : IIwbAsyncCrudAppService<OrderSendDto, int, PagedRequestDto, OrderSendCreateDto, OrderSendUpdateDto >
    {
        Task<List<ViewOrderSend>> GetViewOrderItemAll(PagedRequestDto input);

        Task<string> ExportExcel(List<MultiSearchDto> input);
    }
}

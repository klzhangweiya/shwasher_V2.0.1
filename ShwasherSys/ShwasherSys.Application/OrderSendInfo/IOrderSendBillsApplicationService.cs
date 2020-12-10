using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.OrderSendInfo.Dto;

namespace ShwasherSys.OrderSendInfo
{
    public interface IOrderSendBillAppService : IIwbAsyncCrudAppService<OrderSendBillDto, string, PagedRequestDto, OrderSendBillCreateDto, OrderSendBillUpdateDto >
    {
        Task<List<ViewOrderSend>> GetOrderSendByCustomerId(QuerySendDto input);

        List<SelectListItem> GetHasSendOrderCustomer();

        bool CancelOrderSend(EntityDto<string> input);

        Task<PagedResultDto<ViewOrderSendBill>> GetAllView(PagedRequestDto input);

        Task<string> ExportOrderSend(EntityDto<string> input);
        Task<string> ExportOrderSendCommon(EntityDto<string> input);
    }
}

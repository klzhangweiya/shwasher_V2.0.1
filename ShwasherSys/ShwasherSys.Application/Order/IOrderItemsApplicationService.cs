using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Common.Dto;
using ShwasherSys.Order.Dto;

namespace ShwasherSys.Order
{
    public interface IOrderItemsAppService : IIwbAsyncCrudAppService<OrderItemDto, int, PagedRequestDto, OrderItemCreateDto, OrderItemUpdateDto >
    {
        Task<GetOrderItemDto> GetOrderItemsByOrderNo(string pcOrderNo);

        bool IsAllItemEnd(string orderNo);

        decimal GetItemSend(int orderItemId);


        Task<OrderItem> Audit(EntityDto<int> input);
        Task<List<OrderItem>> AuditAllItems(EntityDto<string> input);
        Task<OrderItemEndCall> End(EntityDto<int> input);
        Task<OrderItem> ChangePrice(ChangeOrderItemInfoDto input);

        Task<OrderItem> ChangeQuantity(ChangeOrderItemInfoDto input);

        Task<OrderItem> ChangeSendDate(ChangeOrderItemInfoDto input);

        Task<OrderItem> SendOrderAction(SendOrderInfoDto input);

        Task<PagedResultDto<ViewOrderItems>> GetViewAll(PagedRequestDto input);
        Task<PagedResultDto<ViewOrderItems>> GetViewAllNot(PagedRequestDto input);
        Task<string> ExportExcel(List<MultiSearchDtoExt> input);

        ViewQueryCurrentProductNum QueryCurrentProductNum(EntityDto<string> input);


        Task<List<OrderItem>> ChangeOrderItemStatus(ChangeOrderItemStatusDto input);
        Task<OrderItemsCallAndEnd> ChangeOrderItemStatusOnHeader(ChangeOrderItemStatusDto input);


        List<StatisticsItem> StatisticsItem(PagedRequestDto input);

        Task<OrderItem> ChangeAfterTaxPrice(ChangeOrderItemInfoDto input);


        Task<LockOrderProductQuantity> GetCurrentProductLock(string productNo, string orderNo);

    }
}

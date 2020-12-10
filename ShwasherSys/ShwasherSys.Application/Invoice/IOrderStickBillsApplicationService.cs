using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Invoice.Dto;

namespace ShwasherSys.Invoice
{
    public interface IOrderStickBillAppService : IIwbAsyncCrudAppService<OrderStickBillDto, string, PagedRequestDto, OrderStickBillCreateDto, OrderStickBillUpdateDto >
    {
        Task<OrderStickBillDto> CreateRed(RedOrderStickBillCreateDto input);
       // Task<List<ViewCustomerStick>> GetAllCreateView(PagedRequestDto input);
        Task<List<ViewStatementBill>> GetAllCreateView(PagedRequestDto input);

        Task<OrderStickBill> UpdateStickNum(OrderStickBillUpdateDto input);

        Task<PagedResultDto<ViewStickBill>> GetViewAll(PagedRequestDto input);

        Task<string> ExportInvoice(EntityDto<string> input);
        Task<string> ExportInvoices(PagedRequestDto input);
        Task<string> ExportStatementBill(EntityDto<string> input);
        Task<OrderStickBill> UpdateState(OrderStickBillUpdateDto input);
    }
}

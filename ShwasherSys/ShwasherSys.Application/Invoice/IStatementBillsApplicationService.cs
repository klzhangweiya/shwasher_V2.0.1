using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Invoice.Dto;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.OrderSendInfo.Dto;

namespace ShwasherSys.Invoice
{
    public interface IStatementBillAppService : IIwbZeroAsyncCrudAppService<StatementBillDto, int, IwbPagedRequestDto, StatementBillCreateDto, StatementBillUpdateDto >
    {


		#region Get

		Task<StatementBill> GetEntity(EntityDto<int> input);
		Task<StatementBill> GetEntityById(int id);
		Task<StatementBill> GetEntityByNo(string no);

		#endregion


        List<SelectListItem> GetHasSendOrderCustomer();

        Task<List<ViewOrderSend>> GetOrderSendByCustomerId(QuerySendDto input);

        Task<List<ViewStatementBill>> QueryStatisticStatementBillItems(QueryStatementBillReportDto input);
    }
}

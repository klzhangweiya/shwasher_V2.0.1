using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Inspection;
using ShwasherSys.Inspection.DisqualifiedProducts.Dto;
using ShwasherSys.ReturnGoods.Dto;

namespace ShwasherSys.ReturnGoods
{
    public interface IReturnGoodOrderAppService : IIwbZeroAsyncCrudAppService<ReturnGoodOrderDto, int, IwbPagedRequestDto, ReturnGoodOrderCreateDto, ReturnGoodOrderUpdateDto >
    {


		#region Get

		Task<ReturnGoodOrder> GetEntity(EntityDto<int> input);
		Task<ReturnGoodOrder> GetEntityById(int id);
		Task<ReturnGoodOrder> GetEntityByNo(string no);

		#endregion

        Task<ReturnGoodOrder> ChangeState(EntityDto<int> input);
        Task<ReturnGoodOrder> RefundApply(RefundAmountDto input);
        Task<ReturnGoodOrder> RefundConfirm(RefundAmountDto input);

      string ExportReturn(EntityDto<int> input);

      DisqualifiedProductDto GetDisqualifiedProductByReturnNo(EntityDto<int> input);

    }
}

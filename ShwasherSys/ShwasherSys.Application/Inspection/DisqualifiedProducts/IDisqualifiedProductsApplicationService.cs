using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Inspection.DisqualifiedProducts.Dto;

namespace ShwasherSys.Inspection.DisqualifiedProducts
{
    public interface IDisqualifiedProductAppService : IIwbZeroAsyncCrudAppService<DisqualifiedProductDto, int, IwbPagedRequestDto, DisqualifiedProductCreateDto, DisqualifiedProductUpdateDto >
    {
        Task CheckDowngrade(ReturnOrderDto input);
        Task UseDowngrade(DowngradeDto input);
        //Task Scrapped(ReturnOrderDto input);
        Task ConfirmScrapped(EntityDto<int> input);
        Task UnScrapped(EntityDto<int> input);
        //Task AntiPlating(EntityDto<int> input);
        Task<List<RelatedProductDto>> QueryRelatedProductionOrder(string productOrderNo);

        Task<PagedResultDto<RelatedProductDto>> QueryRelatedProductionOrderPage(IwbPagedRequestDto input,string productOrderNo);
		#region Get

		Task<DisqualifiedProduct> GetEntity(EntityDto<int> input);
		Task<DisqualifiedProduct> GetEntityById(int id);
		Task<DisqualifiedProduct> GetEntityByNo(string no);
	
        #endregion

    }
}

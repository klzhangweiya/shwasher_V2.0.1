using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductStoreInfo.Dto;

namespace ShwasherSys.ProductStoreInfo
{
    public interface IInventoryCheckAppService : IIwbZeroAsyncCrudAppService<InventoryCheckDto, string, IwbPagedRequestDto, InventoryCheckCreateDto, InventoryCheckUpdateDto >
    {


		#region Get

		Task<InventoryCheckInfo> GetEntity(EntityDto<string> input);
		Task<InventoryCheckInfo> GetEntityById(string id);
		Task<InventoryCheckInfo> GetEntityByNo(string no);

		#endregion


        Task ChangeState(CheckStateDto input);

        Task<PagedResultDto<CurrentStoreItemDto>> QueryCheckStoreItems(IwbPagedRequestDto input);

        Task CheckData(CheckDataDto input);

        Task<List<ViewInventoryCheckRecordProduct>> GetCheckRecordProductNotPage(string checkNo);
        Task<List<ViewInventoryCheckRecordSemi>> GetCheckRecordSemiNotPage(string checkNo);

        Task<PagedResultDto<InventoryCheckDto>> GetAllToEmployee(IwbPagedRequestDto input);

        Task<PagedResultDto<ViewInventoryCheckRecordProduct>> GetCheckRecordProduct(IwbPagedRequestDto input);

        Task<PagedResultDto<ViewInventoryCheckRecordSemi>> GetCheckRecordSemi(IwbPagedRequestDto input);

        Task<List<InventoryReportItem>> QueryStaticsInventoryItems(QueryInventoryReportDto input);
    }
}

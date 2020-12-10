using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.FinshedStoreInfo.Dto;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.FinshedStoreInfo
{
    public interface IFinshedEnterStoreAppService : IIwbAsyncCrudAppService<FinshedEnterStoreDto, int, PagedRequestDto, FinshedEnterStoreCreateDto, FinshedEnterStoreUpdateDto >
    {
        Task<PagedResultDto<ViewProductEnterStore>> GetAllView(PagedRequestDto input);
        Task<FinshedEnterStoreDto> Audit(FinshedEnterStoreAuditDto input);
        Task<FinshedEnterStoreDto> Refuse(EntityDto<int> input);
        Task<FinshedEnterStore> ConfirmEnterStoreQuantity(EntityDto<int> input);

        Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

		#region Get
		Task<FinshedEnterStore> GetEntityById(int id);
		Task<FinshedEnterStore> GetEntityByNo(string no);
		Task<FinshedEnterStoreDto> GetDtoById(int id);
		Task<FinshedEnterStoreDto> GetDtoByNo(string no);

        Task<PagedResultDto<ViewProductEnterStore>> GetViewAll(PagedRequestDto input);

        #endregion

    }
}

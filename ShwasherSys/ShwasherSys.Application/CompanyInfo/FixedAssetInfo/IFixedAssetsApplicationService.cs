using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.CompanyInfo.FixedAssetInfo.Dto;

namespace ShwasherSys.CompanyInfo.FixedAssetInfo
{
    public interface IFixedAssetAppService : IIwbZeroAsyncCrudAppService<FixedAssetDto, int, IwbPagedRequestDto, FixedAssetCreateDto, FixedAssetUpdateDto >
    {
        Task<List<SelectListItem>> GetSelectListName();
        Task<string> GetSelectStrName();

		#region Get

		Task<FixedAsset> GetEntity(EntityDto<int> input);
		Task<FixedAsset> GetEntityById(int id);
		Task<FixedAsset> GetEntityByNo(string no);
	
        #endregion

    }
}

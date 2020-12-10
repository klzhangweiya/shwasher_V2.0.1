using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.FixedAssetTypeInfo.Dto;

namespace ShwasherSys.BasicInfo.FixedAssetTypeInfo
{
    public interface IFixedAssetTypeAppService : IIwbZeroAsyncCrudAppService<FixedAssetTypeDto, string, IwbPagedRequestDto, FixedAssetTypeCreateDto, FixedAssetTypeUpdateDto >
    {


		#region Get

		Task<FixedAssetType> GetEntity(EntityDto<string> input);
		Task<FixedAssetType> GetEntityById(string id);
		Task<FixedAssetType> GetEntityByNo(string no);
	
        #endregion

    }
}

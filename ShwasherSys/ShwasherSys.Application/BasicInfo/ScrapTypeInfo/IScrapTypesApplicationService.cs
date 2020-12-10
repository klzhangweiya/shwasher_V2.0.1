using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.ScrapTypeInfo.Dto;

namespace ShwasherSys.BasicInfo.ScrapTypeInfo
{
    public interface IScrapTypeAppService : IIwbZeroAsyncCrudAppService<ScrapTypeDto, string, IwbPagedRequestDto, ScrapTypeCreateDto, ScrapTypeUpdateDto >
    {


		#region Get

		Task<ScrapType> GetEntity(EntityDto<string> input);
		Task<ScrapType> GetEntityById(string id);
		Task<ScrapType> GetEntityByNo(string no);
	
        #endregion

    }
}

using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.ScrapStore.Dto;

namespace ShwasherSys.ScrapStore
{
    public interface IScrapEnterStoreAppService : IIwbZeroAsyncCrudAppService<ScrapEnterStoreDto, string, IwbPagedRequestDto, ScrapEnterStoreCreateDto, ScrapEnterStoreUpdateDto >
    {


		#region Get

		Task<ScrapEnterStore> GetEntity(EntityDto<string> input);
		Task<ScrapEnterStore> GetEntityById(string id);
		Task<ScrapEnterStore> GetEntityByNo(string no);

		#endregion

        Task<PagedResultDto<ViewScrapEnterStore>> GetViewAll(IwbPagedRequestDto input);


    }
}

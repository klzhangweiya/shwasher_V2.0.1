using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.RmStore.Dto;

namespace ShwasherSys.RmStore
{
    public interface IRmOutStoreAppService : IIwbZeroAsyncCrudAppService<RmOutStoreDto, string, IwbPagedRequestDto, RmOutStoreCreateDto, RmOutStoreUpdateDto >
    {


		#region Get

		Task<RmOutStore> GetEntity(EntityDto<string> input);
		Task<RmOutStore> GetEntityById(string id);
		Task<RmOutStore> GetEntityByNo(string no);

		#endregion

        Task<PagedResultDto<ViewRmOutStore>> GetAllView(IwbPagedRequestDto input);
		Task UpdateState(RwOutStatusUpdateDto input);



	}
}

using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.RmStore.Dto;

namespace ShwasherSys.RmStore
{
    public interface IRmEnterStoreAppService : IIwbZeroAsyncCrudAppService<RmEnterStoreDto, string, IwbPagedRequestDto, RmEnterStoreCreateDto, RmEnterStoreUpdateDto >
    {


		#region Get

		Task<RmEnterStore> GetEntity(EntityDto<string> input);
		Task<RmEnterStore> GetEntityById(string id);
		Task<RmEnterStore> GetEntityByNo(string no);

		#endregion

        Task<PagedResultDto<ViewRmEnterStore>> GetAllView(IwbPagedRequestDto input);

        Task UpdateState(RwEnterStatusUpdateDto input);

    }
}

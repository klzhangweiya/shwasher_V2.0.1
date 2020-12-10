using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.RmStore.Dto;

namespace ShwasherSys.RmStore
{
    public interface ICurrentRmStoreHouseAppService : IIwbZeroAsyncCrudAppService<CurrentRmStoreHouseDto, string, IwbPagedRequestDto, CurrentRmStoreHouseCreateDto, CurrentRmStoreHouseUpdateDto >
    {


		#region Get

		Task<CurrentRmStoreHouse> GetEntity(EntityDto<string> input);
		Task<CurrentRmStoreHouse> GetEntityById(string id);
		Task<CurrentRmStoreHouse> GetEntityByNo(string no);

		#endregion

        Task<PagedResultDto<ViewCurrentRmStoreHouse>> GetAllView(IwbPagedRequestDto input);
        Task<RmEnterStore> AddEnter(AddRmEnterStore input);

        Task<RmOutStore> AddOut(AddRmOutStoreDto input);

    }
}

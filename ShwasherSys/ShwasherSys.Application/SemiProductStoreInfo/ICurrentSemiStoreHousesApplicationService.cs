using IwbZero.AppServiceBase;
using ShwasherSys.SemiProductStoreInfo.Dto;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductStoreInfo.Dto;

namespace ShwasherSys.SemiProductStoreInfo
{
    public interface ICurrentSemiStoreHousesAppService : IIwbAsyncCrudAppService<CurrentSemiStoreHouseDto, int, PagedRequestDto, CurrentSemiStoreHouseCreateDto, CurrentSemiStoreHouseUpdateDto >
    {
        Task<PagedResultDto<ViewCurrentSemiStoreHouse>> GetViewAll(PagedRequestDto input);
        /// <summary>
        /// 更新实时库存千斤重
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateKgWeight(UpdateKgWeightDto input);
        /*Task<string> GetVirtualProOrderNo();*/
        Task<string> GetVirtualProOrderNo();
        Task<SemiEnterStore> AddEnter(AddSemiEnterStoreDto input);

        Task<SemiOutStore> AddOut(AddSemiOutStoreDto input);


        Task ChangeStoreHouse(ChangeStoreHouseDto input);

        Task UpdateStoreLocation(UpdateLocationDto input);

    }
}

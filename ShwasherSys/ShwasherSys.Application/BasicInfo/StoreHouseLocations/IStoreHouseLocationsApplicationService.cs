using IwbZero.AppServiceBase;

using ShwasherSys.BasicInfo.StoreHouseLocations.Dto;

namespace ShwasherSys.BasicInfo.StoreHouseLocations
{
    public interface IStoreHouseLocationsAppService : IIwbAsyncCrudAppService<StoreHouseLocationDto, int, PagedRequestDto, StoreHouseLocationCreateDto, StoreHouseLocationUpdateDto >
    {
    }
}

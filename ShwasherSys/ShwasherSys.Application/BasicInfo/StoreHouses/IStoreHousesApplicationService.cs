using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.StoreHouses.Dto;

namespace ShwasherSys.BasicInfo.StoreHouses
{
    public interface IStoreHousesAppService : IIwbAsyncCrudAppService<StoreHouseDto, int, PagedRequestDto, StoreHouseCreateDto, StoreHouseUpdateDto >
    {
    }
}

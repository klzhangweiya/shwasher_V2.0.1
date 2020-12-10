using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.Common.Dto;
using ShwasherSys.ProductStoreInfo.Dto;
using ShwasherSys.SemiProductStoreInfo;
using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.ProductStoreInfo
{
    public interface ICurrentFinshedStoreHouseAppService : IIwbAsyncCrudAppService<CurrentProductStoreHouseDto, int, PagedRequestDto, CurrentProductStoreHouseCreateDto, CurrentProductStoreHouseUpdateDto >
    {
        Task<PagedResultDto<ViewCurrentProductStoreHouse>> GetViewAll(PagedRequestDto input);

        //Task<string> GetVirtualProOrderNo();

        Task<string> GetVirtualBlanceProOrderNo();

        //Task<CurrentProductStoreHouseDto> AddVirtualStore(CurrentProductStoreHouseCreateDto input);

        Task<string> ExportExcel(List<MultiSearchDtoExt> input);

        Task<FinshedEnterStore> AddEnter(AddEnterStoreDto input);

        Task<ProductOutStore> AddOut(AddOutStoreDto input);


        Task UpdateStoreLocation(ChangeStoreHouseDto input);


        List<ProductionOrderDisCustomerDto> GetDisCustomerInfo(EntityDto<string> input);
        ////void PreMonthExcute();

    }
}

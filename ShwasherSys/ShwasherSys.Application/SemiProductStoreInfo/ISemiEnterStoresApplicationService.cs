using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;

using ShwasherSys.SemiProductStoreInfo.Dto;

namespace ShwasherSys.SemiProductStoreInfo
{
    public interface ISemiEnterStoresAppService : IIwbAsyncCrudAppService<SemiEnterStoreDto, int, PagedRequestDto, SemiEnterStoreCreateDto, SemiEnterStoreUpdateDto >
    {
        Task<SemiEnterStoreDto> Audit(SemiEnterStoreAuditDto input);
        Task<SemiEnterStoreDto> Refuse(EntityDto<int> input);

        Task<PagedResultDto<ViewSemiEnterStore>> GetViewAll(PagedRequestDto input);
    }
}

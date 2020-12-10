using IwbZero.AppServiceBase;
using ShwasherSys.SemiProductStoreInfo.Dto;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ShwasherSys.SemiProductStoreInfo
{
    public interface ISemiOutStoreAppService : IIwbAsyncCrudAppService<SemiOutStoreDto, int, PagedRequestDto, SemiOutStoreCreateDto, SemiOutStoreUpdateDto >
    {
        Task<SemiOutStoreDto> Audit(SemiEnterStoreAuditDto input);
        Task<SemiOutStoreDto> Refuse(EntityDto<int> input);
        Task PackageApply(SemiOutStoreCreateDto input);
        PagedResultDto<ViewSemiOutStore> GetViewAll(PagedRequestDto input);
    }
}

using System.Threading.Tasks;
using IwbZero.AppServiceBase;

using ShwasherSys.BasicInfo.Region.Dto;

namespace ShwasherSys.BasicInfo.Region
{
    public interface IRegionsAppService : IIwbAsyncCrudAppService<RegionDto, string, PagedRequestDto, RegionCreateDto, RegionUpdateDto >
    {
        Task<string> GetRegionSelectStrs();
    }
}

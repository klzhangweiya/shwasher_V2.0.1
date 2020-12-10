using IwbZero.AppServiceBase;
using ShwasherSys.BaseSysInfo.Help.Dto;


namespace ShwasherSys.BaseSysInfo.Help
{
    public interface ISysHelpsAppService : IIwbAsyncCrudAppService<SysHelpDto, int, PagedRequestDto, SysHelpCreateDto, SysHelpUpdateDto >
    {
    }
}

using System.Threading.Tasks;
using ShwasherSys.BaseSysInfo.Settings.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.Settings
{
    public interface ISettingsAppService : IIwbAsyncCrudAppService<SettingDto, int, PagedRequestDto, SettingCreateDto, SettingUpdateDto>
    {
        Task Refresh();

       
    }
}

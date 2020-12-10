using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.NotificationInfo.Dto;

namespace ShwasherSys.NotificationInfo
{
    public interface IShortMsgDetailAppService : IIwbAsyncCrudAppService<ShortMsgDetailDto, int, PagedRequestDto, ShortMsgDetailCreateDto, ShortMsgDetailUpdateDto >
    {

        Task<PagedResultDto<ShortMsgDetailDto>> GetAllByUser(PagedRequestDto input);
        NoticeAlarmDto GetMsgByUser();

        ShortMsgDetail ChangeIsRead(EntityDto<int> input);

        Task SetRead(EntityDto<string> input);
        Task BatchDelete(EntityDto<string> input);

    }
}

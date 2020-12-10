using IwbZero.AppServiceBase;
using ShwasherSys.NotificationInfo.Dto;

namespace ShwasherSys.NotificationInfo
{
    public interface IShortMessagesAppService : IIwbAsyncCrudAppService<ShortMessageDto, int, PagedRequestDto, ShortMessageCreateDto, ShortMessageUpdateDto >
    {
    }
}

using IwbZero.AppServiceBase;

using ShwasherSys.NotificationInfo.Dto;

namespace ShwasherSys.NotificationInfo
{
    public interface IBulletinInfosAppService : IIwbAsyncCrudAppService<BulletinInfoDto, int, PagedRequestDto, BulletinInfoCreateDto, BulletinInfoUpdateDto >
    {
    }
}

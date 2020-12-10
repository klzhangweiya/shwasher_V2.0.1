using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.EntityFramework;
using ShwasherSys.NotificationInfo.Dto;
namespace ShwasherSys.NotificationInfo
{
    [AbpAuthorize]
    public class ShortMsgDetailAppService : ShwasherAsyncCrudAppService<ShortMsgDetail, ShortMsgDetailDto, int, PagedRequestDto, ShortMsgDetailCreateDto, ShortMsgDetailUpdateDto >,IShortMsgDetailAppService
    {
        protected IRepository<ShortMessage> ShortMessageRepository;
        protected ISqlExecuter SqlExecuter { get; }
        public ShortMsgDetailAppService(IRepository<ShortMsgDetail, int> repository, IRepository<ShortMessage> shortMessageRepository, ISqlExecuter sqlExecuter) : base(repository)
        {
            ShortMessageRepository = shortMessageRepository;
            SqlExecuter = sqlExecuter;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesShortMsgDetail;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesShortMsgDetail;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesShortMsgDetailCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesShortMsgDetailUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesShortMsgDetailDelete;

        [DisableAuditing]
        [AbpAuthorize(PermissionNames.PagesNotificationInfoShortMsgMgQuery)]
        public async Task<PagedResultDto<ShortMsgDetailDto>> GetAllByUser(PagedRequestDto input)
        {
            string currentUser = AbpSession.UserName;
            var msgDetails = Repository.GetAll().Where(i =>i.RecvUserID == currentUser);
            var msgs = ShortMessageRepository.GetAll().Where(i => i.IsDelete == "N");
            var shortMsgDetailDtos = from d in msgDetails
                join m in msgs on d.MsgID equals m.Id
                select new ShortMsgDetailDto()
                {
                    Content = m.Content,
                    Id = d.Id,
                    SendTime = m.SendTime,
                    IsRead = d.IsRead,
                    MsgID = m.Id,
                    RecvUserID = d.RecvUserID,
                    SendUserID = m.SendUserID,
                    Title = m.Title
                };
            var totalCount = await AsyncQueryableExecuter.CountAsync(shortMsgDetailDtos);

            shortMsgDetailDtos = shortMsgDetailDtos.OrderByDescending(i=>i.SendTime);
            shortMsgDetailDtos = shortMsgDetailDtos.Skip(input.SkipCount).Take(input.MaxResultCount);

            var entities = await AsyncQueryableExecuter.ToListAsync(shortMsgDetailDtos);
            var dtos = new PagedResultDto<ShortMsgDetailDto>(
                totalCount,
                entities
            );
            return dtos;
        }


        public NoticeAlarmDto GetMsgByUser()
        {
            string currentUser = AbpSession.UserName;
            var msgDetails = Repository.GetAll().Where(i => i.RecvUserID == currentUser);
            var msgs = ShortMessageRepository.GetAll().Where(i => i.IsDelete == "N");
            var shortMsgDetailDtos = from d in msgDetails
                join m in msgs on d.MsgID equals m.Id
                select new ShortMsgDetailDto()
                {
                    Content = m.Content,
                    Id = d.Id,
                    SendTime = m.SendTime,
                    IsRead = d.IsRead,
                    MsgID = m.Id,
                    RecvUserID = d.RecvUserID,
                    SendUserID = m.SendUserID,
                    Title = m.Title
                };
            shortMsgDetailDtos = shortMsgDetailDtos.Where(i => i.IsRead == "N");
            int total = shortMsgDetailDtos.Count();
            shortMsgDetailDtos = shortMsgDetailDtos.OrderByDescending(i => i.SendTime);
            shortMsgDetailDtos = shortMsgDetailDtos.Take(5);
            NoticeAlarmDto loNoticeAlarmDto = new NoticeAlarmDto()
            {
                Total = total,
                Items = shortMsgDetailDtos.ToList()
            };
            return loNoticeAlarmDto;

        }
        [AbpAuthorize(PermissionNames.PagesNotificationInfoShortMsgMgSetRead)]
        public ShortMsgDetail ChangeIsRead(EntityDto<int> input)
        {
            var msgDetails = Repository.FirstOrDefault(input.Id);
            if (msgDetails != null)
            {
                msgDetails.IsRead = "Y";
                Repository.UpdateAsync(msgDetails);
            }
            return msgDetails;
        }
        [AbpAuthorize(PermissionNames.PagesNotificationInfoShortMsgMgSetRead)]
        public async Task SetRead(EntityDto<string> input)
        {
            if (!input.Id.IsNullOrWhiteSpace())
            {
                string condition = input.Id.Substring(0, input.Id.Length - 1);
                await SqlExecuter.ExecuteAsync("update ShortMsgDetail set IsRead='Y' where DetailID in (" + condition + ")");
            }
        }

        [AbpAuthorize(PermissionNames.PagesNotificationInfoShortMsgMgDelete)]
        public async Task BatchDelete(EntityDto<string> input)
        {
            if (!input.Id.IsNullOrWhiteSpace())
            {
                string condition = input.Id.Substring(0, input.Id.Length - 1);
                await SqlExecuter.ExecuteAsync("delete from ShortMsgDetail where DetailID in (" + condition + ")");
            }
        }
    }
}

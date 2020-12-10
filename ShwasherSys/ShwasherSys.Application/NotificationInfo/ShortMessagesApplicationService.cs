using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using IwbZero.AppServiceBase;
using ShwasherSys.NotificationInfo.Dto;
namespace ShwasherSys.NotificationInfo
{
    [AbpAuthorize]
    public class ShortMessagesAppService : ShwasherAsyncCrudAppService<ShortMessage, ShortMessageDto, int, PagedRequestDto, ShortMessageCreateDto, ShortMessageUpdateDto >, IShortMessagesAppService
    {
        protected IRepository<ShortMsgDetail> ShortMsgDetailRepository;
        public ShortMessagesAppService(IRepository<ShortMessage, int> repository, IRepository<ShortMsgDetail> shortMsgDetailRepository) : base(repository)
        {
            ShortMsgDetailRepository = shortMsgDetailRepository;
        }

		protected override string GetPermissionName { get; set; } //= PermissionNames.PagesShortMessage;
		protected override string GetAllPermissionName { get; set; } //= PermissionNames.PagesShortMessage;
		protected override string CreatePermissionName { get; set; } //= PermissionNames.PagesShortMessageCreate;
		protected override string UpdatePermissionName { get; set; } //= PermissionNames.PagesShortMessageUpdate;
		protected override string DeletePermissionName { get; set; } //= PermissionNames.PagesShortMessageDelete;

        public override async Task<ShortMessageDto> Create(ShortMessageCreateDto input)
        {
            CheckCreatePermission();

            var resultEntity = await CreateEntity(input);
            string lcReciveUsers = resultEntity.RecieveUserIds ?? throw new ArgumentNullException("resultEntity.RecieveUserIds");
            string[] userNames = lcReciveUsers.Split(',');
            foreach (var userName in userNames)
            {
                ShortMsgDetail loDetail = new ShortMsgDetail()
                {
                    IsRead = "N",
                    MsgID = resultEntity.Id,
                    RecvUserID = userName
                };
                ShortMsgDetailRepository.Insert(loDetail);
            }

            return resultEntity;
        }

        

    }
}

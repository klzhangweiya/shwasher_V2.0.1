using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.Lambda;
using ShwasherSys.NotificationInfo.Dto;
namespace ShwasherSys.NotificationInfo
{
    [AbpAuthorize]
    public class BulletinInfosAppService : ShwasherAsyncCrudAppService<BulletinInfo, BulletinInfoDto, int, PagedRequestDto, BulletinInfoCreateDto, BulletinInfoUpdateDto >,IBulletinInfosAppService
    {
        protected IStatesAppService StatesAppService;
        public BulletinInfosAppService(IRepository<BulletinInfo, int> repository, IStatesAppService statesAppService) : base(repository)
        {
            StatesAppService = statesAppService;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesNotificationInfoBulletinInfos;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesNotificationInfoBulletinInfos;
        protected override string CreatePermissionName { get; set; } = PermissionNames.PagesNotificationInfoBulletinInfosCreate;
        protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesNotificationInfoBulletinInfosUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesNotificationInfoBulletinInfosDelete;


        [DisableAuditing]
        public override async Task<PagedResultDto<BulletinInfoDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
          
            if (input.SearchList != null && input.SearchList.Count > 0)
            {
                List<LambdaObject> objList = new List<LambdaObject>();
                foreach (var o in input.SearchList)
                {
                    if (o.KeyWords.IsNullOrEmpty())
                        continue;
                    object keyWords = o.KeyWords;
                    objList.Add(new LambdaObject
                    {
                        FieldType = (LambdaFieldType)o.FieldType,
                        FieldName = o.KeyField,
                        FieldValue = keyWords,
                        ExpType = (LambdaExpType)o.ExpType
                    });
                }
                var exp = objList.GetExp<BulletinInfo>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var dtos = new PagedResultDto<BulletinInfoDto>(
                totalCount,
                entities.Select(i=>new BulletinInfoDto()
                {
                    BulletinType = i.BulletinType,
                    BulletinTypeName = StatesAppService.GetDisplayValue("BulletinInfo", "BulletinType", i.BulletinType),
                    Content = i.Content,
                    ExpirationDate = i.ExpirationDate,
                    Id = i.Id,
                    PromulgatTime = i.PromulgatTime,
                    Promulgator = i.Promulgator,
                    TimeLastMod = i.TimeLastMod,
                    TimeCreated = i.TimeCreated,
                    Title = i.Title
                }).ToList()
            );
            return dtos;
        }

    }
}

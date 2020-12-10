using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using IwbZero.AppServiceBase;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo.Help.Dto;
using ShwasherSys.BaseSysInfo.States;
using ShwasherSys.BaseSysInfo.Users.Dto;
using ShwasherSys.Lambda;

namespace ShwasherSys.BaseSysInfo.Help
{
    [AbpAuthorize]
    public class SysHelpsAppService : ShwasherAsyncCrudAppService<SysHelp, SysHelpDto, int, PagedRequestDto, SysHelpCreateDto, SysHelpUpdateDto >, ISysHelpsAppService
    {
        private IStatesAppService StatesAppService;
        public SysHelpsAppService(IStatesAppService statesAppService,IRepository<SysHelp, int> repository) : base(repository)
        {
            StatesAppService = statesAppService;
        }

		protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemSysHelp;
		protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemSysHelp;
		protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemSysHelpCreate;
		protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemSysHelpUpdate;
        protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemSysHelpDelete;

        [AbpAuthorize(PermissionNames.PagesSystemSysHelp)]
        public override async Task<PagedResultDto<SysHelpDto>> GetAll(PagedRequestDto input)
        {
            var query = Repository.GetAll();
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
                var exp = objList.GetExp<SysHelp>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<SysHelpDto>(totalCount, entities.Select(a=>new SysHelpDto()
            {
                Id = a.Id,
                Classification= a.Classification,
                ClassificationShow = StatesAppService.GetDisplayValue("SysHelp", "Classification", a.Classification + ""),
                HelpContent = a.HelpContent,
                HelpKeyWords = a.HelpKeyWords,
                Sequence = a.Sequence,
                TimeCreated = a.TimeCreated,
                HelpTitle = a.HelpTitle,
                TimeLastMod = a.TimeLastMod,
                UserIDLastMod = a.UserIDLastMod
            }).ToList());
            return dtos;
        }

    }
}

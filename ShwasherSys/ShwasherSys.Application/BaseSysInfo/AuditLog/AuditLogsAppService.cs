using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Castle.Core.Internal;
using ShwasherSys.Authorization.Permissions;
using ShwasherSys.BaseSysInfo.AuditLog.Dto;
using ShwasherSys.Lambda;
using IwbZero.AppServiceBase;
using IwbZero.Auditing;

namespace ShwasherSys.BaseSysInfo.AuditLog
{
    [AbpAuthorize, AuditLog("系统日志", "日志")]
    public class AuditLogsAppService : ShwasherAsyncCrudAppService<SysLog, SysLogDto, long, PagedRequestDto>, IAuditLogsAppService
    {

        public AuditLogsAppService(ICacheManager cacheManager, IRepository<SysLog, long> repository) : base(repository)
        {
            CacheManager = cacheManager;
        }

        protected override string GetPermissionName { get; set; } = PermissionNames.PagesSystemSysLog;
        protected override string GetAllPermissionName { get; set; } = PermissionNames.PagesSystemSysLog;
        //protected override string CreatePermissionName { get; set; } = PermissionNames.PagesSystemSysLogCreate;
        //protected override string UpdatePermissionName { get; set; } = PermissionNames.PagesSystemSysLogUpdate;
        //protected override string DeletePermissionName { get; set; } = PermissionNames.PagesSystemSysLogDelete;

        #region GetSelectList

        [DisableAuditing, AllowAnonymous]
        public async Task<List<SelectListItem>> GetLogServiceSelectLists()
        {
            var slist = new List<SelectListItem>();
            var list = await Repository.GetAllListAsync(a => a.LogType != 0);
            list = list.MyDistinct(a => a.ServiceName).ToList();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.ServiceName, Value = l.ServiceName });
            }
            return slist;
        }

        [DisableAuditing, AllowAnonymous]
        public async Task<string> GetLogServiceSelectListStrs()
        {
            var options = "";
            var list = Repository.GetAll().Where(i => i.LogType != 0);
            var ss = await list.GroupBy(i => i.ServiceName).Select(a=>new{ ServiceName = a.Key }).ToListAsync();
            foreach (var l in ss)
            {
                options += $"<option value=\"{l.ServiceName}\" >{l.ServiceName}</option>\r\n";
            }
            return options;
        }

        [DisableAuditing, AllowAnonymous]
        public async Task<List<SelectListItem>> GetLogMethodSelectLists(QueryMethodName input)
        {
            var slist = new List<SelectListItem>();
            var list = await Repository.GetAllListAsync(a =>
                a.LogType != 0 && (string.IsNullOrEmpty(input.ServiceName) || a.ServiceName == input.ServiceName));
            list = list.MyDistinct(a => a.MethodName).ToList();
            foreach (var l in list)
            {
                slist.Add(new SelectListItem { Text = l.MethodName, Value = l.MethodName });
            }
            return slist;
        }

        [DisableAuditing, AllowAnonymous]
        public async Task<string> GetLogMethodSelectListStrs(QueryMethodName input)
        {
            string options = "";
            var list = Repository.GetAll().Where(a =>a.LogType != 0 && (string.IsNullOrEmpty(input.ServiceName) || a.ServiceName == input.ServiceName));
            var ss = await list.GroupBy(i => i.MethodName).Select(a => new { MethodName = a.Key }).ToListAsync();
            foreach (var l in ss)
            {
                options += $"<option value=\"{l.MethodName}\" >{l.MethodName}</option>\r\n";
            }
            return options;
        }

        #endregion
        [DisableAuditing]
        public override async Task<PagedResultDto<SysLogDto>> GetAll(PagedRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input).Where(a => a.LogType != 0);
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
                var exp = objList.GetExp<SysLog>();
                query = query.Where(exp);
            }
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = new PagedResultDto<SysLogDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
            return dtos;
        }
    }
}

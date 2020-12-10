using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ShwasherSys.BaseSysInfo.AuditLog.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.AuditLog
{
    public interface IAuditLogsAppService : IIwbAsyncCrudAppService<SysLogDto, long, PagedRequestDto>
    {
        Task<List<SelectListItem>> GetLogServiceSelectLists();
        Task<string> GetLogServiceSelectListStrs();
        Task<List<SelectListItem>> GetLogMethodSelectLists(QueryMethodName input);
        Task<string> GetLogMethodSelectListStrs(QueryMethodName input);
    }
}

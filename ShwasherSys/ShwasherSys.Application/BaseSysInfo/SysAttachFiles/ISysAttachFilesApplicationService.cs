using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using IwbZero.AppServiceBase;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;

namespace ShwasherSys.BaseSysInfo.SysAttachFiles
{
    public interface ISysAttachFileAppService : IIwbAsyncCrudAppService<SysAttachFileDto, int, PagedRequestDto, SysAttachFileCreateDto, SysAttachFileUpdateDto >
    {

        Task<List<SysAttachFileDto>> QueryAttach(QueryAttachDto input);
        Task<List<SelectListItem>> GetTableSelectList(string tableName, string colName);
        Task<string> GetTableSelectStr(string tableName, string colName);
        Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();
    }
}

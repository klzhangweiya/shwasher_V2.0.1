using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ShwasherSys.BaseSysInfo.Functions.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BaseSysInfo.Functions
{
    public interface IFunctionsAppService : IIwbAsyncCrudAppService<FunctionDto, int, PagedRequestDto, FunctionCreateDto, FunctionUpdateDto>
    {
        Task<SysFunction> GetFunByPermissionName(string name);
        Task<List<SelectListItem>> GetFunctionSelect();
        Task<string> GetFunctionSelectStr();
        Task MoveUp(MoveUpFunctionDto input);
        Task MoveDown(MoveDownFunctionDto input);
        Task Refresh();
    }
}

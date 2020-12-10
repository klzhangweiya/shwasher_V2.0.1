using System.Collections.Generic;
using IwbZero.AppServiceBase;

using ShwasherSys.BasicInfo.Dutys.Dto;
using System.Web.Mvc;

namespace ShwasherSys.BasicInfo.Dutys
{
    public interface IDutysAppService : IIwbAsyncCrudAppService<DutyDto, int, PagedRequestDto, DutyCreateDto, DutyUpdateDto >
    {
        List<SelectListItem> GetDutysSelects();
    }
}

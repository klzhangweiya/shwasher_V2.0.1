using System.Collections.Generic;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.Departments.Dto;
using System.Web.Mvc;

namespace ShwasherSys.BasicInfo.Departments
{
    public interface IDepartmentsAppService : IIwbAsyncCrudAppService<DepartmentDto, string, PagedRequestDto, DepartmentCreateDto, DepartmentUpdateDto >
    {
        List<SelectListItem> GetDepartmentsSelects();
    }
}

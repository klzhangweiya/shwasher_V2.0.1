using System.Collections.Generic;
using System.Web.Mvc;
using IwbZero.AppServiceBase;

using ShwasherSys.BasicInfo.Factory.Dto;

namespace ShwasherSys.BasicInfo.Factory
{
    public interface IFactoriesAppService : IIwbAsyncCrudAppService<FactoriesDto, string, PagedRequestDto, FactoriesCreateDto, FactoriesUpdateDto >
    {
        List<SelectListItem> GetFactoriesSelects();
    }
}

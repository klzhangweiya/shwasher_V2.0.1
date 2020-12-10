using System.Collections.Generic;
using System.Web.Mvc;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductInfo.Dto;

namespace ShwasherSys.ProductInfo
{
    public interface IStandardsAppService : IIwbAsyncCrudAppService<StandardDto, int, PagedRequestDto, StandardCreateDto, StandardUpdateDto >
    {
        List<SelectListItem> GetStandardsList();
    }
}

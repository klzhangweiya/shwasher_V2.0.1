using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.ExpressInfo.Dto;
using ShwasherSys.BasicInfo.Factory.Dto;

namespace ShwasherSys.BasicInfo.ExpressInfo
{
    public interface IExpressAppService : IIwbAsyncCrudAppService<ExpressLogisticsDto, int, PagedRequestDto, ExpressLogisticsCreateDto, ExpressLogisticsUpdateDto>
    {
        List<SelectListItem> GetExpressSelects();

       // Task<string> GetProviderOptions();

       ExpressLogisticsDto GetExpressDtoById(EntityDto<int> input);

    }
}

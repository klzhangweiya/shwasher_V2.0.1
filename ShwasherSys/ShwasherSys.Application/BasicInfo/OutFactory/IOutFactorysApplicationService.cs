using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using IwbZero.AppServiceBase;

using ShwasherSys.BasicInfo.OutFactory.Dto;

namespace ShwasherSys.BasicInfo.OutFactory
{
    public interface IOutFactoryAppService : IIwbAsyncCrudAppService<OutFactoryDto, string, PagedRequestDto, OutFactoryCreateDto, OutFactoryUpdateDto >
    {


		Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

	

    }
}

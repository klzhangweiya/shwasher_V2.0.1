using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Entities;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.Dto;

namespace ShwasherSys.BasicInfo
{
    public interface ICurrencyAppService : IIwbAsyncCrudAppService<CurrencyDto, string, PagedRequestDto, CurrencyCreateDto, CurrencyUpdateDto >
    {


		Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

		#region Get
		Task<Currency> GetEntityById(string id);
		Task<Currency> GetEntityByNo(string no);
		Task<CurrencyDto> GetDtoById(string id);
		Task<CurrencyDto> GetDtoByNo(string no);

       

        #endregion

    }
}

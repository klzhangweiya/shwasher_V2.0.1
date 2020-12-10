using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using IwbZero.AppServiceBase;
using ShwasherSys.BasicInfo.Dto;

namespace ShwasherSys.BasicInfo
{
    public interface ICurrencyExchangeRateAppService : IIwbAsyncCrudAppService<CurrencyExchangeRateDto, int, PagedRequestDto, CurrencyExchangeRateCreateDto, CurrencyExchangeRateUpdateDto >
    {


		Task<List<SelectListItem>> GetSelectList();
		Task<string> GetSelectStr();

		#region Get
		Task<CurrencyExchangeRate> GetEntityById(int id);
		Task<CurrencyExchangeRate> GetEntityByNo(string no);
		Task<CurrencyExchangeRateDto> GetDtoById(int id);
		Task<CurrencyExchangeRateDto> GetDtoByNo(string no);
        #endregion

    }
}

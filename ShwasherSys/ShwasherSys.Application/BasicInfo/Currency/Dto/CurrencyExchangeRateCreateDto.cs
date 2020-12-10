using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(CurrencyExchangeRate))]
    public class CurrencyExchangeRateCreateDto
    {
  //      [StringLength(CurrencyExchangeRate.CurrencyIdMaxLength)]
		//public string FromCurrencyId  { get; set; }
        [StringLength(CurrencyExchangeRate.CurrencyIdMaxLength)]
		public string ToCurrencyId  { get; set; }
		public decimal? ExchangeRate  { get; set; }
		
    }
}

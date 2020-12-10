using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(CurrencyExchangeRate)),AutoMapFrom(typeof(CurrencyExchangeRate))]
    public class CurrencyExchangeRateDto: EntityDto<int>
    {
		public string FromCurrencyId  { get; set; }
		public string ToCurrencyId  { get; set; }
		public decimal? ExchangeRate  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
    }
}
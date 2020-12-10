using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(CurrencyExchangeRate))]
    public class CurrencyExchangeRateUpdateDto: EntityDto<int>
    {
        [StringLength(CurrencyExchangeRate.CurrencyIdMaxLength)]
		public string FromCurrencyId  { get; set; }
        [StringLength(CurrencyExchangeRate.CurrencyIdMaxLength)]
		public string ToCurrencyId  { get; set; }
		public decimal? ExchangeRate  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(CurrencyExchangeRate.UserIdLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}
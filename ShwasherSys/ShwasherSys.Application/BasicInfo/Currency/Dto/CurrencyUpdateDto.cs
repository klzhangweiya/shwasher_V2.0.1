using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AutoMapper;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(Currency))]
    public class CurrencyUpdateDto: EntityDto<string>
    {
        [StringLength(Currency.CurrencyNameMaxLength)]
		public string CurrencyName  { get; set; }
        //public DateTime? TimeCreated  { get; set; }
        //public DateTime? TimeLastMod  { get; set; }
        //      [StringLength(Currency.UserIdLastModMaxLength)]
        //public string UserIDLastMod  { get; set; }
        [IgnoreMap]
        public List<CurrencyExchangeRateCreateDto> CurrencyExchangeRates { get; set; }
    }
}
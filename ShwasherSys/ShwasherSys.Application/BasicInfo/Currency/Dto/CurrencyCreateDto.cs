using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using AutoMapper;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(Currency))]
    public class CurrencyCreateDto:Entity<string>
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

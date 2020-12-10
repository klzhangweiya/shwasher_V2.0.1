using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.BasicInfo.Dto
{
    [AutoMapTo(typeof(Currency)),AutoMapFrom(typeof(Currency))]
    public class CurrencyDto: EntityDto<string>
    {
		public string CurrencyName  { get; set; }

        [IgnoreMap]
        public List<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
        public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
    }

  

}
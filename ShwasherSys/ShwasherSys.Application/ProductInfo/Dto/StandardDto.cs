using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Standard)),AutoMapFrom(typeof(Standard))]
    public class StandardDto: EntityDto<int>
    {
		public string StandardName  { get; set; }
		public string StandardDesc  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BasicInfo.OutFactory.Dto
{
    [AutoMapTo(typeof(OutFactory)),AutoMapFrom(typeof(OutFactory))]
    public class OutFactoryDto: EntityDto<string>
    {
		public string OutFactoryName  { get; set; }
		public string LinkMan  { get; set; }
		public string Address  { get; set; }
		public string WebSite  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
		public string Telephone  { get; set; }
		public string Fax  { get; set; }
		public string Zip  { get; set; }
		public string Email  { get; set; }
    }
}
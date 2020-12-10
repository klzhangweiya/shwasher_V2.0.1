using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Factory.Dto
{
    [AutoMapTo(typeof(Factories)),AutoMapFrom(typeof(Factories))]
    public class FactoriesDto: EntityDto<string>
    {
		public string FactoryName  { get; set; }
		public string ShortNames  { get; set; }
		public string RegionID  { get; set; }
		public string FactoryURL  { get; set; }
		public string Address  { get; set; }
		public string ZIP  { get; set; }
		public string LinkMan  { get; set; }
		public string Telephone  { get; set; }
		public string Remark  { get; set; }
		//public string IsLock  { get; set; }
    }
}
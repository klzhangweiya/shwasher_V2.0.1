using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Region.Dto
{
    [AutoMapTo(typeof(Regions)),AutoMapFrom(typeof(Regions))]
    public class RegionDto: EntityDto<string>
    {
		public string RegionName  { get; set; }
		public string FatherRegionID  { get; set; }
		public string URL  { get; set; }
		public int Depth  { get; set; }
		public string IsLeaf  { get; set; }
		public int Sort  { get; set; }
		public string Path  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Region.Dto
{
    [AutoMapTo(typeof(Regions))]
    public class RegionCreateDto:EntityDto<string>
    {
        [Required] 
        [StringLength(Regions.RegionNameMaxLength)]
		public string RegionName  { get; set; }
        [Required] 
        [StringLength(Regions.FatherRegionIDMaxLength)]
		public string FatherRegionID  { get; set; }
        [StringLength(Regions.URLMaxLength)]
		public string URL  { get; set; }
		public int Depth  { get; set; }
   
        [StringLength(Regions.IsLeafMaxLength)]
		public string IsLeaf  { get; set; }
		public int Sort  { get; set; }

        [StringLength(Regions.PathMaxLength)]
		public string Path  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Regions.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }

        [StringLength(Regions.IsLockMaxLength)]
		public string IsLock  { get; set; }
    }
}

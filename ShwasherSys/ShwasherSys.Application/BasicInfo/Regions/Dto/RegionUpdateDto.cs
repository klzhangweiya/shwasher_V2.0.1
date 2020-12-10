using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Region.Dto
{
    [AutoMapTo(typeof(Regions))]
    public class RegionUpdateDto: EntityDto<string>
    {
        [Required] 
       
		public string RegionName  { get; set; }
        [Required] 
        
		public string FatherRegionID  { get; set; }
    
     
		public int Sort  { get; set; }
   
  
	
    }
}
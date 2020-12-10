using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Standard))]
    public class StandardUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(Standard.StandardNameMaxLength)]
		public string StandardName  { get; set; }
        [StringLength(Standard.StandardDescMaxLength)]
		public string StandardDesc  { get; set; }
		
    }
}
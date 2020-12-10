using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.ScrapTypeInfo.Dto
{
    
    /// <summary>
    /// 报废类型维护
    /// </summary>   
    [AutoMapTo(typeof(ScrapType))]
    public class ScrapTypeUpdateDto: EntityDto<string>
    {
        
        /// <summary>
        /// 类型名称
        /// </summary>   
        [Required] 
        [StringLength(ScrapType.NameMaxLength)]
		public string Name  { get; set; }
        
        /// <summary>
        /// 类型描述
        /// </summary>   
        [StringLength(ScrapType.DescMaxLength)]
		public string Description  { get; set; }
    }
}
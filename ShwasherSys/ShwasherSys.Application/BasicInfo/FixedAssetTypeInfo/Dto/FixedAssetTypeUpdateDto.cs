using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.FixedAssetTypeInfo.Dto
{
    
    /// <summary>
    /// 固定资产类型
    /// </summary>   
    [AutoMapTo(typeof(FixedAssetType))]
    public class FixedAssetTypeUpdateDto: EntityDto<string>
    {
        
        /// <summary>
        /// 类型名称
        /// </summary>   
        [Required] 
        [StringLength(FixedAssetType.NameMaxLength)]
		public string Name  { get; set; }
        
        /// <summary>
        /// 类型描述
        /// </summary>   
        [StringLength(FixedAssetType.DescMaxLength)]
		public string Description  { get; set; }
        [StringLength(FixedAssetType.RemarkMaxLength)]
		public string Remark  { get; set; }
    }
}
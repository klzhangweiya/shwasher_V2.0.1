using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BasicInfo.FixedAssetTypeInfo.Dto
{
    
    /// <summary>
    /// 固定资产类型
    /// </summary>   
    [AutoMapTo(typeof(FixedAssetType))]
    public class FixedAssetTypeCreateDto
    {
		
        public string Id { get; set; }
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

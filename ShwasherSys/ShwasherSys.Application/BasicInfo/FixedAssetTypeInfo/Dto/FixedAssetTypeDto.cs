using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BasicInfo.FixedAssetTypeInfo.Dto
{
    
    /// <summary>
    /// 固定资产类型
    /// </summary>   
    [AutoMapTo(typeof(FixedAssetType)),AutoMapFrom(typeof(FixedAssetType))]
    public class FixedAssetTypeDto: EntityDto<string>
    {
        /// <summary>
        /// 类型名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>   
		public string Description  { get; set; }
		public string Remark  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BasicInfo.ScrapTypeInfo.Dto
{
    
    /// <summary>
    /// 报废类型维护
    /// </summary>   
    [AutoMapTo(typeof(ScrapType)),AutoMapFrom(typeof(ScrapType))]
    public class ScrapTypeDto: EntityDto<string>
    {
        /// <summary>
        /// 类型名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>   
		public string Description  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.BasicInfo.QualityIssueLabelInfo.Dto
{
    
    /// <summary>
    /// 质量问题标签维护
    /// </summary>   
    [AutoMapTo(typeof(QualityIssueLabel)),AutoMapFrom(typeof(QualityIssueLabel))]
    public class QualityIssueLabelDto: EntityDto<string>
    {
        /// <summary>
        /// 标签名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 标签描述
        /// </summary>   
		public string Description  { get; set; }
    }
}
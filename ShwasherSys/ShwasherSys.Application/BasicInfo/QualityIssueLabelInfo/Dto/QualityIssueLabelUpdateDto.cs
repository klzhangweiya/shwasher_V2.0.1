using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.QualityIssueLabelInfo.Dto
{
    
    /// <summary>
    /// 质量问题标签维护
    /// </summary>   
    [AutoMapTo(typeof(QualityIssueLabel))]
    public class QualityIssueLabelUpdateDto: EntityDto<string>
    {
        
        /// <summary>
        /// 标签名称
        /// </summary>   
        [Required] 
        [StringLength(QualityIssueLabel.NameMaxLength)]
		public string Name  { get; set; }
        
        /// <summary>
        /// 标签描述
        /// </summary>   
        [StringLength(QualityIssueLabel.DescMaxLength)]
		public string Description  { get; set; }
    }
}
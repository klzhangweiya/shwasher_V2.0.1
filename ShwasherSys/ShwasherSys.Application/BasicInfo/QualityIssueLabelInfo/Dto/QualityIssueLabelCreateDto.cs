using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.BasicInfo.QualityIssueLabelInfo.Dto
{
    
    /// <summary>
    /// 质量问题标签维护
    /// </summary>   
    [AutoMapTo(typeof(QualityIssueLabel))]
    public class QualityIssueLabelCreateDto
    {
		
        public string Id { get; set; }
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

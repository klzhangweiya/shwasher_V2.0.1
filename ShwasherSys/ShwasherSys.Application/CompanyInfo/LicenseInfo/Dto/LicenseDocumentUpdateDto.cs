using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;

namespace ShwasherSys.CompanyInfo.LicenseInfo.Dto
{
    
    /// <summary>
    /// 证照信息维护
    /// </summary>   
    [AutoMapTo(typeof(LicenseDocument))]
    public class LicenseDocumentUpdateDto: EntityDto<int>
    {
        
        /// <summary>
        /// 证照编码
        /// </summary>   
        [StringLength(LicenseDocument.NoMaxLength)]
		public string No  { get; set; }
        
        /// <summary>
        /// 证照名称
        /// </summary>   
        [StringLength(LicenseDocument.NameMaxLength)]
		public string Name  { get; set; }
        
        /// <summary>
        /// 证照描述
        /// </summary>   
        [StringLength(LicenseDocument.DescMaxLength)]
		public string Description  { get; set; }
        
        /// <summary>
        /// 证照组
        /// </summary>   
        [StringLength(LicenseDocument.TypeMaxLength)]
		public string LicenseGroup  { get; set; }
        
        /// <summary>
        /// 证照类型
        /// </summary>   
        [StringLength(LicenseDocument.TypeMaxLength)]
		public string LicenseType  { get; set; }
        
        /// <summary>
        /// 附件路径
        /// </summary>   
        [StringLength(LicenseDocument.PathMaxLength)]
		public string FilePath  { get; set; }
        
        /// <summary>
        /// 过期时间
        /// </summary>   
		public DateTime ExpireDate  { get; set; }
        [IgnoreMap]
        public List<SysAttachFileCreateDto> AttachFiles { get; set; }

    }
}
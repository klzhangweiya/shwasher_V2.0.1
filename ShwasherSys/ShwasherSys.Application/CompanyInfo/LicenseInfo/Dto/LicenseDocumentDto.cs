using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.LicenseInfo.Dto
{
    
    /// <summary>
    /// 证照信息维护
    /// </summary>   
    [AutoMapTo(typeof(LicenseDocument)),AutoMapFrom(typeof(LicenseDocument))]
    public class LicenseDocumentDto: EntityDto<int>
    {
        /// <summary>
        /// 证照编码
        /// </summary>   
		public string No  { get; set; }
        /// <summary>
        /// 证照名称
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 证照描述
        /// </summary>   
		public string Description  { get; set; }
        /// <summary>
        /// 证照组
        /// </summary>   
		public string LicenseGroup  { get; set; }
        /// <summary>
        /// 证照类型
        /// </summary>   
		public string LicenseType  { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>   
		public string FilePath  { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>   
		public DateTime ExpireDate  { get; set; }
    }
}
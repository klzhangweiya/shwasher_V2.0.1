using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.LicenseInfo.Dto
{
    
    /// <summary>
    /// 证照类型
    /// </summary>   
    [AutoMapTo(typeof(LicenseType)),AutoMapFrom(typeof(LicenseType))]
    public class LicenseTypeDto: EntityDto<int>
    {
        /// <summary>
        /// 证照类型
        /// </summary>   
		public string Name  { get; set; }
        /// <summary>
        /// 证照组名称
        /// </summary>   
		public string GroupName  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CompanyInfo.LicenseInfo.Dto
{
    
    /// <summary>
    /// 证照类型
    /// </summary>   
    [AutoMapTo(typeof(LicenseType))]
    public class LicenseTypeCreateDto:IwbEntityDto<int>
    {
		
        /// <summary>
        /// 证照类型
        /// </summary>   
        [StringLength(LicenseType.NameMaxLength)]
		public string Name  { get; set; }
        /// <summary>
        /// 证照组名称
        /// </summary>   
        [StringLength(LicenseType.NameMaxLength)]
		public string GroupName  { get; set; }
    }
}

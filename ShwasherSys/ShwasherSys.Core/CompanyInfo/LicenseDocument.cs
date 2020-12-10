using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    /// <summary>
    /// 证照信息维护
    /// </summary>
    [Table("LicenseDocumentInfo")]
    public class LicenseDocument:FullAuditedEntity<int,SysUser>
    {
        public const int NoMaxLength = 50;
        public const int NameMaxLength = 50;
        public const int DescMaxLength = 500;
        public const int TypeMaxLength = CompanyInfo.LicenseType.NameMaxLength;
        public const int PathMaxLength = 300;
        
        /// <summary>
        /// 证照编码
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string No { get; set; }
        /// <summary>
        /// 证照名称
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 证照描述
        /// </summary>
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 证照组
        /// </summary>
        [MaxLength(TypeMaxLength)]
        public string LicenseGroup { get; set; }
        /// <summary>
        /// 证照类型
        /// </summary>
        [MaxLength(TypeMaxLength)]
        public string LicenseType { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        [MaxLength(PathMaxLength)]
        public string FilePath { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; } 
        
        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
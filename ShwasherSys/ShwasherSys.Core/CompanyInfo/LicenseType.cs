using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    /// <summary>
    /// 证照类型
    /// </summary>
    [Table("LicenseTypeInfo")]
    public class LicenseType : AuditedEntity<int, SysUser>
    {
        public const int NameMaxLength = 50;
        /// <summary>
        /// 证照类型
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 证照组名称
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string GroupName { get; set; }
    }
}
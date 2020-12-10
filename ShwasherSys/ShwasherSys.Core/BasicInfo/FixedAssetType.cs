using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.BasicInfo
{
    [Table("FixedAssetTypeInfo")]
    public class FixedAssetType : FullAuditedEntity<string, SysUser>
    {
        public const int NameMaxLength = 50;
        public const int DescMaxLength = 500;
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        public string  Name { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        [MaxLength(DescMaxLength)]
        public string  Description { get; set; }

        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
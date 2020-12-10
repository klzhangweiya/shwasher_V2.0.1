using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using IwbZero.Authorization.Users;

namespace IwbZero.BaseSysInfo
{
    [Table("Sys_Settings")]
    public class IwbSysSetting<TUser> : FullAuditedEntity<int>, IFullAudited<TUser>
        where TUser : UserBase
    {
        public const int SettingNoMaxLength = 32;
        public const int SettingNameMaxLength = 50;
        public const int CodeMaxLength = 100;
        public const int ValueMaxLength = 500;
        public const int DesckMaxLength = 1000;
        public const int RemarkMaxLength = 1000;

        [StringLength(SettingNoMaxLength)]
        public string SettingNo { get; set; }
        [StringLength(SettingNameMaxLength)]
        public string SettingName { get; set; }
        public int SettingType { get; set; }
        [StringLength(CodeMaxLength)]
        public string Code { get; set; }
        [StringLength(ValueMaxLength)]
        public string Value { get; set; }
        [StringLength(DesckMaxLength)]
        public string Description { get; set; }
        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public TUser CreatorUser { get; set; }
        public TUser LastModifierUser { get; set; }
        public TUser DeleterUser { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using IwbZero.Authorization.Users;

namespace IwbZero.BaseSysInfo
{
    [Table("Sys_States")]
    public class IwbSysState<TUser> : FullAuditedEntity<int>, IFullAudited<TUser>
        where TUser : IwbSysUser<TUser>
    {
        public const int StateNoMaxLength = 32;
        public const int StateNameMaxLength = 50;
        public const int TableNameMaxLength = 50;
        public const int ColNameMaxLength = 50;
        public const int CodeValueMaxLength = 100;
        public const int DisplayValueMaxLength = 100;

        [StringLength(StateNoMaxLength)]
        public string StateNo { get; set; }
        [StringLength(StateNameMaxLength)]
        public string StateName { get; set; }
        [Required]
        [StringLength(TableNameMaxLength)]
        public string TableName { get; set; }
        [Required]
        [StringLength(ColNameMaxLength)]
        public string ColumnName { get; set; }
        [Required]
        [StringLength(CodeValueMaxLength)]
        public string CodeValue { get; set; }
        [Required]
        [StringLength(DisplayValueMaxLength)]
        public string DisplayValue { get; set; }
        public TUser CreatorUser { get; set; }
        public TUser LastModifierUser { get; set; }
        public TUser DeleterUser { get; set; }
    }
}

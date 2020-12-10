using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using IwbZero.Authorization.Users;

namespace IwbZero.BaseSysInfo
{
    [Table("Sys_Functions")]
    public class IwbSysFunction<TUser> : FullAuditedEntity<int>, IFullAudited<TUser>
        where TUser : IwbSysUser<TUser>
    {
        public const int FunctionNoMaxLength = 100;
        public const int FunctionNameMaxLength = 100;
        public const int PermissionNameMaxLength = 500;
        public const int FunctionPathMaxLength = 500;
        public const int ActionMaxLength = 50;
        public const int ControllerMaxLength = 50;
        public const int IconMaxLength = 20;
        public const int ClassMaxLength = 100;

        [MaxLength(FunctionNoMaxLength)]
        public string FunctionNo { get; set; }
        [MaxLength(FunctionNoMaxLength)]
        public string ParentNo { get; set; }
        [MaxLength(FunctionNameMaxLength)]
        public string FunctionName { get; set; }
        [MaxLength(PermissionNameMaxLength)]
        public string PermissionName { get; set; }
        public int FunctionType { get; set; }
        [MaxLength(FunctionPathMaxLength)]
        public string FunctionPath { get; set; }
        [StringLength(ActionMaxLength)]
        public string Action { get; set; }
        [StringLength(ControllerMaxLength)]
        public string Controller { get; set; }
        public string Url { get; set; }
        [StringLength(IconMaxLength)]
        public string Icon { get; set; }
        [StringLength(ClassMaxLength)]
        public string Class { get; set; }
        public string Script { get; set; }
        public int Sort { get; set; }
        public int Depth { get; set; }
        public bool? IsLeaf { get; set; }

        public TUser CreatorUser { get; set; }
        public TUser LastModifierUser { get; set; }
        public TUser DeleterUser { get; set; }
    }
}

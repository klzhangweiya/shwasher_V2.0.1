using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace IwbZero.Authorization.Permissions
{
    [Table("Sys_Permissions")]
    public class SysPermission : CreationAuditedEntity<long>
    {
        public const int PermissionNoMaxLength = 32;
        public const int PermissionNameMaxLength = 500;

        [StringLength(PermissionNoMaxLength)]
        public string PermissionNo { get; set; }
        [StringLength(PermissionNameMaxLength)]
        public string PermissionName { get; set; }

        public int? Master { get; set; }

        public string MasterValue { get; set; }

        public int? Access { get; set; }

        public string AccessValue { get; set; }

        public bool IsGranted { get; set; }

    }
}

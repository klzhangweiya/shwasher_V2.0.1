using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using ShwasherSys.Authorization.Roles;
using IwbZero.Authorization.Roles;

namespace ShwasherSys.BaseSysInfo.Roles.Dto
{
    [AutoMapTo(typeof(SysRole))]
    public class RoleCreateDto
    {
        [Required]
        [StringLength(RoleBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(RoleBase.MaxDisplayNameLength)]
        public string RoleDisplayName { get; set; }

        [StringLength(RoleBase.MaxDescriptionLength)]
        public string Description { get; set; }
        public int RoleType { get; set; }

        //public virtual bool IsStatic { get; set; }

        //public virtual bool IsDefault { get; set; }
    }
}

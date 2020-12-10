using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.Authorization.Roles;
using IwbZero.Authorization.Roles;

namespace ShwasherSys.BaseSysInfo.Roles.Dto
{
    [AutoMapTo(typeof(SysRole))]
    public class RoleUpdateDto : EntityDto<int>
    {
        [Required]
        [StringLength(RoleBase.MaxNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(RoleBase.MaxDisplayNameLength)]
        public virtual string RoleDisplayName { get; set; }

        [StringLength(RoleBase.MaxDescriptionLength)]
        public string Description { get; set; }
        public int RoleType { get; set; }
    }
}

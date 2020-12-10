using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.Authorization.Roles;

namespace ShwasherSys.BaseSysInfo.Roles.Dto
{
    [AutoMapTo(typeof(SysRole)), AutoMapFrom(typeof(SysRole))]
    public class RoleDto : EntityDto<int>
    {
        public string Name { get; set; }
        public int RoleType { get; set; }
        public string Description { get; set; }
        public string RoleDisplayName { get; set; }
        public bool IsStatic { get; set; }
    }
    public class RoleDtoModel : EntityDto<int>
    {
        public string Name { get; set; }
        public int RoleType { get; set; }
        public string RoleDisplayName { get; set; }
        public string Description { get; set; }
        public string RoleTypeName { get; set; }
        public bool IsStatic { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public List<string> Permissions { get; set; }
        public string LastModifierUserName { get; set; }
    }
}

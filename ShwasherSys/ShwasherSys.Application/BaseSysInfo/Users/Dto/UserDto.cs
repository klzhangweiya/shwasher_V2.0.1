using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.BaseSysInfo.Users.Dto
{
    [AutoMapTo(typeof(SysUser)), AutoMapFrom(typeof(SysUser))]
    public class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public int UserType { get; set; }
        public string EmailAddress { get; set; }
        public string RealName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string[] RoleNames { get; set; }

        public string FactoryID { get; set; }

        public string DepartmentID { get; set; }

        public int? DutyID { get; set; }
    }
    public class UserDtoModel : EntityDto<long>
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string EmailAddress { get; set; }
        public int UserType { get; set; }
        public string UserTypeName { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string[] RoleNames { get; set; }
        public string LastModifierUserName { get; set; }
        public string FactoryID { get; set; }

        public string DepartmentID { get; set; }

        public int? DutyID { get; set; }
    }
}

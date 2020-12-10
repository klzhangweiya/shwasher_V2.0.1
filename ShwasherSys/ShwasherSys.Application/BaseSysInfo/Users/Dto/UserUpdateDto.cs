using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.Authorization.Users;
using IwbZero.Authorization.Users;

namespace ShwasherSys.BaseSysInfo.Users.Dto
{
    [AutoMapTo(typeof(SysUser))]
    public class UserUpdateDto : EntityDto<long>
    {
        [Required]
        [StringLength(UserBase.MaxUserNameLength)]
        public string UserName { get; set; }
        public int UserType { get; set; }
        [Required]
        [StringLength(UserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(UserBase.MaxNameLength)]
        public string RealName { get; set; }
        [StringLength(UserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string RoleNames { get; set; }

        public string FactoryID { get; set; }

        public string DepartmentID { get; set; }

        public int? DutyID { get; set; }
    }
}

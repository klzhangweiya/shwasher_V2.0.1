using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using IwbZero.Authorization.Users;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    /// <summary>
    /// 人员信息维护
    /// </summary>
    [Table("EmployeeInfo")]
    public class Employee:FullAuditedEntity<int,SysUser>
    {
        public const int NoMaxLength = 20;
        public const int NameMaxLength = 50;
        public const int CardIdMaxLength = 18;
        public const int PhoneMaxLength = 18;
        public const int DescMaxLength = 500;

        /// <summary>
        /// 人员编号
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string No { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [MaxLength(CardIdMaxLength)]
        public string CardId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(PhoneMaxLength)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [MaxLength(20)]
        public string DepartmentNo { get; set; }
        /// <summary>
        /// 职务
        /// </summary>

        [MaxLength(20)]
        public string DutyNo { get; set; }

       /// <summary>
       /// 详情
       /// </summary>
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }

       /// <summary>
       /// 登陆账号
       /// </summary>
        [MaxLength(UserBase.MaxNameLength)]
        public string UserName { get; set; }


        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }

    [Table("NV_ViewEmployeeInfo")]
    public class ViewEmployee:Entity
    {
       
        public string No { get; set; }
        public string Name { get; set; }
        public string CardId { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string DepartmentNo { get; set; }
        public string DepartmentName { get; set; }
        public string DutyNo { get; set; }
        public string DutyName { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
    }
}
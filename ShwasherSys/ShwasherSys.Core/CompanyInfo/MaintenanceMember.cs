using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    [Table("MaintenanceMemberInfo")]
    public class MaintenanceMember : CreationAuditedEntity<string, SysUser>
    {
        /// <summary>
        /// 维修记录编码
        /// </summary>
        public string MaintenanceNo{ get; set; }
        /// <summary>
        /// 维护员工
        /// </summary>
        public int EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee EmployeeInfo { get; set; }
        /// <summary>
        /// 维修人员
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 工时
        /// </summary>
        public decimal WorkHour { get; set; }
        public bool IsConfirm { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string WorkDesc { get; set; }
        
        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
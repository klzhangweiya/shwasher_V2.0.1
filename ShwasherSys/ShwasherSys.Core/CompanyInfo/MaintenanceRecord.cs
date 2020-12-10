using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    /// <summary>
    /// 机模维护记录
    /// </summary>
    [Table("MaintenanceRecordInfo")]
    public class MaintenanceRecord: CreationAuditedEntity<string, SysUser>
    {
        public const int DeviceNoMaxLength = 50;
        public const int DeviceTypeMaxLength = 50;
        public const int DescMaxLength = 500;
        public const int AddressMaxLength = 200;
        /// <summary>
        /// 计划编号
        /// </summary>
        [MaxLength(DeviceNoMaxLength)]
        public string DeviceMgPlanNo { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        [MaxLength(DeviceNoMaxLength)]
        public string DeviceNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(DeviceNoMaxLength)]
        public string DeviceName { get; set; }
        /// <summary>
        /// 维护类型
        /// </summary>
        public int MgType { get; set; }
        /// <summary>
        /// 维护内容
        /// </summary>
        
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 维护地点
        /// </summary>

        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }
        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanDate { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public int CompleteState { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteDate { get; set; }
        
        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
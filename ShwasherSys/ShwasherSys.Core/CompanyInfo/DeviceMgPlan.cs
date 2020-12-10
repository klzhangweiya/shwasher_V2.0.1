using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;

namespace ShwasherSys.CompanyInfo
{
    [Table("DeviceMgPlanInfo")]
    public class DeviceMgPlan : FullAuditedEntity<int, SysUser>
    {
        public const int NoMaxLength = 50;
        public const int NameMaxLength = 50;
        public const int DescMaxLength = 5000;
        public const int ModelMaxLength = 50;
        public const int MaterialMaxLength = 50;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        /// <summary>
        /// 计划编号
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string No { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public int PlanType { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        [MaxLength(NoMaxLength)]
        public string DeviceNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 维护内容
        /// </summary>
        [MaxLength(DescMaxLength)]
        public string Description { get; set; }

        /// <summary>
        /// 有效期限
        /// </summary>
        public DateTime ExpireDate { get; set; } 
        /// <summary>
        /// 维护周期
        /// </summary>
        public int MaintenanceCycle { get; set; } 
        /// <summary>
        /// 维护时间
        /// </summary>
        public DateTime MaintenanceDate { get; set; } 
        /// <summary>
        /// 下一次维护时间
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; } 

        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
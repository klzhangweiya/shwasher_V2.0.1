using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.CompanyInfo
{
    [Table("EmployeeWorkPerformanceInfo")]
    public class EmployeeWorkPerformance : CreationAuditedEntity<int,SysUser>
    {
        public const int PerformanceMaxLength = 20;
        public const int RelatedNoMaxLength = 20;
        public const int PerformanceUnitMaxLength = 10;
        public const int PerformanceDescMaxLength = 500;

        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(PerformanceMaxLength)]
        public string PerformanceNo { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public int EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee EmployeeInfo { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        [MaxLength(RelatedNoMaxLength)]
        public string RelatedNo { get; set; }

        /// <summary>
        /// 排产单号
        /// </summary>
        [MaxLength(ProductionOrder.ProductionOrderNoMaxLength)]
        public string ProductOrderNo { get; set; }
        /// <summary>
        /// 工作类型
        /// </summary>
        public int WorkType { get; set; }
        /// <summary>
        /// 绩效量化
        /// </summary>
        [DecimalPrecision]
        public decimal? Performance { get; set; }
        /// <summary>
        /// 量化单位
        /// </summary>
        [MaxLength(PerformanceUnitMaxLength)]
        public string PerformanceUnit { get; set; }
        /// <summary>
        /// 绩效描述
        /// </summary>
        [MaxLength(PerformanceDescMaxLength)]
        public string PerformanceDesc { get; set; }

        public const int RemarkMaxLength = 500;
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }
    }
}
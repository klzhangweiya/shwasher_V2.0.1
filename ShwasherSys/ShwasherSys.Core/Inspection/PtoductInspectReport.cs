using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.Inspection
{
    [Table("ProductInspectReportInfos")]
    public class ProductInspectReport : CreationAuditedEntity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int ProductInspectNoMaxLength = 32;
        public const int InspectMemberMaxLength = 100;
        public const int InspectContentMaxLength = 1000;
        public const int IsLockMaxLength = 1;
        
        /// <summary>
        /// 检验报告编号
        /// </summary>
        [Required]
        [StringLength(ProductInspectNoMaxLength)]
        public string ProductInspectReportNo { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>
        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }

        /// <summary>
        /// 确认状态（1：未确认 2：最终确认）
        /// </summary>
        public int ConfirmStatus { get; set; }

        /// <summary>
        /// 检验次数
        /// </summary>
        public int InspectCount { get; set; }
       
        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 确认人员
        /// </summary>
        [StringLength(InspectMemberMaxLength)]
        public string ConfirmUser { get; set; }

        /// <summary>
        /// 检验详情
        /// </summary>
        [StringLength(InspectContentMaxLength)]
        public string InspectContent { get; set; }

    }
}

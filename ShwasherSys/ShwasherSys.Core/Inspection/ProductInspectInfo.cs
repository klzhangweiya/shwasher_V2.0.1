using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.Inspection
{
    [Table("ProductInspectInfos")]
    public class ProductInspectInfo:Entity
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int ProductInspectNoMaxLength = 32;
        public const int InspectSubjectMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
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
        /// 检验编号
        /// </summary>
        [Required]
        [StringLength(ProductInspectNoMaxLength)]
        public string ProductInspectNo { get; set; }
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
        /// 检验状态（0：未检验 1：已检验）
        /// </summary>
        public int InspectStatus { get; set; }

        /// <summary>
        /// 检测项目
        /// </summary>
        [StringLength(InspectSubjectMaxLength)]
        public string InspectSubject  { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public int InspectResult { get; set; }
       
        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectDate { get; set; }

        /// <summary>
        /// 检验人员
        /// </summary>
        [StringLength(InspectMemberMaxLength)]
        public string InspectMember { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        /// <summary>
        /// 检验详情
        /// </summary>
        [StringLength(InspectContentMaxLength)]
        public string InspectContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TimeCreated { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
    }
}

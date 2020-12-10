using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.Inspection
{
    [Table("ProductInspectReportContents")]
    public class ProductInspectReportContent : Entity<int>
    {
        public const int ProductInspectNoMaxLength = 32;
        public const int ReportContentNoMaxLength = int.MaxValue;

        /// <summary>
        /// 排产单号
        /// </summary>
        [StringLength(ProductInspectNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 检验报告编码
        /// </summary>
        [StringLength(ProductInspectNoMaxLength)]
        public string PtoductInspectNo { get; set; }
        /// <summary>
        /// 检验报告
        /// </summary>
        [StringLength(ReportContentNoMaxLength)]
        public string ReportContent { get; set; }
       
    }
}
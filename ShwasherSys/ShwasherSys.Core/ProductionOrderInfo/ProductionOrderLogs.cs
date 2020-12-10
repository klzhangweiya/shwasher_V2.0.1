using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;
using ShwasherSys.CompanyInfo;

namespace ShwasherSys.ProductionOrderInfo
{
    [Table("ProductionOrderLogInfo")]
    public class ProductionLog:CreationAuditedEntity<int,SysUser>
    {
        public const int ProductionNoMaxLength = 20;
        public const int CarNoMaxLength = 20;

        [MaxLength(ProductionNoMaxLength)]
        public string ProductionNo { get; set; }
        /// <summary>
        /// 流转单号
        /// </summary>
        [MaxLength(ProductionOrder.ProductionOrderNoMaxLength)]
        public string ProductOrderNo { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public int EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee EmployeeInfo { get; set; }
        /// <summary>
        /// 车号
        /// </summary>
        [MaxLength(CarNoMaxLength)]
        public  string CarNo { get; set; }
        /// <summary>
        /// 产品数量（Kg）
        /// </summary>
        [DecimalPrecision]
        public decimal QuantityWeight { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 产品数量（千件）
        /// </summary>
        [DecimalPrecision]
        public decimal QuantityPcs { get; set; }
    }
}

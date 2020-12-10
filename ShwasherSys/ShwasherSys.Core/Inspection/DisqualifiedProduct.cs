using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ShwasherSys.Authorization.Users;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ReturnGoods;

namespace ShwasherSys.Inspection
{
    [Table("DisqualifiedProductInfo")]
    public class DisqualifiedProduct:CreationAuditedEntity<int,SysUser>
    {
        public const int DisqualifiedNoMaxLength = 20;

        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(DisqualifiedNoMaxLength)]
        public string DisqualifiedNo { get; set; }
        /// <summary>
        /// 流转单号
        /// </summary>
        [MaxLength(ProductionOrder.ProductionOrderNoMaxLength)]
        public string ProductOrderNo { get; set; }
        /// <summary>
        /// 退货单号
        /// </summary>
        [MaxLength(ReturnGoodOrder.ReturnOrderNoMaxLength)]
        public string ReturnOrderNo { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [MaxLength(50)]
        public string ProductNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [MaxLength(100)]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public int ProductType { get; set; } //1.半成品 2.成品
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
        
        /// <summary>
        /// 处理类型
        /// </summary>
        public int HandleType { get; set; } //1.降级 2.报废 3.否决报废后降级
       
        /// <summary>
        /// 检验人员
        /// </summary>
        [MaxLength(20)]
        public string CheckUser { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? CheckDate { get; set; }

        /// <summary>
        /// 最后处理人
        /// </summary>
        [MaxLength(20)]
        public string HandleUser { get; set; }
        /// <summary>
        /// 最后处理时间
        /// </summary>
        public DateTime? HandleDate { get; set; }

        /// <summary>
        /// 不合格类型  1:生产检验 2：退货检验
        /// </summary>
        public int? DisqualifiedType { get; set; }
    }
}
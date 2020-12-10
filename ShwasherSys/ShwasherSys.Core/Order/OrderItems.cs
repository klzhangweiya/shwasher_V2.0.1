using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.Order
{
    [Table("OrderItems")]
    public class OrderItem:Entity<int>
    {
        public const int OrderNoMaxLength = 30;
        public const int ProductNoMaxLength = 30;
        public const int CurrencyIdMaxLength = 10;
        public const int IsReportMaxLength = 1;
        public const int IsPartSendMaxLength = 1;
        public const int WareHouseMaxLength = 50;
        public const int OrderItemDescMaxLength = 500;
        public const int PartNoMaxLength = 30;
        public const int UserIDLastModMaxLength = 20;
        public const int IsLockMaxLength = 1;

        public const int FromCurrenyIdMaxLength = 20;

        [Required]
        [StringLength(OrderNoMaxLength)]
        public string OrderNo { get; set; }

        [Required]
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal AfterTaxPrice { get; set; }

        [Required]
        [StringLength(CurrencyIdMaxLength)]
        public string CurrencyId { get; set; }

        [Column(TypeName = "numeric")]
        [DecimalPrecision]
        public decimal Quantity { get; set; }

        public int OrderUnitId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime SendDate { get; set; }

        [Required]
        [StringLength(IsReportMaxLength)]
        public string IsReport { get; set; }

        [Required]
        [StringLength(IsPartSendMaxLength)]
        public string IsPartSend { get; set; }

        public int? OrderItemStatusId { get; set; }

        [StringLength(WareHouseMaxLength)]
        public string WareHouse { get; set; }

        [StringLength(OrderItemDescMaxLength)]
        public string OrderItemDesc { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [StringLength(PartNoMaxLength)]
        public string PartNo { get; set; }

        [DecimalPrecision]
        public decimal? ToCnyRate { get; set; }
      
        //[DecimalPrecision]
        //public decimal? CurrencyPrice { get; set; }
        /// <summary>
        /// 订单明细紧急程度
        /// </summary>
        public int EmergencyLevel { get; set; }
        
        //是否删除
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

    }
    [Table("OrderSendExceed")]
    public class OrderSendExceed : AuditedEntity<int>
    {

        public const int OperatorManMaxLength = 30;

        [Required]
        public int OrderItemId { get; set; }

       

        [Column(TypeName = "numeric")]
        [DecimalPrecision]
        public decimal ExceedQuantity { get; set; }

        [StringLength(OperatorManMaxLength)]
        public string OperatorMan { get; set; }
        public string OrderNo { get; set; }

        public string ProductNo { get; set; }

  

    }
}

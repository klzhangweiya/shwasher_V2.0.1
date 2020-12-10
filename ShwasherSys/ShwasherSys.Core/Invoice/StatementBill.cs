using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.Invoice
{
    [Table("StatementBill")]
    public class StatementBill:AuditedEntity<int>
    {

        public const int CustomerIdMaxLength = 30;
        public const int StatementBillNoMaxLength = 30;
        public const int OrderStickBillNoMaxLength = 30;
        public const int BillManMaxLength = 20;
        public const int DescriptionMaxLength = 4000;
        [Required]
        [StringLength(StatementBillNoMaxLength)]
        [Index(IsUnique = true)]
        public string StatementBillNo { get; set; }

        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }
        //对账单开票人
        [StringLength(BillManMaxLength)]
        public string BillMan { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 1:已开票 0:未开票
        /// </summary>
        public int? StatementState { get; set; }

        [StringLength(OrderStickBillNoMaxLength)]
        public string OrderStickBillNo { get; set; }
    }
    [Table("N_ViewStatementBill")]
    public class ViewStatementBill : AuditedEntity<int>
    {

        public const int CustomerIdMaxLength = 30;
        public const int StatementBillNoMaxLength = 30;
        public const int BillManMaxLength = 20;
        public const int DescriptionMaxLength = 4000;
        [Required]
        [StringLength(StatementBillNoMaxLength)]
        [Index(IsUnique = true)]
        public string StatementBillNo { get; set; }

        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }
        //对账单开票人
        [StringLength(BillManMaxLength)]
        public string BillMan { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 1:已开票 0:未开票
        /// </summary>
        public int? StatementState { get; set; }

        public string   CustomerName{ get; set; }
        //[StringLength(OrderStickBillNoMaxLength)]
        public string OrderStickBillNo { get; set; }
        [DecimalPrecision()]
        public decimal? TotalPrice { get; set; }
        [DecimalPrecision()]
        public decimal? AfterTaxTotalPrice { get; set; }

        public string CurrencyId { get; set; }
    }
}

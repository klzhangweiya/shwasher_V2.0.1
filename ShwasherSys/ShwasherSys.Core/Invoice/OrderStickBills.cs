using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Invoice
{
    [Table("OrderStickBills")]
    public class OrderStickBill:Entity<string>
    {
        public const int CustomerIdMaxLength = 30;
        public const int StickNumMaxLength = 30;
        public const int ReturnOrderNoMaxLength = 30;
        public const int OrderNoMaxLength = 500;
        public const int StickManMaxLength = 20;
        public const int DescriptionMaxLength = 4000;
        public const int UserIDLastModMaxLength = 20;
        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreatDate { get; set; }

        [StringLength(StickNumMaxLength)]
        public string StickNum { get; set; }

        [StringLength(StickManMaxLength)]
        public string StickMan { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
//开票状态（1:未开票 2：已开票）
        public int? InvoiceState { get; set; }
        //金额
        public decimal? Amount { get; set; }
        [StringLength(StickNumMaxLength)]
        public string OriginalStickNum { get; set; }
        [MaxLength(ReturnOrderNoMaxLength)]
        public string ReturnOrderNo { get; set; }
        [MaxLength(OrderNoMaxLength)]
        public string OrderNo { get; set; }
        public int InvoiceType { get; set; }

    }
}

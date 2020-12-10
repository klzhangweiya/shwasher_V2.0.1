using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.Invoice
{
    [Table("RedStampInvoiceInfo")]
    public class RedStampInvoice:FullAuditedEntity<int>
    {
        public const int NoMaxLength = 32;
        public const int InvoiceNoMaxLength = 50;
        [MaxLength(NoMaxLength)]
        public string No { get; set; }
        [MaxLength(InvoiceNoMaxLength)]
        public string InvoiceNo { get; set; }
        [MaxLength(InvoiceNoMaxLength)]
        public string OriginalInvoiceNo { get; set; }
        [MaxLength(InvoiceNoMaxLength)]
        public string ReturnOrderNo { get; set; }
        [MaxLength(InvoiceNoMaxLength)]
        public string OrderNo { get; set; }
        [MaxLength(InvoiceNoMaxLength)]
        public string InvoiceMan { get; set; }
        public DateTime InvoiceDate{ get; set; }

    }
}
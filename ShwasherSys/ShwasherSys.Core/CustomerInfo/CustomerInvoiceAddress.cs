using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.CustomerInfo
{
    [Table("CustomerInvoiceAddress")]
    public partial class CustomerInvoiceAddress:FullAuditedEntity<int>
    {
        //public int CustomerSendId { get; set; }
        public const int CustomerIdMaxLength = 30;
        public const int CustomerSendNameMaxLength = 150;
        public const int InvoiceAddressMaxLength = 250;
        public const int LinkManMaxLength = 30;
        public const int TelephoneMaxLength = 50;
        public const int ZipMaxLength = 8;
        public const int EmailMaxLength = 50;
        public const int MobileMaxLength = 50;
        public const int FaxMaxLength = 50;

        public const int IsLockMaxLength = 1;
        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(CustomerSendNameMaxLength)]
        public string InvoiceAddressName { get; set; }

        [Required]
        [StringLength(InvoiceAddressMaxLength)]
        public string InvoiceAddress { get; set; }

        [StringLength(LinkManMaxLength)]
        public string LinkMan { get; set; }

        [StringLength(TelephoneMaxLength)]
        public string Telephone { get; set; }

        [StringLength(ZipMaxLength)]
        public string Zip { get; set; }

        [StringLength(EmailMaxLength)]
        public string Email { get; set; }

        [StringLength(MobileMaxLength)]
        public string Mobile { get; set; }

        [StringLength(FaxMaxLength)]
        public string Fax { get; set; }

       

     
    }
}
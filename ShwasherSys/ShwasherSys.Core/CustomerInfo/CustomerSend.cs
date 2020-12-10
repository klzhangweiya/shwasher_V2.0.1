using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace ShwasherSys.CustomerInfo
{
    

    [Table("CustomerSend")]
    public partial class CustomerSend:Entity<int>
    {
        //public int CustomerSendId { get; set; }
        public const int CustomerIdMaxLength = 30;
        public const int CustomerSendNameMaxLength = 150;
        public const int SendAdressMaxLength = 250;
        public const int LinkManMaxLength = 30;
        public const int TelephoneMaxLength = 50;
        public const int ZipMaxLength = 8;
        public const int EmailMaxLength = 50;
        public const int MobileMaxLength = 50;
        public const int FaxMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;

        public const int IsLockMaxLength = 1;
        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(CustomerSendNameMaxLength)]
        public string CustomerSendName { get; set; }

        [Required]
        [StringLength(SendAdressMaxLength)]
        public string SendAdress { get; set; }

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

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

     
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Abp.Domain.Entities;

namespace ShwasherSys.CustomerInfo
{
   
    [Table("Customers")]
    public  class Customer:Entity<string>
    {
        public const int CustomerNameMaxLength = 50;
        public const int LinkManMaxLength = 20;
        public const int AddressMaxLength = 150;
        public const int WebSiteMaxLength = 50;
        public const int TelephoneMaxLength = 50;
        public const int FaxMaxLength = 50;
        public const int ZipMaxLength = 6;
        public const int EmailMaxLength = 200;

        public const int UserIDLastModMaxLength = 20;

        public const int IsLockMaxLength = 1;
        //public string CustomerId { get; set; }

        [Required]
        [StringLength(CustomerNameMaxLength)]
        public string CustomerName { get; set; }

        [StringLength(LinkManMaxLength)]
        public string LinkMan { get; set; }

        [StringLength(AddressMaxLength)]
        public string Address { get; set; }

        [StringLength(WebSiteMaxLength)]
        public string WebSite { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        [StringLength(TelephoneMaxLength)]
        public string Telephone { get; set; }

        [StringLength(FaxMaxLength)]
        public string Fax { get; set; }

        [StringLength(ZipMaxLength)]
        [Column("zip")]
        public string Zip { get; set; }

        [StringLength(EmailMaxLength)]
        public string Email { get; set; }

        public int? SaleType { get; set; }

    }
}
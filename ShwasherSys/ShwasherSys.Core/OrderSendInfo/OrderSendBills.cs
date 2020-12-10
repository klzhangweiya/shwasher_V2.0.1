using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.OrderSendInfo
{
    [Table("OrderSendBills")]
    public class OrderSendBill:Entity<string>
    {
        public const int CustomerIdMaxLength = 30;
        public const int SendAddressMaxLength = 250;
        public const int ContactTelsMaxLength = 50;
        public const int ContactManMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;
        public const int ExpressBillNoMaxLength = 50;

        public const int IsDoBillMaxLength = 1;
        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SendDate { get; set; }

        [StringLength(SendAddressMaxLength)]
        public string SendAddress { get; set; }

        [StringLength(ContactTelsMaxLength)]
        public string ContactTels { get; set; }

        [StringLength(ContactManMaxLength)]
        public string ContactMan { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [StringLength(IsDoBillMaxLength)]
        public string IsDoBill { get; set; }


        public int? ExpressId { get; set; }
        [StringLength(ExpressBillNoMaxLength)]
        public string ExpressBillNo { get; set; }
    }
}

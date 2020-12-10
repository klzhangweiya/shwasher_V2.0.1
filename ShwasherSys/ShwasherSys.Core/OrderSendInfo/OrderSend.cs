using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.OrderSendInfo
{
    [Table("OrderSend")]
    public class OrderSend:Entity<int>
    {
        public const int RemarkMaxLength = 250;
        public const int OrderSendBillNoMaxLength = 20;
        public const int OrderStickBillNoMaxLength = 40;
        public const int UserIDLastModMaxLength = 20;
        public const int StoreLocationNoMaxLength = 32;
        public const int StatementBillNoMaxLength = 32;
        public const int CreatorUserIdMaxLength = 20;
        public int OrderItemId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SendDate { get; set; }

        [DecimalPrecision]
        public decimal SendQuantity { get; set; }

        public int? OrderUnitId { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }


        [StringLength(OrderSendBillNoMaxLength)]
        public string OrderSendBillNo { get; set; }

        [StringLength(OrderStickBillNoMaxLength)]
        public string OrderStickBillNo { get; set; }


        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [DecimalPrecision]
        public Decimal? QuantityPerPack { get; set; }
        [DecimalPrecision]
        public Decimal? PackageCount { get; set; }

        public string ProductBatchNum { get; set; }

        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
        public string CurrentProductStoreHouseNo { get; set; }

        [StringLength(StatementBillNoMaxLength)]
        public string StatementBillNo { get; set; }
    }
}

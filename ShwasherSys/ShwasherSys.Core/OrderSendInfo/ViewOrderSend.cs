using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.OrderSendInfo
{
    [Table("N_ViewOrderSends")]
    public class ViewOrderSend:Entity<int>
    {
        public int OrderItemId { get; set; }
        public DateTime? SendDate { get; set; }

        public decimal SendQuantity { get; set; }

        public string Remark { get; set; }

        public string OrderSendBillNo { get; set; }

        public string OrderStickBillNo { get; set; }
        public Decimal? QuantityPerPack { get; set; }

        public Decimal? PackageCount { get; set; }

        public string ProductBatchNum { get; set; }

        public string UserIDLastMod { get; set; }

        public string OrderNo { get; set; }

        public string ProductNo { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }
        public string IsReport { get; set; }
        public string PartNo { get; set; }

        public string ProductName { get; set; }
        public string Model { get; set; }

        public int? StandardId { get; set; }

        public string Material { get; set; }

        public string ProductDesc { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string LinkName { get; set; }

        public DateTime OrderDate { get; set; }
        public int CustomerSendId { get; set; }

        public string StockNo { get; set; }

        public string OrderUnitName { get; set; }

        public int? SaleType { get; set; }

        public decimal AfterTaxPrice { get; set; }

        public string StatementBillNo { get; set; }

    }
}

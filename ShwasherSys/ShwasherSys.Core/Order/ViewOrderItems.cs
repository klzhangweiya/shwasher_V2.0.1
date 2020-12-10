using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Order
{
    [Table("N_ViewOrderItems")]
    public class ViewOrderItems:Entity<int>
    {
        public string OrderNo { get; set; }
        public string ProductNo { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Column(TypeName = "money")]
        public decimal AfterTaxPrice { get; set; }
        public string CurrencyId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Quantity { get; set; }

        public int OrderUnitId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime SendDate { get; set; }

      
        public string IsReport { get; set; }

      
        public string IsPartSend { get; set; }

        public int? OrderItemStatusId { get; set; }
        public string WareHouse { get; set; }
        public string OrderItemDesc { get; set; }
        public string PartNo { get; set; }

        public decimal TotalPrice { get; set; }

        public string CustomerId { get; set; }
        public string LinkName { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? OrderDate { get; set; }
        public string Fax { get; set; }
        public string Telephone { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }

        public int? StandardId { get; set; }

        public string Material { get; set; }


        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string IsStandard { get; set; }

        public decimal? IsSendQuantity { get; set; }
        public string OrderUnitName { get; set; }

        public decimal? RemainingQuantity => Quantity - (IsSendQuantity??0);
        //{
        //    get
        //    {
               
        //        return Quantity - IsSendQuantity??0;
        //    }
        //}

        public string StockNo { get; set; }

        public string CustomerName { get; set; }
        public decimal AfterTaxTotalPrice { get; set; }

       
        public int? SaleType { get; set; }
        public string SaleMan { get; set; }


        //public decimal? CurrencyPrice { get; set; }

        public int EmergencyLevel { get; set; }

        public string IsLock { get; set; }

    }
}

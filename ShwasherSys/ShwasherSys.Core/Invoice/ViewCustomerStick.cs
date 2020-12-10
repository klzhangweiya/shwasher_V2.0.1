using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Invoice
{
    [Table("v_customerstick")]
    public class ViewCustomerStick:Entity<int>
    {
        public string OrderNo { get; set; }

        public string StockNo { get; set; }

        public string OrderSendBillNo { get; set; }

        public DateTime? SendDate { get; set; }

        public string PartNo { get; set; }
        public string Model { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }

        public decimal? SendQuantity { get; set; }
        public int? OrderUnitId { get; set; }
        public decimal? Price { get; set; }

        public decimal? total { get; set; }
        public string CustomerId { get; set; }
        /*public int? OrderSendId { get; set; }*/

        public string OrderUnitName { get; set; }

        public string IsDoBill { get; set; }

        public string Remark { get; set; }

        public string OrderStickBillNo { get; set; }
        public string StatementBillNo { get; set; }

        //public string IsLock { get; set; }


    }
}

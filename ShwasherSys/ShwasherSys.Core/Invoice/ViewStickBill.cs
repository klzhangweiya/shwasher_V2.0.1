using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Invoice
{
    [Table("v_OrderStickBill")]
    public class ViewStickBill : Entity<string>
    {
        public string CustomerId { get; set; }

        public DateTime? CreatDate { get; set; }

        public string StickNum { get; set; }

        public string StickMan { get; set; }

        public string Description { get; set; }

        public DateTime? TimeCreated { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string LinkMan { get; set; }
        [DecimalPrecision()]
        public decimal? TotalPrice { get; set; }
        [DecimalPrecision()]
        public decimal? AfterTaxTotalPrice { get; set; }

        public string CurrencyId { get; set; }


        //开票状态（1:未开票 2：已开票）
        public int? InvoiceState { get; set; }
        //金额
        public decimal? Amount { get; set; }

        public string OriginalStickNum { get; set; }
        public string ReturnOrderNo { get; set; }
        public string OrderNo { get; set; }
        public int InvoiceType { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.OrderSendInfo
{
   [Table("v_OrderSendBill")]
   public class ViewOrderSendBill:Entity<string>
    {
        
        public string CustomerId { get; set; }

        public DateTime? SendDate { get; set; }

        public string SendAddress { get; set; }

        public string ContactTels { get; set; }

        public string ContactMan { get; set; }

        public DateTime? TimeCreated { get; set; }

        public DateTime? TimeLastMod { get; set; }

        public string UserIDLastMod { get; set; }

        public string IsDoBill { get; set; }

        //public string StickNum { get; set; }

        //public string StickMan { get; set; }

       // public DateTime? CreatDate { get; set; }

       // public string isbill { get; set; }

       public int OrderSendCount { get; set; }
       public int StatementCount { get; set; }

        public int? ExpressId { get; set; }

        public string ExpressBillNo { get; set; }

        public string ExpressName { get; set; }

        public string CreatorUserId { get; set; }

        [DecimalPrecision()]
        public decimal? TotalPrice { get; set; }
        [DecimalPrecision()]
        public decimal? AfterTaxTotalPrice { get; set; }

        public string CurrencyId { get; set; }

    }
}

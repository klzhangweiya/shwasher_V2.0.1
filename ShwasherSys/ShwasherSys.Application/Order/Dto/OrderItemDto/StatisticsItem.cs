using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class StatisticsItem
    {
        public string ProductNo { get; set; }

        public string ProductName { get; set; }

        public int OrderCount { get; set; }

        public decimal TotalPrice { get; set; }

        public string CustomerName { get; set; }

        public string CustomerId { get; set; }

        public object QueryValue { get; set; }
        public string QueryUnit { get; set; }
        public string CurrencyId { get; set; }
    }
}

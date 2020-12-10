using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class LockOrderProductQuantity
    {
        public string ProductNo { get; set; }

        public decimal? Quantity { get; set; }
    }
}

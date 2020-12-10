using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class GetOrderItemDto
    {
        public bool IsAllSend { get; set; }

        public List<ViewOrderItems> OrderItems { get; set; }
    }
}

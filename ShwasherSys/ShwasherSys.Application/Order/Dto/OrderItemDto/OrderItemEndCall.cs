using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class OrderItemEndCall
    {
        public OrderItemEndCall()
        {
            IsAllEnd = false;
        }
        
        public OrderItem OrderItem { get; set; }

        public bool IsAllEnd { get; set; }
    }
    public class OrderItemsCallAndEnd
    {
        public OrderItemsCallAndEnd()
        {
            IsAllEnd = false;
        }

        public List<OrderItem> OrderItems { get; set; }

        public bool IsAllEnd { get; set; }
    }
}

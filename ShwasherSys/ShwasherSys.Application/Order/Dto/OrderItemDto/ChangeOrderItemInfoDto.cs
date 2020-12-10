using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class ChangeOrderItemInfoDto
    {
        public int OrderItemNo { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public decimal NewAfterTaxPrice { get; set; }

        public DateTime OldSendDate { get; set; }

        public DateTime NewSendDate { get; set; }

        public decimal OldQuantity { get; set; }

        public decimal NewQuantity { get; set; }
    }

    public class ChangeOrderItemStatusDto
    {
        public string Id { get; set; }
        public int OrderItemStatusId { get; set; }
    }
}

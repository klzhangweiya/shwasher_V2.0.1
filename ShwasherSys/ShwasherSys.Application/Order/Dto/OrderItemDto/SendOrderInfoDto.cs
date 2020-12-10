using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Order.Dto
{
    public class SendOrderInfoDto:Entity<int>
    {
        //public int OrderItemId { get; set; }
        public string ProductNo { get; set; }
        public string CustomerNo { get; set; }
        public List<OrderSendItemDto> SendItems { get; set; }

        public decimal SendAllQuantity { get; set; }

    }
    public class OrderSendItemDto
    {
     
        public string ProductBatchNum { get; set; }

        public string CurrentProductStoreHouseNo { get; set; }

        public decimal SendQuantity { get; set; }
        public decimal AvgQuantity { get; set; }

        public string StoreLocationNo { get; set; }
    }
}

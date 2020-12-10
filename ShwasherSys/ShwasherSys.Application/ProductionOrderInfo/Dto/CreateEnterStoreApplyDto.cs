using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class CreateEnterStoreApplyDto:Entity<int>
    {
        public decimal EnterStoreQuantity { get; set; }
        public decimal EnterStoreQuantity2 { get; set; }
        public decimal KgWeight { get; set; }
        public int StoreHouseId { get; set; }
        public string Remark { get; set; }
        public string CarNo { get; set; }
        public List<int> ProductUser { get; set; }
    }
}

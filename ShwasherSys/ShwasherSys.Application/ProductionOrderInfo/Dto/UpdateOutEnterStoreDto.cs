using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class UpdateOutEnterStoreDto:Entity<int>
    {
        public decimal Quantity { get; set; }

    }
}

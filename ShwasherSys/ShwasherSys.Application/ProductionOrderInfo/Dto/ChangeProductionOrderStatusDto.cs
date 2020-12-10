using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class ChangeProductionOrderStatusDto:Entity<int>
    {
        public int ProductionOrderStatus { get; set; }

    }
    public class ChangeSemiOutStoreApplyStatusDto : Entity<int>
    {
        public int ApplyStatus { get; set; }

    }
}

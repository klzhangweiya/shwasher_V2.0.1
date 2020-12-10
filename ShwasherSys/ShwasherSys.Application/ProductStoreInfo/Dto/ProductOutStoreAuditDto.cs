using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    public class ProductOutStoreAuditDto : Entity<int>
    {
        public decimal ActualQuantity { get; set; }
    }
    public class ProductOutStoreBatchAuditDto 
    {
        public int[] Ids { get; set; }
    }
}

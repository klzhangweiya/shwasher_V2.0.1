using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ProductInfo
{
    public class ProductProperty:FullAuditedEntity<int>
    {
        public string PrNo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    public class SemiEnterStoreAuditDto:Entity<int>
    {
        public decimal ActualQuantity { get; set; }
        public int? CreateSourceType { get; set; }

        public int StoreHouseId { get; set; }

        public string StoreLocationNo { get; set; }

    }
}

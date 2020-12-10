using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    public class UpdateKgWeightDto : Entity<int>
    {
        public decimal KgWeight { get; set; }

    }

    public class ChangeStoreHouseDto : Entity<int>
    {
        public decimal ChangeQuantity { get; set; }
        public int StoreHouseId { get; set; }

        public string StoreLocationNo { get; set; }

    }
}

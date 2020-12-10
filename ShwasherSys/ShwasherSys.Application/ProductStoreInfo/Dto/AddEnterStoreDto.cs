using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    [AutoMapTo(typeof(FinshedEnterStore)), AutoMapFrom(typeof(FinshedEnterStore))]
    public class AddEnterStoreDto
    {
        public decimal Quantity { get; set; }

        public string ProductionOrderNo { get; set; }

        public string ProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public string StoreLocationNo { get; set; }

        public decimal? PackageCount { get; set; }
        public decimal? PackageSpecification { get; set; }
    }

    public class AddOutStoreDto
    {
        public decimal? Quantity { get; set; }
        public string CurrentProductStoreHouseNo { get; set; }
    }

    public class ProductionOrderDisCustomerDto
    {
        public string ProductionOrderNo { get; set; }

        public string CustomerName { get; set; }

        public string CustomerId { get; set; }
    }
}

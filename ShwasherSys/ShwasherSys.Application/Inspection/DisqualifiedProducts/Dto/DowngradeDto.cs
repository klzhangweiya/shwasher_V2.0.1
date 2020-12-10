using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ShwasherSys.Inspection.DisqualifiedProducts.Dto
{
    public class DowngradeDto : EntityDto<int>
    {
        public int StoreHouseType { get; set; } 
        public int StoreHouseId { get; set; } 
        public string StoreLocationNo { get; set; } 
        public string ProductNo { get; set; } 
        public  List<string> CustomerNos { get; set; } 
        public  List<string> ProductOrderNos { get; set; }

        public decimal? Quantity { get; set; }
    }
}
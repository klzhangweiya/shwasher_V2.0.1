using Abp.Application.Services.Dto;

namespace ShwasherSys.Inspection.DisqualifiedProducts.Dto
{
    public class ReturnOrderDto : EntityDto<int>
    {
        public string SurveyReason { get; set; } 
        public string SurveyDetail { get; set; } 
        public string Solution { get; set; } 
        public int InspectType { get; set; } 
        public int? StoreHouseId { get; set; } 
        public int? StoreHouseId2 { get; set; } 
        public string StoreLocationNo { get; set; }

        public string SpecialPurchaseRemark { get; set; }
    }


    public class ReturnOrderDetailInfoDto
    {
        public string Reason { get; set; }
        public string ReturnOrderNo { get; set; }
        public string ProductNo { get; set; }
        public string Model { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }
        public string Material { get; set; }
        public string ProductName { get; set; }
    }
}
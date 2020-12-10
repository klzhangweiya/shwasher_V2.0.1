using Abp.Application.Services.Dto;

namespace ShwasherSys.Inspection.DisqualifiedProducts.Dto
{
    public class RelatedProductDto:EntityDto
    {
        public string ProductOrderNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string PartNo { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }
        public decimal Quantity { get; set; }
    }
}
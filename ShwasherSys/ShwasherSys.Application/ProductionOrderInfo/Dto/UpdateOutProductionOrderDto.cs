using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class UpdateOutProductionOrderDto
    {
        public string ProductionOrderNo { get; set; }
        public string SourceProductionOrderNo { get; set; }
        public string ProcessingTypeNo { get; set; }
        public string ProcessingType { get; set; }
        public decimal Quantity { get; set; }
        public string SemiProductNo { get; set; }
        public int SemiOutStoreId { get; set; }
        public DateTime PlanProduceDate { get; set; }
        public string OutsourcingFactory { get; set; }
        public decimal? KgWeight { get; set; }
        public string Remark { get; set; }
    }
}

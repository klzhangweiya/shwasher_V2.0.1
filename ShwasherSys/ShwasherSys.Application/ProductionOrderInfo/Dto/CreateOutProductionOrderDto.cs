using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class CreateOutProductionOrderDto
    {
        public int OutStoreId { get; set; }
        public string SourceProductionOrderNo { get; set; }
        public string ProcessingTypeNo { get; set; }

        public string ProcessingType { get; set; }

        public decimal Quantity { get; set; }

        public string SemiProductNo { get; set; }

        public int SemiOutStoreId { get; set; }
        public string OutsourcingFactory { get; set; }
        public string Remark { get; set; }

        public decimal? KgWeight { get; set; }

        public DateTime? PlanProduceDate { get; set; }
        /// <summary>
        /// 1:成品返镀 其它：
        /// </summary>
        public int? IsReplating { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ReturnGoods
{
    [Table("ReturnGoodOrder")]
    public class ReturnGoodOrder:FullAuditedEntity<int>
    {
        public const int ReturnOrderNoMaxLength = 32;
        public const int SendOrderNoMaxLength = 32;
        public const int OrderNoMaxLength = 32;
        public const int ProductNoMaxLength = 32;
        public const int HandleUserMaxLength = 32;
        public const int ProductionOrderNoMaxLength = 32;
        public const int CustomerIdMaxLength = 32;
        public const int LinkNameMaxLength = 50;
        public const int ReasonMaxLength = 500;

        [MaxLength(ReturnOrderNoMaxLength)]
        [Index(IsUnique = true)]
        public string ReturnOrderNo { get; set; }
        [MaxLength(SendOrderNoMaxLength)]
        public string SendOrderNo { get; set; }
        [MaxLength(OrderNoMaxLength)]
        public string OrderNo { get; set; }
        
        [MaxLength(OrderNoMaxLength)]
        public string OrderItemNo { get; set; }
        [MaxLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
        [MaxLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [MaxLength(CustomerIdMaxLength)]
        public string  CustomerId { get; set; }
        [MaxLength(ReasonMaxLength)]
        public string  Reason { get; set; }

        public Decimal? Amount { get; set; }
        public Decimal? AuditAmount { get; set; }
        public Decimal? Quantity { get; set; }
        [MaxLength(HandleUserMaxLength)]
        public string HandleUser { get; set; }

        [MaxLength(HandleUserMaxLength)]
        public string ApplyUser { get; set; }

        [MaxLength(HandleUserMaxLength)]
        public string ConfirmUser { get; set; }

        public DateTime? ReturnDate { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? ConfirmDate { get; set; }

        public int ReturnState { get; set; }
        public int ReturnType { get; set; }
        
        [MaxLength(LinkNameMaxLength)]
        public string LinkName { get; set; } 
        [MaxLength(ReasonMaxLength)]
        public string SurveyReason { get; set; } 
        [MaxLength(ReasonMaxLength)]
        public string SurveyDetail { get; set; } 
        [MaxLength(ReasonMaxLength)]
        public string Solution { get; set; } 
    }
}

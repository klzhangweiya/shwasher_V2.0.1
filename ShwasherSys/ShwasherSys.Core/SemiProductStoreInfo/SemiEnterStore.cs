using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.SemiProductStoreInfo
{
    [Table("SemiEnterStore")]
    public class SemiEnterStore:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int ApplyStatusMaxLength = 5;
        public const int ApplySourceMaxLength = 5;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;

        public const int StoreLocationNoMaxLength = 32;
        /*public const int SemiEnterStoreNoMaxLength = 32;

        [Required]
        [StringLength(SemiEnterStoreNoMaxLength)]
        public string SemiEnterStoreNo { get; set; }*/

        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库 
        /// </summary>
        [Required]
        [StringLength(ApplyStatusMaxLength)]
        public string ApplyStatus { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClose { get; set; }

        /// <summary>
        /// 申请来源（1.车间加工生产 2.外购单申请入库）
        /// </summary>
        [Required]
        [StringLength(ApplySourceMaxLength)]
        public string ApplySource { get; set; }
       
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 申请入库数量
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 实际入库数量
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string EnterStoreUser { get; set; }

        public DateTime? EnterStoreDate { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>
        public DateTime? ApplyEnterDate { get; set; }
        
        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }

        /// <summary>
        /// 创建入库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }
        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
    }
}

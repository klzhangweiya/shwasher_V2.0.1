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
    [Table("SemiOutStores")]
    public class SemiOutStore:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int CurrentSemiStoreHouseNoMaxLength = 32;
        public const int ApplyStatusMaxLength = 5;
        public const int ApplySourceMaxLength = 5;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int ApplyOutStoreSourceMaxLength = 32;
        public const int AuditUserMaxLength = 20;

        public const int RemarkMaxLength = 150;
        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 半成品库存中记录编号
        /// </summary>
        [StringLength(CurrentSemiStoreHouseNoMaxLength)]
        public string CurrentSemiStoreHouseNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库 
        /// </summary>
        [Required]
        [StringLength(ApplyStatusMaxLength)]
        public string ApplyStatus { get; set; }

        /// <summary>
        /// 1.生产 2.包装
        /// </summary>
        [Required]
        public int ApplyTypes { get; set; } = OutStoreApplyTypeEnum.OutAssistant.ToInt();
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClose { get; set; }
        /// <summary>
        /// 申请出库来源（1.外协加工申请 2.包装申请）
        /// </summary>
        [Required]
        [StringLength(ApplySourceMaxLength)]
        public string ApplyOutStoreSource { get; set; }
        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
        public Boolean? IsConfirm { get; set; }

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
        /// 实际出库数量（用于填入实时库存中的数量）
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 申请出库时间
        /// </summary>
        public DateTime? ApplyOutDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        [StringLength(AuditUserMaxLength)]
        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }



        [StringLength(UserIDLastModMaxLength)]
        public string OutStoreUser { get; set; }

        public DateTime? OutStoreDate { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }

        /// <summary>
        /// 创建入库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }
    }
}

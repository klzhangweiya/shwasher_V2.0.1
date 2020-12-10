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
    [Table("CurrentSemiStoreHouse")]
    public class CurrentSemiStoreHouse:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int CurrentSemiStoreHouseNoMaxLength = 32;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;
        public const int StoreLocationNoMaxLength = 32;
        /// <summary>
        /// 库位编号
        /// </summary>
        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
        [Required]
        [StringLength(CurrentSemiStoreHouseNoMaxLength)]
        public string CurrentSemiStoreHouseNo { get; set; }
        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>
        [DecimalPrecision]
        public decimal FreezeQuantity { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

        /*/// <summary>
        /// 检验状态（0：未检验 1：已检验）
        /// </summary>
        [StringLength()]
        public string InspectionStatus { get; set; }*/
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
        /// 上月底剩余数量
        /// </summary>
        [DecimalPrecision]
        public decimal? PreMonthQuantity { get; set; }

        /// <summary>
        /// 盘点冻结状态（1:未被盘点 2:正在被盘点）
        /// </summary>
        public int? InventoryCheckState { get; set; }

        /// <summary>
        /// 退货冻结状态（1：未被退货 2：被退货冻结）
        /// </summary>

        public int? ReturnState { get; set; }

    }
}

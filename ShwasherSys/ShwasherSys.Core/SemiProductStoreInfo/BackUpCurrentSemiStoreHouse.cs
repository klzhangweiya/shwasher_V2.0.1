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
    /// <summary>
    /// 半成品库存进行结算时备份被清零的数据
    /// </summary>
    [Table("BackUpCurrentSemiStoreHouse")]
    public class BackUpCurrentSemiStoreHouse:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int CurrentSemiStoreHouseNoMaxLength = 32;
        
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;

        public const int RemarkMaxLength = 150;
        /*public const int SemiEnterStoreNoMaxLength = 32;
        /// <summary>
        /// 入库记录编号
        /// </summary>
        [Required]
        [StringLength(SemiEnterStoreNoMaxLength)]
        public string SemiEnterStoreNo { get; set; }*/
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
    }
}

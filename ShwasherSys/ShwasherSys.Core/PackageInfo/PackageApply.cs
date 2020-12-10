using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.PackageInfo
{
    [Table("PackageApply")]
    public class PackageApply:Entity<int>
    {
        

        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int ProductNoMaxLength = 32;
        public const int CurrentSemiStoreHouseNoMaxLength = 32;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;
        public const int PackageApplyNoMaxLength = 32;

        public const int ApplyStatusMaxLength = 1;
        /// <summary>
        /// 申请流水号
        /// </summary>
        [Required]
        [StringLength(PackageApplyNoMaxLength)]
        public string PackageApplyNo { get; set; }
        /// <summary>
        /// 仓库库存信息编号
        /// </summary>
        [StringLength(CurrentSemiStoreHouseNoMaxLength)]
        [Column("CurrentStoreHouseNo")]
        public string CurrentSemiStoreHouseNo { get; set; }
        /// <summary>
        /// 流转单编号
        /// </summary>
        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
        /// <summary>
        /// 申请包装数量
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity { get; set; }

        /// <summary>
        /// 实际包装的千件数
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 仓库来源
        /// </summary>
        public int SourceStore { get; set; }

        [Required]
        [StringLength(ApplyStatusMaxLength)]
        public string ApplyStatus { get; set; }
        public bool IsClose { get; set; }
        /// <summary>
        /// 发起申请时间
        /// </summary>
        public DateTime? ApplyDate { get; set; }

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

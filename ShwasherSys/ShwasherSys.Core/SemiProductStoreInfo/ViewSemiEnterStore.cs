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
    [Table("N_ViewSemiEnterStore")]
    public class ViewSemiEnterStore:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int ApplyStatusMaxLength = 1;
        public const int ApplySourceMaxLength = 1;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;

        public const int SemiProductNameMaxLength = 50;
        public const int ModelMaxLength = 20;
        public const int MaterialMaxLength = 50;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        public const int PartNoMaxLength = 50;
        public const int IsStandardMaxLength = 1;
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
        /// 1.申请中 2.已入库 3.已取消
        /// </summary>
        [Required]
        [StringLength(ApplyStatusMaxLength)]
        public string ApplyStatus { get; set; }
        public bool? IsClose { get; set; }
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
        public decimal Quantity { get; set; }
        /// <summary>
        /// 实际入库数量
        /// </summary>
        public decimal ActualQuantity { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }
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

        [StringLength(SemiProductNameMaxLength)]
        public string SemiProductName { get; set; }

        [StringLength(ModelMaxLength)]
        public string Model { get; set; }



        [StringLength(MaterialMaxLength)]
        public string Material { get; set; }



        [StringLength(SurfaceColorMaxLength)]
        public string SurfaceColor { get; set; }

        [StringLength(RigidityMaxLength)]
        public string Rigidity { get; set; }
        [StringLength(IsStandardMaxLength)]
        public string IsStandard { get; set; }
        [StringLength(PartNoMaxLength)]
        public string PartNo { get; set; }

        public decimal? TranUnitValue { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 创建入库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }

        public string StoreLocationNo { get; set; }

    }
}

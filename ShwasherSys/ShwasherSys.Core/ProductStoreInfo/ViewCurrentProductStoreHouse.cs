using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo
{
    [Table("N_ViewCurrentProductStoreHouse")]
    [AutoMapTo(typeof(CurrentStoreItemDto))]
    public class ViewCurrentProductStoreHouse : Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 32;
        public const int CurrentSemiStoreHouseNoMaxLength = 32;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;

        public const int SemiProductNameMaxLength = 50;
        public const int ModelMaxLength = 20;
        public const int MaterialMaxLength = 50;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        public const int IsStandardMaxLength = 1;

        [Required]
        public string CurrentProductStoreHouseNo { get; set; }
        [Required]
        public string ProductionOrderNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>
        public string StoreLocationNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>
        public decimal FreezeQuantity { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Quantity-FreezeQuantity  可用数量
        /// </summary>
        public decimal ActualQuantity { get; set; }

       


        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        [StringLength(SemiProductNameMaxLength)]
        public string ProductName { get; set; }

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
        public string ProductDesc { get; set; }
        public string PartNo { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 上月底剩余数量
        /// </summary>
        [DecimalPrecision]
        public decimal? PreMonthQuantity { get; set; }
        public int? InventoryCheckState { get; set; }

        /// <summary>
        /// 退货冻结状态（1：未被退货 2：被退货冻结）
        /// </summary>

        public int? ReturnState { get; set; }

        public string StoreAreaCode { get; set; }

        public string ShelfNumber { get; set; }

        public string ShelfLevel { get; set; }

        public string SequenceNo { get; set; }
    }
}

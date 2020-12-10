using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductStoreInfo
{
    [Table("CurrentProductStoreHouse")]
    public class CurrentProductStoreHouse:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int ProductNoMaxLength = 32;
        public const int CurrentProductStoreHouseNoMaxLength = 32;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;
        public const int StoreLocationNoMaxLength = 32;
        /*public const int SemiEnterStoreNoMaxLength = 32;  
        /// <summary>
        /// 入库记录编号
        /// </summary>
        [Required]
        [StringLength(SemiEnterStoreNoMaxLength)]
        public string SemiEnterStoreNo { get; set; }*/
        [Required]
        [StringLength(CurrentProductStoreHouseNoMaxLength)]
        public string CurrentProductStoreHouseNo { get; set; }
       
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>
        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>
        [DecimalPrecision]
        public decimal FreezeQuantity { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }

        /*/// <summary>
        /// 检验状态（0：未检验 1：已检验）
        /// </summary>
        [StringLength()]
        public string InspectionStatus { get; set; }*/
       

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

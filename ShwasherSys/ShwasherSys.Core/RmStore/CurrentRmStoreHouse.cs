using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.RmStore
{
    [Table("CurrentRmStoreHouse")]
    public class CurrentRmStoreHouse:FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int RmProductNoMaxLength = 32;
        public const int RemarkMaxLength = 150;
        public const int StoreLocationNoMaxLength = 32;
        public const int ProductBatchNumMaxLength = 32;
       
      
       //批次号
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
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
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

        [StringLength(ProductBatchNumMaxLength)]
        public string ProductBatchNum { get; set; }
        /// <summary>
        /// 上月底剩余数量
        /// </summary>
        [DecimalPrecision]
        public decimal? PreMonthQuantity { get; set; }
    }

    [Table("N_ViewCurrentRmStoreHouse")]
    public class ViewCurrentRmStoreHouse : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int RmProductNoMaxLength = 32;
        public const int RemarkMaxLength = 150;
        public const int StoreLocationNoMaxLength = 32;


        //批次号
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
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
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



        /// <summary>
        /// 上月底剩余数量
        /// </summary>
        [DecimalPrecision]
        public decimal? PreMonthQuantity { get; set; }

        public string ProductName { get; set; }

        public string Material { get; set; }

        public string Model { get; set; }

        public string ProductDesc { get; set; }

        public string StoreHouseName { get; set; }

        public string ProductBatchNum { get; set; }
    }
}

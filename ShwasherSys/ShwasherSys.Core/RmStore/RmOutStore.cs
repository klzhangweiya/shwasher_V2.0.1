using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.RmStore
{
    [Table("RmOutStore")]
    public class RmOutStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int CurrentRwProductStoreHouseNoMaxLength = 32;
        public const int RmProductNoMaxLength = 50;
        public const int RemarkMaxLength = 150;
        public const int UserMaxLength = 150;
        public const int ProductBatchNumMaxLength = 32;

        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 原材料库存中记录编号
        /// </summary>
        [StringLength(CurrentRwProductStoreHouseNoMaxLength)]
        public string CurrentRmStoreHouseNo { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        ///0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库
        /// </summary>
        [Required]
        public int ApplyStatus { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClose { get; set; }

        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
        public Boolean? IsConfirm { get; set; }

        /// <summary>
        /// 申请出库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 审核后出库数量
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

     
        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(UserMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }


        [StringLength(UserMaxLength)]
        public string OutStoreUser { get; set; }

        public DateTime? OutStoreDate { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>
        public DateTime? ApplyOutDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [StringLength(ProductBatchNumMaxLength)]
        public string ProductBatchNum { get; set; }

        //手动平衡:2 常规流程:1  默认1
        public int CreateSourceType { get; set; } = 1;
    }


    [Table("N_ViewRmOutStore")]
    public class ViewRmOutStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int CurrentRwProductStoreHouseNoMaxLength = 32;
        public const int RmProductNoMaxLength = 50;
        public const int RemarkMaxLength = 150;
        public const int UserMaxLength = 150;


        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 原材料库存中记录编号
        /// </summary>
        [StringLength(CurrentRwProductStoreHouseNoMaxLength)]
        public string CurrentRmStoreHouseNo { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        ///0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库
        /// </summary>
        [Required]
        public int ApplyStatus { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClose { get; set; }

        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
        public Boolean? IsConfirm { get; set; }

        /// <summary>
        /// 申请出库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 审核后出库数量
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }


        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(UserMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }


        [StringLength(UserMaxLength)]
        public string OutStoreUser { get; set; }

        public DateTime? OutStoreDate { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>
        public DateTime? ApplyOutDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public string ProductName { get; set; }

        public string Material { get; set; }

        public string Model { get; set; }

        public string ProductDesc { get; set; }

        public string StoreHouseName { get; set; }

        public string ProductBatchNum { get; set; }

        //手动平衡:2 常规流程:1  默认1
        public int? CreateSourceType { get; set; }

    }
}

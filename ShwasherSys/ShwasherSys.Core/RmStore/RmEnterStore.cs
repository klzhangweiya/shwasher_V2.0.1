using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.RmStore
{
    [Table("RmEnterStore")]
    public class RmEnterStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int RmProductNoMaxLength = 50;
        public const int ProductNoMaxLength = 50;
        public const int AuditUserMaxLength = 32;
        public const int ProductBatchNumMaxLength = 32;

        public const int StoreLocationNoMaxLength = 32;
        
        public const int RemarkMaxLength = 150;

      
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
       
       
        /// <summary>
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
      
        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
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
        /// 入库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 申请入库数量(千斤/千件)
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity { get; set; }
        
      
        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(AuditUserMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>
        public DateTime? ApplyEnterDate { get; set; }


        [StringLength(AuditUserMaxLength)]
        public string EnterStoreUser { get; set; }

        public DateTime? EnterStoreDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [StringLength(ProductBatchNumMaxLength)]
        public string ProductBatchNum { get; set; }
        //手动平衡:2 常规流程:1  默认1
        public int CreateSourceType { get; set; } = 1;

    }

    [Table("N_ViewRmEnterStore")]
    public class ViewRmEnterStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int RmProductNoMaxLength = 50;
        public const int ProductNoMaxLength = 50;
        public const int AuditUserMaxLength = 32;

        public const int StoreLocationNoMaxLength = 32;

        public const int RemarkMaxLength = 150;

        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }


        /// <summary>
        /// 原材料编号
        /// </summary>
        [StringLength(RmProductNoMaxLength)]
        public string RmProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }

        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
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
        /// 入库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 申请入库数量(千斤/千件)
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity { get; set; }


        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(AuditUserMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>
        public DateTime? ApplyEnterDate { get; set; }


        [StringLength(AuditUserMaxLength)]
        public string EnterStoreUser { get; set; }

        public DateTime? EnterStoreDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }


        public string ProductName { get; set; }

        public string Material { get; set; }

        public string Model { get; set; }

        public string ProductDesc { get; set; }

        public string StoreHouseName { get; set; }

       
        public string ProductBatchNum { get; set; }

        public int? CreateSourceType { get; set; }
    }
}

﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ScrapStore
{
    [Table("ScrapEnterStore")]
    public class ScrapEnterStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
        //public const int ProductNoMaxLength = 50;
        public const int ProductNoMaxLength = 50;
        public const int AuditUserMaxLength = 32;

        public const int StoreLocationNoMaxLength = 32;
        
        public const int RemarkMaxLength = 150;
        public const int ScrapSourceNoMaxLength = 50;


        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
       
       
        /// <summary>
        /// 产品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
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

     
        // 成品1/半成品2/原材料3:  默认1
        public int ProductType { get; set; } = 1;
        //报废来源 1：成品退货 2：半成品检验报废
        public int ScrapSource { get; set; }
        //来源编码  记录来源数据主键编号  用于回溯
        [MaxLength(ScrapSourceNoMaxLength)]
        public string ScrapSourceNo { get; set; }

    }

    [Table("N_ViewScrapEnterStore")]
    public class ViewScrapEnterStore : FullAuditedEntity<string>
    {
        public const int ProductionOrderNoMaxLength = 11;
       // public const int ProductNoMaxLength = 50;
        public const int ProductNoMaxLength = 50;
        public const int AuditUserMaxLength = 32;

        public const int StoreLocationNoMaxLength = 32;

        public const int RemarkMaxLength = 150;

        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }


        /// <summary>
        /// 产品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
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

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }


        //public string ProductBatchNum { get; set; }
        // 成品1/半成品2/原材料3:  默认1
        public int? ProductType { get; set; }

        //报废来源 1：成品退货 2：半成品检验报废
        public int ScrapSource { get; set; }
        //来源编码  记录来源数据主键编号  用于回溯
        public string ScrapSourceNo { get; set; }
    }
}

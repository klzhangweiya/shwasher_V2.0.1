using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductStoreInfo
{
    [Table("FinshedEnterStore")]
    public class FinshedEnterStore : Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength = 50;
        public const int ProductNoMaxLength = 50;
        public const int PackageApplyNoMaxLength = 32;

        public const int StoreLocationNoMaxLength = 32;
        //public const int ApplyStatusMaxLength = 1;
        //public const int ApplySourceMaxLength = 1;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;
        public const int PackageSpecificationMaxLength = 500;

        public const int CreateSourceTypeMaxLength = 1;

        public const int PackageEnterNumMaxLength = 20;
        /*public const int SemiEnterStoreNoMaxLength = 32;

        [Required]
        [StringLength(SemiEnterStoreNoMaxLength)]
        public string SemiEnterStoreNo { get; set; }*/

        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        [StringLength(PackageApplyNoMaxLength)]
        public string PackageApplyNo { get; set; }
        /// <summary>
        /// 待包装产品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string PackageProductNo { get; set; } 
        /// <summary>
        /// 成品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        public int SourceStoreHouseId { get; set; }
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
        /// 申请来源(1.半成品，2.成品)
        /// </summary>
        [Required]
        public int ApplySourceType { get; set; }
       
        /// <summary>
        /// 入库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 申请入库数量(千斤)
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity { get; set; }
        
        /// <summary>
        /// 包装规格(千件/包）
        /// </summary>
        [DecimalPrecision]
        public decimal PackageSpecification { get; set; }
        /// <summary>
        /// 申请入库包数
        /// </summary>
        [DecimalPrecision]
        public decimal PackageCount { get; set; }
        /// <summary>
        /// 实际入库包数
        /// </summary>
        [DecimalPrecision]
        public decimal ActualPackageCount { get; set; }
    
        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(UserIDLastModMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>
        public DateTime? ApplyEnterDate { get; set; }


        [StringLength(UserIDLastModMaxLength)]
        public string EnterStoreUser { get; set; }

        public DateTime? EnterStoreDate { get; set; }

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
        /// 创建入库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }
        /// <summary>
        /// 包装入库号
        /// </summary>
        public string PackageEnterNum { get; set; }
        /// <summary>
        /// 包装负责人
        /// </summary>
        [StringLength(UserIDLastModMaxLength)]
        public string PackageUser { get; set; }
        /// <summary>
        /// 核实人员
        /// </summary>
        [StringLength(UserIDLastModMaxLength)]
        public string VerifyUser { get; set; } 
        
        /// <summary>
        /// 申请入库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity2 { get; set; }
    }
}

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
    [Table("N_ViewProductEnterStore")]
    public class ViewProductEnterStore : Entity<int>
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
        public const int IsStandardMaxLength = 1;
        /*public const int SemiEnterStoreNoMaxLength = 32;

        [Required]
        [StringLength(SemiEnterStoreNoMaxLength)]
        public string SemiEnterStoreNo { get; set; }*/


        
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        public string PackageApplyNo { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string PackageProductNo { get; set; } 
        public string ProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
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
        /// 申请来源（1.车间包装 ）
        /// </summary>
        public int ApplySourceType { get; set; }

        /// <summary>
        /// 申请入库数量(千斤)
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 包装规格(千件/包）
        /// </summary>
        public decimal PackageSpecification { get; set; }
        /// <summary>
        /// 申请入库包数
        /// </summary>
        public decimal PackageCount { get; set; }
        /// <summary>
        /// 实际入库包数
        /// </summary>
        public decimal ActualPackageCount { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>
        public DateTime? ApplyEnterDate { get; set; }

        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        public string CreatorUserId { get; set; }
        
        public string UserIDLastMod { get; set; }

       
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
        /// <summary>
        /// 千件重
        /// </summary>
        public decimal KgWeight { get; set; }
        public int SourceStoreHouseId { get; set; }

        /// <summary>
        /// 创建出库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }

        public string PackageEnterNum { get; set; }

    }
}

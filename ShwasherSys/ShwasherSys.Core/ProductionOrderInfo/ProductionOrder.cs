using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using JetBrains.Annotations;

namespace ShwasherSys.ProductionOrderInfo
{
    [Table("ProductionOrders")]
    public class ProductionOrder:Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int SourceProductionOrderNoMaxLength = 11;
        public const int SemiProductNoMaxLength =32;
        public const int StoveNoMaxLength = 20;
        public const int CarNoMaxLength = 20;
        public const int ModelMaxLength = 50;
        public const int PartNoMaxLength = 50;
        public const int MaterialMaxLength = 50;
        public const int SizeMaxLength = 50;
        public const int RawMaterialsMaxLength = 100;
        public const int SurfaceColorMaxLength = 50;
        public const int RigidityMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int IsLockMaxLength = 1;
        public const int RemarkMaxLength = 150;

        public const int ProcessingTypeMaxLength = 1;
        public const int ProcessingLevelMaxLength = 1;
        public const int ProductionTypeMaxLength = 1;
        [Required]
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        [Required]
        public int ProductionOrderStatus { get; set; }
        [StringLength(StoveNoMaxLength)]
        public string StoveNo { get; set; }
        [StringLength(CarNoMaxLength)]
        public string CarNo { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 计划加工数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 原材料来源（关联至原材料仓库，一对多，暂未实现）
        /// </summary>
        [StringLength(RawMaterialsMaxLength)]
        public string RawMaterials { get; set; }

        [StringLength(MaterialMaxLength)]
        public string Material { get; set; }
        [StringLength(PartNoMaxLength)]
        public string PartNo { get; set; }
        [StringLength(ModelMaxLength)]
        public string Model { get; set; }
        [StringLength(SurfaceColorMaxLength)]
        public string SurfaceColor { get; set; }
        [StringLength(RigidityMaxLength)]
        public string Rigidity { get; set; }
        [StringLength(SizeMaxLength)]
        public string Size { get; set; }

        /// <summary>
        /// 工序类型（1.车间生产2.热处理3.表面处理）
        /// </summary>
        [StringLength(ProcessingTypeMaxLength)]
        public string ProcessingType { get; set; }
        /// <summary>
        /// 加工阶段  1.第一阶段车间 2.外协
        /// </summary>
        [StringLength(ProcessingLevelMaxLength)]
        public string ProcessingLevel { get; set; }
        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }
        public int? IsChecked { get; set; } 
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }
        /// <summary>
        /// 外协阶段加工的产品来源于上一个半成品的流转单号（从半成品出库记录种带出）
        /// </summary>
        [StringLength(SourceProductionOrderNoMaxLength)]
        public string SourceProductionOrderNo { get; set; }

        /// <summary>
        /// 计划排产时间
        /// </summary>
        public DateTime? PlanProduceDate { get; set; }
        /// <summary>
        /// 可入库数量（生产完成数据）
        /// </summary>
        [DecimalPrecision]
        public decimal EnterQuantity { get; set; }
        /// <summary>
        /// 生产单类型 0:车间 1外购
        /// </summary>
        [StringLength(ProductionTypeMaxLength)]
        public string ProductionType { get; set; }

        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }

        /// <summary>
        /// 外协厂商
        /// </summary>
        public string OutsourcingFactory { get; set; }


        public DateTime? EnterDate { get; set; }
        public DateTime? InspectDate { get; set; }
        public bool HasExported { get; set; }


        

    }
}

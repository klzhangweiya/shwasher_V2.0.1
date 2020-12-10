using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    [AutoMapTo(typeof(ProductionOrder))]
    public class ProductionOrderCreateDto
    {
        [Required] 
        [StringLength(ProductionOrder.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        [Required] 
		public int ProductionOrderStatus  { get; set; }
        [StringLength(ProductionOrder.StoveNoMaxLength)]
		public string StoveNo  { get; set; }
        [StringLength(ProductionOrder.CarNoMaxLength)]
		public string CarNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
        [StringLength(ProductionOrder.SemiProductNoMaxLength)]
		public string SemiProductNo  { get; set; }
        /// <summary>
        /// 计划加工数量
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 原材料来源（关联至原材料仓库，一对多，暂未实现）
        /// </summary>   
        [StringLength(ProductionOrder.RawMaterialsMaxLength)]
		public string RawMaterials  { get; set; }
        [StringLength(ProductionOrder.MaterialMaxLength)]
		public string Material  { get; set; }
        [StringLength(ProductionOrder.ModelMaxLength)]
		public string Model  { get; set; }
        [StringLength(ProductionOrder.SurfaceColorMaxLength)]
		public string SurfaceColor  { get; set; }
        [StringLength(ProductionOrder.RigidityMaxLength)]
		public string Rigidity  { get; set; }
        [StringLength(ProductionOrder.SizeMaxLength)]
		public string Size  { get; set; }
        /// <summary>
        /// 工序类型（1.车间生产2.热处理3.表面处理）
        /// </summary>   
        [StringLength(ProductionOrder.ProcessingTypeMaxLength)]
		public string ProcessingType  { get; set; }
        /// <summary>
        /// 加工阶段  1.第一阶段车间 2.外协
        /// </summary>   
        [StringLength(ProductionOrder.ProcessingLevelMaxLength)]
		public string ProcessingLevel  { get; set; }
        [StringLength(ProductionOrder.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(ProductionOrder.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(ProductionOrder.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        [StringLength(ProductionOrder.IsLockMaxLength)]
		public string IsLock  { get; set; }
        /// <summary>
        /// 外协阶段加工的产品来源于上一个半成品的流转单号（从半成品出库记录种带出）
        /// </summary>   
        [StringLength(ProductionOrder.SourceProductionOrderNoMaxLength)]
		public string SourceProductionOrderNo  { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlanProduceDate { get; set; }
        /// <summary>
        /// 已入库数量
        /// </summary>
        public decimal EnterQuantity { get; set; }
        [StringLength(ProductionOrder.ProductionTypeMaxLength)]
        public string ProductionType { get; set; }
        public string OutsourcingFactory { get; set; }

        public decimal? KgWeight { get; set; }
    }
}

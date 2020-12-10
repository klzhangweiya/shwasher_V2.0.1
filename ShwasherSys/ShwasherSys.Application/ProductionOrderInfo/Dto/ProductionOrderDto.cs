using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    [AutoMapTo(typeof(ProductionOrder)),AutoMapFrom(typeof(ProductionOrder))]
    public class ProductionOrderDto: EntityDto<int>
    {
        public ProductionOrderDto()
        {
        }
        public ProductionOrderDto(ProductionOrder p)
        {
            Id = p.Id;
            ProductionOrderNo = p.ProductionOrderNo;
            ProductionOrderStatus = p.ProductionOrderStatus;
            StoveNo = p.StoveNo;
            CarNo = p.CarNo;
            SemiProductNo = p.SemiProductNo;
            Quantity = p.Quantity;
            RawMaterials = p.RawMaterials;
            Material = p.Material;
            Model = p.Model;
            SurfaceColor = p.SurfaceColor;
            Rigidity = p.Rigidity;
            PartNo = p.PartNo;
            Size = p.Size;
            ProcessingType = p.ProcessingType;
            ProcessingLevel = p.ProcessingLevel;
            Remark = p.Remark;
            TimeCreated = p.TimeCreated;
            TimeLastMod = p.TimeLastMod;
            CreatorUserId = p.CreatorUserId;
            UserIDLastMod = p.UserIDLastMod;
            IsChecked = p.IsChecked;
            IsLock = p.IsLock;
            SourceProductionOrderNo = p.SourceProductionOrderNo;
            PlanProduceDate = p.PlanProduceDate;
            EnterQuantity = p.EnterQuantity;
            EnterDate = p.EnterDate;
            InspectDate = p.InspectDate;
        }

        public ProductionOrderDto(int id, string productionOrderNo, int productionOrderStatus, string stoveNo, string carNo, string semiProductNo, decimal quantity, string rawMaterials, string material, string model, string surfaceColor, string rigidity, string size, string processingType, string processingLevel, string remark, DateTime? timeCreated, DateTime? timeLastMod, string creatorUserId, string userIdLastMod, int? isChecked, string isLock, string sourceProductionOrderNo, DateTime? planProduceDate, decimal enterQuantity, DateTime? enterDate, DateTime? inspectDate) : base(id)
        {
            ProductionOrderNo = productionOrderNo;
            ProductionOrderStatus = productionOrderStatus;
            StoveNo = stoveNo;
            CarNo = carNo;
            SemiProductNo = semiProductNo;
            Quantity = quantity;
            RawMaterials = rawMaterials;
            Material = material;
            Model = model;
            SurfaceColor = surfaceColor;
            Rigidity = rigidity;
            Size = size;
            ProcessingType = processingType;
            ProcessingLevel = processingLevel;
            Remark = remark;
            TimeCreated = timeCreated;
            TimeLastMod = timeLastMod;
            CreatorUserId = creatorUserId;
            UserIDLastMod = userIdLastMod;
            IsChecked = isChecked;
            IsLock = isLock;
            SourceProductionOrderNo = sourceProductionOrderNo;
            PlanProduceDate = planProduceDate;
            EnterQuantity = enterQuantity;
            EnterDate = enterDate;
            InspectDate = inspectDate;
        }

        public string ProductionOrderNo  { get; set; }
		public int ProductionOrderStatus  { get; set; }
		public string StoveNo  { get; set; }
		public string CarNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string SemiProductNo  { get; set; }
        /// <summary>
        /// 计划加工数量
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 原材料来源（关联至原材料仓库，一对多，暂未实现）
        /// </summary>   
		public string RawMaterials  { get; set; }
		public string Material  { get; set; }
		public string Model  { get; set; }
		public string SurfaceColor  { get; set; }
		public string Rigidity  { get; set; }
		public string Size  { get; set; }
        /// <summary>
        /// 工序类型（1.车间生产2.热处理3.表面处理）
        /// </summary>   
		public string ProcessingType  { get; set; }
        /// <summary>
        /// 加工阶段  1.第一阶段车间 2.外协
        /// </summary>   
		public string ProcessingLevel  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }
        public int? IsChecked { get; set; } 
		public string IsLock  { get; set; }
        /// <summary>
        /// 外协阶段加工的产品来源于上一个半成品的流转单号（从半成品出库记录种带出）
        /// </summary>   
		public string SourceProductionOrderNo  { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlanProduceDate { get; set; }
        /// <summary>
        /// 已入库数量
        /// </summary>
        public decimal EnterQuantity { get; set; }

        public string PartNo { get; set; }

        public string SemiProductName { get; set; }
        public string ProductionType { get; set; }
        public decimal KgWeight { get; set; }
        public string OutsourcingFactory { get; set; }

        public string OutsourcingFactoryName { get; set; }

        public DateTime? EnterDate { get; set; }

        public DateTime? InspectDate { get; set; }
        public bool HasExported { get; set; }

    }
}
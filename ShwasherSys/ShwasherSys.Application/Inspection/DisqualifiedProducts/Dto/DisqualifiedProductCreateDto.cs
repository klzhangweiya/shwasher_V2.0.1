using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.Inspection.DisqualifiedProducts.Dto
{
    
    /// <summary>
    /// 不合格产品
    /// </summary>   
    [AutoMapTo(typeof(DisqualifiedProduct))]
    public class DisqualifiedProductCreateDto:IwbEntityDto<int>
    {
		
        /// <summary>
        /// 编号
        /// </summary>   
        [StringLength(DisqualifiedProduct.DisqualifiedNoMaxLength)]
		public string DisqualifiedNo  { get; set; }
        /// <summary>
        /// 流转单号
        /// </summary>   
        [StringLength(ProductionOrder.ProductionOrderNoMaxLength)]
		public string ProductOrderNo  { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>   
        [StringLength(50)]
		public string ProductNo  { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>   
        [StringLength(100)]
		public string ProductName  { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>   
		public int ProductType  { get; set; }
        /// <summary>
        /// 产品数量（Kg）
        /// </summary>   
		public decimal QuantityWeight  { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>   
		public decimal KgWeight  { get; set; }
        /// <summary>
        /// 产品数量（千件）
        /// </summary>   
		public decimal QuantityPcs  { get; set; }
        /// <summary>
        /// 处理类型
        /// </summary>   
		public int HandleType  { get; set; }
        /// <summary>
        /// 检验人员
        /// </summary>   
        [StringLength(20)]
		public string CheckUser  { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>   
		public DateTime? CheckDate  { get; set; }
        /// <summary>
        /// 最后处理人
        /// </summary>   
        [StringLength(20)]
		public string HandleUser  { get; set; }
        /// <summary>
        /// 最后处理时间
        /// </summary>   
		public DateTime? HandleDate  { get; set; }

        /// <summary>
        /// 不合格类型  1:生产检验 2：退货检验
        /// </summary>
        public int? DisqualifiedType { get; set; }
    }
}

using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.Inspection.DisqualifiedProducts.Dto
{

    /// <summary>
    /// 不合格产品
    /// </summary>   
    [AutoMapTo(typeof(DisqualifiedProduct)), AutoMapFrom(typeof(DisqualifiedProduct))]
    public class DisqualifiedProductDto: EntityDto<int>
    {
        /// <summary>
        /// 编号
        /// </summary>   
		public string DisqualifiedNo  { get; set; }
        /// <summary>
        /// 流转单号
        /// </summary>   
		public string ProductOrderNo  { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>   
		public string ProductNo  { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>   
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
		public string CheckUser  { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>   
		public DateTime? CheckDate  { get; set; }


        /// <summary>
        /// 最后处理人
        /// </summary>   
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
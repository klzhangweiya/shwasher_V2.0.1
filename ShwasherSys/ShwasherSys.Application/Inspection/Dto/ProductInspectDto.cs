using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.Inspection.Dto
{
    
    /// <summary>
    /// 技术检验信息
    /// </summary>   
    [AutoMapTo(typeof(ProductInspectInfo)),AutoMapFrom(typeof(ProductInspectInfo))]
    public class ProductInspectDto: EntityDto<int>
    {
        /// <summary>
        /// 检验编号
        /// </summary>   
		public string ProductInspectNo  { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>   
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string SemiProductNo  { get; set; }
       
        /// <summary>
        /// 检验项目
        /// </summary>   
		public string InspectSubject  { get; set; }
        /// <summary>
        /// 检验结果
        /// </summary>   
		public int InspectResult  { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>   
		public DateTime? InspectDate  { get; set; }
        /// <summary>
        /// 检验人员
        /// </summary>   
		public string InspectMember  { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>   
		public string IsLock  { get; set; }
        /// <summary>
        /// 检验详情
        /// </summary>   
		public string InspectContent  { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>   
		public DateTime? TimeCreated  { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>   
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }

        public string SemiProductName { get; set; }
        public string Material { get; set; }
        public string Model { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }
        [IgnoreMap]
        public string ProductionType { get; set; }
        [IgnoreMap]
        public string ProcessingLevel  { get; set; }

    }
}
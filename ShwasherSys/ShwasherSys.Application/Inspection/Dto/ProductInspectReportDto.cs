using System;
using Abp.AutoMapper;
using AutoMapper;

namespace ShwasherSys.Inspection.Dto
{
    [AutoMapTo(typeof(ProductInspectReport)),AutoMapFrom(typeof(ProductInspectReport))]
    public class ProductInspectReportDto
    {
        /// <summary>
        /// 检验报告编号
        /// </summary>
        public string ProductInspectReportNo { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>
        public string ProductionOrderNo { get; set; }
        
        /// <summary>
        /// 半成品编号
        /// </summary>
        public string SemiProductNo { get; set; }

        /// <summary>
        /// 确认状态（1：未确认 2：最终确认）
        /// </summary>
        public int ConfirmStatus { get; set; }

        /// <summary>
        /// 检验次数
        /// </summary>
        public int InspectCount { get; set; }
       
        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 确认人员
        /// </summary>
        public string ConfirmUser { get; set; }

        /// <summary>
        /// 检验详情
        /// </summary>
        public string InspectContent { get; set; }
        [IgnoreMap]
        public string SemiProductName { get; set; }

        [IgnoreMap]
        public string Material  { get; set; }
        [IgnoreMap]
        public string Model  { get; set; }
        [IgnoreMap]
        public string SurfaceColor  { get; set; }
        [IgnoreMap]
        public string Rigidity  { get; set; }
        [IgnoreMap]
        public string PartNo { get; set; }

        
        /// <summary>
        /// 工序类型（1.车间生产2.热处理3.表面处理）
        /// </summary>   
        [IgnoreMap]
        public string ProcessingType  { get; set; }
        /// <summary>
        /// 加工阶段  1.第一阶段车间 2.外协
        /// </summary>   
        [IgnoreMap]
        public string ProcessingLevel  { get; set; }
        [IgnoreMap]
        public string ProductionType  { get; set; }

    }
}
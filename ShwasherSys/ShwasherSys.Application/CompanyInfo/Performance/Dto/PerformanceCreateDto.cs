using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;
using ShwasherSys.ProductionOrderInfo;

namespace ShwasherSys.CompanyInfo.Performance.Dto
{
    [AutoMapTo(typeof(EmployeeWorkPerformance))]
    public class PerformanceCreateDto:IwbEntityDto<int>
    {
		
        /// <summary>
        /// 编号
        /// </summary>   
        [StringLength(EmployeeWorkPerformance.PerformanceMaxLength)]
		public string PerformanceNo  { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>   
		public int EmployeeId  { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>   
        [StringLength(EmployeeWorkPerformance.RelatedNoMaxLength)]
		public string RelatedNo  { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>   
        [StringLength(ProductionOrder.ProductionOrderNoMaxLength)]
		public string ProductOrderNo  { get; set; }
        /// <summary>
        /// 工作类型
        /// </summary>   
		public int WorkType  { get; set; }
        /// <summary>
        /// 绩效量化
        /// </summary>   
		public decimal Performance  { get; set; }
        /// <summary>
        /// 量化单位
        /// </summary>   
        [StringLength(EmployeeWorkPerformance.PerformanceUnitMaxLength)]
		public string PerformanceUnit  { get; set; }
        /// <summary>
        /// 绩效描述
        /// </summary>   
        [StringLength(EmployeeWorkPerformance.PerformanceDescMaxLength)]
		public string PerformanceDesc  { get; set; }
    }
}

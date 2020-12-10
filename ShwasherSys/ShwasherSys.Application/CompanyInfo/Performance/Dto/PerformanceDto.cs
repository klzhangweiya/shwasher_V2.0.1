using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.Performance.Dto
{
    [AutoMapTo(typeof(EmployeeWorkPerformance)),AutoMapFrom(typeof(EmployeeWorkPerformance))]
    public class PerformanceDto: EntityDto<int>
    {
        /// <summary>
        /// 编号
        /// </summary>   
		public string PerformanceNo  { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>   
		public int EmployeeId  { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>   
		public string RelatedNo  { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>   
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
		public string PerformanceUnit  { get; set; }
        /// <summary>
        /// 绩效描述
        /// </summary>   
		public string PerformanceDesc  { get; set; }
        public string EmployeeNo  { get; set; }
        public string EmployeeName  { get; set; }
        public DateTime CreationTime  { get; set; }

    }
}
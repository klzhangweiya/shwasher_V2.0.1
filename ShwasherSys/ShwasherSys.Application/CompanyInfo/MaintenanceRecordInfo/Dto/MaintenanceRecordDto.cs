using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto
{
    
    /// <summary>
    /// 机模维护记录
    /// </summary>   
    [AutoMapTo(typeof(MaintenanceRecord)),AutoMapFrom(typeof(MaintenanceRecord))]
    public class MaintenanceRecordDto: EntityDto<string>
    {
        public string DeviceMgPlanNo { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 维护类型
        /// </summary>
        public int MgType { get; set; }
        /// <summary>
        /// 维护内容
        /// </summary>   
		public string Description  { get; set; }
        /// <summary>
        /// 维护地点
        /// </summary>   
		public string Address  { get; set; }
        /// <summary>
        /// 计划时间
        /// </summary>   
		public DateTime PlanDate  { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>   
		public int CompleteState  { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>   
		public DateTime? CompleteDate  { get; set; }
    }
}
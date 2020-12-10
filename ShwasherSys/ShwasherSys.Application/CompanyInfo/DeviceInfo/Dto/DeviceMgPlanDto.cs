using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.DeviceInfo.Dto
{
    
    /// <summary>
    /// 设备维护计划
    /// </summary>   
    [AutoMapTo(typeof(DeviceMgPlan)),AutoMapFrom(typeof(DeviceMgPlan))]
    public class DeviceMgPlanDto: EntityDto<int>
    {
        /// <summary>
        /// 计划编号
        /// </summary>   
		public string No  { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>   
		public string Name  { get; set; }
        
        /// <summary>
        /// 设备编码
        /// </summary>   
        [StringLength(DeviceMgPlan.NoMaxLength)]
        public string DeviceNo  { get; set; }
        
        /// <summary>
        /// 计划类型
        /// </summary>   
        public int PlanType  { get; set; }
        /// <summary>
        /// 维护内容
        /// </summary>   
		public string Description  { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>   
		public DateTime ExpireDate  { get; set; }
        /// <summary>
        /// 维护周期
        /// </summary>   
		public int MaintenanceCycle  { get; set; }
        /// <summary>
        /// 维护时间
        /// </summary>   
		public DateTime MaintenanceDate  { get; set; }
        /// <summary>
        /// 下一次维护时间
        /// </summary>   
		public DateTime NextMaintenanceDate  { get; set; }
    }
}
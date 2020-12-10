using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto
{
    
    /// <summary>
    /// 机模维护记录
    /// </summary>   
    [AutoMapTo(typeof(MaintenanceRecord))]
    public class MaintenanceRecordCreateDto
    {
		public string Id { get; set; }
       
        /// <summary>
        /// 计划编号
        /// </summary>
        [MaxLength(MaintenanceRecord.DeviceNoMaxLength)]
        public string DeviceMgPlanNo { get; set; }
        ///// <summary>
        ///// 设备编号
        ///// </summary>
        //[MaxLength(MaintenanceRecord.DeviceNoMaxLength)]
        //public string DeviceNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(MaintenanceRecord.DeviceNoMaxLength)]
        public string DeviceName { get; set; }
        /// <summary>
        /// 维护类型
        /// </summary>
        public int MgType { get; set; }
        /// <summary>
        /// 维护内容
        /// </summary>   
        [Required] 
        [StringLength(MaintenanceRecord.DescMaxLength)]
		public string Description  { get; set; }
        /// <summary>
        /// 维护地点
        /// </summary>   
        [StringLength(MaintenanceRecord.AddressMaxLength)]
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

using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto
{
    [AutoMapTo(typeof(MaintenanceMember)), AutoMapFrom(typeof(MaintenanceMember))]

    public class MemberDto: EntityDto<string>
    {
        /// <summary>
        /// 维修记录编码
        /// </summary>
        public string MaintenanceNo{ get; set; }
        /// <summary>
        /// 维护员工
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// 维修人员
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 工时
        /// </summary>
        public decimal? WorkHour { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string WorkDesc { get; set; }
        public bool IsConfirm { get; set; }

        public string EmployeeNo { get; set; }
        //public string EmployeeName { get; set; }

    }
}
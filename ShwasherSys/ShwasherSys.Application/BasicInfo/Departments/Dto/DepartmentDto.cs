using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Departments.Dto
{
    [AutoMapTo(typeof(Department)),AutoMapFrom(typeof(Department))]
    public class DepartmentDto: EntityDto<string>
    {
		public string DepartmentName  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
		public string OrderStatusList  { get; set; }
    }
}
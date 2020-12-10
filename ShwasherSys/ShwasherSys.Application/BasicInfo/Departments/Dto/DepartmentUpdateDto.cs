using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Departments.Dto
{
    [AutoMapTo(typeof(Department))]
    public class DepartmentUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(Department.DepartmentNameMaxLength)]
		public string DepartmentName  { get; set; }
        [StringLength(Department.RemarkMaxLength)]
		public string Remark  { get; set; }
		/*public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Department.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        
        [StringLength(Department.IsLockMaxLength)]
		public string IsLock  { get; set; }
        [StringLength(Department.OrderStatusListMaxLength)]
		public string OrderStatusList  { get; set; }*/
    }
}
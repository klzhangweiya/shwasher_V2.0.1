using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;


namespace ShwasherSys.BasicInfo.Departments.Dto
{
    [AutoMapTo(typeof(Department))]
    public class DepartmentCreateDto:Entity<string>
    {
        [Required] 
        [StringLength(Department.DepartmentNameMaxLength)]
		public string DepartmentName  { get; set; }
        [StringLength(Department.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Department.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
  
        [StringLength(Department.IsLockMaxLength)]
		public string IsLock  { get; set; }
        [StringLength(Department.OrderStatusListMaxLength)]
		public string OrderStatusList  { get; set; }
    }
}

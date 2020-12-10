using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Dutys.Dto
{
    [AutoMapTo(typeof(Duty))]
    public class DutyCreateDto
    {
        [Required] 
        [StringLength(Duty.DutyNameMaxLength)]
		public string DutyName  { get; set; }
        [StringLength(Duty.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Duty.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
       
        [StringLength(Duty.IsLockMaxLength)]
		public string IsLock  { get; set; }
    }
}

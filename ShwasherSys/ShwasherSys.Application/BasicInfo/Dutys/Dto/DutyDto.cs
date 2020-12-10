using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Dutys.Dto
{
    [AutoMapTo(typeof(Duty)),AutoMapFrom(typeof(Duty))]
    public class DutyDto: EntityDto<int>
    {
		public string DutyName  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
    }
}
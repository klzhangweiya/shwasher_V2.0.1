using System;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.DeviceInfo.Dto
{
    public class MaintainDto : EntityDto<int>
    {
        public string Address  { get; set; }
        public string Description  { get; set; }
        public DateTime? PlanDate  { get; set; }
       

    }
}
using System;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CompanyInfo.MaintenanceRecordInfo.Dto
{
    public class CompleteDto : EntityDto<string>
    {
        public DateTime? CompleteDate { get; set; }

    }
}
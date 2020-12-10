using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.AuditLog.Dto
{
    [AutoMapTo(typeof(SysLog)), AutoMapFrom(typeof(SysLog))]
    public class SysLogDto : EntityDto<long>
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int ExecutionDuration { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public string BrowserInfo { get; set; }

    }
}

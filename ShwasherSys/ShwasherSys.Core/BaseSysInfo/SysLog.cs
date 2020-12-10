using Abp.Auditing;
using Abp.Extensions;
using IwbZero.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo
{
    public class SysLog : IwbSysLog
    {
        public static SysLog CreateFromAuditInfo(AuditInfo auditInfo, string userName, int logType)
        {
            var exceptionMessage = auditInfo.Exception?.ToString();
            return new SysLog
            {
                //TenantId = auditInfo.TenantId,
                UserId = auditInfo.UserId,
                UserName = userName,
                LogType = logType,
                ServiceName = auditInfo.ServiceName.TruncateWithPostfix(MaxServiceNameLength),
                MethodName = auditInfo.MethodName.TruncateWithPostfix(MaxMethodNameLength),
                Parameters = auditInfo.Parameters.TruncateWithPostfix(MaxParametersLength),
                ExecutionTime = auditInfo.ExecutionTime,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(MaxClientIpAddressLength),
                ClientName = auditInfo.ClientName.TruncateWithPostfix(MaxClientNameLength),
                BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(MaxBrowserInfoLength),
                Exception = exceptionMessage.TruncateWithPostfix(MaxExceptionLength),
                ImpersonatorUserId = auditInfo.ImpersonatorUserId,
                ImpersonatorTenantId = auditInfo.ImpersonatorTenantId,
                CustomData = auditInfo.CustomData.TruncateWithPostfix(MaxCustomDataLength)
            };
        }
    }
   
}

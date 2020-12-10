using System;
using System.Linq;
using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;

namespace IwbZero.Session
{
    public class IwbSession : ClaimsAbpSession, IIwbSession
    {
        public IwbSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {

        }

        public virtual string UserName
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserName);
                return claim?.Value;
            }
        }
        public virtual string RealName
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserName);
                return claim?.Value;
            }
        }
        public virtual int? UserType
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserType);
                if (string.IsNullOrEmpty(claim?.Value))
                    return null;
                return !int.TryParse(claim.Value, out var userType) ? (int?)null : userType;
            }
        }
        public virtual string[] UserRoles
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserRoles);
                return claim?.Value.Split(new char[','], StringSplitOptions.RemoveEmptyEntries);
            }
        }
        public virtual bool? RememberMe
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.RememberMe);
                if (string.IsNullOrEmpty(claim?.Value))
                    return null;
                return !bool.TryParse(claim.Value, out var remmberMe) ? (bool?)null : remmberMe;
            }
        }
        public virtual DateTimeOffset? ExpireTime
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.ExpireTime);
                if (string.IsNullOrEmpty(claim?.Value))
                    return null;
                return !DateTimeOffset.TryParse(claim.Value, out var expireTime) ? (DateTimeOffset?)null : expireTime;
            }
        }
        public virtual string EmailAddress
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.EmailAddress);
                return claim?.Value;
            }
        }

        public virtual string EmployeeNo
        {
            get
            {
                var claim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.EmployeeNo);
                return claim?.Value;
            }
        }
    }

}

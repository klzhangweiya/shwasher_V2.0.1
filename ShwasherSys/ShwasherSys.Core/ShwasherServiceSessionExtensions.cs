using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace ShwasherSys
{
    public static class ShwasherServiceSessionExtensions
    {
        public static string GetClaimValue(this IAbpSession session, string claimTypes)
        {
            ClaimsPrincipal principal = DefaultPrincipalAccessor.Instance.Principal;
            Claim claim = principal?.Claims.FirstOrDefault(c => c.Type == claimTypes);
            return claim?.Value ?? "";
        }
        public static T GetClaimValueEx<T>(this IAbpSession session, string claimTypes) 
        {
            ClaimsPrincipal principal = DefaultPrincipalAccessor.Instance.Principal;
            Claim claim = principal?.Claims.FirstOrDefault(c => c.Type == claimTypes);
            if (string.IsNullOrEmpty(claim?.Value))
            {
                return default(T);
            }
            var result = (T)Convert.ChangeType(claim.Value, typeof(T));
            return result;
        }
    }
}

using System;
using System.Linq;
using Abp.Runtime.Session;

namespace IwbZero.Session
{
    public static class IwbSessionExtensions
    {
        public static string GetUserName(this IAbpSession session)
        {
            var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserName);
            return claim?.Value;
        }
        public static string GetRealName(this IAbpSession session)
        {
            var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.RealName);
            return claim?.Value;
        }
        public static string[] GetUserRoles(this IAbpSession session)
        {
            var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserRoles);
            return claim?.Value.Split(new char[','], StringSplitOptions.RemoveEmptyEntries);
        }
        public static int? GetUserType(this IAbpSession session)
        {
            //var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserType);
            //if (string.IsNullOrEmpty(claim?.Value))
            //    return null;
            //return !int.TryParse(claim.Value, out var userType) ? (int?)null : userType;
            return GetClaimValue<int>(session, IwbClaimTypes.UserType);
        }
        public static int? GetEmailAddresse(this IAbpSession session)
        {
            //var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.UserType);
            //if (string.IsNullOrEmpty(claim?.Value))
            //    return null;
            //return !int.TryParse(claim.Value, out var userType) ? (int?)null : userType;
            return GetClaimValue<int>(session, IwbClaimTypes.EmailAddress);
        }
        public static bool? GetRememberMe(this IAbpSession session)
        {
            //var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.RememberMe);
            //if (string.IsNullOrEmpty(claim?.Value))
            //    return null;
            //return !bool.TryParse(claim.Value, out var remmberMe) ? (bool?)null : remmberMe;
            return GetClaimValue<bool>(session, IwbClaimTypes.RememberMe);
        }
        public static DateTimeOffset? GetExpireTime(this IAbpSession session)
        {
            //var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == IwbClaimTypes.ExpireTime);
            //if (string.IsNullOrEmpty(claim?.Value))
            //    return null;
            //return !DateTime.TryParse(claim.Value, out var expireTime) ? (DateTime?)null : expireTime;
            return GetClaimValue<DateTimeOffset>(session, IwbClaimTypes.ExpireTime);
        }
        public static T? GetClaimValue<T>(this IAbpSession session, string claimTypes)
            where T : struct
        {
            var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == claimTypes);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;
            var result = (T)Convert.ChangeType(claim.Value, typeof(T));
            return result;
        }

        public static string GetClaimValue(this IAbpSession session, string claimTypes)
        {
            var claim = DefaultPrincipalAccessor.Instance.Principal?.Claims.FirstOrDefault(c => c.Type == claimTypes);
            return claim?.Value;
        }
    }

}

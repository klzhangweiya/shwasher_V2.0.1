using System.Security.Claims;

namespace IwbZero.Session
{
    public static class IwbClaimTypes
    {
        /// <summary>
        /// UserId.
        /// Default: <see cref="ClaimTypes.Name"/>
        /// </summary>
        public static string UserName { get; set; } = ClaimTypes.Name;

        /// <summary>
        /// UserId.
        /// Default: <see cref="ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

        /// <summary>
        /// UserId.
        /// Default: <see cref="ClaimTypes.Role"/>
        /// </summary>
        public static string UserRoles { get; set; } = ClaimTypes.Role;

        /// <summary>
        /// TenantId.
        /// Default: http://www.iwbnet.com/identity/claims/realName
        /// </summary>
        public static string RealName { get; set; } = "http://www.iwbnet.com/identity/claims/realName";
        /// <summary>
        /// TenantId.
        /// Default: http://www.iwbnet.com/identity/claims/userType
        /// </summary>
        public static string UserType { get; set; } = "http://www.iwbnet.com/identity/claims/userType";

        /// <summary>
        /// ImpersonatorUserId.
        /// Default: http://www.iwbnet.com/identity/claims/rememberMe
        /// </summary>
        public static string RememberMe { get; set; } = "http://www.iwbnet.com/identity/claims/rememberMe";

        /// <summary>
        /// ImpersonatorTenantId
        /// Default: http://www.iwbnet.com/identity/claims/expireTime
        /// </summary>
        public static string ExpireTime { get; set; } = "http://www.iwbnet.com/identity/claims/expireTime";
        /// <summary>
        /// ImpersonatorTenantId
        /// Default: http://www.iwbnet.com/identity/claims/emailAddress
        /// </summary>
        public static string EmailAddress { get; set; } = "http://www.iwbnet.com/identity/claims/emailAddress";

        public static string EmployeeNo { get; set; } = "http://www.iwbnet.com/identity/claims/EmployeeNo";
        public static string EmployeeName { get; set; } = "http://www.iwbnet.com/identity/claims/EmployeeName";
    }
}

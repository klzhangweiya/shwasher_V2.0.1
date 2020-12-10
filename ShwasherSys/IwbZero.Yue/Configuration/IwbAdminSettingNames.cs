namespace IwbZero.Configuration
{
    public static class IwbAdminSettingNames
    {
        public static class UserManagement
        {
            /// <summary>
            /// "IwbAdmin.UserManagement.IsEmailConfirmationRequiredForLogin".
            /// </summary>
            public const string IsEmailConfirmationRequiredForLogin = "IwbAdmin.UserManagement.IsEmailConfirmationRequiredForLogin";

            public static class UserLockOut
            {
                /// <summary>
                /// "IwbAdmin.UserManagement.UserLockOut.IsEnabled".
                /// </summary>
                public const string IsEnabled = "IwbAdmin.UserManagement.UserLockOut.IsEnabled";

                /// <summary>
                /// "IwbAdmin.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout".
                /// </summary>
                public const string MaxFailedAccessAttemptsBeforeLockout = "IwbAdmin.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout";

                /// <summary>
                /// "IwbAdmin.UserManagement.UserLockOut.DefaultAccountLockoutSeconds".
                /// </summary>
                public const string DefaultAccountLockoutSeconds = "IwbAdmin.UserManagement.UserLockOut.DefaultAccountLockoutSeconds";
            }

            public static class TwoFactorLogin
            {
                /// <summary>
                /// "IwbAdmin.UserManagement.TwoFactorLogin.IsEnabled".
                /// </summary>
                public const string IsEnabled = "IwbAdmin.UserManagement.TwoFactorLogin.IsEnabled";

                /// <summary>
                /// "IwbAdmin.UserManagement.TwoFactorLogin.IsEmailProviderEnabled".
                /// </summary>
                public const string IsEmailProviderEnabled = "IwbAdmin.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

                /// <summary>
                /// "IwbAdmin.UserManagement.TwoFactorLogin.IsSmsProviderEnabled".
                /// </summary>
                public const string IsSmsProviderEnabled = "IwbAdmin.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";

                /// <summary>
                /// "IwbAdmin.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled".
                /// </summary>
                public const string IsRememberBrowserEnabled = "IwbAdmin.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
            }

            public static class PasswordComplexity
            {
                /// <summary>
                /// "IwbAdmin.UserManagement.PasswordComplexity.RequiredLength"
                /// </summary>
                public const string RequiredLength = "IwbAdmin.UserManagement.PasswordComplexity.RequiredLength";

                /// <summary>
                /// "IwbAdmin.UserManagement.PasswordComplexity.RequireNonAlphanumeric"
                /// </summary>
                public const string RequireNonAlphanumeric = "IwbAdmin.UserManagement.PasswordComplexity.RequireNonAlphanumeric";

                /// <summary>
                /// "IwbAdmin.UserManagement.PasswordComplexity.RequireLowercase"
                /// </summary>
                public const string RequireLowercase = "IwbAdmin.UserManagement.PasswordComplexity.RequireLowercase";

                /// <summary>
                /// "IwbAdmin.UserManagement.PasswordComplexity.RequireUppercase"
                /// </summary>
                public const string RequireUppercase = "IwbAdmin.UserManagement.PasswordComplexity.RequireUppercase";

                /// <summary>
                /// "IwbAdmin.UserManagement.PasswordComplexity.RequireDigit"
                /// </summary>
                public const string RequireDigit = "IwbAdmin.UserManagement.PasswordComplexity.RequireDigit";
            }
        }

        public static class OrganizationUnits
        {
            /// <summary>
            /// "IwbAdmin.OrganizationUnits.MaxUserMembershipCount".
            /// </summary>
            public const string MaxUserMembershipCount = "IwbAdmin.OrganizationUnits.MaxUserMembershipCount";
        }
    }
}

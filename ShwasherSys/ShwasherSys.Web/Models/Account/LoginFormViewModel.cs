using Abp.MultiTenancy;

namespace ShwasherSys.Models.Account
{
    public class LoginFormViewModel
    {
        public string ReturnUrl { get; set; }
        public string ErrorMsg { get; set; }
        public bool IsMultiTenancyEnabled { get; set; }

        public bool IsSelfRegistrationAllowed { get; set; }

        public MultiTenancySides MultiTenancySide { get; set; }
    }

}
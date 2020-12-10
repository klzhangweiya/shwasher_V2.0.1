using Abp.Collections;

namespace IwbZero.Configuration
{
    public class IwbUserManagementConfig : IIwbUserManagementConfig
    {
        public ITypeList<object> ExternalAuthenticationSources { get; set; }

        public IwbUserManagementConfig()
        {
            ExternalAuthenticationSources = new TypeList();
        }
    }
}
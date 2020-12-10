using Abp.Collections;

namespace IwbZero.Configuration
{
    /// <summary>
    /// User management configuration.
    /// </summary>
    public interface IIwbUserManagementConfig
    {
        ITypeList<object> ExternalAuthenticationSources { get; set; }
    }
}
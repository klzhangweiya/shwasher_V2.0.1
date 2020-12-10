
namespace IwbZero.Configuration
{
    internal class IwbZeroConfig : IIwbZeroConfig
    {
        public IIwbRoleManagementConfig RoleManagement { get; }

        public IIwbUserManagementConfig IwbUserManagement { get; }

        public IIwbLanguageManagementConfig LanguageManagement { get; }

        public IIwbZeroEntityTypes EntityTypes { get; }


        public IwbZeroConfig(
            IIwbRoleManagementConfig roleManagementConfig,
            IIwbUserManagementConfig userManagementConfig,
            IIwbLanguageManagementConfig languageManagement,
            IIwbZeroEntityTypes entityTypes)
        {
            EntityTypes = entityTypes;
            RoleManagement = roleManagementConfig;
            IwbUserManagement = userManagementConfig;
            LanguageManagement = languageManagement;
        }
    }
}
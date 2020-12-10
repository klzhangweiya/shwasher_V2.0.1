
namespace IwbZero.Configuration
{
    /// <summary>
    /// Configuration options for zero module.
    /// </summary>
    public interface IIwbZeroConfig
    {
        /// <summary>
        /// Gets role management config.
        /// </summary>
        IIwbRoleManagementConfig RoleManagement { get; }

        /// <summary>
        /// Gets user management config.
        /// </summary>
        IIwbUserManagementConfig IwbUserManagement { get; }

        /// <summary>
        /// Gets language management config.
        /// </summary>
        IIwbLanguageManagementConfig LanguageManagement { get; }

        /// <summary>
        /// Gets entity type config.
        /// </summary>
        IIwbZeroEntityTypes EntityTypes { get; }
    }
}
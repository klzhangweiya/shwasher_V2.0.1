using Abp.Configuration.Startup;

namespace IwbZero.Configuration
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// </summary>
    public static class ModuleZeroConfigurationExtensions
    {
        /// <summary>
        /// Used to configure module zero.
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static IIwbZeroConfig IwbZero(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<IIwbZeroConfig>();
        }
    }
}
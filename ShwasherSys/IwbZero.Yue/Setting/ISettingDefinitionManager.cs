using System.Collections.Generic;
using Abp.Configuration;
using Abp.Dependency;

namespace IwbZero.Setting
{
    /// <summary>
    /// Defines setting definition manager.
    /// </summary>
    public interface IIwbSettingDefinitionManager: ITransientDependency
    {
        void Referesh();

        void ChangeSettingDefinition(string name, string value);

        /// <summary>
        /// Gets the <see cref="SettingDefinition"/> object with given unique name.
        /// Throws exception if can not find the setting.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>The <see cref="SettingDefinition"/> object.</returns>
        SettingDefinition GetSettingDefinition(string name);

        /// <summary>
        /// Gets a list of all setting definitions.
        /// </summary>
        /// <returns>All settings.</returns>
        IReadOnlyList<SettingDefinition> GetAllSettingDefinitions();
    }
}

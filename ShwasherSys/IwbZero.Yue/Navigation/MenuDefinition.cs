using System;
using System.Collections.Generic;
using Abp.Application.Navigation;
using Abp.Localization;

namespace IwbZero.Navigation
{
    /// <summary>
    /// Represents a navigation menu for an application.
    /// </summary>
    public class IwbMenuDefinition : IIwbHasMenuItemDefinitions

    {
        /// <summary>
        /// Unique name of the menu in the application. Required.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display name of the menu. Required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Display name of the menu. Required.
        /// </summary>
        public ILocalizableString LocalizableDisplayName { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this menu. Optional.
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Menu items (first level).
        /// </summary>
        public List<IwbMenuItemDefinition> Items { get; set; }

        /// <summary>
        /// Creates a new <see cref="MenuDefinition"/> object.
        /// </summary>
        /// <param name="name">Unique name of the menu</param>
        /// <param name="displayName">Display name of the menu</param>
        /// <param name="localizableDisplayName">Display name of the menu</param>
        /// <param name="customData">Can be used to store a custom object related to this menu.</param>
        public IwbMenuDefinition(string name, string displayName, ILocalizableString localizableDisplayName = null, object customData = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Menu name can not be empty or null.");
            }



            Name = name;
            DisplayName = displayName;
            LocalizableDisplayName = localizableDisplayName;
            CustomData = customData;

            Items = new List<IwbMenuItemDefinition>();
        }

        /// <summary>
        /// Adds a <see cref="MenuItemDefinition"/> to <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="MenuItemDefinition"/> to be added</param>
        /// <returns>This <see cref="MenuDefinition"/> object</returns>
        public IwbMenuDefinition AddItem(IwbMenuItemDefinition menuItem)
        {
            Items.Add(menuItem);
            return this;
        }

        /// <summary>
        /// Remove menu item with given name
        /// </summary>
        /// <param name="name"></param>
        public void RemoveItem(string name)
        {
            Items.RemoveAll(m => m.Name == name);
        }
    }
}

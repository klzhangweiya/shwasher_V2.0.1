using System.Collections.Generic;
using Abp.Application.Navigation;
using Abp.Localization;

namespace IwbZero.Navigation
{
    public class IwbUserMenuItem : UserMenuItem
    {
        public new IList<IwbUserMenuItem> Items { get; set; }

        public IwbUserMenuItem(IwbMenuItemDefinition menuItemDefinition)
        {
            Name = menuItemDefinition.Name;
            Icon = menuItemDefinition.Icon;
            DisplayName = menuItemDefinition.DisplayName;
            Order = menuItemDefinition.Order;
            Url = menuItemDefinition.Url;
            CustomData = menuItemDefinition.CustomData;
            Target = menuItemDefinition.Target;
            IsEnabled = menuItemDefinition.IsEnabled;
            IsVisible = menuItemDefinition.IsVisible;
            Items = new List<IwbUserMenuItem>();
        }
        public IwbUserMenuItem(IwbMenuItemDefinition menuItemDefinition, ILocalizationContext localizationContext)
        {
            Name = menuItemDefinition.Name;
            Icon = menuItemDefinition.Icon;
            DisplayName = menuItemDefinition.LocalizableDisplayName.Localize(localizationContext);
            Order = menuItemDefinition.Order;
            Url = menuItemDefinition.Url;
            CustomData = menuItemDefinition.CustomData;
            Target = menuItemDefinition.Target;
            IsEnabled = menuItemDefinition.IsEnabled;
            IsVisible = menuItemDefinition.IsVisible;
            Items = new List<IwbUserMenuItem>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Localization;

namespace IwbZero.Navigation
{
    public class IwbUserMenu : UserMenu
    {
        public new IList<IwbUserMenuItem> Items { get; set; }

        public IwbUserMenu(IwbMenuDefinition menuDefinition)
        {
            Name = menuDefinition.Name;
            DisplayName = menuDefinition.DisplayName;
            CustomData = menuDefinition.CustomData;
            Items = new List<IwbUserMenuItem>();

        }
        public IwbUserMenu(IwbMenuDefinition menuDefinition, ILocalizationContext localizationContext)
        {
            Name = menuDefinition.Name;
            DisplayName = menuDefinition.LocalizableDisplayName.Localize(localizationContext);
            CustomData = menuDefinition.CustomData;
            Items = new List<IwbUserMenuItem>();

        }
    }
}

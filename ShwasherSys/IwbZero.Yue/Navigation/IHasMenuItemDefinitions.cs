using System.Collections.Generic;

namespace IwbZero.Navigation
{
    public interface IIwbHasMenuItemDefinitions
    {
        /// <summary>
        /// List of menu items.
        /// </summary>
        List<IwbMenuItemDefinition> Items { get; }
    }
}

using System.Collections.Generic;
using IwbZero.Navigation;

namespace ShwasherSys.Models.Layout
{
    public class SideBarNavViewModel
    {
        public IwbUserMenu MainMenu { get; set; }
        public List<string> ActiveNames { get; set; }
        public string PageTitle { get; set; }
    }
}
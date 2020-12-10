using System.Collections.Generic;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.Models.Layout
{
    public class PermissionButtonViewModel
    {
        public PermissionButtonViewModel()
        {
        }

        public string Name { get; private set; }
        public string FunctionNo { get; private set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public string Script { get; set; }
        public int MenuType { get; set; }
        public string Description { get; set; }
        public IList<PermissionButtonViewModel> Children { get; set; }

        public PermissionButtonViewModel(SysFunction function)
        {
            Name = function.PermissionName;
            FunctionNo = function.FunctionNo;
            DisplayName = function.FunctionName;
            Url = function.Url;
            Icon = function.Icon;
            Class = function.Class;
            Script = function.Script;
            MenuType = function.FunctionType;
            Children = new List<PermissionButtonViewModel>();
        }
    }
}
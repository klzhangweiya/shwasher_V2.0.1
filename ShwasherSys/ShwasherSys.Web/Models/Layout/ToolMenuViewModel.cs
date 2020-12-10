
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.Models.Layout
{
    public class ToolMenuViewModel
    {
        public ToolMenuViewModel()
        {
        }

        public ToolMenuViewModel(SysFunction function)
        {
            Name = function.PermissionName;
            FunctionNo = function.FunctionNo;
            DisplayName = function.FunctionName;
            Url = function.Url;
            Icon = function.Icon;
            Class = function.Class;
            Script = function.Script;
            MenuType = function.FunctionType;
        }
        public ToolMenuViewModel(string name, string functionNo, string displayName, string url, string icon, string @class, string script, int menuType, string description)
        {
            Name = name;
            FunctionNo = functionNo;
            DisplayName = displayName;
            Url = url;
            Icon = icon;
            Class = @class;
            Script = script;
            MenuType = menuType;
            Description = description;
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
    }
}
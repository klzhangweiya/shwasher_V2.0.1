using System.Collections.Generic;
using System.Web.Mvc;

namespace ShwasherSys.Views.Shared.New.SearchForm
{
    public class SearchItemSelect
    {
        public SearchItemSelect(string filed, List<SelectListItem> items, bool isAddBlank)
        {
            Filed = filed;
            Items = items;
            IsAddBlank = isAddBlank;
            ItemStrs = IsAddBlank ? "<option value=\'\' selected>请选择</option>" : "";
            foreach (var item in Items)
            {
                ItemStrs += $"<option value=\'{item.Value}\'>{item.Text}</option>";
            }
        }
        public SearchItemSelect(string filed, string itemStr, bool isAddBlank)
        {
            Filed = filed;
            IsAddBlank = isAddBlank;
            ItemStrs = (isAddBlank ? "<option value=\'\' selected>请选择</option>" : "") + itemStr;
        }
        public string Filed { get; set; }
        public bool IsAddBlank { get; set; }
        public List<SelectListItem> Items { get; set; }
        public string ItemStrs { get; }
    }
}
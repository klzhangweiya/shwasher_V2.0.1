using System.Collections.Generic;
using ShwasherSys.Models.Layout;

namespace ShwasherSys.Views.Shared.New.Table
{

    public class TableViewModel
    {

        public TableViewModel(string url, string activeMenu, SearchFormViewModal searchForm, string tableId = "table",bool hasBox=true)
        {
            TableId = tableId;
            Url = url;
            ActiveMenu = activeMenu;
            SearchForm = searchForm;
            SingleSelect = true;
            PageArray = new[] { 25, 50, 100, 200 };
            HasBox = hasBox;
            Other = "";
        }

        public TableViewModel(string url, string activeMenu=null, string tableId = "table", bool hasBox = true)
            : this(url, activeMenu, null, tableId, hasBox)
        {
            TableId = tableId;
            Url = url;
            ActiveMenu = activeMenu;
            SingleSelect = true;
            PageArray = new[] { 25, 50, 100, 200 };
            Other = "";
        }

        public TableViewModel(string url, string activeMenu, SearchFormViewModal searchForm, List<FieldItem> fieldItems,
            string tableId = "table")
            : this(url, activeMenu, searchForm, tableId)
        {
            TableId = tableId;
            Url = url;
            ActiveMenu = activeMenu;
            FieldItems = fieldItems;
            SearchForm = searchForm;

        }

        public SearchFormViewModal SearchForm { get; set; }
        public string TableId { get; set; }
        public string Url { get; set; }
        public bool HasBox { get; set; }
        public string ActiveMenu { get; set; }
        public int[] PageArray { get; set; }
        public string PageArrayStr => string.Join(",", PageArray);
        public bool SingleSelect { get; set; }
        public string SingleSelectStr => SingleSelect ? "true" : "false";
        public string Other { get; set; }
        public string RowAttributes { get; set; }
        public List<FieldItem> FieldItems { get; set; }

        public TableViewModel SetPageArray(params int[] page)
        {
            PageArray = page;
            return this;
        }
        public TableViewModel SetMultipleSelect()
        {
            SingleSelect = false;
            return this;
        }
        public TableViewModel SetOther(string other)
        {
            Other = other ?? "";
            return this;
        }
        public TableViewModel SetFields(List<FieldItem> filedItems)
        {
            FieldItems = filedItems;
            return this;
        }
        public TableViewModel AddFields(FieldItem fieldItem)
        {
            FieldItems = FieldItems ?? new List<FieldItem>();
            FieldItems.Add(fieldItem);
            return this;
        }

    }

    public class FieldItem
    {
        public FieldItem(string filed, string displayName, string formatter = "", string align = "center",bool isTip=false,bool isSort=true)
        {
            Filed = filed;
            DisplayName = displayName;
            Formatter = formatter;
            Align = align;
            IsTip = isTip;
            IsSort = isSort;
        }
        public FieldItem SetWidth(int width,string unit="px")
        {
            Width = width;
            WidthUnit = unit;
            return this;
        }
        public FieldItem SetTip()
        {
            IsTip = true;
            return this;
        }
        public FieldItem SetNotSort()
        {
            IsSort = false;
            return this;
        }
        public string Filed { get; set; }
        public string DisplayName { get; set; }
        public string Formatter { get; set; }
        public string Align { get; set; }
        public int? Width { get; set; }
        public string WidthUnit { get; set; }
        public string WidthStr => Width != null ? $" data-width=\"{Width}{WidthUnit}\"" : "";
        public bool IsSort { get; set; }
        public string IsSortStr => IsSort ? " data-sortable=\"true\"" : "";
        private bool IsTip { get; set; }
        public string IsTipStr => IsTip ? "data-class=\"iwb-tips\"" : "";

    }
}

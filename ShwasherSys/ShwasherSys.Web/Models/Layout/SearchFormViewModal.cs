using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShwasherSys.Models.Layout
{


    public class SearchFormViewModal
    {
        public SearchFormViewModal(List<SearchItem> searchItems,string formId="search-form", bool isSingle = false)
        {
            SearchItems = searchItems;
            IsSingle = isSingle;
            FormId = formId;
        }
        public SearchFormViewModal(List<SearchItem> searchItems, bool isSingle )
        {
            SearchItems = searchItems;
            IsSingle = isSingle;
            FormId = "search-form";
        }
        public bool IsSingle { get; set; }
        public string FormId { get; set; }
        public List<SearchItem> SearchItems { get; set; }
    }

    public class SearchItem
    {
        public SearchItem(string field, string displayName, FiledType filedType = FiledType.S, ExpType expType = ExpType.Contains,string value="", string formId = "")
        {
            Field = field;
            DisplayName = displayName;
            FiledType = filedType;
            ExpType = expType;
            if (FiledType != FiledType.S)
            {
                ExpType = ExpType == ExpType.Contains ? ExpType.Equal : expType;
            }
            SelectItems = new List<SearchItemSelect>();
            Value = value;
            FormId = formId;
        }
        public string FormId { get; set; }
        public string Field { get; set; }
        public string DisplayName { get; set; }
        public string SearchModalId { get; set; }
        public FiledType FiledType { get; set; }
        public ExpType ExpType { get; set; }
        public List<SearchItemSelect> SelectItems { get; set; }
        public string Value { get; set; }

        public string SelectItemStr
        {
            get
            {
                if (!SelectItems.Any())
                {
                    return "";
                }
                string str = "";
                foreach (var searchItemSelect in SelectItems)
                {
                    str += $"\"{searchItemSelect.Filed}\":\"<option value=\\\'\\\' selected>请选择</option>";
                    foreach (var item in searchItemSelect.Items)
                    {
                        str += $"<option value=\\\'{item.Value}\\\'>{item.Text}</option>";
                    }
                    str += "\"";
                }
                return str;
            }
        }
        private string _target;
        public string Target => !string.IsNullOrEmpty(_target) ? _target : "";
        public SearchItem SetSearchIcon(string modalId,string target = null)
        {
            SearchModalId = modalId;
            _target = target;
            return this;
        }
        public SearchItem SetSearchItem(List<SelectListItem> items, string filed = null,bool isAddBlank=true)
        {
            filed = filed ?? Field;
            SelectItems.Add(new SearchItemSelect(filed, items,isAddBlank));
            ExpType = ExpType == ExpType.Contains ? ExpType.Equal : ExpType;
            return this;
        }
       
    }

    public class SearchItemSelect
    {
        public SearchItemSelect(string filed, List<SelectListItem> items)
        {
            Filed = filed;
            Items = items;
            IsAddBlank = true;
        }
        public SearchItemSelect(string filed, List<SelectListItem> items,bool isAddBlank)
        {
            Filed = filed;
            Items = items;
            IsAddBlank = isAddBlank;
        }
        public bool IsAddBlank { get; set; }
        public string Filed { get; set; }
        public List<SelectListItem> Items { get; set; }
    }
    public enum FiledType
    {
        /// <summary>
        /// string
        /// </summary>
        S = 0,
        /// <summary>
        /// int
        /// </summary>
        I = 1,
        /// <summary>
        /// int?
        /// </summary>
        Inull = 2,
        /// <summary>
        /// bool
        /// </summary>
        B = 3,
        /// <summary>
        /// bool?
        /// </summary>
        Bnull = 4,
        /// <summary>
        /// Datetime
        /// </summary>
        D = 5,
        /// <summary>
        /// Datetime?
        /// </summary>
        Dnull = 6,
        /// <summary>
        /// Decimal
        /// </summary>
        Dec = 7,
        /// <summary>
        /// Decimal?
        /// </summary>
        DecNull = 8
    }
    public enum ExpType
    {
        Equal
        , NotEqual
        , Greater
        , Less
        , GreaterOrEqual
        , LessOrEqual
        , Contains
        , NotContains

    }
}
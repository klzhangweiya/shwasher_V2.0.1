using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShwasherSys.Views.Shared.New.SearchForm
{
    public class SearchItem
    {
        public SearchItem(string field, string displayName, FieldType fieldType = FieldType.S, ExpType expType = ExpType.Contains, string searchModalId = "", string other = "", string formId = "", bool isShow = true, bool isOnlyView = false)
        {
            Field = field;
            DisplayName = displayName;
            FieldType = fieldType;
            ExpType = expType;
            SearchModalId = searchModalId;
            if (FieldType != FieldType.S)
            {
                ExpType = ExpType == ExpType.Contains ? ExpType.Equal : expType;
            }
            SelectItems = new List<SearchItemSelect>();
            DateItem = new DateItem();
            Other = other;
            FormId = formId;
            IsOnlyView = isOnlyView;
            IsShow = isShow;
            IsTree = false;
        }
        public string FormId { get; set; }
        public string Field { get; set; }
        public string DisplayName { get; set; }
        public string SearchModalId { get; set; }
        public string Other { get; set; }
        private string _target;
        public string Target => !string.IsNullOrEmpty(_target) ? _target : string.IsNullOrEmpty(FormId) ? "search-form" : FormId;

        public string Value { get; set; }
        public bool IsShow { get; set; }
        public bool IsTree { get; set; }
        public string ShowId { get; set; }

        /// <summary>
        /// 是否参与后台查询，只前台显示
        /// </summary>
        public bool IsOnlyView { get; set; }
        public FieldType FieldType { get; set; }
        public ExpType ExpType { get; set; }
        public List<SearchItemSelect> SelectItems { get; set; }
        public SearchItem SetSearchIcon(string modalId, string showId = null, string target = null)
        {
            SearchModalId = modalId;
            ShowId = showId;
            _target = target;
            return this;
        }
        public SearchItem SetValue(string value)
        {
            Value = value;
            return this;
        }
        public SearchItem SetDateItem(DateItem dateOption)
        {
            DateItem = dateOption;
            return this;
        }
        public SearchItem SetDateItem(int interval)
        {
            DateItem = new DateItem(interval);
            return this;
        }
        public DateItem DateItem { get; set; }

        public string SelectItemStr
        {
            get
            {
                if (!SelectItems.Any())
                {
                    return "";
                }
                string str = "";
                foreach (var itemSelect in SelectItems)
                {
                    str += $"\"{itemSelect.Filed}\":\"{itemSelect.ItemStrs}\"";
                }
                return str;
            }
        }

        public SearchItem SetSelectItem(List<SelectListItem> items, bool isAddBlank = true, bool isTree = false, string filed = null)
        {
            filed = filed ?? Field;
            SelectItems.Add(new SearchItemSelect(filed, items, isAddBlank));
            ExpType = ExpType == ExpType.Contains ? ExpType.Equal : ExpType;
            IsTree = isTree;
            return this;
        }

        public SearchItem SetSelectItem(string itemStr, bool isAddBlank = true, bool isTree = false, string filed = null)
        {
            filed = filed ?? Field;
            SelectItems.Add(new SearchItemSelect(filed, itemStr, isAddBlank));
            ExpType = ExpType == ExpType.Contains ? ExpType.Equal : ExpType;
            IsTree = isTree;
            return this;
        }
    }
}
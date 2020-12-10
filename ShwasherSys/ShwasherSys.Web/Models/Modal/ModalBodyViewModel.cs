using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Extensions;
using Abp.Localization;

namespace ShwasherSys.Models.Modal
{
    public class ModalBodyViewModel
    {
        public ModalBodyViewModel(List<InputViewModel> inputs, string formId = "form", List<SpecialInputModel> specials = null)
        {
            FormId = formId;
            Inputs = inputs;
            Specials = specials;
        }
        public string FormId { get; set; }
        public string ModalId { get; set; }
        public List<InputViewModel> Inputs { get; set; }
        public List<SpecialInputModel> Specials { get; set; }
    }

    public class InputViewModel
    {
        public InputViewModel(string id, InputTypes types = InputTypes.Text, string displayName = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "", string searchModalId = "",string name="")
        {

            Id = id;
            DisplayName = displayName.IsNullOrEmpty() ? L(id) : displayName;
            Types = types;
            Value = value;
            _class = @class;
            DataOptions = dataOptions;
            Events = events;
            Styles = styles;
            Other = other;
            SearchModalId = searchModalId;
            IsHidden = hide;
            IsRequired = !hide;
            IsDisabled = false;
            IsReadOnly = false;
            _lable = label;
            Name = string.IsNullOrEmpty(name) ? id : name;
        }
        #region 方法
        public InputViewModel SetDataOptions(string dataOptions)
        {
            DataOptions = dataOptions;
            return this;
        }
        public InputViewModel SetEvents(string events)
        {
            Events = events;
            return this;
        }
        public InputViewModel SetHidden()
        {
            IsHidden = true;
            return this;
        }
        public InputViewModel SetNotRequired()
        {
            IsRequired = false;
            return this;
        }
        public InputViewModel SetDisabled()
        {
            IsDisabled = true;
            return this;
        }
        public InputViewModel SetReadOnly()
        {
            IsReadOnly = true;
            return this;
        }
        public InputViewModel SetSelectOptions(List<SelectListItem> poSelectOptions)
        {
            string lcRetval = "";

            if (poSelectOptions != null && poSelectOptions.Any())
            {
                foreach (var s in poSelectOptions)
                {
                    lcRetval += "<option value=\"" + s.Value + "\">" + s.Text + "</option>\r\n";
                }
            }
            SelectOptions = lcRetval;
            return this;
        }
        public InputViewModel SetSelectOptions(string options)
        {
            SelectOptions = options;
            return this;
        }
        public InputViewModel SetOuterDiv(string classStr, string other = "")
        {
            DivClass = classStr;
            DivOther = other;
            return this;
        }

        /// <summary>
        /// 外层上面其他标签
        /// </summary>
        /// <param name="beforeStr"></param>
        /// <returns></returns>
        public InputViewModel SetOuterBefore(string beforeStr)
        {
            DivOutBefore = beforeStr;
            return this;
        }

        /// <summary>
        /// 外层下面其他标签
        /// </summary>
        /// <param name="afterStr"></param>
        /// <returns></returns>
        public InputViewModel SetOuterAfter(string afterStr)
        {
            DivOutAfter = afterStr;
            return this;
        }

        /// <summary>
        /// 内层上面其他标签
        /// </summary>
        /// <param name="beforeStr"></param>
        /// <returns></returns>
        public InputViewModel SetInterBefore(string beforeStr)
        {
            DivInterBefore = beforeStr;
            return this;
        }

        /// <summary>
        /// 内层下面其他标签
        /// </summary>
        /// <param name="afterStr"></param>
        /// <returns></returns>
        public InputViewModel SetInterAfter(string afterStr)
        {
            DivInterAfter = afterStr;
            return this;
        }

        /// <summary>
        /// 内层下面其他标签
        /// </summary>
        /// <param name="searchModalId"></param>
        /// <returns></returns>
        public InputViewModel SetSearchIcon(string searchModalId,string targetDom="")
        {
            SearchModalId = searchModalId;
            TargetDom = targetDom;
            return this;
        }

        #endregion

        #region 字段属性

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public InputTypes Types { get; set; }

        public string Value { get; set; }
        public string SelectOptions { get; private set; }

        private string _class;
        public string Class
        {
            get => "form-control " + _class;
            set => _class = value;
        }
        public string DataOptions { get; set; }
        public string Events { get; set; }
        public string Styles { get; set; }
        public string Other { get; set; }
        public bool IsHidden { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsReadOnly { get; set; }
        public string TypeStr
        {
            get
            {
                switch (Types)
                {
                    case InputTypes.Text:
                        return "text";
                    case InputTypes.Password:
                        return "password";
                    case InputTypes.Checkbox:
                        return "checkbox";
                    case InputTypes.Radio:
                        return "radio";
                    case InputTypes.File:
                        return "file";
                    default:
                        return "text";
                }
            }
        }
        public string Required => IsHidden ? "" : (IsRequired ? " required" : "");

        public string DivClass { get; set; }
        public string DivOther { get; set; }
        public string DivOutBefore { get; set; }
        public string DivOutAfter { get; set; }

        public string DivInterBefore { get; set; }
        public string DivInterAfter { get; set; }
        public string SearchModalId { get; set; }

        private readonly string _lable;
        public string Lable => IsHidden
            ? ""
            : string.IsNullOrEmpty(_lable)
                ? (IsRequired
                    ? "<label class=\"iwb-label col-md-2 control-label iwb-label-required\" for=\"" + Id + "\">" +
                      DisplayName + "</label>"
                    : "<label class=\"iwb-label col-md-2 control-label\" for=\"" + Id + "\">" + DisplayName +
                      "</label>")
                : _lable;

        public string Disabled => IsDisabled ? "disabled=\"disabled\"" : "";
        public string ReadOnly => IsReadOnly ? "readonly=\"readonly\"" : "";

        public string Name { get; set; }

        public string  TargetDom { get; set; }

        #endregion
        private static string L(string name)
        {
            var str = LocalizationHelper.GetSource(ShwasherConsts.LocalizationSourceName).GetString(name);
            return str;
        }

    }

    public class SpecialInputModel
    {
        public string Id { get; set; }
        public string InputStr { get; set; }

    }

    //public class SelectOption
    //{
    // public SelectOption(string text, string value = "")
    // {
    //  Text = text;
    //  Value = value;
    // }
    //    public string Value { get; set; }
    //    public string Text { get; set; }

    //}

    public enum InputTypes
    {
        Text, List, Checkbox, Radio, Password, Textarea, File
    }

    
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Localization;
using IwbZero;

namespace ShwasherSys.Views.Shared.New.Modals
{
    #region ModalBody

    public class Input
    {
        public Input(string id, string displayName = "", InputTypes inputType = InputTypes.Text, string placeholder = "", string name = null, string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "", string help = "")
        {
            Id = id;
            Name = string.IsNullOrEmpty(name) ? id : name;
            DisplayName = string.IsNullOrEmpty(displayName) ? L(id) : displayName;
            InputType = inputType;
            _placeholder = placeholder;
            Value = value;
            _class = @class;
            DataOptions = dataOptions;
            Events = events;
            Styles = styles;
            Other = other;
            IsHidden = hide;
            IsRequired = !hide;
            IsDisabled = false;
            IsReadOnly = false;
            _label = label;
            Help = help;
            IsSm = true;
        }
        #region 方法
        public Input SetDataOptions(string dataOptions)
        {
            DataOptions = dataOptions;
            return this;
        }
        public Input SetEvents(string events)
        {
            Events = events;
            return this;
        }
        public Input SetName(string name)
        {
            Name = name;
            return this;
        }
        public Input SetHelp(string help)
        {
            Help = help;
            return this;
        }
        public Input SetHidden()
        {
            IsHidden = true;
            return this;
        }
        public Input SetLabel(string label)
        {
            _label = label;
            return this;
        }
        public Input SetLabelName(string label)
        {
            _label = "<label class=\"iwb-label col-md-2 form-control-label\" for=\"" + Id + "\">" + label +
                     "</label>";
            return this;
        }
        public Input SetLabelNameRequired(string label)
        {
            _label = "<label class=\"iwb-label col-md-2 form-control-label iwb-label-required\" for=\"" + Id + "\">" + label +
                     "</label>";
            return this;
        }
        public Input SetNotRequired()
        {
            IsRequired = false;
            return this;
        }
        public Input SetDisabled()
        {
            IsDisabled = true;
            return this;
        }
        public Input SetReadonly()
        {
            IsReadOnly = true;
            return this;
        }
        public Input SetSelectOptions(List<SelectListItem> poSelectOptions, bool isMultiple = false, bool isAddBlank = true)
        {
            string lcRetval = isAddBlank ? "<option value=\"\" selected>请选择</option>" : "";
            IsMultiple = isMultiple;
            if (poSelectOptions != null && poSelectOptions.Any())
            {
                foreach (var s in poSelectOptions)
                {
                    lcRetval += "<option value=\"" + s.Value + "\">" + s.Text + "</option>\r\n";
                }
            }
            SelectOptions = lcRetval;
            InputType = InputTypes.List;
            return this;
        }

        public Input SetSelectOptions(string poSelectOptions, bool isMultiple = false, bool isAddBlank = true)
        {
            string lcRetval = isAddBlank ? "<option value=\"\" selected>请选择</option>" : "";
            IsMultiple = isMultiple;
            if (!string.IsNullOrEmpty(poSelectOptions))
            {
                lcRetval += poSelectOptions;
            }
            SelectOptions = lcRetval;
            InputType = InputTypes.List;
            return this;
        }
        public Input SetLayout(string labelLayout, string inputLayout)
        {
            LabelLayoutClass = labelLayout;
            InputLayoutClass = inputLayout;
            return this;
        }
        //public Input SetLayout(int labelLayout, int? inputLayout = null, string layoutPrefix = "col-md-")
        //{
        //    inputLayout = inputLayout ?? 12 - labelLayout;
        //    LabelLayout = $"{layoutPrefix}{labelLayout}";
        //    InputLayout = $"{layoutPrefix}{inputLayout}";
        //    return this;
        //}
        public Input SetOuterDiv(string classStr, string other = "")
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
        public Input SetOuterBefore(string beforeStr)
        {
            DivOutBefore = beforeStr;
            return this;
        }

        /// <summary>
        /// 外层下面其他标签
        /// </summary>
        /// <param name="afterStr"></param>
        /// <returns></returns>
        public Input SetOuterAfter(string afterStr)
        {
            DivOutAfter = afterStr;
            return this;
        }

        /// <summary>
        /// 内层上面其他标签
        /// </summary>
        /// <param name="beforeStr"></param>
        /// <returns></returns>
        public Input SetInterBefore(string beforeStr)
        {
            DivInterBefore = beforeStr;
            return this;
        }

        /// <summary>
        /// 内层下面其他标签
        /// </summary>
        /// <param name="afterStr"></param>
        /// <returns></returns>
        public Input SetInterAfter(string afterStr)
        {
            DivInterAfter = afterStr;
            return this;
        }

        /// <summary>
        /// 添加搜索按钮
        /// </summary>
        /// <param name="searchModalId"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public Input SetSearchIcon(string searchModalId, string target = "")
        {
            SearchModalId = searchModalId;
            _target = target;
            return this;
        }

        /// <summary>
        /// 输入提示
        /// </summary>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public Input SetPlaceholder(string placeholder)
        {
            _placeholder = placeholder;
            return this;
        } 
        
        public Input SetNotSmall()
        {
            IsSm = false;
            return this;
        } 
        
        #endregion

        #region 字段属性

        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Help { get; set; }
        private string PlaceholderPrefix => InputType == InputTypes.List ? L("PlaceholderSelectHeader") : L("PlaceholderHeader");
        private string _placeholder;
        public string Placeholder => string.IsNullOrEmpty(_placeholder) ? $"{PlaceholderPrefix}{DisplayName}..." : _placeholder;
        public InputTypes InputType { get; set; }
        public string TypeStr
        {
            get
            {
                switch (InputType)
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
                    case InputTypes.Number:
                        return "number";
                    default:
                        return "text";
                }
            }
        }
        public DateTypes DateType { get; set; }


        public bool IsSm { get; set; }
        public string Number { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public string Value { get; set; }
        public string SelectOptions { get; private set; }

        private string _class;
        public string Class
        {
            get
            {
                var classStr = $"form-control {_class}";
                if (IsDisabled)
                {
                    classStr += " disabled ";
                }

                if (IsReadOnly)
                {
                    classStr += " readonly ";
                }

                if (InputType == InputTypes.Number)
                {
                    classStr += $" {Number} ";

                }
                if (InputType == InputTypes.Date)
                {
                    classStr += DateType == DateTypes.DateTime ? " iwb-date-time " : " iwb-date ";
                }
                return classStr;
            }
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
        public bool IsMultiple { get; private set; }

        public string Required => IsHidden ? "" : (IsRequired ? " required" : "");
        public string DivClass { get; set; }
        public string DivOther { get; set; }
        public string DivOutBefore { get; set; }
        public string DivOutAfter { get; set; }

        public string DivInterBefore { get; set; }
        public string DivInterAfter { get; set; }
        public string SearchModalId { get; set; }

        public string LabelLayoutClass { get; set; }
        public string InputLayoutClass { get; set; }

        private string _label;

        public string Label => IsHidden
            ? ""
            : string.IsNullOrEmpty(_label)
                ? (IsRequired
                    ? $"<label class=\"iwb-label {LabelLayoutClass} control-label iwb-label-required\" for=\"" + Id + "\">" +
                      DisplayName + "</label>"
                    : $"<label class=\"iwb-label {LabelLayoutClass} control-label\" for=\"" + Id + "\">" + DisplayName +
                      "</label>")
                : _label;

        public string Disabled => IsDisabled ? "disabled=\"disabled\"" : "";
        public string ReadOnly => IsReadOnly ? "readonly=\"readonly\"" : "";
        public FileInputOption FileOption { get; set; }
        private string _target;
        public string Target =>
            string.IsNullOrEmpty(_target) ? DefaultTarget ?? "" :
            _target.StartsWith(".") ? _target :
            _target.StartsWith("#") ? _target : $"#{_target}";

        public string DefaultTarget { get; set; }

        #endregion
        private static string L(string name)
        {
            var str = LocalizationHelper.GetSource(ShwasherConsts.LocalizationSourceName).GetString(name);
            return str;
        }

    }
    public class InputHide : Input
    {
        public InputHide(string id, string displayName = "", string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", string label = "")
            : base(id, displayName, InputTypes.Textarea, placeholder, name, value, @class, dataOptions, events, styles, other, true, label)
        {

        }
    }
    public class InputTextarea : Input
    {
        public InputTextarea(string id, string displayName = "", string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, InputTypes.Textarea, placeholder, name, value, @class, dataOptions, events, styles, other, hide, label)
        {

        }
    }
    public class InputKindeditor : InputTextarea
    {
        public InputKindeditor(string id, string displayName = "", string placeholder = "", string name = "", string value = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, placeholder, name, value, "kindeditor", dataOptions, events, styles, other, hide, label)
        {

        }
    }
    public class InputDate : Input
    {
        public InputDate(string id, string displayName = "", DateTypes date = DateTypes.Date, string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, InputTypes.Date, placeholder, name, value, @class, dataOptions, events, styles, other, hide, label)
        {
            DateType = date;
        }
        public InputDate SetDateType(DateTypes date)
        {
            DateType = date;
            return this;
        }

    }
    public class InputDateTime : Input
    {
        public InputDateTime(string id, string displayName = "", DateTypes date = DateTypes.DateTime, string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, InputTypes.Date, placeholder, name, value, @class, dataOptions, events, styles, other, hide, label)
        {
            DateType = date;
        }
        public InputDateTime SetDateType(DateTypes date)
        {
            DateType = date;
            return this;
        }

    }
    public class InputNumber : Input
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayName"></param>
        /// <param name="numberType">0是整数，其他是小数</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="placeholder"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="class"></param>
        /// <param name="dataOptions"></param>
        /// <param name="events"></param>
        /// <param name="styles"></param>
        /// <param name="other"></param>
        /// <param name="hide"></param>
        /// <param name="label"></param>
        public InputNumber(string id, string displayName = "", int numberType = 0, int? min = null, int? max = null, string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, InputTypes.Number, placeholder, name, value, @class, dataOptions, events, styles, other, hide, label)
        {
            Number = numberType == 0 ? "number" : "digits";
            Min = min;
            Max = max;
        }

        public InputNumber SetRange(int? min = null, int? max = null, int numberType = 1)
        {
            Min = min;
            Max = max;
            Number = numberType == 0 ? "number" : "digits";
            return this;
        }

        public InputNumber SetNumberType(int numberType = 1, int? min = null, int? max = null)
        {
            Number = numberType == 0 ? "number" : "digits";
            Min = min;
            Max = max;
            return this;
        }
        public InputNumber SetMin(int min)
        {
            Min = min;
            return this;
        }
        public InputNumber SetMax(int max)
        {
            Max = max;
            return this;
        }


    }

    public class InputFile : Input
    {
        public InputFile(string id, string displayName = "",  string placeholder = "", string name = "", string value = "", string @class = "", string dataOptions = "", string events = "", string styles = "", string other = "", bool hide = false, string label = "")
            : base(id, displayName, InputTypes.File, placeholder, name, value, @class, dataOptions, events, styles, other, hide, label)
        {
        }

        /// <summary>
        /// 文件选择框设置信息
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public InputFile SetFileOption(FileInputOption opt)
        {
            opt.FileInfoField = string.IsNullOrEmpty(opt.FileInfoField) ? Id : opt.FileInfoField;
            FileOption = opt;
            InputType = InputTypes.File;
            return this;
        }

        /// <summary>
        /// 文件选择框设置信息
        /// </summary>
        /// <returns></returns>
        public InputFile SetFileOption(string fileInfoField="", string fileNameField = "", string fileExtField = "", int maxSize = 5)
        {
            fileInfoField = string.IsNullOrEmpty(fileInfoField) ? Id : fileInfoField;
            FileOption = new FileInputOption(fileInfoField,fileNameField,fileExtField,false,maxSize);
            return this;
        }
        /// <summary>
        /// 图片选择框设置信息
        /// </summary>
        /// <returns></returns>
        public InputFile SetImageOption(string fileInfoField="", string fileNameField = "", string fileExtField = "", int maxSize = 5)
        {
            fileInfoField = string.IsNullOrEmpty(fileInfoField) ? Id : fileInfoField;
            FileOption = new FileInputOption(fileInfoField,fileNameField,fileExtField,true,maxSize);
            return this;
        }
    }
    public class SpecialInputModel
    {
        public string Id { get; set; }
        public string InputStr { get; set; }

    }
    public class FileInputOption
    {
        public FileInputOption(string fileInfoField, string fileNameField = "", string fileExtField = "", bool isImage = true, int maxSize = 5, string fileIdField = null)
        {
            FileInfoField = fileInfoField;
            FileNameField = fileNameField;
            FileExtField = fileExtField;
            IsImage = isImage;
            MaxSize = maxSize;
            FileIdField = fileIdField;
        }
        public string FileIdField { get; set; }
        public string FileInfoField { get; set; }
        public string FileNameField { get; set; }
        public string FileExtField { get; set; }
        public bool IsImage { get; set; }

        /// <summary>
        /// 文件最大M(默认5M)
        /// </summary>
        public int MaxSize { get; set; } = 5;
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
        Text, List, Checkbox, Radio, Password, Textarea, File, Number, Date
    }
    public enum DateTypes
    {
        Date, DateTime
    }


    #endregion

    
}

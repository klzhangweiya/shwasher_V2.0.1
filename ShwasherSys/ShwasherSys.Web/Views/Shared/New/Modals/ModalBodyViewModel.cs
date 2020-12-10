using System.Collections.Generic;
using System.Linq;

namespace ShwasherSys.Views.Shared.New.Modals
{
    public class ModalBodyViewModel
    {
        public ModalBodyViewModel(List<Input> inputs, string formId = "form", string modalId = "modalId", string labelLayoutDefaultClass = "col-md-2", string inputLayoutDefaultClass = "col-md-10", string divClass = "", List<SpecialInputModel> specials = null)
        {
            FormId = formId;
            ModalId = modalId;
            _inputs = inputs;
            LabelLayoutDefaultClass = labelLayoutDefaultClass;
            InputLayoutDefaultClass = inputLayoutDefaultClass;
            Specials = specials;
            DivClass = divClass;
        }
        public string FormId { get; set; }
        public string ModalId { get; set; }
        public string DivClass { get; set; }
        public string LabelLayoutDefaultClass { get; set; }
        public string InputLayoutDefaultClass { get; set; }
        private readonly List<Input> _inputs;
        public List<Input> Inputs => _inputs.Select(SetDefault).ToList();

        public List<SpecialInputModel> Specials { get; set; }

        private Input SetDefault(Input input)
        {

            if (string.IsNullOrEmpty(input.LabelLayoutClass))
            {
                input.LabelLayoutClass = LabelLayoutDefaultClass;
            }
            if (string.IsNullOrEmpty(input.InputLayoutClass))
            {
                input.InputLayoutClass = InputLayoutDefaultClass;
            }
            if (string.IsNullOrEmpty(input.DivClass))
            {
                input.DivClass = DivClass;
            }

            input.DefaultTarget = $"#{ModalId}";
            return input;
        }
    }
}
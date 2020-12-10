namespace ShwasherSys.Views.Shared.New.Modals
{
    public class ModalHeaderViewModel
    {
        public string Title { get; set; }
        public string Operation { get; set; }

        public ModalHeaderViewModel(string title, string operation = null)
        {
            Title = title;
            Operation = operation;
        }
    }
}
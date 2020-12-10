namespace ShwasherSys.Models.Modal
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
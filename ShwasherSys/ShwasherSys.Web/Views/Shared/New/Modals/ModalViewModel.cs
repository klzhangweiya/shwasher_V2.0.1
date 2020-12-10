
namespace ShwasherSys.Views.Shared.New.Modals
{
    public class ModalViewModel
    {
        public ModalViewModel(string header, string modalId = "modal", int footer = 0,string sizeClass="", int? width = null)
        {
            Header = new ModalHeaderViewModel(header);
            Footer = footer+"";
            ModalId = modalId;
            Width = width;
            SizeClass = sizeClass;
        }
        
        public ModalViewModel(string header1, string header2,  string modalId = "modal", int footer = 0,string sizeClass="", int? width = null)
        {
            Header = new ModalHeaderViewModel(header1, header2);
            Footer = footer+"";
            ModalId = modalId;
            Width = width;
            SizeClass = sizeClass;
        }
        public ModalViewModel(string header, ModalBodyViewModel body, string modalId = "modal", string footer = "0",string sizeClass="", int? width = null)
        {
            Header = new ModalHeaderViewModel(header);
            Body = body;
            Body.ModalId = modalId;
            Footer = footer;
            ModalId = modalId;
            Width = width;
            SizeClass = sizeClass;
        }
     
        public ModalViewModel(string header1, string header2, ModalBodyViewModel body, string modalId = "modal", int footer = 0,string sizeClass="", int? width = null)
        {
            Header = new ModalHeaderViewModel(header1, header2);
            Body = body;
            if (body != null)
            {
                Body.ModalId = modalId;
            }
            Footer = footer+"";
            ModalId = modalId;
            Width = width;
            SizeClass = sizeClass;
        }
        public ModalViewModel(ModalHeaderViewModel header, ModalBodyViewModel body, string modalId = "modal", int footer = 0,string sizeClass="", int? width = null)
        {
            Header = header;
            Body = body;
            Body.ModalId = modalId;
            Footer = footer+"";
            ModalId = modalId;
            Width = width;
            SizeClass = sizeClass;
        }
        public ModalHeaderViewModel Header { get; set; }
        public ModalBodyViewModel Body { get; set; }
        public string Footer { get; set; }
        public string ModalId { get; set; }
        public int? Width { get; set; }
        public string SizeClass { get; set; }

        public ModalViewModel SetModalSize( string sizeClass)
        {
            SizeClass = sizeClass;
            return this;
        }
        public ModalViewModel SetModalSize( int width)
        {
            Width = width;
            return this;
        }
    }
}
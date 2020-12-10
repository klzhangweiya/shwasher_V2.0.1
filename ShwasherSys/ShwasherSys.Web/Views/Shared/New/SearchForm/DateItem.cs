namespace ShwasherSys.Views.Shared.New.SearchForm
{
    public class DateItem
    {
        public DateItem(int interval = 30, string formatter = "YYYY-MM-DD",bool isAutoApply=false)
        {
            Interval = interval;
            Formatter = formatter;
            IsAutoApply = isAutoApply;
        }
        public DateItem(string formatter, bool isAutoApply = false)
        {
            Interval = 30;
            Formatter = formatter;
            IsAutoApply = isAutoApply;
        }

        public bool IsAutoApply { get; set; }
        public int Interval { get; set; }
        public string Formatter { get; set; }
        public string IsAutoApplyStr => IsAutoApply ? "true" : "false";
    }
}
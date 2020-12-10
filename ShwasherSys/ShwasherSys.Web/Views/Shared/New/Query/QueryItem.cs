using ShwasherSys.Views.Shared.New.SearchForm;

namespace ShwasherSys.Views.Shared.New.Query
{
    public class QueryItem
    {
        public QueryItem(string key, string name, bool isSearch = false, FieldType fieldType = FieldType.S,
            ExpType expType = ExpType.Contains, string formatter = "")
        {
            Key = key;
            Name = name;
            IsSearch = isSearch;
            FieldType = fieldType;
            ExpType = expType;
            Formatter = formatter;
        }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Formatter { get; set; }
        public bool IsSearch { get; set; }
        public FieldType FieldType { get; set; }
        public ExpType ExpType { get; set; }
    }
}
namespace ShwasherSys.BaseSysInfo.States.Dto
{
    public class QueryStateDisplayValue
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string CodeValue { get; set; }

        public QueryStateDisplayValue(string tableName, string columnName, string codeValue)
        {
            TableName = tableName;
            ColumnName = columnName;
            CodeValue = codeValue;
        }
    }
}

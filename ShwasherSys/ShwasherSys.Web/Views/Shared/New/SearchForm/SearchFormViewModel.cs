using System.Collections.Generic;
using System.Linq;

namespace ShwasherSys.Views.Shared.New.SearchForm
{
    public class SearchFormViewModel
    {
        public SearchFormViewModel(List<SearchItem> searchItems, string formId = "search-form", bool isSingle = false)
        {
            FormId = formId;
            SearchItems = searchItems.Select(SetDefault).ToList();
            IsSingle = isSingle;
        }
        public bool IsSingle { get; set; }
        public string FormId { get; set; }
        public string Field { get; set; }
        public int FType { get; set; }
        public int EType { get; set; }
        public List<SearchItem> SearchItems { get; set; }

        private SearchItem SetDefault(SearchItem item)
        {
            if (string.IsNullOrEmpty(item.FormId))
                item.FormId = FormId;
            return item;
        }
        public SearchFormViewModel SetSearchOption(string field, FieldType fieldType = FieldType.I, ExpType expType = ExpType.Equal)
        {
            Field = field;
            FType = (int)fieldType;
            EType = (int)expType;
            return this;
        }
    }

    public enum FieldType
    {
        /// <summary>
        /// STRING
        /// </summary>
        S = 0,

        /// <summary>
        /// INT
        /// </summary>
        I = 1,

        /// <summary>
        /// INT
        /// </summary>
        In = 2,

        /// <summary>
        /// BOOL
        /// </summary>
        B = 3,

        /// <summary>
        /// BOOL
        /// </summary>
        Bn = 4,

        /// <summary>
        /// DATETIME
        /// </summary>
        D = 5,

        /// <summary>
        /// DATETIME?
        /// </summary>
        Dn = 6,

        /// <summary>
        /// long
        /// </summary>
        L = 7,

        /// <summary>
        /// long?
        /// </summary>
        Ln = 8,

        /// <summary>
        /// short
        /// </summary>
        Short = 9,

        /// <summary>
        /// Short?
        /// </summary>
        Sn = 10,

        /// <summary>
        /// float
        /// </summary>
        F = 11,

        /// <summary>
        /// float
        /// </summary>
        Fn = 12,
        /// <summary>
        /// Decimal
        /// </summary>
        Decimal = 13,
        /// <summary>
        /// Decimal
        /// </summary>
        DecimalNull=14,
        /// <summary>
        /// Double
        /// </summary>
        Double = 15,
        /// <summary>
        /// Double
        /// </summary>
        DoubleNull=16

    }
    public enum ExpType
    {
        Equal
        , NotEqual
        , Greater
        , Less
        , GreaterOrEqual
        , LessOrEqual
        , Contains
        , NotContains

    }
}
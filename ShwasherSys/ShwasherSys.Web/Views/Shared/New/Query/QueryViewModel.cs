using System;
using System.Collections.Generic;

namespace ShwasherSys.Views.Shared.New.Query
{

    #region Query

    public class QueryViewModel
    {
        public QueryViewModel(string name, string url, List<QueryItem> queryItems, string originField, string targetField = null, string submitEventName = null,
            string modalId = "query_modal", int modalWidth = 900, string itemDbClickEventName = null,
            string itemClickEventName = null, QueryTreeSearch queryTreeSearch = null, string searchBindFunc = "")
        {
            ModalName = name;
            QueryUrl = url;
            QueryItems = queryItems;
            SearchBindFunc = searchBindFunc;
            QueryTreeSearch = queryTreeSearch;
            OriginFields = originField.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var targets = new string[OriginFields.Length];
            TargetFields = new List<string[]>();
            if (string.IsNullOrEmpty(targetField))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    TargetFields.Add(new[] { "#" + OriginFields[i] });
                }
            }
            else
            {
                var targetArr = targetField.Split(new[] { ',' }, StringSplitOptions.None);
                for (int i = 0; i < targets.Length; i++)
                {
                    if (i < targetArr.Length)
                    {
                        if (!string.IsNullOrEmpty(targetArr[i]))
                        {
                            var unitArr = targetArr[i].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (unitArr.Length > 1)
                            {
                                if (int.TryParse(unitArr[0], out int index) && index < OriginFields.Length && index >= i)
                                {
                                    for (; i < index; i++)
                                    {
                                        TargetFields.Add(new[] { "#" + OriginFields[i] });
                                    }
                                    AddTargetField(unitArr[1], i);
                                }
                                else
                                {
                                    AddTargetField(unitArr[1], i);
                                }
                            }
                            else
                            {
                                AddTargetField(targetArr[i], i);
                            }
                        }
                        else
                        {
                            TargetFields.Add(new[] { "#" + OriginFields[i] });
                        }
                    }
                    else
                    {
                        TargetFields.Add(new[] { "#" + OriginFields[i] });
                    }

                }
            }


            SubmitEventName = submitEventName;
            ModalId = modalId;
            ModalWidth = modalWidth;
            ItemClickEventName = itemClickEventName;
            ItemDbClickEventName = itemDbClickEventName ?? submitEventName;
        }
        public string QueryUrl { get; set; }
        public List<QueryItem> QueryItems { get; set; }
        public string ModalId { get; set; }
        public string ModalName { get; set; }
        public int ModalWidth { get; set; }
        public string[] OriginFields { get; set; }
        public List<string[]> TargetFields { get; set; }
        public string SubmitEventName { get; set; }
        public string ItemClickEventName { get; set; }
        public string ItemDbClickEventName { get; set; }

        public QueryTreeSearch QueryTreeSearch { get; set; }

        public string SearchBindFunc { get; set; }

        private void AddTargetField(string targetStr, int index)
        {
            var target = targetStr.Split(new[] { '|' }, StringSplitOptions.None);
            for (int i = 0; i < target.Length; i++)
            {
                target[i] = string.IsNullOrEmpty(target[i]) ? "#" + OriginFields[index] : (target[i].StartsWith("#") || target[i].StartsWith("."))
                    ? target[i]
                    : "#" + target[i];
            }
            TargetFields.Add(target);
        }
    }

    //public class SearchBindDto
    //{
    //    public  string KeyField { get; set; }

    //    public  string KeyWords { get; set; }

    //    public  int FieldType { get; set; }

    //    public  int ExpType { get; set; }
    //}

    #endregion
}

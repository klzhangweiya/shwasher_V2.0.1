using System;
using System.Collections.Generic;
using IwbZero.AppServiceBase;
using ShwasherSys.Models.Layout;

namespace ShwasherSys.Models.Modal
{
    public class QueryModalModel
    {
        public QueryModalModel(string name,string url, List<QueryItem> queryItems, string originFiled,
            string targetFiled = null, string submitEventName = null,
            string modalId = "query_modal", int modalWidth = 900, string itemDbClickEventName = null,
            string itemClickEventName = null,string submitEx="",List<MultiSearchDto> searchDtos=null)
        {
            ModalName = name;
            QueryUrl = url;
            QueryItems = queryItems;
            OriginFileds =  originFiled.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var targets = new string[OriginFileds.Length];
            TargetFileds=new List<string[]>();
            if (string.IsNullOrEmpty(targetFiled))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    TargetFileds.Add(new []{ "#" + OriginFileds[i] });
                }
            }
            else
            {
                var targetArr= targetFiled.Split(new[] { ',' }, StringSplitOptions.None);
                int j = 0;
                for (int i = 0; i < targets.Length; i++)
                {
                    if (j < targetArr.Length)
                    {
                        if (!string.IsNullOrEmpty(targetArr[j]))
                        {
                            var unitArr = targetArr[j].Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                            if (unitArr.Length>1)
                            {
                                if (int.TryParse(unitArr[0],out int index)&& index< OriginFileds.Length && index>=i)
                                {
                                    for (; i < index; i++)
                                    {
                                        TargetFileds.Add(new[] { "#" + OriginFileds[i] });
                                    }
                                    AddTargetFiled(unitArr[1],i);
                                }
                                else
                                {
                                    AddTargetFiled(unitArr[1],i);
                                }
                            }
                            else
                            {
                                AddTargetFiled(targetArr[j],i);
                            }
                        }
                        else
                        {
                            TargetFileds.Add(new[] { "#" + OriginFileds[i] });
                        }

                        j++;
                    }
                    else
                    {
                        TargetFileds.Add(new[] { "#" + OriginFileds[i] });
                    }
                    
                }
            }


            SubmitEventName = submitEventName;
            ModalId = modalId;
            ModalWidth = modalWidth;
            ItemClickEventName = itemClickEventName;
            ItemDbClickEventName = itemDbClickEventName ?? submitEventName;
            SubmitEx = submitEx;
            SearchList = searchDtos;
        }
        public string QueryUrl { get; set; }
        public List<QueryItem> QueryItems { get; set; }
        public string ModalId { get; set; }
        public string ModalName { get; set; }
        public int ModalWidth { get; set; }
        public string[] OriginFileds { get; set; }
        public List<string[]> TargetFileds { get; set; }
        public string SubmitEventName { get; set; }
        public string ItemClickEventName { get; set; }
        public string ItemDbClickEventName { get; set; }
        public string SubmitEx { get; set; }
        public  List<MultiSearchDto> SearchList { get; set; }

        public string DefaultValueFunction { get; set; } = "QueryDefaultValueFunction";

        public QueryModalModel SetDefaultValueFunction(string v)
        {
            DefaultValueFunction = v;
            return this;
        }
        private void AddTargetFiled(string targetStr,int index)
        {
            var target = targetStr.Split(new[] { '|' }, StringSplitOptions.None);
            for (int i = 0; i < target.Length; i++)
            {
                target[i] =string.IsNullOrEmpty(target[i])? "#" + OriginFileds[index] : (target[i].StartsWith("#") || target[i].StartsWith("."))
                    ? target[i]
                    : "#" + target[i];
            }
            TargetFileds.Add(target);
        }
    }

    public class QueryItem
    {
        public QueryItem(string key, string name, bool isSearch = false, FiledType filedType = FiledType.S,
            ExpType expType = ExpType.Contains, string formatter = "",bool isSort = false)
        {
            Key = key;
            Name = name;
            IsSearch = isSearch;
            FiledType = filedType;
            ExpType = expType;
            Formatter = formatter;
            IsSort = isSort;
        }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Formatter { get; set; }
        public bool IsSearch { get; set; }
        public string IsHidden { get; set; } = "";
        public FiledType FiledType { get; set; }
        public ExpType ExpType { get; set; }

        public bool IsSort { get; set; }

        public QueryItem SetHidden()
        {
            IsSearch = true;
            IsHidden = "style=display:none;";
            return this;
        }
    }

    public class QueryParamModel
    {
        public QueryParamModel(string targetDom,string submitEx="", List<MultiSearchDto> searchList=null)
        {
            SubmitEx = submitEx;
            TargetDom = targetDom;
            SearchList = searchList;
        }
        public string TargetDom { get; set; }

        public string SubmitEx { get; set; }

        public List<MultiSearchDto> SearchList { get; set; }
    }
}
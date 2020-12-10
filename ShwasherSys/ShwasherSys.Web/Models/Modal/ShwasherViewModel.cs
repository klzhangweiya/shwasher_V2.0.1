using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShwasherSys.Models.Modal
{
    public class ShowQueryModelModal
    {
        public ShowQueryModelModal(string queryType, string submitEvent, string modelId = "queryModal", string formId = "queryForm", string tableId = "queryTable")
        {
            FormId = formId;
            QueryType = queryType;
            SubmitEvent = submitEvent;
            TableId = tableId;
            ModelId = modelId;
        }

        public string QueryType { get; set; }
        public string SubmitEvent { get; set; }

        public string ModelId { get; set; }

        public string FormId { get; set; }

        public string TableId { get; set; }
    }

    public class ModelFooterModel
    {
        public ModelFooterModel(string modalId,string submitEvent, bool isCancel=true,string cancelName="取消",string submitName = "保存",string footStyle="")
        {
            ModalId = modalId;
            IsCancel = isCancel;
            SubmitEvent = submitEvent;
            CancelName = cancelName;
            SubmitName = submitName;
            FootStyle = footStyle;
        }

        public string ModalId { get; set; }

        public bool IsCancel { get; set; }

        public string SubmitEvent { get; set; }


        public string CancelName  { get; set; }

        public string SubmitName { get; set; }
        public string FootStyle { get; set; }
    }
}
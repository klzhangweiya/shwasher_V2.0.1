using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.Invoice;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(OrderStickBill)),AutoMapFrom(typeof(OrderStickBill))]
    public class OrderStickBillDto: EntityDto<string>
    {
		public string CustomerId  { get; set; }
		public string CustomerName  { get; set; }
		public DateTime? CreatDate  { get; set; }
		public string StickNum  { get; set; }
		public string StickMan  { get; set; }
		public string Description  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }

        public string OriginalStickNum { get; set; }
        public string ReturnOrderNo { get; set; }
        public string OrderNo { get; set; }
        public int InvoiceType { get; set; }
    }
}
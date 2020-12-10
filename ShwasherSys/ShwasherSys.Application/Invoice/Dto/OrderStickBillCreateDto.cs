using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using ShwasherSys.Invoice;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(OrderStickBill))]
    public class OrderStickBillCreateDto:EntityDto<string>
    {
        public string OrderSendIds { get; set; }

        [Required] 
        [StringLength(OrderStickBill.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
		public DateTime? CreatDate  { get; set; }
        [StringLength(OrderStickBill.StickNumMaxLength)]
		public string StickNum  { get; set; }
        [StringLength(OrderStickBill.StickManMaxLength)]
		public string StickMan  { get; set; }
        [StringLength(OrderStickBill.DescriptionMaxLength)]
		public string Description  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OrderStickBill.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }

        //开票状态（1:未开票 2：已开票）
        public int? InvoiceState { get; set; }
        //金额
        public decimal? Amount { get; set; }
        public int InvoiceType { get; set; }
    }

    [AutoMapTo(typeof(OrderStickBill))]
    public class RedOrderStickBillCreateDto:OrderStickBillCreateDto
    {
        
        public string OriginalStickNum { get; set; }
        public string ReturnOrderNo { get; set; }
        public string OrderNo { get; set; }
    }
}

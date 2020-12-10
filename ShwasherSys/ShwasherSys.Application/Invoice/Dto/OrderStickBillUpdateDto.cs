using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.Invoice;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(OrderStickBill))]
    public class OrderStickBillUpdateDto: EntityDto<string>
    {
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
    }
}
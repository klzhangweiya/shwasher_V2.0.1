using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.OrderSendInfo.Dto
{
    [AutoMapTo(typeof(OrderSendBill))]
    public class OrderSendBillUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(OrderSendBill.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
		public DateTime? SendDate  { get; set; }
        [StringLength(OrderSendBill.SendAddressMaxLength)]
		public string SendAddress  { get; set; }
        [StringLength(OrderSendBill.ContactTelsMaxLength)]
		public string ContactTels  { get; set; }
        [StringLength(OrderSendBill.ContactManMaxLength)]
		public string ContactMan  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OrderSendBill.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        [StringLength(OrderSendBill.IsDoBillMaxLength)]
		public string IsDoBill  { get; set; }
        public int? ExpressId { get; set; }

        public string ExpressBillNo { get; set; }
    }
}
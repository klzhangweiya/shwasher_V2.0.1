using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.OrderSendInfo.Dto
{
    [AutoMapTo(typeof(OrderSendBill)),AutoMapFrom(typeof(OrderSendBill))]
    public class OrderSendBillDto: EntityDto<string>
    {
		public string CustomerId  { get; set; }
		public DateTime? SendDate  { get; set; }
		public string SendAddress  { get; set; }
		public string ContactTels  { get; set; }
		public string ContactMan  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsDoBill  { get; set; }
        public int? ExpressId { get; set; }

        public string ExpressBillNo { get; set; }
	}
}
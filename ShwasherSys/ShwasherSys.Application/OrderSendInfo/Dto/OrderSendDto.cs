using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.OrderSendInfo.Dto
{
    [AutoMapTo(typeof(OrderSend)),AutoMapFrom(typeof(OrderSend))]
    public class OrderSendDto: EntityDto<int>
    {
		public int OrderItemId  { get; set; }
		public DateTime? SendDate  { get; set; }
		public decimal SendQuantity  { get; set; }
		public int? OrderUnitId  { get; set; }
		public string Remark  { get; set; }
		public string OrderSendBillNo  { get; set; }
		public string OrderStickBillNo  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }

        public string CreatorUserId { get; set; }
		public decimal? QuantityPerPack  { get; set; }
		public decimal? PackageCount  { get; set; }
		public string ProductBatchNum  { get; set; }
    }
}
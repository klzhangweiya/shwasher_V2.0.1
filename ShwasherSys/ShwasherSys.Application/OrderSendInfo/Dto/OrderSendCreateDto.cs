using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.OrderSendInfo;

namespace ShwasherSys.OrderSendInfo.Dto
{
    [AutoMapTo(typeof(OrderSend))]
    public class OrderSendCreateDto
    {
		public int OrderItemId  { get; set; }
		public DateTime? SendDate  { get; set; }
		public decimal SendQuantity  { get; set; }
		public int? OrderUnitId  { get; set; }
        [StringLength(OrderSend.RemarkMaxLength)]
		public string Remark  { get; set; }
        [StringLength(OrderSend.OrderSendBillNoMaxLength)]
		public string OrderSendBillNo  { get; set; }
        [StringLength(OrderSend.OrderStickBillNoMaxLength)]
		public string OrderStickBillNo  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OrderSend.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
		public decimal? QuantityPerPack  { get; set; }
		public decimal? PackageCount  { get; set; }
        //[StringLength(OrderSend.ProductBatchNumMaxLength)]
		public string ProductBatchNum  { get; set; }
		public string CreatorUserId { get; set; }

	}
}

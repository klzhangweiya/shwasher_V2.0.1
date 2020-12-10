using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.Order;

namespace ShwasherSys.Order.Dto
{
    [AutoMapTo(typeof(OrderItem)),AutoMapFrom(typeof(OrderItem))]
    public class OrderItemDto: EntityDto<int>
    {
		public string OrderNo  { get; set; }
		public string ProductNo  { get; set; }
		public decimal Price  { get; set; }
      
        public decimal AfterTaxPrice { get; set; }
        public string CurrencyId  { get; set; }
		public decimal Quantity  { get; set; }
		public int OrderUnitId  { get; set; }
		public DateTime SendDate  { get; set; }
		public string IsReport  { get; set; }
		public string IsPartSend  { get; set; }
		public int? OrderItemStatusId  { get; set; }
		public string WareHouse  { get; set; }
		public string OrderItemDesc  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string PartNo  { get; set; }

        public decimal? ToCnyRate { get; set; }

        public int EmergencyLevel { get; set; }

		//是否删除
        public string IsLock { get; set; }
	}
}
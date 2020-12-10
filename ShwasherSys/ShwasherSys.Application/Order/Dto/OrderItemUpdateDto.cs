using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.Order;

namespace ShwasherSys.Order.Dto
{
    [AutoMapTo(typeof(OrderItem))]
    public class OrderItemUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(OrderItem.OrderNoMaxLength)]
		public string OrderNo  { get; set; }
        [Required] 
        [StringLength(OrderItem.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
		public decimal Price  { get; set; }
     
        public decimal AfterTaxPrice { get; set; }
        [Required] 
        [StringLength(OrderItem.CurrencyIdMaxLength)]
		public string CurrencyId  { get; set; }
		public decimal Quantity  { get; set; }
		public int OrderUnitId  { get; set; }
		public DateTime? SendDate  { get; set; }
        [Required] 
        [StringLength(OrderItem.IsReportMaxLength)]
		public string IsReport  { get; set; }
        [Required] 
        [StringLength(OrderItem.IsPartSendMaxLength)]
		public string IsPartSend  { get; set; }
		public int? OrderItemStatusId  { get; set; }
        [StringLength(OrderItem.WareHouseMaxLength)]
		public string WareHouse  { get; set; }
        [StringLength(OrderItem.OrderItemDescMaxLength)]
		public string OrderItemDesc  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OrderItem.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        [StringLength(OrderItem.PartNoMaxLength)]
		public string PartNo  { get; set; }

        public decimal? ToCnyRate { get; set; }

        public int EmergencyLevel { get; set; }

        //是否删除
        public string IsLock { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.ReturnGoods.Dto
{
    [AutoMapTo(typeof(ReturnGoodOrder))]
    public class ReturnGoodOrderUpdateDto: EntityDto<int>
    {
        [StringLength(ReturnGoodOrder.ReturnOrderNoMaxLength)]
		public string ReturnOrderNo  { get; set; }
        public string SendOrderNo { get; set; }
        public string OrderNo { get; set; }
        public string OrderItemNo { get; set; }
        [StringLength(ReturnGoodOrder.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
        [StringLength(ReturnGoodOrder.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
		public decimal? Quantity  { get; set; }
        [StringLength(ReturnGoodOrder.HandleUserMaxLength)]
		public string HandleUser  { get; set; }
		public DateTime? ReturnDate  { get; set; }
		//public int ReturnState  { get; set; }
        public string  CustomerId { get; set; }
        public string  Reason { get; set; }
        //public Decimal? Amount { get; set; }
        //public Decimal? AuditAmount { get; set; }
        public int? ReturnType { get; set; }
        public string LinkName { get; set; }
    }
}
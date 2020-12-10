using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IwbZero.AppServiceBase;

namespace ShwasherSys.ReturnGoods.Dto
{
    [AutoMapTo(typeof(ReturnGoodOrder))]
    public class ReturnGoodOrderCreateDto:IwbEntityDto<int>
    {
		
        [StringLength(ReturnGoodOrder.ReturnOrderNoMaxLength)]
		public string ReturnOrderNo  { get; set; }
        public string OrderNo { get; set; }
        public string OrderItemNo { get; set; }
        public string SendOrderNo { get; set; }
        [StringLength(ReturnGoodOrder.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
        [StringLength(ReturnGoodOrder.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
		public decimal? Quantity  { get; set; }
        [StringLength(ReturnGoodOrder.HandleUserMaxLength)]
		public string HandleUser  { get; set; }
		public DateTime? ReturnDate  { get; set; }
		public int ReturnState  { get; set; }
        
        public string  CustomerId { get; set; }
        public string  Reason { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? AuditAmount { get; set; }
        public int? ReturnType { get; set; }
        [IgnoreMap]
        public  string Fax { get; set; }
        [IgnoreMap]
        public  string LinkName { get; set; }
        [IgnoreMap]
        public  string WareHouse { get; set; }
        [IgnoreMap]
        public  string Telephone { get; set; }
        [IgnoreMap]
        public  string StockNo { get; set; }
        [IgnoreMap]
        public DateTime? OrderDate { get; set; }
        [IgnoreMap]
        public DateTime? SendDate { get; set; }
        [IgnoreMap]
        public int? CustomerSendId  { get; set; }
    }
}

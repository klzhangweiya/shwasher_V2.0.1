using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;
using ShwasherSys.Order;

namespace ShwasherSys.Order.Dto
{
    [AutoMapTo(typeof(OrderHeader)),AutoMapFrom(typeof(OrderHeader))]
    public class OrderHeaderDto: EntityDto<string>
    {
		public string CustomerId  { get; set; }
		public string LinkName  { get; set; }
		public DateTime OrderDate  { get; set; }
		public string Fax  { get; set; }
		public string Telephone  { get; set; }
		public int CustomerSendId  { get; set; }
        public string CustomerSendName { get; set; }
        public string SendAdress { get; set; }
        public string StockNo  { get; set; }
		public int OrderStatusId  { get; set; }
        public string OrderStatusName { get; set; }
        public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }

     
        public int? SaleType { get; set; }
        public string SaleTypeName { get; set; }
        public string SaleMan { get; set; }
        [IgnoreMap]
        public string SaleManName { get; set; }

        //是否删除
        public string IsLock { get; set; }
    }
}
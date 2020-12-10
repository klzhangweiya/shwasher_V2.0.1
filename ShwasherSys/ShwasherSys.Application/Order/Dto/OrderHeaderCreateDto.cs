using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using ShwasherSys.Order;

namespace ShwasherSys.Order.Dto
{
    [AutoMapTo(typeof(OrderHeader))]
    public class OrderHeaderCreateDto:Entity<string>
    {
        [Required] 
        [StringLength(OrderHeader.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
        [Required] 
        [StringLength(OrderHeader.LinkNameMaxLength)]
		public string LinkName  { get; set; }
		public DateTime OrderDate  { get; set; }
        [StringLength(OrderHeader.FaxMaxLength)]
		public string Fax  { get; set; }
        [StringLength(OrderHeader.TelephoneMaxLength)]
		public string Telephone  { get; set; }
		public int CustomerSendId  { get; set; }
        [StringLength(OrderHeader.StockNoMaxLength)]
		public string StockNo  { get; set; }
		public int OrderStatusId  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OrderHeader.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }

        public int? SaleType { get; set; }

        public string SaleMan { get; set; }

        public string IsLock { get; set; }
    }
}

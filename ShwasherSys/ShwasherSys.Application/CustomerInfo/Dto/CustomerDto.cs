using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(Customer)),AutoMapFrom(typeof(Customer))]
    public class CustomerDto: EntityDto<string>
    {
		public string CustomerName  { get; set; }
		public string LinkMan  { get; set; }
		public string Address  { get; set; }
		public string WebSite  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
		public string Telephone  { get; set; }
		public string Fax  { get; set; }
		public string Zip  { get; set; }
		public string Email  { get; set; }
        public int SaleType { get; set; }
    }
}
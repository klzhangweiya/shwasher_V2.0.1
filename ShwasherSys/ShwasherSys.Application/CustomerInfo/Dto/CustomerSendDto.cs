using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(CustomerSend)),AutoMapFrom(typeof(CustomerSend))]
    public class CustomerSendDto: EntityDto<int>
    {
		public string CustomerId  { get; set; }
		public string CustomerSendName  { get; set; }
		public string SendAdress  { get; set; }
		public string LinkMan  { get; set; }
		public string Telephone  { get; set; }
		public string Zip  { get; set; }
		public string Email  { get; set; }
		public string Mobile  { get; set; }
		public string Fax  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string IsLock  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.CustomerInfo.InvoiceAddress.Dto
{
    [AutoMapTo(typeof(CustomerInvoiceAddress)),AutoMapFrom(typeof(CustomerInvoiceAddress))]
    public class CustomerInvoiceAddressDto: EntityDto<int>
    {
		public string CustomerId  { get; set; }
		public string InvoiceAddressName  { get; set; }
		public string InvoiceAddress  { get; set; }
		public string LinkMan  { get; set; }
		public string Telephone  { get; set; }
		public string Zip  { get; set; }
		public string Email  { get; set; }
		public string Mobile  { get; set; }
		public string Fax  { get; set; }
    }
}
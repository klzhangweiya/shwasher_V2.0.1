using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using IwbZero.AppServiceBase;

namespace ShwasherSys.CustomerInfo.InvoiceAddress.Dto
{
    [AutoMapTo(typeof(CustomerInvoiceAddress))]
    public class CustomerInvoiceAddressCreateDto:IwbEntityDto<int>
    {
		
        [Required] 
		public string CustomerId  { get; set; }
        [Required] 
		public string InvoiceAddressName  { get; set; }
        [Required] 
		public string InvoiceAddress  { get; set; }
		public string LinkMan  { get; set; }
		public string Telephone  { get; set; }
		public string Zip  { get; set; }
		public string Email  { get; set; }
		public string Mobile  { get; set; }
		public string Fax  { get; set; }
    }
}

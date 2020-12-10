using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(Customer))]
    public class CustomerUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(Customer.CustomerNameMaxLength)]
		public string CustomerName  { get; set; }
        [StringLength(Customer.LinkManMaxLength)]
		public string LinkMan  { get; set; }
        [StringLength(Customer.AddressMaxLength)]
		public string Address  { get; set; }
        [StringLength(Customer.WebSiteMaxLength)]
		public string WebSite  { get; set; }
	
        [StringLength(Customer.TelephoneMaxLength)]
		public string Telephone  { get; set; }
        [StringLength(Customer.FaxMaxLength)]
		public string Fax  { get; set; }
        [StringLength(Customer.ZipMaxLength)]
		public string Zip  { get; set; }
        [StringLength(Customer.EmailMaxLength)]
		public string Email  { get; set; }

        public int SaleType { get; set; }
    }
}
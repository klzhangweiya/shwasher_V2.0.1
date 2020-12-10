using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(CustomerDefaultProduct))]
    public class CustomerDefaultProductCreateDto
    {
        [StringLength(CustomerDefaultProduct.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
      
		public string ProductNo  { get; set; }
        [StringLength(CustomerDefaultProduct.CustomerProductNameMaxLength)]
		public string CustomerProductName  { get; set; }
		public int Sequence  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
    }
}

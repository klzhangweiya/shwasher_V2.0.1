using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(CustomerDefaultProduct))]
    public class CustomerDefaultProductUpdateDto: EntityDto<int>
    {
        [StringLength(CustomerDefaultProduct.CustomerIdMaxLength)]
		public string CustomerId  { get; set; }
        [StringLength(CustomerDefaultProduct.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
        [StringLength(CustomerDefaultProduct.CustomerProductNameMaxLength)]
		public string CustomerProductName  { get; set; }
		public int Sequence  { get; set; }
		
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.CustomerInfo;

namespace ShwasherSys.CustomerInfo.Dto
{
    [AutoMapTo(typeof(CustomerDefaultProduct)),AutoMapFrom(typeof(CustomerDefaultProduct))]
    public class CustomerDefaultProductDto: EntityDto<int>
    {
		public string CustomerId  { get; set; }
		public string ProductNo  { get; set; }
        public string ProductName { get; set; }
        public string CustomerProductName  { get; set; }
		public int Sequence  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
    }
}
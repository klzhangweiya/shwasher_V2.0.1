using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(RmProduct)),AutoMapFrom(typeof(RmProduct))]
    public class RmProductDto: EntityDto<string>
    {
		public string ProductName  { get; set; }
		public string Material  { get; set; }
		public string Model  { get; set; }
		public string ProductDesc  { get; set; }
    }
}
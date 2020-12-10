using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(RmProduct))]
    public class RmProductUpdateDto: EntityDto<string>
    {
		public string ProductName  { get; set; }
		public string Material  { get; set; }
		public string Model  { get; set; }
		public string ProductDesc  { get; set; }
    }
}
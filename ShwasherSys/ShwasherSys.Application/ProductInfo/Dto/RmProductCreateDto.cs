using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using IwbZero.AppServiceBase;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(RmProduct))]
    public class RmProductCreateDto:EntityDto<string>
    {
		
		public string ProductName  { get; set; }
		public string Material  { get; set; }
		public string Model  { get; set; }
		public string ProductDesc  { get; set; }
    }
}

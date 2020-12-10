using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Product)),AutoMapFrom(typeof(Product))]
    public class ProductDto: EntityDto<string>
    {
		public string ProductName  { get; set; }
		public string Model  { get; set; }
		public int? StandardId  { get; set; }
		public string Material  { get; set; }
		public string ProductDesc  { get; set; }
		public string SurfaceColor  { get; set; }
		public string Rigidity  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public int Sequence  { get; set; }
		public string IsStandard  { get; set; }
		public string IsLock  { get; set; }
		public decimal? Defprice  { get; set; }
        public decimal? TranUnitValue { get; set; }

        public string PartNo { get; set; }
    }
}
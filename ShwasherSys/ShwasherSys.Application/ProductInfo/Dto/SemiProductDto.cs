using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(SemiProducts)),AutoMapFrom(typeof(SemiProducts))]
    public class SemiProductDto: EntityDto<string>
    {
		public string SemiProductName  { get; set; }
		public string Model  { get; set; }
		public string Material  { get; set; }
		public string ProductDesc  { get; set; }
		public string SurfaceColor  { get; set; }
		public string Rigidity  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public int Sequence  { get; set; }
		public string IsLock  { get; set; }
       
        public string IsStandard { get; set; }

        public string PartNo { get; set; }
        public decimal? TranUnitValue { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Product))]
    public class ProductUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(Product.ProductNameMaxLength)]
		public string ProductName  { get; set; }
        [StringLength(Product.ModelMaxLength)]
		public string Model  { get; set; }
		public int? StandardId  { get; set; }
        [StringLength(Product.MaterialMaxLength)]
		public string Material  { get; set; }
        [StringLength(Product.ProductDescMaxLength)]
		public string ProductDesc  { get; set; }
        [StringLength(Product.SurfaceColorMaxLength)]
		public string SurfaceColor  { get; set; }
        [StringLength(Product.RigidityMaxLength)]
		public string Rigidity  { get; set; }
	
		public int Sequence  { get; set; }
        [Required] 
        [StringLength(Product.IsStandardMaxLength)]
		public string IsStandard  { get; set; }
       
		public decimal? Defprice  { get; set; }
        public decimal? TranUnitValue { get; set; }
        [StringLength(Product.PartNoMaxLength)]
        public string PartNo { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Product))]
    public class ProductCreateDto:Entity<string>
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
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Product.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
		public int Sequence  { get; set; }
        [Required] 
        [StringLength(Product.IsStandardMaxLength)]
		public string IsStandard  { get; set; }
        /*[Required] 
        [StringLength(Product.IsLockMaxLength)]
		public string IsLock  { get; set; }*/
		public decimal? Defprice  { get; set; }
        public decimal? TranUnitValue { get; set; }
        [StringLength(Product.PartNoMaxLength)]
        public string PartNo { get; set; }
    }
}

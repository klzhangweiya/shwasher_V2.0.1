using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductInfo;
using AutoMapper;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(SemiProducts))]
    public class SemiProductUpdateDto: EntityDto<string>
    {
        [StringLength(SemiProducts.SemiProductNameMaxLength)]
		public string SemiProductName  { get; set; }
        [StringLength(SemiProducts.ModelMaxLength)]
		public string Model  { get; set; }
        [StringLength(SemiProducts.MaterialMaxLength)]
		public string Material  { get; set; }
        [StringLength(SemiProducts.ProductDescMaxLength)]
		public string ProductDesc  { get; set; }
        [StringLength(SemiProducts.SurfaceColorMaxLength)]
		public string SurfaceColor  { get; set; }
        [StringLength(SemiProducts.RigidityMaxLength)]
		public string Rigidity  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(SemiProducts.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
		public int Sequence  { get; set; }
        
        [StringLength(SemiProducts.IsLockMaxLength)]
		public string IsLock  { get; set; }
        [StringLength(SemiProducts.IsStandardMaxLength)]
        public string IsStandard { get; set; }
        [StringLength(SemiProducts.PartNoMaxLength)]
        public string PartNo { get; set; }

        [IgnoreMap]
        public string FileInfo { get; set; }
        [IgnoreMap]
        public string FileName { get; set; }
        [IgnoreMap]
        public string FileExt { get; set; }
        public decimal? TranUnitValue { get; set; }
    }
}
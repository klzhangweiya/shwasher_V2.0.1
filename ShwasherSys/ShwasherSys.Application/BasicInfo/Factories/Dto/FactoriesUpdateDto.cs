using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.Factory.Dto
{
    [AutoMapTo(typeof(Factories))]
    public class FactoriesUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(Factories.FactoryNameMaxLength)]
		public string FactoryName  { get; set; }
    
        [StringLength(Factories.ShortNamesMaxLength)]
		public string ShortNames  { get; set; }
        
        [StringLength(Factories.RegionIDMaxLength)]
		public string RegionID  { get; set; }
        [StringLength(Factories.FactoryURLMaxLength)]
		public string FactoryURL  { get; set; }
        [StringLength(Factories.AddressMaxLength)]
		public string Address  { get; set; }
        [StringLength(Factories.ZIPMaxLength)]
		public string ZIP  { get; set; }
        [StringLength(Factories.LinkManMaxLength)]
		public string LinkMan  { get; set; }
        [StringLength(Factories.TelephoneMaxLength)]
		public string Telephone  { get; set; }
        [StringLength(Factories.RemarkMaxLength)]
		public string Remark  { get; set; }
     
        [StringLength(Factories.IsLockMaxLength)]
		public string IsLock  { get; set; }
    }
}
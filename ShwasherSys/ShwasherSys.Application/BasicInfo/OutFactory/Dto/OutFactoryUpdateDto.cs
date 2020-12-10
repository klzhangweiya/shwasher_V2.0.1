using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.BasicInfo.OutFactory.Dto
{
    [AutoMapTo(typeof(OutFactory))]
    public class OutFactoryUpdateDto: EntityDto<string>
    {
        [Required] 
        [StringLength(OutFactory.OutFactoryNameMaxLength)]
		public string OutFactoryName  { get; set; }
        [StringLength(OutFactory.LinkManMaxLength)]
		public string LinkMan  { get; set; }
        [StringLength(OutFactory.AddressMaxLength)]
		public string Address  { get; set; }
        [StringLength(OutFactory.WebSiteMaxLength)]
		public string WebSite  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(OutFactory.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
       
        [StringLength(OutFactory.IsLockMaxLength)]
		public string IsLock  { get; set; }
        [StringLength(OutFactory.TelephoneMaxLength)]
		public string Telephone  { get; set; }
        [StringLength(OutFactory.FaxMaxLength)]
		public string Fax  { get; set; }
        [StringLength(OutFactory.ZipMaxLength)]
		public string Zip  { get; set; }
        [StringLength(OutFactory.EmailMaxLength)]
		public string Email  { get; set; }
    }
}
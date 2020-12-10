using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductInfo;

namespace ShwasherSys.ProductInfo.Dto
{
    [AutoMapTo(typeof(Standard))]
    public class StandardCreateDto
    {
        [Required] 
        [StringLength(Standard.StandardNameMaxLength)]
		public string StandardName  { get; set; }
        [StringLength(Standard.StandardDescMaxLength)]
		public string StandardDesc  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(Standard.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}

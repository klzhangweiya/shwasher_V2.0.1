using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(BulletinInfo))]
    public class BulletinInfoCreateDto
    {
        [Required] 
        [StringLength(BulletinInfo.BulletinTypeMaxLength)]
		public string BulletinType  { get; set; }
        [Required] 
        [StringLength(BulletinInfo.TitleMaxLength)]
		public string Title  { get; set; }
        [Required] 
        //[StringLength(BulletinInfo.ContentMaxLength)]
		public string Content  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(BulletinInfo.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
        [StringLength(BulletinInfo.PromulgatorMaxLength)]
		public string Promulgator  { get; set; }
		public DateTime? PromulgatTime  { get; set; }
		public DateTime? ExpirationDate  { get; set; }
    }
}

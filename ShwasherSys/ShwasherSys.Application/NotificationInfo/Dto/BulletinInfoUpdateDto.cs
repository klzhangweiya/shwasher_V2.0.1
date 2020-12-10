using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(BulletinInfo))]
    public class BulletinInfoUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(BulletinInfo.BulletinTypeMaxLength)]
		public string BulletinType  { get; set; }
        [Required] 
        [StringLength(BulletinInfo.TitleMaxLength)]
		public string Title  { get; set; }
        [Required] 
     
		public string Content  { get; set; }
	
        [StringLength(BulletinInfo.PromulgatorMaxLength)]
		public string Promulgator  { get; set; }
		public DateTime? PromulgatTime  { get; set; }
		public DateTime? ExpirationDate  { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(BulletinInfo)),AutoMapFrom(typeof(BulletinInfo))]
    public class BulletinInfoDto: EntityDto<int>
    {
		public string BulletinType  { get; set; }
		public string BulletinTypeName  { get; set; }
		public string Title  { get; set; }
		public string Content  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string Promulgator  { get; set; }
		public DateTime? PromulgatTime  { get; set; }
		public DateTime? ExpirationDate  { get; set; }
    }
}
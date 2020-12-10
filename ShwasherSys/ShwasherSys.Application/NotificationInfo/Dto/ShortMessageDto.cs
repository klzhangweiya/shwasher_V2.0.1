using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(ShortMessage)),AutoMapFrom(typeof(ShortMessage))]
    public class ShortMessageDto: EntityDto<int>
    {
		public string SendUserID  { get; set; }
		public string Title  { get; set; }
		public string Content  { get; set; }
		public DateTime? SendTime  { get; set; }
		public string IsDelete  { get; set; }
		public string RecieveUserIds  { get; set; }
    }
}
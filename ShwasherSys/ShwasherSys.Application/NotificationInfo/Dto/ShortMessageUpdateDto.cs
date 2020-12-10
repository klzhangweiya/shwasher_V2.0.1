using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(ShortMessage))]
    public class ShortMessageUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(ShortMessage.SendUserIDMaxLength)]
		public string SendUserID  { get; set; }
        [StringLength(ShortMessage.TitleMaxLength)]
		public string Title  { get; set; }
        [StringLength(ShortMessage.ContentMaxLength)]
		public string Content  { get; set; }
		public DateTime? SendTime  { get; set; }
       
        [StringLength(ShortMessage.RecieveUserIdsMaxLength)]
		public string RecieveUserIds  { get; set; }
    }
}
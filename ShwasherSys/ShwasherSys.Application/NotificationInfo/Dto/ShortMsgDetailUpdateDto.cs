using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(ShortMsgDetail))]
    public class ShortMsgDetailUpdateDto: EntityDto<int>
    {
		public int MsgID  { get; set; }
        [Required] 
        [StringLength(ShortMsgDetail.RecvUserIDMaxLength)]
		public string RecvUserID  { get; set; }
        [Required] 
        [StringLength(ShortMsgDetail.IsReadMaxLength)]
		public string IsRead  { get; set; }
    }
}
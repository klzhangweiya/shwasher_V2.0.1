using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(ShortMsgDetail))]
    public class ShortMsgDetailCreateDto
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

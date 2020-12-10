using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.NotificationInfo;

namespace ShwasherSys.NotificationInfo.Dto
{
    [AutoMapTo(typeof(ShortMsgDetail)),AutoMapFrom(typeof(ShortMsgDetail))]
    public class ShortMsgDetailDto: EntityDto<int>
    {
		public int MsgID  { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public string SendUserID { get; set; }

        public DateTime? SendTime { get; set; }
		public string RecvUserID  { get; set; }
		public string IsRead  { get; set; }
    }

    public class NoticeAlarmDto
    {
        public int Total { get; set; }
        public List<ShortMsgDetailDto> Items { get; set; }
    }
    
}
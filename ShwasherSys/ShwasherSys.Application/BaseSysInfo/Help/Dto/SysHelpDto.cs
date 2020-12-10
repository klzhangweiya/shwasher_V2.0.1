using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.Help.Dto
{
    [AutoMapTo(typeof(SysHelp)),AutoMapFrom(typeof(SysHelp))]
    public class SysHelpDto: EntityDto<int>
    {
        /// <summary>
        /// 分类
        /// </summary>   
		public string Classification  { get; set; }
		public string HelpTitle  { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>   
		public string HelpKeyWords  { get; set; }
        /// <summary>
        /// 内容
        /// </summary>   
		public string HelpContent  { get; set; }
		public int Sequence  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }
        public string ClassificationShow { get; set; }
    }
}
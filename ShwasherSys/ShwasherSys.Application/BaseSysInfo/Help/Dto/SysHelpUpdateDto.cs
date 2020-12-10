using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.Help.Dto
{
    [AutoMapTo(typeof(SysHelp))]
    public class SysHelpUpdateDto: EntityDto<int>
    {
        
        /// <summary>
        /// 分类
        /// </summary>   
        [StringLength(SysHelp.ClassificationMaxLength)]
		public string Classification  { get; set; }
        [Required] 
        [StringLength(SysHelp.HelpTitleMaxLength)]
		public string HelpTitle  { get; set; }
        
        /// <summary>
        /// 关键字
        /// </summary>   
        [StringLength(SysHelp.HelpKeyWordsMaxLength)]
		public string HelpKeyWords  { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>   
       
		public string HelpContent  { get; set; }
		public int Sequence  { get; set; }
		/*public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string UserIDLastMod  { get; set; }*/
    }
}
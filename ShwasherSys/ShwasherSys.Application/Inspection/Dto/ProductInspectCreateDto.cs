using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BaseSysInfo.SysAttachFiles.Dto;

namespace ShwasherSys.Inspection.Dto
{
    
    /// <summary>
    /// 技术检验信息
    /// </summary>   
    [AutoMapTo(typeof(ProductInspectInfo))]
    public class ProductInspectCreateDto
    {
        /// <summary>
        /// 检验编号
        /// </summary>   
        [StringLength(ProductInspectInfo.ProductInspectNoMaxLength)]
		public string ProductInspectNo  { get; set; }
		public string ProductInspectReportNo  { get; set; }
        /// <summary>
        /// 检验报告
        /// </summary>
		public string ReportContent { get; set; }
        /// <summary>
        /// 排产单号
        /// </summary>   
        [Required] 
        [StringLength(ProductInspectInfo.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
        [Required] 
        [StringLength(ProductInspectInfo.SemiProductNoMaxLength)]
		public string SemiProductNo  { get; set; }
        
		public string InspectSubject  { get; set; }
        /// <summary>
        /// 检验结果
        /// </summary>   
		public int InspectResult  { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>   
		public DateTime? InspectDate  { get; set; }
        /// <summary>
        /// 检验人员
        /// </summary>   
        [StringLength(ProductInspectInfo.InspectMemberMaxLength)]
		public string InspectMember  { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>   
        [StringLength(ProductInspectInfo.IsLockMaxLength)]
		public string IsLock  { get; set; }
        /// <summary>
        /// 检验详情
        /// </summary>   
        [StringLength(ProductInspectInfo.InspectContentMaxLength)]
		public string InspectContent  { get; set; }
		public List<SysAttachFileCreateDto> AttachFiles { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>   
		public DateTime? TimeCreated  { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>   
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(ProductInspectInfo.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(ProductInspectInfo.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}

using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.PackageInfo.Dto
{
    
    /// <summary>
    /// 半成品包装信息
    /// </summary>   
    [AutoMapTo(typeof(PackageApply))]
    public class PackageApplyRefuseDto : EntityDto<int>
    {
        
        /// <summary>
        /// 申请流水号
        /// </summary>   
        [StringLength(PackageApply.PackageApplyNoMaxLength)]
        [Required] 
		public string PackageApplyNo { get; set; }
        
        /// <summary>
        /// 半成品仓库库存信息编号
        /// </summary>   
        [StringLength(PackageApply.CurrentSemiStoreHouseNoMaxLength)]
		public string CurrentSemiStoreHouseNo  { get; set; }
        
        /// <summary>
        /// 流转单编号
        /// </summary>   
        [Required] 
        [StringLength(PackageApply.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        
        /// <summary>
        /// 半成品编号
        /// </summary>   
        [StringLength(PackageApply.SemiProductNoMaxLength)]
		public string SemiProductNo  { get; set; }
        
        /// <summary>
        /// 申请包装数量
        /// </summary>   
		public decimal ApplyQuantity  { get; set; }
        
        /// <summary>
        /// 实际包装的千件数
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
        [StringLength(PackageApply.ApplyStatusMaxLength)]
		public string ApplyStatus  { get; set; }
        
        /// <summary>
        /// 发起申请时间
        /// </summary>   
		public DateTime? ApplyDate  { get; set; }
        [StringLength(PackageApply.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(PackageApply.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(PackageApply.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}
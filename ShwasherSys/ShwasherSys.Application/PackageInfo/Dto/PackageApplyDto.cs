using System;
using System.Security.Permissions;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.PackageInfo.Dto
{
    
    /// <summary>
    /// 半成品包装信息
    /// </summary>   
    [AutoMapTo(typeof(PackageApply)),AutoMapFrom(typeof(PackageApply))]
    public class PackageApplyDto: EntityDto<int>
    {
        /// <summary>
        /// 申请流水号
        /// </summary>   
		public string PackageApplyNo  { get; set; }
        /// <summary>
        /// 半成品仓库库存信息编号
        /// </summary>   
		public string CurrentSemiStoreHouseNo  { get; set; }
        /// <summary>
        /// 流转单编号
        /// </summary>   
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string SemiProductNo  { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary> 
        public string ProductNo { get; set; }

        /// <summary>
        /// 包装类型（1.半成品，2.成品）
        /// </summary>
        public int PackType { get; set; }

        /// <summary>
        /// 申请包装数量
        /// </summary>   
		public decimal ApplyQuantity  { get; set; }
        /// <summary>
        /// 实际包装的千件数
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
		public string ApplyStatus  { get; set; }
        public bool IsClose { get; set; }
        /// <summary>
        /// 发起申请时间
        /// </summary>   
		public DateTime? ApplyDate  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }

        public string ProductName { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }
        public string PartNo { get; set; }
        [IgnoreMap]
        public int ProcessingNum { get; set; }
        [IgnoreMap]
        public decimal? IsApplyEnterQuantity { get; set; }
        [IgnoreMap]
        public decimal RemainApplyQuantity { get; set; }
        public decimal KgWeight { get; set; }

    }
}
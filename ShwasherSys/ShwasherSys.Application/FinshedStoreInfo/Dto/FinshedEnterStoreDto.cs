using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.FinshedStoreInfo.Dto
{
    
    /// <summary>
    /// 成品仓库维护
    /// </summary>   
    [AutoMapTo(typeof(FinshedEnterStore)),AutoMapFrom(typeof(FinshedEnterStore))]
    public class FinshedEnterStoreDto: EntityDto<int>
    {
		public string ProductionOrderNo  { get; set; }
		public string PackageApplyNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string SemiProductNo  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string ProductNo  { get; set; }
		public int StoreHouseId  { get; set; }
		public string StoreLocationNo  { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>   
		public int ApplyStatus  { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>   
		public bool IsClose  { get; set; }
        /// <summary>
        /// 申请来源（1.车间包装 ）
        /// </summary>   
		public int ApplySource  { get; set; }
        /// <summary>
        /// 申请入库数量(千斤)
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 包装规格
        /// </summary>   
		public decimal PackageSpecification  { get; set; }
        /// <summary>
        /// 申请入库包数
        /// </summary>   
		public decimal PackageCount  { get; set; }
        /// <summary>
        /// 实际入库包数
        /// </summary>   
		public decimal ActualPackageCount  { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>   
		public string AuditUser  { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>   
		public DateTime? AuditDate  { get; set; }
        /// <summary>
        /// 申请入库时间
        /// </summary>   
		public DateTime? ApplyEnterDate  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }

        public int SourceStoreHouseId { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    [AutoMapTo(typeof(SemiEnterStore))]
    public class SemiEnterStoreUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(SemiEnterStore.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        [Required] 
		public int StoreHouseId  { get; set; }
        
        /// <summary>
        /// 1.申请中 2.已入库 3.已取消
        /// </summary>   
        [Required] 
        [StringLength(SemiEnterStore.ApplyStatusMaxLength)]
		public string ApplyStatus  { get; set; }
        
        /// <summary>
        /// 申请来源（1.车间加工生产 2.外购单申请入库）
        /// </summary>   
        [Required] 
        [StringLength(SemiEnterStore.ApplySourceMaxLength)]
		public string ApplySource  { get; set; }

        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }

        /// <summary>
        /// 半成品编号
        /// </summary>   
        [StringLength(SemiEnterStore.SemiProductNoMaxLength)]
		public string SemiProductNo  { get; set; }
        
        /// <summary>
        /// 申请入库数量
        /// </summary>   
		public decimal Quantity  { get; set; }
        
        /// <summary>
        /// 实际入库数量
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
        
        /// <summary>
        /// 申请入库时间
        /// </summary>   
		public DateTime? ApplyEnterDate  { get; set; }
        [StringLength(SemiEnterStore.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(SemiEnterStore.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(SemiEnterStore.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }

        public string StoreLocationNo { get; set; }
    }
}
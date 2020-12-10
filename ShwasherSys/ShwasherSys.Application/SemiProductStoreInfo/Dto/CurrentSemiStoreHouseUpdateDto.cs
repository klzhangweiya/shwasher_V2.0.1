using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    [AutoMapTo(typeof(CurrentSemiStoreHouse))]
    public class CurrentSemiStoreHouseUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(CurrentSemiStoreHouse.CurrentSemiStoreHouseNoMaxLength)]
		public string CurrentSemiStoreHouseNo  { get; set; }
        [Required] 
        [StringLength(CurrentSemiStoreHouse.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        [Required] 
		public int StoreHouseId  { get; set; }
        
        /// <summary>
        /// 半成品编号
        /// </summary>   
        [StringLength(CurrentSemiStoreHouse.SemiProductNoMaxLength)]
		public string SemiProductNo  { get; set; }
        
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>   
		public decimal FreezeQuantity  { get; set; }
        
        /// <summary>
        /// 当前实际数量
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
        
        /// <summary>
        /// 申请入库时间
        /// </summary>   
		public DateTime? ApplyEnterDate  { get; set; }
        [StringLength(CurrentSemiStoreHouse.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(CurrentSemiStoreHouse.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(CurrentSemiStoreHouse.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}
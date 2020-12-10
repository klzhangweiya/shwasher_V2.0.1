using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    [AutoMapTo(typeof(CurrentProductStoreHouse))]
    public class CurrentProductStoreHouseUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(CurrentProductStoreHouse.CurrentProductStoreHouseNoMaxLength)]
		public string CurrentProductStoreHouseNo  { get; set; }
        [Required] 
        [StringLength(CurrentProductStoreHouse.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        [Required] 
		public int StoreHouseId  { get; set; }
        
        /// <summary>
        /// 库位编码
        /// </summary>   
        [StringLength(CurrentProductStoreHouse.StoreLocationNoMaxLength)]
		public string StoreLocationNo  { get; set; }
        
        /// <summary>
        /// 成品编号
        /// </summary>   
        [StringLength(CurrentProductStoreHouse.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
        
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>   
		public decimal FreezeQuantity  { get; set; }
        
        /// <summary>
        /// 当前实际数量
        /// </summary>   
		public decimal Quantity  { get; set; }
        [StringLength(CurrentProductStoreHouse.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(CurrentProductStoreHouse.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(CurrentProductStoreHouse.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}
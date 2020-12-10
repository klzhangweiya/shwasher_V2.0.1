using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    [AutoMapTo(typeof(ProductOutStore))]
    public class ProductOutStoreUpdateDto: EntityDto<int>
    {
        [Required] 
        [StringLength(ProductOutStore.ProductionOrderNoMaxLength)]
		public string ProductionOrderNo  { get; set; }
        
        /// <summary>
        /// 成品库存中记录编号
        /// </summary>   
        [StringLength(ProductOutStore.CurrentProductStoreHouseNoMaxLength)]
		public string CurrentProductStoreHouseNo  { get; set; }
        
        /// <summary>
        /// 成品编号
        /// </summary>   
        [StringLength(ProductOutStore.ProductNoMaxLength)]
		public string ProductNo  { get; set; }
        [Required] 
		public int StoreHouseId  { get; set; }

        /// <summary>
        /// 0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库
        /// </summary>   
        [Required] 
		public int ApplyStatus  { get; set; }
        
        /// <summary>
        /// 是否已关闭
        /// </summary>   
		public bool IsClose  { get; set; }
        
        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
		public bool? IsConfirm  { get; set; }
        
        /// <summary>
        /// 申请出库数量(千件)
        /// </summary>   
		public decimal Quantity  { get; set; }
        
        /// <summary>
        /// 审核人员
        /// </summary>   
        [StringLength(ProductOutStore.UserIDLastModMaxLength)]
		public string AuditUser  { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>   
		public DateTime? AuditDate  { get; set; }
        
        /// <summary>
        /// 申请出库时间
        /// </summary>   
		public DateTime? ApplyOutDate  { get; set; }
        [StringLength(ProductOutStore.RemarkMaxLength)]
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
        [StringLength(ProductOutStore.CreatorUserIdMaxLength)]
		public string CreatorUserId  { get; set; }
        [StringLength(ProductOutStore.UserIDLastModMaxLength)]
		public string UserIDLastMod  { get; set; }
    }
}
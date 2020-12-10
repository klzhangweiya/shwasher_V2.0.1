using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    [AutoMapTo(typeof(ProductOutStore)),AutoMapFrom(typeof(ProductOutStore))]
    public class ProductOutStoreDto: EntityDto<int>
    {
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 成品库存中记录编号
        /// </summary>   
		public string CurrentProductStoreHouseNo  { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>   
		public string ProductNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库
        /// </summary>   
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
		public string AuditUser  { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>   
		public DateTime? AuditDate  { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>   
		public DateTime? ApplyOutDate  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }
    }
}
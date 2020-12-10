using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.RmStore.Dto
{
    [AutoMapTo(typeof(RmOutStore)),AutoMapFrom(typeof(RmOutStore))]
    public class RmOutStoreDto: EntityDto<string>
    {
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 原材料库存中记录编号
        /// </summary>   
		public string CurrentRmStoreHouseNo  { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>   
		public string RmProductNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// <doc>
/// <summary>
///0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库
/// </summary>
///</doc>
        /// </summary>   
		public int ApplyStatus  { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>   
		public bool IsClose  { get; set; }
        /// <summary>
        /// 1.未确认 2.确认
///</summary>   
///</doc>
        /// </summary>   
		public bool? IsConfirm  { get; set; }
        /// <summary>
        /// 申请出库数量(千件)
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 审核后出库数量
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>   
		public string AuditUser  { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>   
		public DateTime? AuditDate  { get; set; }
		public string OutStoreUser  { get; set; }
		public DateTime? OutStoreDate  { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>   
		public DateTime? ApplyOutDate  { get; set; }
		public string Remark  { get; set; }

        public string ProductBatchNum { get; set; }

        //手动平衡:2 常规流程:1  默认1
        public int CreateSourceType { get; set; }
    }

    public class RwOutStatusUpdateDto
    {
        public string Id { get; set; }
        public int ApplyStatus { get; set; }
        //出库数量
        public decimal ActualQuantity { get; set; }
    }
}
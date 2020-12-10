using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.RmStore.Dto
{
    [AutoMapTo(typeof(RmEnterStore)),AutoMapFrom(typeof(RmEnterStore))]
    public class RmEnterStoreDto: EntityDto<string>
    {
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>   
		public string RmProductNo  { get; set; }
		public int StoreHouseId  { get; set; }
		public string StoreLocationNo  { get; set; }
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
        /// 入库数量(千件)
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 申请入库数量(千斤/千件)
        /// </summary>   
		public decimal ApplyQuantity  { get; set; }
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
		public string EnterStoreUser  { get; set; }
		public DateTime? EnterStoreDate  { get; set; }
		public string Remark  { get; set; }

        public string ProductBatchNum { get; set; }

        public int CreateSourceType { get; set; }
    }

    public class RwEnterStatusUpdateDto
    {
        public string Id { get; set; }
        public int ApplyStatus { get; set; }
        //入库数量
        public decimal Quantity { get; set; }

    }
}
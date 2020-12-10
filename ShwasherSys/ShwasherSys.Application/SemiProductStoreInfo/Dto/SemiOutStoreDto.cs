using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    [AutoMapTo(typeof(SemiOutStore)),AutoMapFrom(typeof(SemiOutStore))]
    public class SemiOutStoreDto: EntityDto<int>
    {
		public string ProductionOrderNo  { get; set; }
        /// <summary>
        /// 半成品库存中记录编号
        /// </summary>   
		public string CurrentSemiStoreHouseNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 1.申请中 2.已出库 3.已取消
        /// </summary>   
		public string ApplyStatus  { get; set; }
        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
        public Boolean? IsConfirm { get; set; }
        /// <summary>
        /// 申请出库来源（1.外协加工申请 2.包装申请）
        /// </summary>   
		public string ApplyOutStoreSource  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
		public string SemiProductNo  { get; set; }
        /// <summary>
        /// 申请入库数量
        /// </summary>   
		public decimal Quantity  { get; set; }
        /// <summary>
        /// 实际出库数量（用于填入实时库存中的数量）
        /// </summary>   
		public decimal ActualQuantity  { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>   
		public DateTime? ApplyOutDate  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }
		public string AuditUser  { get; set; }
		public DateTime? AuditDate  { get; set; }
    }
}
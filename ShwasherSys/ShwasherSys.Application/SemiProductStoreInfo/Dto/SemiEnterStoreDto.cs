using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    [AutoMapTo(typeof(SemiEnterStore)),AutoMapFrom(typeof(SemiEnterStore))]
    public class SemiEnterStoreDto: EntityDto<int>
    {
		public string ProductionOrderNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已入库
        /// </summary>   
		public string ApplyStatus  { get; set; }
        /// <summary>
        /// 申请来源（1.车间加工生产 2.外购单申请入库）
        /// </summary>   
		public string ApplySource  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
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

        public DateTime? AuditDate { get; set; }
        public string AuditUser { get; set; }
        public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }

        public string StoreLocationNo { get; set; }
    }
}
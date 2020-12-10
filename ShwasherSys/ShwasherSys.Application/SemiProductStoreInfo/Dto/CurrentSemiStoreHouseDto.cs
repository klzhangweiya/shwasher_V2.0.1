using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.SemiProductStoreInfo.Dto
{
    [AutoMapTo(typeof(CurrentSemiStoreHouse)),AutoMapFrom(typeof(CurrentSemiStoreHouse))]
    public class CurrentSemiStoreHouseDto: EntityDto<int>
    {
		public string CurrentSemiStoreHouseNo  { get; set; }
		public string ProductionOrderNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>   
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
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }
        public string StoreLocationNo { get; set; }
    }

    [AutoMapTo(typeof(SemiEnterStore)), AutoMapFrom(typeof(SemiEnterStore))]
    public class AddSemiEnterStoreDto
    {
        public decimal Quantity { get; set; }

        public string ProductionOrderNo { get; set; }

        public string SemiProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public string StoreLocationNo { get; set; }

    }

    public class AddSemiOutStoreDto
    {
        public decimal? Quantity { get; set; }
        public string CurrentSemiStoreHouseNo { get; set; }
    }
}
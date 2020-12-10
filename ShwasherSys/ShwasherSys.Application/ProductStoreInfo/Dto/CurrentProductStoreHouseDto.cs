using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    [AutoMapTo(typeof(CurrentProductStoreHouse)),AutoMapFrom(typeof(CurrentProductStoreHouse))]
    public class CurrentProductStoreHouseDto: EntityDto<int>
    {
		public string CurrentProductStoreHouseNo  { get; set; }
		public string ProductionOrderNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>   
		public string StoreLocationNo  { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>   
		public string ProductNo  { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>   
		public decimal FreezeQuantity  { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>   
		public decimal Quantity  { get; set; }
		public string Remark  { get; set; }
		public DateTime? TimeCreated  { get; set; }
		public DateTime? TimeLastMod  { get; set; }
		public string CreatorUserId  { get; set; }
		public string UserIDLastMod  { get; set; }
    }

    public class UpdateLocationDto : EntityDto<int>
    {
        /// <summary>
        /// 库位编码
        /// </summary>   
        public string StoreLocationNo { get; set; }
    }
}
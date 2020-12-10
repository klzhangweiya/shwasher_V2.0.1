﻿using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace ShwasherSys.RmStore.Dto
{
    [AutoMapTo(typeof(CurrentRmStoreHouse)),AutoMapFrom(typeof(CurrentRmStoreHouse))]
    public class CurrentRmStoreHouseDto: EntityDto<string>
    {
		public string ProductionOrderNo  { get; set; }
		public int StoreHouseId  { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>   
		public string StoreLocationNo  { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>   
		public string RmProductNo  { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>   
		public decimal FreezeQuantity  { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>   
		public decimal Quantity  { get; set; }
		public string Remark  { get; set; }
        /// <summary>
        /// 上月底剩余数量
        /// </summary>   
		public decimal? PreMonthQuantity  { get; set; }

        public string ProductBatchNum { get; set; }
    }
    [AutoMapTo(typeof(RmEnterStore))]
    public class AddRmEnterStore
    {
        public decimal Quantity { get; set; }

        public string ProductionOrderNo { get; set; }

        public string ProductBatchNum { get; set; }

        public string RmProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public string StoreLocationNo { get; set; }

   
    }

    public class AddRmOutStoreDto
    {
        public decimal? Quantity { get; set; }
        public string CurrentRmStoreHouseNo { get; set; }
    }
}
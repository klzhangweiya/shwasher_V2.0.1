using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.StoreHouseLocations.Dto
{
    [AutoMapTo(typeof(StoreHouseLocation)),AutoMapFrom(typeof(StoreHouseLocation))]
    public class StoreHouseLocationDto: EntityDto<int>
    {
		public string StoreLocationNo  { get; set; }
        /// <summary>
        /// 库区
        /// </summary>   
		public string StoreAreaCode  { get; set; }
        /// <summary>
        /// 货架号
        /// </summary>   
		public string ShelfNumber  { get; set; }
        /// <summary>
        /// 层次
        /// </summary>   
		public string ShelfLevel  { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>   
		public string SequenceNo  { get; set; }
		public string Remark  { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>   
		public int? StoreHouseId  { get; set; }
    }
}
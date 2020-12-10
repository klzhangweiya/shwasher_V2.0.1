using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.StoreHouseLocations.Dto
{
    [AutoMapTo(typeof(StoreHouseLocation))]
    public class StoreHouseLocationCreateDto
    {
        [StringLength(StoreHouseLocation.StoreLocationNoMaxLength)]
		public string StoreLocationNo  { get; set; }
        /// <summary>
        /// 库区
        /// </summary>   
        [StringLength(StoreHouseLocation.StoreAreaCodeMaxLength)]
		public string StoreAreaCode  { get; set; }
        /// <summary>
        /// 货架号
        /// </summary>   
        [StringLength(StoreHouseLocation.ShelfNumberMaxLength)]
		public string ShelfNumber  { get; set; }
        /// <summary>
        /// 层次
        /// </summary>   
        [StringLength(StoreHouseLocation.ShelfLevelMaxLength)]
		public string ShelfLevel  { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>   
        [StringLength(StoreHouseLocation.SequenceNoMaxLength)]
		public string SequenceNo  { get; set; }
        [StringLength(StoreHouseLocation.RemarkMaxLength)]
		public string Remark  { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>   
		public int? StoreHouseId  { get; set; }
    }
}

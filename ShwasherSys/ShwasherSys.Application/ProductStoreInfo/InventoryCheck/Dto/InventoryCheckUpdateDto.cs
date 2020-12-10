using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    
    /// <summary>
    /// 盘点计划
    /// </summary>   
    [AutoMapTo(typeof(InventoryCheckInfo))]
    public class InventoryCheckUpdateDto: EntityDto<string>
    {
		public string CheckNo  { get; set; }
        
        /// <summary>
        /// 盘点的仓库类型
        /// </summary>   
		//public int? StoreHouseTypeId  { get; set; }
		public int StoreHouseId  { get; set; }
        
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
        
        /// <summary>
        /// 盘点的依据类型（暂时只根据库位，产品或其它）预留
        /// </summary>   
		public string CheckType  { get; set; }
        
        /// <summary>
        /// 计划开始时间
        /// </summary>   
		public DateTime? PlanStartDate  { get; set; }
        
        /// <summary>
        /// 计划完成时间
        /// </summary>   
		public DateTime? PlanEndDate  { get; set; }
		public string Remark  { get; set; }
        
        /// <summary>
        /// 待盘点人员
        /// </summary>   
		public string CheckUser  { get; set; }
        
        /// <summary>
        /// 发布人
        /// </summary>   
		public string PublishUser  { get; set; }
        
        /// <summary>
        /// 盘点完成时间
        /// </summary>   
		public DateTime? FinishDate  { get; set; }
    }
}
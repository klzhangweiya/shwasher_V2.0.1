using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.ProductStoreInfo.Dto
{
    
    /// <summary>
    /// 盘点计划
    /// </summary>   
    [AutoMapTo(typeof(InventoryCheckInfo)),AutoMapFrom(typeof(InventoryCheckInfo))]
    public class InventoryCheckDto: EntityDto<string>
    {
		public string CheckNo  { get; set; }
  //      /// <summary>
  //      /// 盘点的仓库类型
  //      /// </summary>   
		//public int? StoreHouseTypeId  { get; set; }
		public int StoreHouseId  { get; set; }
       
        // 库区 
		public string StoreAreaCode  { get; set; }
      
        // 货架号

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
        /// 待盘点人员
        /// </summary>
        [IgnoreMap] 
		public string CheckUserName  { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>   
		public string PublishUser  { get; set; }
        /// <summary>
        /// 盘点完成时间
        /// </summary>   
		public DateTime? FinishDate  { get; set; }

        /// <summary>
        /// 盘点状态（1:新建 2:盘点中 3:盘点完成  4:取消）
        /// </summary>
        public int CheckState { get; set; }
        ///// <summary>
        ///// 是否关闭() 
        ///// </summary>
        //public int IsClose { get; set; }
        public long? CreatorUserId  { get; set; }

    }

    public class CheckStateDto:EntityDto<string>
    {
        public int CheckState { get; set; }

        //public bool IsClose { get; set; }

    }
    public class CheckDataDto : EntityDto<string>
    {
        public decimal CheckQuantity { get; set; }

        //public bool IsClose { get; set; }

    }

    public class QueryInventoryReportDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public int? EmployeeId { get; set; }

        public int? HouseType { get; set; }

        public int? CheckState { get; set; }
    }
    public class InventoryReportItem
    {
        public string CheckNo { get; set; }
        //      /// <summary>
        //      /// 盘点的仓库类型
        //      /// </summary>   
        //public int? StoreHouseTypeId  { get; set; }
        public int StoreHouseId { get; set; }
        public string StoreHouseName { get; set; }

        // 库区 
        public string StoreAreaCode { get; set; }

        // 货架号

        /// </summary>   
        public string ShelfNumber { get; set; }
        /// <summary>
        /// 层次

        /// </summary>   
        public string ShelfLevel { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>   
        public string SequenceNo { get; set; }
        /// <summary>
        /// 盘点的依据类型（暂时只根据库位，产品或其它）预留
        /// </summary>   
        public string CheckType { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>   
        public DateTime? PlanStartDate { get; set; }
        /// <summary>
        /// 计划完成时间
        /// </summary>   
        public DateTime? PlanEndDate { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 待盘点人员
        /// </summary>   
        public string CheckUser { get; set; }
        /// <summary>
        /// 待盘点人员
        /// </summary>
        [IgnoreMap]
        public string CheckUserName { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>   
        public string PublishUser { get; set; }
        /// <summary>
        /// 盘点完成时间
        /// </summary>   
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 盘点状态（1:新建 2:盘点中 3:盘点完成  4:取消）
        /// </summary>
        public int CheckState { get; set; }
        
        
    } 
}
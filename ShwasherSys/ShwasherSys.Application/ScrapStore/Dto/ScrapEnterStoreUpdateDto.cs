using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.ScrapStore.Dto
{
    [AutoMapTo(typeof(ScrapEnterStore))]
    public class ScrapEnterStoreUpdateDto: EntityDto<string>
    {
		public string ProductionOrderNo  { get; set; }
        
        /// <summary>
        /// 产品编号
        /// </summary>   
		public string ProductNo  { get; set; }
        [Required] 
		public int StoreHouseId  { get; set; }
		public string StoreLocationNo  { get; set; }
        
        /// <summary>
        /// <doc>
// <summary>
//0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库 
// </summary>
//</doc>
        /// </summary>   
        [Required] 
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
		public int ProductType  { get; set; }

        //报废来源 1：成品退货 2：半成品检验报废
        public int ScrapSource { get; set; }
    
        public string ScrapSourceNo { get; set; }
    }
}
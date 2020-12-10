using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    [AutoMapTo(typeof(ProductionLog)),AutoMapFrom(typeof(ProductionLog))]
    public class ProductionLogDto:Entity<int>
    {
        public string ProductionNo { get; set; }
        /// <summary>
        /// 流转单号
        /// </summary>
        public string ProductOrderNo { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// 车号
        /// </summary>
        public  string CarNo { get; set; }
        /// <summary>
        /// 产品数量（Kg）
        /// </summary>
        [DecimalPrecision]
        public decimal QuantityWeight { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 产品数量（千件）
        /// </summary>
        [DecimalPrecision]
        public decimal QuantityPcs { get; set; }

        public DateTime CreationTime{ get; set; }

        
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
    }
}
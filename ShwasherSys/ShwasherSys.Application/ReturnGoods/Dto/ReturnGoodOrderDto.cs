using System;
using System.Runtime.Serialization;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.ReturnGoods.Dto
{
    [AutoMapTo(typeof(ReturnGoodOrder)),AutoMapFrom(typeof(ReturnGoodOrder))]
    public class ReturnGoodOrderDto: EntityDto<int>
    {
		public string ReturnOrderNo  { get; set; }
        public string SendOrderNo { get; set; }
		public string OrderNo  { get; set; }
		public string ProductNo  { get; set; }
		public string ProductionOrderNo  { get; set; }
		public decimal? Quantity  { get; set; }
		public string HandleUser  { get; set; }
		public string HandleUserName  { get; set; }
		public DateTime? ReturnDate  { get; set; }
		public int ReturnState  { get; set; }

        //Model = s.Model,
        //Material = s.Material,
        //SurfaceColor = s.SurfaceColor,
        //Rigidity = s.Rigidity,

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        
        public string  CustomerId { get; set; }
        public string  Reason { get; set; }

        public Decimal? Amount { get; set; }
        public Decimal? AuditAmount { get; set; }
        public int ReturnType { get; set; }
        [IgnoreMap]
        public string  CustomerName { get; set; }
         public string ApplyUser { get; set; }

        public string ConfirmUser { get; set; }
        
        public DateTime? ApplyDate { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string LinkName { get; set; }
    }
}
using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(StatementBill)),AutoMapFrom(typeof(StatementBill))]
    public class StatementBillDto: EntityDto<int>
    {
		public string StatementBillNo  { get; set; }
		public string CustomerId  { get; set; }
		public string BillMan  { get; set; }
		public string Description  { get; set; }
		public int? StatementState { get; set; }
        public string OrderStickBillNo { get; set; }
        
        public string CustomerName { get; set; }

        public  DateTime CreationTime { get; set; }
    }


    public class QueryStatementBillReportDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public string CustomerId { get; set; }
    }
}
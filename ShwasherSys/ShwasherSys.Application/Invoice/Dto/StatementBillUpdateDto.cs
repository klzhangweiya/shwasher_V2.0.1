using System;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(StatementBill))]
    public class StatementBillUpdateDto: EntityDto<int>
    {
        [Required] 
		public string StatementBillNo  { get; set; }
        [Required] 
		public string CustomerId  { get; set; }
		public string BillMan  { get; set; }
		public string Description  { get; set; }
		public int? StatementState { get; set; }
        public string OrderStickBillNo { get; set; }
    }
}
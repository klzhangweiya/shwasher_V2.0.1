using System;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IwbZero.AppServiceBase;

namespace ShwasherSys.Invoice.Dto
{
    [AutoMapTo(typeof(StatementBill))]
    public class StatementBillCreateDto:IwbEntityDto<int>
    {
		
        [Required] 
		public string StatementBillNo  { get; set; }
        [Required] 
		public string CustomerId  { get; set; }
		public string BillMan  { get; set; }
		public string Description  { get; set; }
		public int? StatementState { get; set; }

        [IgnoreMap]
        public string OrderSendIds { get; set; }
        public string OrderStickBillNo { get; set; }
    }
}

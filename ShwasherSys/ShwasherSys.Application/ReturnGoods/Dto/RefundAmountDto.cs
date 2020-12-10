using Abp.Application.Services.Dto;

namespace ShwasherSys.ReturnGoods.Dto
{
    public class RefundAmountDto: EntityDto<int>
    {
        public decimal? Amount { get; set; }
        public decimal? AuditAmount { get; set; }
    }
}
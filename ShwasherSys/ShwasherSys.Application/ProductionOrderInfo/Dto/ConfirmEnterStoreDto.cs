using Abp.Domain.Entities;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class ConfirmEnterStoreDto:Entity<int>
    {
        public string CurrentRmHouseId { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal LaveQuantity { get; set; }
    }
}
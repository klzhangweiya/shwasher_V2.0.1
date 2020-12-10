using Abp.Domain.Entities;

namespace ShwasherSys.FinshedStoreInfo.Dto
{
    public class FinshedEnterStoreAuditDto:Entity<int>
    {
        public decimal ActualPackageCount { get; set; }
        public decimal PackageSpecification { get; set; }
        public string StoreLocationNo { get; set; }

        public int? CreateSourceType { get; set; }

    }
}

using System.Collections.Generic;

namespace ShwasherSys.PackageInfo.Dto
{
    public class CreatePackInfosDto
    {
      
        public string PackageApplyNo {get;set;}
        public string ProductionOrderNo { get;set;}
        public string PackageProductNo { get;set;}
        public int PackType { get; set; }
        public string ProductNo { get;set; }
        public List<PackInfoDto> PackageInfos { get;set;}
    }
    public class PackInfoDto
    {
        public string PackageEnterNum { get; set; }
        public decimal ActualQuantity { get;set;}
        public decimal ActualQuantity2 { get;set;}
        public decimal KgWeight { get;set;}
        public decimal PackageSpecification { get;set;}
        public decimal PackageCount { get;set;}
        public string VerifyUser { get;set;}
        public string PackageUser { get;set;}
        public int VerifyUserId { get;set;}
        public int PackageUserId { get;set;}
    }
}

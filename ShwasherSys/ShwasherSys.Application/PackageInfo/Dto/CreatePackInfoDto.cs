using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ShwasherSys.PackageInfo.Dto
{
    public class CreatePackInfoDto
    {
        public string PackageApplyNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string PackageProductNo { get; set; }
        public string ProductNo { get; set; }
        public int PackType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Quantity2 { get; set; }
        public decimal KgWeight { get; set; }
        public decimal PackageSpecification { get; set; }
        public decimal PackageCount { get; set; }
        public string PackageEnterNum { get; set; }
        public string VerifyUser { get;set;}
        public string PackageUser { get;set;}
    }
}

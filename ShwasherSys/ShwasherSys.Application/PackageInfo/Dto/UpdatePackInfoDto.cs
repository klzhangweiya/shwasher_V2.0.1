using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ShwasherSys.PackageInfo.Dto
{
    public class UpdatePackInfoDto:EntityDto
    {
        public decimal Quantity { get; set; }
        public decimal PackageSpecification { get; set; }
        public decimal PackageCount { get; set; }

        public string PackageEnterNum { get; set; }
    }
    public class RefusePackInfoDto : EntityDto
    {
        public int PackType { get; set; }
        
    }
}

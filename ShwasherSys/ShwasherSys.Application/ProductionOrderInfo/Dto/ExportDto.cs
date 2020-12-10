using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class ExportDto: EntityDto
    {
        public string HandleType { get; set; }
    }
}

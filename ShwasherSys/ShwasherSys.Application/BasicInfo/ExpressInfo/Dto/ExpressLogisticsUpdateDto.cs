using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace ShwasherSys.BasicInfo.ExpressInfo.Dto
{
    [AutoMapTo(typeof(ExpressLogistics))]
    public  class ExpressLogisticsUpdateDto:EntityDto<int>
    {
        [Required]
        [StringLength(ExpressLogistics.ExpressNameMaxLength)]
        public string ExpressName { get; set; }
        [Required]
        [StringLength(ExpressLogistics.CodeMaxLength)]
        public string Code { get; set; }

        public int Sort { get; set; }
        [IgnoreMap]
        public List<ExpressProviderMapper> ExpressProviderMapper { get; set; }
    }
}

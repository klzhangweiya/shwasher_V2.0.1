using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using AutoMapper;
using ShwasherSys.BasicInfo;

namespace ShwasherSys.BasicInfo.ExpressInfo.Dto
{
    [AutoMapTo(typeof(ExpressLogistics)),AutoMapFrom(typeof(ExpressLogistics))]
    public class ExpressLogisticsDto: EntityDto<int>
    {
        [StringLength(ExpressLogistics.ExpressNameMaxLength)]
        public string ExpressName { get; set; }

        [StringLength(ExpressLogistics.CodeMaxLength)]
        public string Code { get; set; }

        public int Sort { get; set; }

        [IgnoreMap]
        public virtual List<ExpressProviderMapper> ExpressProviderMapper { get; set; }
    }
}
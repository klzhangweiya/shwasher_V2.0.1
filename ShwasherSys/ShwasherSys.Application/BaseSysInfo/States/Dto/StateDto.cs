using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.States.Dto
{
    [AutoMapTo(typeof(SysState)), AutoMapFrom(typeof(SysState))]
    public class StateDto : EntityDto<int>
    {
        public string StateNo { get; set; }
        public string StateName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string CodeValue { get; set; }
        public string DisplayValue { get; set; }
    }
}

using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.Settings.Dto
{
    [AutoMapTo(typeof(SysSetting)), AutoMapFrom(typeof(SysSetting))]
    public class SettingDto : EntityDto<int>
    {
        public string SettingNo { get; set; }
        public string SettingName { get; set; }
        public int SettingType { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
    }
}

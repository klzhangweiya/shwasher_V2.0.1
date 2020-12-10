using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.Settings.Dto
{
    [AutoMapTo(typeof(SysSetting))]
    public class SettingCreateDto
    {
        [StringLength(SysSetting.SettingNoMaxLength)]
        public string SettingNo { get; set; }
        [StringLength(SysSetting.SettingNameMaxLength)]
        public string SettingName { get; set; }
        public int SettingType { get; set; }
        [StringLength(SysSetting.CodeMaxLength)]
        public string Code { get; set; }
        [StringLength(SysSetting.ValueMaxLength)]
        public string Value { get; set; }
        [StringLength(SysSetting.DesckMaxLength)]
        public string Description { get; set; }
        [StringLength(SysSetting.RemarkMaxLength)]
        public string Remark { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.States.Dto
{
    [AutoMapTo(typeof(SysState))]
    public class StateUpdateDto : EntityDto<int>
    {
        [StringLength(SysState.StateNoMaxLength)]
        public string StateNo { get; set; }

        [StringLength(SysState.StateNameMaxLength)]
        public string StateName { get; set; }
        [Required]
        [StringLength(SysState.TableNameMaxLength)]
        public string TableName { get; set; }
        [Required]
        [StringLength(SysState.ColNameMaxLength)]
        public string ColumnName { get; set; }
        [Required]
        [StringLength(SysState.CodeValueMaxLength)]
        public string CodeValue { get; set; }
        [Required]
        [StringLength(SysState.DisplayValueMaxLength)]
        public string DisplayValue { get; set; }
    }
}

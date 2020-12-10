using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.Functions.Dto
{
    [AutoMapTo(typeof(SysFunction)), AutoMapFrom(typeof(SysFunction))]
    public class FunctionUpdateDto : EntityDto<int>
    {
        [MaxLength(SysFunction.FunctionNoMaxLength)]
        public string FunctionNo { get; set; }
        [MaxLength(SysFunction.FunctionNoMaxLength)]
        public string ParentNo { get; set; }
        [MaxLength(SysFunction.FunctionNameMaxLength)]
        public string FunctionName { get; set; }
        [MaxLength(SysFunction.PermissionNameMaxLength)]
        public string PermissionName { get; set; }
        public int FunctionType { get; set; }
        [MaxLength(SysFunction.FunctionPathMaxLength)]
        public string FunctionPath { get; set; }
        [StringLength(SysFunction.ActionMaxLength)]
        public string Action { get; set; }
        [StringLength(SysFunction.ControllerMaxLength)]
        public string Controller { get; set; }
        public string Url { get; set; }
        [StringLength(SysFunction.IconMaxLength)]
        public string Icon { get; set; }
        [StringLength(SysFunction.ClassMaxLength)]
        public string Class { get; set; }
        public string Script { get; set; }
        public int Sort { get; set; }
        public int Depth { get; set; }
    }
}

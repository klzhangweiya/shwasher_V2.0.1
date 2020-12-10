using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.BaseSysInfo;

namespace ShwasherSys.BaseSysInfo.Functions.Dto
{
    [AutoMapTo(typeof(SysFunction))]
    public class FunctionDto : EntityDto<int>
    {
        public string FunctionNo { get; set; }
        public string ParentNo { get; set; }
        public string FunctionName { get; set; }
        public string PermissionName { get; set; }
        public int FunctionType { get; set; }
        public string FunctionPath { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public string Script { get; set; }
        public int Sort { get; set; }
        public int Depth { get; set; }

        public string FunctionTypeName { get; set; }

    }
}

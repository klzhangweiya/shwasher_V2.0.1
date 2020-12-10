using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.SysAttachFiles.Dto
{
    [AutoMapTo(typeof(SysAttachFile)),AutoMapFrom(typeof(SysAttachFile))]
    public class SysAttachFileDto: EntityDto<int>
    {
		public string AttachNo  { get; set; }
		public string TableName  { get; set; }
		public string ColumnName  { get; set; }
		public string SourceKey  { get; set; }
		public string FileTitle  { get; set; }
		public string FileName  { get; set; }
		public string FilePath  { get; set; }
		public string FileType  { get; set; }
		public string FileExt  { get; set; }
		public string Description  { get; set; }
    }
}
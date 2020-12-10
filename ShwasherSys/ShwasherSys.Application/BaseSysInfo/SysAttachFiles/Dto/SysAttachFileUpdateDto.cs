using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ShwasherSys.BaseSysInfo.SysAttachFiles.Dto
{
    [AutoMapTo(typeof(SysAttachFile))]
    public class SysAttachFileUpdateDto: EntityDto<int>
    {
        [StringLength(SysAttachFile.AttachNoMaxLength)]
		public string AttachNo  { get; set; }
        [StringLength(SysAttachFile.TableNameMaxLength)]
		public string TableName  { get; set; }
        [StringLength(SysAttachFile.ColumnNameMaxLength)]
		public string ColumnName  { get; set; }
        [StringLength(SysAttachFile.SourceKeyMaxLength)]
		public string SourceKey  { get; set; }
        [StringLength(SysAttachFile.FileTitleMaxLength)]
		public string FileTitle  { get; set; }
        [StringLength(SysAttachFile.FileNameMaxLength)]
		public string FileName  { get; set; }
        [StringLength(SysAttachFile.FilePathMaxLength)]
		public string FilePath  { get; set; }
        [StringLength(SysAttachFile.FileTypeMaxLength)]
		public string FileType  { get; set; }
        [StringLength(SysAttachFile.FileExtMaxLength)]
		public string FileExt  { get; set; }
        [StringLength(SysAttachFile.DescriptionMaxLength)]
		public string Description  { get; set; }
        public string FileInfo { get; set; }

    }
}
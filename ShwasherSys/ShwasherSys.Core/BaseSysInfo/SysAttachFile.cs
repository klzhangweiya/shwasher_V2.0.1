using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.BaseSysInfo
{

    [Table("Sys_AttachFiles")]
    public class SysAttachFile : CreationAuditedEntity
    {
        public const int AttachNoMaxLength = 32;
        public const int TableNameMaxLength = 50;
        public const int ColumnNameMaxLength = 50;
        public const int SourceKeyMaxLength = 50;
        public const int FileTitleMaxLength = 50;
        public const int FileNameMaxLength = 50;
        public const int FilePathMaxLength = 500;
        public const int FileTypeMaxLength = 20;
        public const int FileExtMaxLength = 10;
        public const int DescriptionMaxLength = 500;


        [MaxLength(AttachNoMaxLength)]
        public string AttachNo { get; set; }

        [MaxLength(TableNameMaxLength)]
        public string TableName { get; set; }
        [MaxLength(ColumnNameMaxLength)]
        public string ColumnName { get; set; }
        [MaxLength(SourceKeyMaxLength)]
        public string SourceKey { get; set; }
        [MaxLength(FileTitleMaxLength)]
        public string FileTitle { get; set; }
        [MaxLength(FileNameMaxLength)]
        public string FileName { get; set; }
        [MaxLength(FilePathMaxLength)]
        public string FilePath { get; set; }
        [MaxLength(FileTypeMaxLength)]
        public string FileType { get; set; }
        [MaxLength(FileExtMaxLength)]
        public string FileExt { get; set; }
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

    }
}

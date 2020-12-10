using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.Inspection
{
    [Table("TemplateInfos")]
    public class TemplateInfo : Entity<int>
    {
        public const int ProductInspectNoMaxLength = 50;
        public const int ContentNoMaxLength = int.MaxValue;
        public const int NameMaxLength = 50;
        public const int DescriptionMaxLength = 500;
        public const int FileExtMaxLength = 50;
        public const int FilePathMaxLength = 300;
        public const int TempKeyMaxLength = 50;

        public const int ClassPathMaxLength = 100;

        [StringLength(ProductInspectNoMaxLength)]
        public string TemplateNo { get; set; }
        [StringLength(NameMaxLength)]
        public string Name { get; set; }
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 检验报告
        /// </summary>
        [StringLength(ContentNoMaxLength)]
        public string Content { get; set; }
        public int Type { get; set; }
        /// <summary>
        /// 模板信息标识键
        /// </summary>
        [StringLength(TempKeyMaxLength)]
        public string TempKey { get; set; }

        [StringLength(FilePathMaxLength)]
        public string FilePath { get; set; }
        [StringLength(FileExtMaxLength)]
        public string FileExt { get; set; }
        [StringLength(ClassPathMaxLength)]
        public string ClassPath { get; set; }

    }
}

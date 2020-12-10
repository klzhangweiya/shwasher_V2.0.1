using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.BaseSysInfo
{
    [Table("Sys_Helps")]
    public class SysHelp:Entity<int>
    {
        public const int ClassificationMaxLength = 40;
        public const int HelpTitleMaxLength = 20;
        public const int HelpKeyWordsMaxLength = 20;
        public const int UserIDLastModMaxLength = 20;

        //[Required]
        /// <summary>
        /// 分类
        /// </summary>
        [StringLength(ClassificationMaxLength)]
        public string Classification { get; set; }

        [Required]
        [StringLength(HelpTitleMaxLength)]
        public string HelpTitle { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [StringLength(HelpKeyWordsMaxLength)]
        public string HelpKeyWords { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "ntext")]
        public string HelpContent { get; set; }

        public int Sequence { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
        
    }
}

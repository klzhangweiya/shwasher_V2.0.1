using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.BasicInfo
{
    [Table("Dutys")]
    public class Duty:Entity<int>
    {
        public const int DutyNameMaxLength = 50;

        public const int RemarkMaxLength = 200;

        public const int UserIDLastModMaxLength = 20;

        public const int IsLockMaxLength = 1;
        [Required]
        [StringLength(50)]
        public string DutyName { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(20)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(1)]
        public string IsLock { get; set; }
    }
}

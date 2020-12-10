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
    [Table("Departments")]
    public class Department:Entity<string>
    {
        public const int DepartmentNameMaxLength = 50;

        public const int RemarkMaxLength = 200;

        public const int UserIDLastModMaxLength = 20;

        public const int IsLockMaxLength = 1;

        public const int OrderStatusListMaxLength = 300;

        [Required]
        [StringLength(DepartmentNameMaxLength)]
        public string DepartmentName { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        [StringLength(OrderStatusListMaxLength)]
        public string OrderStatusList { get; set; }
    }
}

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
    [Table("Factories")]
    public class Factories:Entity<string>
    {
        public const int FactoryNameMaxLength = 100;

        public const int ShortNamesMaxLength = 20;
        public const int RegionIDMaxLength = 50;
        public const int FactoryURLMaxLength = 80;
        public const int AddressMaxLength = 100;
        public const int ZIPMaxLength = 50;
        public const int LinkManMaxLength = 50;
        public const int TelephoneMaxLength = 50;
        public const int RemarkMaxLength = 200;
        public const int IsLockMaxLength = 1;

        [Required]
        [StringLength(FactoryNameMaxLength)]
        public string FactoryName { get; set; }

        [Required]
        [StringLength(ShortNamesMaxLength)]
        public string ShortNames { get; set; }

        [Required]
        [StringLength(RegionIDMaxLength)]
        public string RegionID { get; set; }

        [StringLength(FactoryURLMaxLength)]
        public string FactoryURL { get; set; }

        [StringLength(AddressMaxLength)]
        public string Address { get; set; }

        [StringLength(ZIPMaxLength)]
        public string ZIP { get; set; }

        [StringLength(LinkManMaxLength)]
        public string LinkMan { get; set; }

        [StringLength(TelephoneMaxLength)]
        public string Telephone { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }
    }
}

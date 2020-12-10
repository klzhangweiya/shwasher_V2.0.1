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
    [Table("Regions")]
    public class Regions:Entity<string>
    {
        public const int RegionNameMaxLength = 50;

        public const int FatherRegionIDMaxLength = 50;

        public const int URLMaxLength = 80;

        public const int IsLeafMaxLength = 1;
        public const int PathMaxLength = 220;
        public const int UserIDLastModMaxLength =20;
        public const int IsLockMaxLength = 1;

        [Required]
        [StringLength(RegionNameMaxLength)]
        public string RegionName { get; set; }

        [Required]
        [StringLength(FatherRegionIDMaxLength)]
        public string FatherRegionID { get; set; }

        [StringLength(URLMaxLength)]
        public string URL { get; set; }

        public int Depth { get; set; }

        [Required]
        [StringLength(IsLeafMaxLength)]
        public string IsLeaf { get; set; }

        public int Sort { get; set; }

        [Required]
        [StringLength(PathMaxLength)]
        public string Path { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }
    }
}

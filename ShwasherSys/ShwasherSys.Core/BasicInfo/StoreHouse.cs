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
    [Table("StoreHouse")]
    public class StoreHouse:Entity<int>
    {
        //public int StoreHouseID { get; set; }
        public const int StoreHouseNameMaxLength = 50;

        public const int AddressMaxLength = 250;

        public const int TelMaxLength = 50;

        public const int FaxMaxLength = 50;

        public const int ContactManMaxLength = 50;

        public const int RemarkMaxLength = 500;
        public const int UserIDLastModMaxLength = 20;

        public const int IsLockMaxLength = 1;
        public const int StoreHouseNoMaxLength = 20;

        [Required]
        [StringLength(StoreHouseNameMaxLength)]
        public string StoreHouseName { get; set; }

        public int? StoreHouseTypeId { get; set; }

        [StringLength(AddressMaxLength)]
        public string Address { get; set; }

        [StringLength(TelMaxLength)]
        public string Tel { get; set; }

        [StringLength(FaxMaxLength)]
        public string Fax { get; set; }

        [StringLength(ContactManMaxLength)]
        public string ContactMan { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [StringLength(StoreHouseNoMaxLength)]
        public string StoreHouseNo { get; set; }
    }

  
}

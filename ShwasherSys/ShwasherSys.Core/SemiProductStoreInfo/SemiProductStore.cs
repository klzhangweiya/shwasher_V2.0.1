using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.SemiProductStoreInfo
{
    [Table("SemiProductStore")]
    public class SemiProductStore:Entity<int>
    {
      
        public const int SemiProductNoMaxLength = 32;
        public const int StoreYearMaxLength = 5;
        public const int ActiveMaxLength = 1;
        public const int StoreMonthMaxLength = 4;
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 半成品编号
        /// </summary>
        [StringLength(SemiProductNoMaxLength)]
        public string SemiProductNo { get; set; }
        
        /// <summary>
        /// 半成品数量
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        [StringLength(StoreYearMaxLength)]
        public string StoreYear { get; set; }
        [StringLength(StoreMonthMaxLength)]
        public string StoreMonth { get; set; }
        [Required]
        [StringLength(ActiveMaxLength)]
        public string Active { get; set; }
        
        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }
    }
}

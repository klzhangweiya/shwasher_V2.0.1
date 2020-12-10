using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.NotificationInfo
{
    [Table("BulletinInfo")]
    public class BulletinInfo:Entity<int>
    {
        public const int BulletinTypeMaxLength = 1;
        public const int TitleMaxLength = 100;
        public const int PromulgatorMaxLength = 50;
        public const int UserIDLastModMaxLength = 20;
        [Required]
        [StringLength(BulletinTypeMaxLength)]
        public string BulletinType { get; set; }

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [StringLength(PromulgatorMaxLength)]
        public string Promulgator { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PromulgatTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpirationDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductInfo
{
    

    [Table("Standard")]
    public  class Standard:Entity<int>
    {
       
        //public int StandardId { get; set; }
        public const int StandardNameMaxLength = 50;
        public const int StandardDescMaxLength = 150;
        public const int UserIDLastModMaxLength = 20;

        [Required]
        [StringLength(StandardNameMaxLength)]
        public string StandardName { get; set; }

        [StringLength(StandardDescMaxLength)]
        public string StandardDesc { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

    }
}

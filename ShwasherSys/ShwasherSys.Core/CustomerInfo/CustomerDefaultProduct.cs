using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Abp.Domain.Entities;

namespace ShwasherSys.CustomerInfo
{
   

    [Table("CustomerDefaultProduct")]
    public  class CustomerDefaultProduct:Entity<int>
    {
        public const int CustomerIdMaxLength = 30;
        public const int ProductNoMaxLength = 30;
        public const int CustomerProductNameMaxLength = 30;
       

        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }

        [StringLength(CustomerProductNameMaxLength)]
        public string CustomerProductName { get; set; }

        public int Sequence { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }
    }
}

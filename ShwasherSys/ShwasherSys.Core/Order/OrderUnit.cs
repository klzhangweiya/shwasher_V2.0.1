using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Order
{
    [Table("OrderUnit")]
    public class OrderUnit:Entity<int>
    {
        [StringLength(50)]
        public string OrderUnitName { get; set; }
        public int? ProductNum { get; set; }
        [StringLength(250)]
        public string OrderUnitDesc { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }
        public string UserIDLastMod { get; set; }
    }
}

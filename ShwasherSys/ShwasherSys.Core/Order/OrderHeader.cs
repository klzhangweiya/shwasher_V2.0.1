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
    [Table("OrderHeader")]
    public class OrderHeader:Entity<string>
    {
        public const int OrderNoMaxLength = 30;
        public const int DelUserIdMaxLength = 30;
        public const int CustomerIdMaxLength = 30;
        public const int LinkNameMaxLength = 50;
        public const int FaxMaxLength = 50;
        public const int TelephoneMaxLength = 50;
        public const int StockNoMaxLength = 50;
        public const int UserIDLastModMaxLength = 30;
        public const int IsOutSaleMaxLength = 30;
        public const int SaleManMaxLength = 50;
        public const int IsLockMaxLength = 1;
        /*[StringLength(OrderNoMaxLength)]
        public string OrderNo { get; set; }*/

        [Required]
        [StringLength(CustomerIdMaxLength)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(LinkNameMaxLength)]
        public string LinkName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime OrderDate { get; set; }

        [StringLength(FaxMaxLength)]
        public string Fax { get; set; }

        [StringLength(TelephoneMaxLength)]
        public string Telephone { get; set; }

        public int CustomerSendId { get; set; }

        [StringLength(StockNoMaxLength)]
        public string StockNo { get; set; }

        public int OrderStatusId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

      /// <summary>
      /// 内销：0 外销 ：1
      /// </summary>
        public int? SaleType { get; set; }

      //销售人员
      [StringLength(SaleManMaxLength)]
      public string SaleMan { get; set; }
      //是否删除
      [StringLength(IsLockMaxLength)]
      public string IsLock { get; set; }
    }
}

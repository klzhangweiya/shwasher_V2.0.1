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
    [Table("Currency")]
    public class Currency:Entity<string>
    {
        public const int CurrencyNameMaxLength = 20;
        public const int UserIdLastModMaxLength = 20;

        [StringLength(CurrencyNameMaxLength)]
        public string CurrencyName { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeLastMod { get; set; }
        [StringLength(UserIdLastModMaxLength)]
        public string UserIDLastMod { get; set; }
    }

    [Table("CurrencyExchangeRate")]
    public class CurrencyExchangeRate : Entity<int>
    {
        public const int CurrencyNameMaxLength = 20;
        public const int CurrencyIdMaxLength = 20;
        public const int UserIdLastModMaxLength = 20;

        [StringLength(CurrencyIdMaxLength)]
        public string FromCurrencyId { get; set; }
        [StringLength(CurrencyIdMaxLength)]
        public string ToCurrencyId { get; set; }
        [DecimalPrecision(scale:4)]
        public decimal? ExchangeRate { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeLastMod { get; set; }
        [StringLength(UserIdLastModMaxLength)]
        public string UserIDLastMod { get; set; }
    }
}

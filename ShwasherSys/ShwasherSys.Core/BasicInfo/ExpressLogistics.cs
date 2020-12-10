using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using IwbZero.Caching;

namespace ShwasherSys.BasicInfo
{
    [Table("ExpressLogistics")]
    public class ExpressLogistics : Entity<int>
    {
        public const int ExpressNameMaxLength = 50;
        public const int CodeMaxLength = 50;
        public const int UserIDLastModMaxLength = 50;
        public const int IsLockMaxLength = 1;

        [StringLength(ExpressNameMaxLength)]
        public string ExpressName { get; set; }
        [StringLength(CodeMaxLength)]
        public string Code { get; set; }

        public int Sort { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TimeLastMod { get; set; }

        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }

        [Required]
        [StringLength(IsLockMaxLength)]
        public string IsLock { get; set; }

        // public virtual  List<ExpressProviderMapper> ExpressProviderMapper { get; set; }
    }
    [Table("ExpressServiceProviders")]
    public class ExpressServiceProvider : Entity<int>
    {
        public const int IsLockMaxLength = 1;
        public const int UserIDLastModMaxLength = 50;
        public const int ProviderNameMaxLength = 50;
        public const int QueryApiUrlMaxLength = 150;
        public const int CallBackUrlMaxLength = 150;
        public const int ExcuteNamespaceAndMethodMaxLength = 150;
        /// <summary>
        /// 快递查询服务商的名称
        /// </summary>
        [StringLength(ProviderNameMaxLength)]
        public string ProviderName { get; set; }

        /// <summary>
        /// 查询的API接口地址（预留备用）
        /// </summary>
        [StringLength(QueryApiUrlMaxLength)]
        public string QueryApiUrl { get; set; }
        /// <summary>
        /// 服务商回调的接口地址（预留备用）
        /// </summary>
        [StringLength(CallBackUrlMaxLength)]
        public string CallBackUrl { get; set; }

        /// <summary>
        /// 执行调用服务商方法（预留备用,不同服务商执行逻辑不同，后续可采用反射的方式执行）
        /// </summary>
        [StringLength(ExcuteNamespaceAndMethodMaxLength)]
        public string ExcuteNamespaceAndMethod { get; set; }



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
    [Table("ExpressProviderMappers")]
    public class ExpressProviderMapper : Entity<int>
    {
        public const int MapperCodeMaxLength = 50;
        public const int ExtendInfoMaxLength = 500;
        public const int QueryUrlMaxLength = 500;

        [ForeignKey("ExpressId")]
        public ExpressLogistics ExpressLogistics { get; set; }
        public int ExpressId { get; set; }
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public ExpressServiceProvider ExpressServiceProvider { get; set; }

        [StringLength(MapperCodeMaxLength)]
        public string MapperCode { get; set; }
        [StringLength(QueryUrlMaxLength)]
        public string QueryUrl { get; set; }

        [StringLength(ExtendInfoMaxLength)]
        public string ExtendInfo { get; set; }

        public int? ActiveStatus { get; set; }
    }
}

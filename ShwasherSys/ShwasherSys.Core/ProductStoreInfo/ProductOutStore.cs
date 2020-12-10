using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.ProductStoreInfo
{
    [Table("ProductOutStore")]
    public class ProductOutStore : Entity<int>
    {
        public const int ProductionOrderNoMaxLength = 11;
        public const int CurrentProductStoreHouseNoMaxLength = 32;
        public const int ProductNoMaxLength = 50;
      
        public const int UserIDLastModMaxLength = 20;
        public const int CreatorUserIdMaxLength = 20;
        public const int RemarkMaxLength = 150;

        
        [StringLength(ProductionOrderNoMaxLength)]
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 成品库存中记录编号
        /// </summary>
        [StringLength(CurrentProductStoreHouseNoMaxLength)]
        public string CurrentProductStoreHouseNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }
        [Required]
        public int StoreHouseId { get; set; }
        /// <summary>
        ///0.新建 1.申请中 2.已审核 3.已取消 4.已拒绝 5.已出库 
        /// </summary>
        [Required]
        public int ApplyStatus { get; set; }
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClose { get; set; }

        /// <summary>
        /// 1.未确认 2.确认
        /// </summary>   
        public Boolean? IsConfirm { get; set; }

        /// <summary>
        /// 申请出库数量(千件)
        /// </summary>
        [DecimalPrecision]
        public decimal Quantity { get; set; }
        /// <summary>
        /// 审核后出库数量
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

     
        /// <summary>
        /// 审核人员
        /// </summary>
        [StringLength(UserIDLastModMaxLength)]
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }


        [StringLength(UserIDLastModMaxLength)]
        public string OutStoreUser { get; set; }

        public DateTime? OutStoreDate { get; set; }
        /// <summary>
        /// 申请出库时间
        /// </summary>
        public DateTime? ApplyOutDate { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        [StringLength(CreatorUserIdMaxLength)]
        public string CreatorUserId { get; set; }
        [StringLength(UserIDLastModMaxLength)]
        public string UserIDLastMod { get; set; }


        public int OrderSendId { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        [DecimalPrecision]
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 申请出库源
        /// </summary>
        public int ApplyOutStoreSourceType { get; set; }

        /// <summary>
        /// 创建出库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }

    }
}

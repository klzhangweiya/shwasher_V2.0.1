using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.PackageInfo
{
    [Table("N_ViewPackageApply")]
    public class ViewPackageApply:Entity<int>
    {
        /// <summary>
        /// 申请流水号
        /// </summary>
        public string PackageApplyNo { get; set; }
        /// <summary>
        /// 仓库库存信息编号
        /// </summary>
   
        [Column("CurrentStoreHouseNo")]
        public string CurrentSemiStoreHouseNo { get; set; }
        /// <summary>
        /// 流转单编号
        /// </summary>
        public string ProductionOrderNo { get; set; }

        /// <summary>
        /// 半成品编号
        /// </summary>
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 申请包装数量
        /// </summary>
        [DecimalPrecision]
        public decimal ApplyQuantity { get; set; }

        /// <summary>
        /// 实际包装的千件数
        /// </summary>
        [DecimalPrecision]
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 仓库来源
        /// </summary>
        public int SourceStore { get; set; }

      
        public string ApplyStatus { get; set; }
        public bool IsClose { get; set; }
        /// <summary>
        /// 发起申请时间
        /// </summary>
        public DateTime? ApplyDate { get; set; }

       
        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        public string CreatorUserId { get; set; }
        public string UserIDLastMod { get; set; }
        /// <summary>
        /// 待处理明细数量
        /// </summary>
        public int ProcessingNum { get; set; }
       
        /// <summary>
        /// 半成品申请入库数量（KG）
        /// </summary>
        public decimal? IsApplyEnterQuantity { get; set; }
        /// 成品申请入库数量（千件）
        public decimal? IsApplyEnterQuantity2 { get; set; }
        public decimal? KgWeight { get; set; }

    }
}

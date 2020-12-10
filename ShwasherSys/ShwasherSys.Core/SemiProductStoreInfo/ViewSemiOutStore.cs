using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.SemiProductStoreInfo
{
    [Table("N_ViewSemiOutStore")]
    public class ViewSemiOutStore : Entity<int>
    {
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// 半成品库存中记录编号
        /// </summary>
        public string CurrentSemiStoreHouseNo { get; set; }
      
        public int StoreHouseId { get; set; }
        /// <summary>
        /// 1.申请中 2.已出库 3.已取消
        /// </summary>
       
        public string ApplyStatus { get; set; }
        /// <summary>
        /// 1.生产 2.包装
        /// </summary>
        public int ApplyTypes { get; set; }
        public bool? IsClose { get; set; }

        public Boolean? IsConfirm { get; set; }
        /// <summary>
        /// 申请出库来源（1.外协加工申请 2.包装申请）
        /// </summary>
      
        public string ApplyOutStoreSource { get; set; }

        /// <summary>
        /// 半成品编号
        /// </summary>
       
        public string SemiProductNo { get; set; }
        /// <summary>
        /// 申请入库数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 实际出库数量（用于填入实时库存中的数量）
        /// </summary>
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 申请出库时间
        /// </summary>
        public DateTime? ApplyOutDate { get; set; }

        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        public string CreatorUserId { get; set; }
        public string UserIDLastMod { get; set; }
        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }
        public string SemiProductName { get; set; }

        public string Model { get; set; }



        public string Material { get; set; }



        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string IsStandard { get; set; }
        public string PartNo { get; set; }

        public decimal? TranUnitValue { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        public decimal KgWeight { get; set; }

        /// <summary>
        /// 创建入库来源类型（1:默认，2:手动调节）
        /// </summary>
        public int? CreateSourceType { get; set; }
    }
}

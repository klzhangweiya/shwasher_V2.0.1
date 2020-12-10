using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.ProductStoreInfo
{
    /// <summary>
    /// 盘点计划
    /// </summary>
    [Table("InventoryCheck")]
    public class InventoryCheckInfo:FullAuditedEntity<string>
    {
        public const int ProductNoMaxLength = 32;
        public const int CheckUserMaxLength = 32;
        public const int PublishUserMaxLength = 32;
        public const int CheckNoMaxLength = 32;
        public const int StoreAreaCodeMaxLength = 32;
        public const int ShelfNumberMaxLength = 32;
        public const int ShelfLevelMaxLength = 32;
        public const int SequenceNoMaxLength = 32;
        public const int CheckTypeMaxLength = 10;
        public const int RemarkMaxLength = 200;
        [MaxLength(CheckNoMaxLength)]
        public string CheckNo { get; set; }

        ///// <summary>
        ///// 盘点的仓库类型
        ///// </summary>
        //public int? StoreHouseTypeId { get; set; }

        public int StoreHouseId { get; set; }

        /// <summary>
        /// 库区
        /// </summary>   
        [MaxLength(StoreAreaCodeMaxLength)]
        public string StoreAreaCode { get; set; }
        /// <summary>
        /// 货架号
        /// </summary>   
        [MaxLength(ShelfNumberMaxLength)]
        public string ShelfNumber { get; set; }
        /// <summary>
        /// 层次
        /// </summary>   
        [MaxLength(ShelfLevelMaxLength)]
        public string ShelfLevel { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>   
        [MaxLength(SequenceNoMaxLength)]
        public string SequenceNo { get; set; }
        /// <summary>
        /// 盘点的依据类型（暂时只根据库位，产品或其它）预留
        /// </summary>
        [MaxLength(CheckTypeMaxLength)]
        public string CheckType { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartDate { get; set; }
        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlanEndDate { get; set; }
        [MaxLength(RemarkMaxLength)]
        public string Remark { get; set; }

        /// <summary>
        /// 待盘点人员
        /// </summary>
        [MaxLength(CheckUserMaxLength)]
        public string CheckUser { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [MaxLength(PublishUserMaxLength)]
        public string PublishUser { get; set; }

        /// <summary>
        /// 盘点完成时间
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 盘点状态（1:新建 2:盘点中 3:盘点完成  4:取消）
        /// </summary>
        public int CheckState { get; set; }

        [StringLength(ProductNoMaxLength)]
        public string ProductNo { get; set; }

    }
    /// <summary>
    /// 盘点记录明细
    /// </summary>
    [Table("InventoryCheckRecord")]
    public class InventoryCheckRecord : AuditedEntity<string>
    {
        public const int CheckNoMaxLength = 32;
        public const int CurrentStoreHouseNoMaxLength = 32;

        [MaxLength(CheckNoMaxLength)]
        public string CheckNo { get; set; }

        /// <summary>
        /// 半成品或者成品库存记录编号
        /// </summary>
        [MaxLength(CurrentStoreHouseNoMaxLength)]
        public string CurrentStoreHouseNo { get; set; }


        public decimal CheckQuantity { get; set; }

        public decimal StoreQuantity { get; set; }

        public string InsertSql()
        {
            return
                $" insert into InventoryCheckRecord (Id,CheckNo,CurrentStoreHouseNo,CheckQuantity,StoreQuantity,CreationTime,CreatorUserId) values ('{Id}','{CheckNo}','{CurrentStoreHouseNo}','{CheckQuantity}','{StoreQuantity}','{CreationTime}',{CreatorUserId});";
        }
    }

    public class CurrentStoreItemDto:Entity<int>
    {
        public string CurrentProductStoreHouseNo { get; set; }

        public string ProductionOrderNo { get; set; }

        public int StoreHouseId { get; set; }
        /// <summary>
        /// 库位编码
        /// </summary>
        public string StoreLocationNo { get; set; }
        /// <summary>
        /// 成品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 冻结数量（用于出库申请之后还未正式出库的数量）
        /// </summary>
        public decimal FreezeQuantity { get; set; }
        /// <summary>
        /// 当前实际数量
        /// </summary>
        public decimal Quantity { get; set; }

        public string Remark { get; set; }

        public DateTime? TimeCreated { get; set; }


        public DateTime? TimeLastMod { get; set; }
        public string CreatorUserId { get; set; }
        public string UserIDLastMod { get; set; }
        public string ProductName { get; set; }

        public string Model { get; set; }


        public string Material { get; set; }


        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string IsStandard { get; set; }
        public string ProductDesc { get; set; }
        /// <summary>
        /// 千件重
        /// </summary>
        public decimal KgWeight { get; set; }
        /// <summary>
        /// 上月底剩余数量
        /// </summary>
        [DecimalPrecision]
        public decimal? PreMonthQuantity { get; set; }
        public int? InventoryCheckState { get; set; }

        /// <summary>
        /// 退货冻结状态（1：未被退货 2：被退货冻结）
        /// </summary>

        public int? ReturnState { get; set; }

        public string StoreAreaCode { get; set; }

        public string ShelfNumber { get; set; }

        public string ShelfLevel { get; set; }

        public string SequenceNo { get; set; }
    }
    [Table("N_ViewInventoryCheckRecord_Semi")]
    public class ViewInventoryCheckRecordSemi : AuditedEntity<string>
    {
        public string ProductNo { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }


        public string Material { get; set; }


        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string ProductDesc { get; set; }

        public string CheckNo { get; set; }

      
        public string CurrentStoreHouseNo { get; set; }


        public decimal CheckQuantity { get; set; }

        public decimal StoreQuantity { get; set; }

        public decimal FreezeQuantity { get; set; }
     
        public decimal Quantity { get; set; }

        public string StoreLocationNo { get; set; }

        public int StoreHouseId { get; set; }

        public string ProductionOrderNo { get; set; }
    }

    [Table("N_ViewInventoryCheckRecord_Product")]
    public class ViewInventoryCheckRecordProduct : AuditedEntity<string>
    {
        public string ProductNo { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }


        public string Material { get; set; }


        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string ProductDesc { get; set; }

        public string CheckNo { get; set; }


        public string CurrentStoreHouseNo { get; set; }


        public decimal CheckQuantity { get; set; }

        public decimal StoreQuantity { get; set; }

        public decimal FreezeQuantity { get; set; }

        public decimal Quantity { get; set; }

        public string StoreLocationNo { get; set; }

        public int StoreHouseId { get; set; }

        public string ProductionOrderNo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ShwasherSys.BasicInfo
{
    [Table("StoreHouseLocation")]
    public class StoreHouseLocation : FullAuditedEntity<int>
    {
        public const int StoreLocationNoMaxLength = 32;

        public const int StoreAreaCodeMaxLength = 32;

        public const int ShelfNumberMaxLength = 50;

        public const int ShelfLevelMaxLength = 10;

        public const int SequenceNoMaxLength = 10;

        public const int RemarkMaxLength = 250;
        [StringLength(StoreLocationNoMaxLength)]
        public string StoreLocationNo { get; set; }
        /// <summary>
        /// 库区
        /// </summary>
        [StringLength(StoreAreaCodeMaxLength)]
        public string StoreAreaCode { get; set; }
        /// <summary>
        /// 货架号
        /// </summary>
        [StringLength(ShelfNumberMaxLength)]
        public string ShelfNumber { get; set; }
        /// <summary>
        /// 层次
        /// </summary>
        [StringLength(ShelfLevelMaxLength)]
        public string ShelfLevel { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        [StringLength(SequenceNoMaxLength)]
        public string SequenceNo { get; set; }

        [StringLength(RemarkMaxLength)]
        public string Remark { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public int? StoreHouseId { get; set; }

    }
}

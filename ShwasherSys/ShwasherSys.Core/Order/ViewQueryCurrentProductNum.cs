using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace ShwasherSys.Order
{
    [Table("v_QueryCurrentProductNum")]
    public class ViewQueryCurrentProductNum:Entity<string>
    {
        /// <summary>
        /// 被预定数量
        /// </summary>
        public decimal? BookedQuantity { get; set; }
        /// <summary>
        /// 库存可用数量
        /// </summary>
        public decimal? CanUserQuantity { get; set; }
    }
    [Table("v_BookedProductNum")]
    public class ViewBookedProductNum : Entity<string>
    {
        /// <summary>
        /// 被预定数量
        /// </summary>
        public decimal? BookedQuantity { get; set; }
    }
    [Table("v_CanProductStore")]
    public class ViewCanProductStore : Entity<string>
    {
        /// <summary>
        /// 库存可用数量
        /// </summary>
        public decimal? CanUserQuantity { get; set; }
    }
}

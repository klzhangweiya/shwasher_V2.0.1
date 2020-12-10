using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace ShwasherSys.ProductStoreInfo
{
    [Table("v_Store_Query")]
    public class ViewEnterOutProductStore:Entity<string>
    {
        public int ActualId { get; set; }
        /// <summary>
        /// 进出口标识位（1:入库 2:出库）
        /// </summary>
        public int EnterOutFlag { get; set; }

        public string ProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public decimal Quantity { get; set; }

        public DateTime? DateTiem { get; set; }

        public string UserIDLastMod { get; set; }

        public string Remark { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string PartNo { get; set; }

        public decimal? AllQuantity { get; set; }

        public decimal? AllFreezeQuantity { get; set; }
        public decimal? AllPreMonthQuantity { get; set; }
    }
    [Table("vEnterOutLogDetail_c")]
    public class ViewEnterOutLogCus : Entity<string>
    {
        public int ActualId { get; set; }
        /// <summary>
        /// 进出口标识位（1:入库 2:出库）
        /// </summary>
        public int EnterOutFlag { get; set; }

        public string ProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public decimal Quantity { get; set; }

        public DateTime? DateTiem { get; set; }

        public string UserIDLastMod { get; set; }

        public string Remark { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string PartNo { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }


        public string ProductionOrderNo { get; set; }
        

    }
    [Table("v_ProductStoreInfo")]
    public class ViewCurrentStoreTotal : Entity<string>
    {
        //public string ProductNo { get; set; }

        public decimal AllQuantity { get; set; }

        public decimal AllFreezeQuantity { get; set; }
        public decimal? AllPreMonthQuantity { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string PartNo { get; set; }
        [DecimalPrecision()]
        public Decimal? Defprice { get; set; }
    }

    [Table("v_SemiProductStoreInfo")]
    public class ViewCurrentSemiStoreTotal : Entity<string>
    {
        //public string ProductNo { get; set; }

        public decimal AllQuantity { get; set; }

        public decimal AllFreezeQuantity { get; set; }
        public decimal? AllPreMonthQuantity { get; set; }

        public string SemiProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string PartNo { get; set; }

        public string ProductDesc { get; set; }
    }

    [Table("v_EnterOutSemiProductStore")]
    public class ViewEnterOutSemiProductStore : Entity<string>
    {
        public int ActualId { get; set; }
        /// <summary>
        /// 进出口标识位（1:入库 2:出库）
        /// </summary>
        public int EnterOutFlag { get; set; }

        public string SemiProductNo { get; set; }

        public int StoreHouseId { get; set; }

        public decimal Quantity { get; set; }

        public DateTime? DateTiem { get; set; }

        public string UserIDLastMod { get; set; }

        public string Remark { get; set; }

        public string SemiProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string PartNo { get; set; }

       

        public string ProductDesc { get; set; }
    }
}

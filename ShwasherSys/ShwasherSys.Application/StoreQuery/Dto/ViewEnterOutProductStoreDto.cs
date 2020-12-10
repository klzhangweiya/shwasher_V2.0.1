using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ShwasherSys.ProductStoreInfo;

namespace ShwasherSys.StoreQuery.Dto
{
    [AutoMapTo(typeof(ViewEnterOutProductStore)), AutoMapFrom(typeof(ViewEnterOutProductStore))]
    public class ViewEnterOutProductStoreDto : EntityDto<string>
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

        public decimal AllQuantity { get; set; }

        public decimal AllFreezeQuantity { get; set; }
    }
    [AutoMapTo(typeof(ViewCurrentStoreTotal)), AutoMapFrom(typeof(ViewCurrentStoreTotal))]
    public class ViewCurrentStoreTotalDto : EntityDto<string>
    {
        //public string ProductNo { get; set; }

        public decimal AllQuantity { get; set; }

        public decimal AllFreezeQuantity { get; set; }

        public string ProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }
        public string PartNo { get; set; }
    }
}

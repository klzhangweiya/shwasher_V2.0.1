using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.StoreQuery.Dto
{
    public class ProductStoreCount
    {
        public string ProductNo { get; set; }
        public string ProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string PartNo { get; set; }
       
        public decimal? Defprice { get; set; }

        public decimal? AllQuantity { get; set; }

        public decimal? AllFreezeQuantity { get; set; }

        public decimal? AllOutQuantity { get; set; }

        public decimal? AllEnterQuantity { get; set; }

        public decimal? AllPreMonthQuantity { get; set; }
    }

    public class SemiProductStoreCount
    {
        public string SemiProductNo { get; set; }
        public string SemiProductName { get; set; }

        public string Model { get; set; }

        public string Material { get; set; }

        public string SurfaceColor { get; set; }

        public string Rigidity { get; set; }

        public string PartNo { get; set; }

        public string ProductDesc { get; set; }

        public decimal? AllQuantity { get; set; }

        public decimal? AllFreezeQuantity { get; set; }

        public decimal? AllOutQuantity { get; set; }

        public decimal? AllEnterQuantity { get; set; }

        public decimal? AllPreMonthQuantity { get; set; }
    }
}

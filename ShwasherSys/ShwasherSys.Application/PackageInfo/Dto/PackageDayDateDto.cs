using System;
using System.Collections.Generic;
using System.Linq;

namespace ShwasherSys.PackageInfo.Dto
{
    public class PackageDayDateDto 
    {
        public PackageDayDateDto(DateTime dayDate,List<PackageDayDateItem> packageItems,int index ,decimal kgTotal,decimal pcsTotal)
        {
            DayDate = dayDate;
            
            KgTotal = kgTotal;
            PcsTotal = pcsTotal;
            if (packageItems != null && packageItems.Any())
            {
                var items1 = packageItems.Where(a => a.ApplySourceType == 1).ToList();
                KgQuantity1 = items1.Sum(a => a.KgQuantity);
                PcsQuantity1 = items1.Sum(a => a.PcsQuantity);
                var items2 = packageItems.Where(a => a.ApplySourceType == 2).ToList();
                KgQuantity2 = items2.Sum(a => a.KgQuantity);
                PcsQuantity2 = items2.Sum(a => a.PcsQuantity);
                if (index > 0)
                {
                    index = index - packageItems.Count;
                }
                foreach (var item in packageItems)
                {
                    index++;
                    item.Index = index;
                    item.PackageDate = DayDate.ToString("yyyy-MM-dd");
                }

                PackageItems = packageItems;
            }
            else
            {
                KgQuantity1 = 0;
                PcsQuantity1 = 0;
                KgQuantity2 = 0;
                PcsQuantity2 = 0;
                PackageItems=new List<PackageDayDateItem>();
            }

        }

        public decimal KgQuantity1{ get; set; }
        public decimal KgQuantity2{ get; set; }
        public decimal PcsQuantity1{ get; set; }
        public decimal PcsQuantity2{ get; set; }
        public decimal KgTotal{ get; set; }
        public decimal PcsTotal{ get; set; }
        public DateTime DayDate { get; set; }
        public List<PackageDayDateItem> PackageItems { get; set; }

    }
    public class PackageDayDateItem
    {
        public int Index { get; set; }

        public string PackageDate { get; set; }
        public string PackageApplyNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string PartNo { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public string  SurfaceColor{ get; set; }
        public string Rigidity { get; set; }
        public decimal KgQuantity { get; set; } = 0;
        public decimal PcsQuantity { get; set; } = 0;
        public decimal KgWeight { get; set; } = 0;
        public string PackageDetail => $"{PackageCount}包({PackageSpecification}千件/包)";
        public decimal PackageCount { get; set; } = 0;
        public decimal PackageSpecification { get; set; } = 0;
        public string PackageEnterNum { get; set; }
        public string PackageUser { get; set; }
        public string VerifyUser { get; set; } 
        public int ApplySourceType { get; set; }
        public string PackageType => ApplySourceType == 1 ? "生产包装" : "改包装";

    }
}
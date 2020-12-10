using System;
using System.Collections.Generic;
using System.Linq;

namespace ShwasherSys.ProductionOrderInfo.Dto
{
    public class QueryProductionReportDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public int? EmployeeId { get; set; }
    }

    
    public class ProductionReportDto 
    {
        public ProductionReportDto(string dayDate,List<ProductionReportItem> items,int? employeeId )
        {
            DayDate = dayDate;
            if (items != null && items.Any())
            {
                if (employeeId==null)
                {
                    Items=new List<ProductionReportItem>();
                    var temps = items.GroupBy(a =>a.EmployeeId).Select(a=>new ProductionReportItem
                    {
                        EmployeeId= a.Key,
                        KgQuantity = a.Sum(s=>s.KgQuantity),
                        PcsQuantity = a.Sum(s=>s.PcsQuantity)

                    } );
                    foreach (var item in temps)
                    {
                        var temp = items.FirstOrDefault(a => a.EmployeeId == item.EmployeeId);
                        if (temp == null)
                        {
                            continue;
                        }
                        var newItem= new ProductionReportItem()
                        {
                            ProductDate = temp.ProductDate,
                            EmployeeId = item.EmployeeId,
                            EmployeeNo = temp.EmployeeNo,
                            EmployeeName = temp.EmployeeName,
                            KgQuantity = item.KgQuantity,
                            PcsQuantity = item.PcsQuantity
                        };
                        Items.Add(newItem);
                    }
                }
                else
                {
                    var employee = items.FirstOrDefault();
                    EmployeeNo = employee?.EmployeeNo;
                    EmployeeName = employee?.EmployeeName;
                    Items = items;
                }
                KgTotal = items.Sum(a => a.KgQuantity);
                PcsTotal = items.Sum(a => a.PcsQuantity);
            }
            else
            {
                KgTotal = 0;
                PcsTotal = 0;
               
            }
        }
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public decimal KgTotal{ get; set; }
        public decimal PcsTotal{ get; set; }
        public string DayDate { get; set; }
        public List<ProductionReportItem> Items { get; set; }

    }
    public class ProductionReportItem
    {
        public DateTime? ProductDate { get; set; }
        public string ProductionOrderNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string CarNo { get; set; }
        public string PartNo { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public string SurfaceColor { get; set; }
        public string Rigidity { get; set; }
        public decimal KgQuantity{ get; set; }
        public decimal PcsQuantity{ get; set; }
        public decimal KgWeight { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
       

    }
}
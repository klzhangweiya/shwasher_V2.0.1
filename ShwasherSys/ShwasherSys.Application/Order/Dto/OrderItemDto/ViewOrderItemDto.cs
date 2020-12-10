using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys.Order.Dto
{
    public class ViewOrderItemDtoForExcel
    {
        //i.OrderNo,
        //OrderItemStatusName = StatesAppService.GetDisplayValue("OrderItems", "OrderItemStatusId", i.OrderItemStatusId.ToString()),
        //i.CustomerName,
        //i.Model,
        //i.Rigidity,
        //i.SurfaceColor,
        //i.Material,
        //i.PartNo,
        //i.OrderDate,
        //i.Quantity,
        //IsPartSend = i.IsPartSend == "Y" ? "是" : "否",
        //IsReport = i.IsReport == "Y" ? "需要" : "不需要",
        //i.TotalPrice,
        //i.IsSendQuantity,
        //i.RemainingQuantity,
        //i.OrderUnitName,
        //i.StockNo,
        //i.ProductNo,
        //i.ProductName,
        //i.SendDate,
        //i.OrderItemDesc
        public string OrderNo { get; set; }
        public string OrderItemStatusName { get; set; }
        public string CustomerName { get; set; }
        public string Model { get; set; }
        public string Rigidity { get; set; }
        public string SurfaceColor { get; set; }
        public string Material { get; set; }
        public string PartNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Quantity { get; set; }
        public string IsPartSend { get; set; }
        public string IsReport { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? IsSendQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public string OrderUnitName { get; set; }
        public string StockNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public DateTime? SendDate { get; set; }
        public string OrderItemDesc { get; set; }
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.AutoMapper;
using AutoMapper;
using ShwasherSys.Invoice;
using ShwasherSys.Order;

namespace ShwasherSys.Models.OrderInfo
{
    [AutoMapFrom(typeof(ViewOrderItems))]
    public class OrderItemSendDto:ViewOrderItems
    {
        [IgnoreMap]
        public List<ViewOrderSendStickBill> ViewOrderSendStickBills { get; set; }
    }
}
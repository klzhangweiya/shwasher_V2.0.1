using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Timing;
using IwbZero.IdentityFramework;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Inspection;
using ShwasherSys.OrderSendInfo;
using static System.Double;

namespace ShwasherSys.Common
{
    public class OrderSendBillExport
    {
        /// <summary>
        /// 艾维尼尔森
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderSends"></param>
        /// <param name="customer"></param>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public string ExportTempAwens(OrderSendBill bill,List<ViewOrderSend> orderSends,Customer customer,TemplateInfo templateInfo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory +templateInfo.FilePath;
            var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(7, 2).SetCellValue(bill.SendDate?.ToString("D"));
            sheet1.GenerateCell(9, 2).SetCellValue("");
            int index = 0;
            foreach (var send in orderSends)
            {
                sheet1.GenerateCell(19+index, 1).SetCellValue(send.StockNo);
                sheet1.GenerateCell(19+index, 2).SetCellValue(send.PartNo);
                sheet1.GenerateCell(19+index, 3).SetCellValue((send.Model??"")+","+(send.ProductName ?? "") + ","+(send.SurfaceColor ?? "") + ","+(send.Rigidity ?? ""));
                sheet1.GenerateCell(19+index, 4).SetCellValue(send.ProductBatchNum);
                var sendQuantity = Math.Floor(send.SendQuantity * 1000) / 1000;//发货数量
                var quantityPerPack = Math.Floor((send.QuantityPerPack ?? 0)*1000)/1000;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                sheet1.GenerateCell(19+index, 5).SetValue<decimal>(sendQuantity*1000); 
                sheet1.GenerateCell(19+index, 8).SetValue<decimal>(quantityPerPack * 1000);
                sheet1.GenerateCell(19+index, 9).SetValue<decimal>(packageCount);
                sheet1.GenerateCell(19+index, 10).SetValue<decimal>(sysl * 1000);
                index++;
            }
            var fileName = $"艾维尼尔森-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }
        /// <summary>
        /// 美国柏中
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderSends"></param>
        /// <param name="customer"></param>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public string ExportTempMgbz(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer, TemplateInfo templateInfo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateInfo.FilePath;
            var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(5, 1).SetCellValue("发货日期"+bill.SendDate?.ToString("yyyy-MM-dd"));
            for (int i = 8; i <= 14; i++)
            {
                sheet1.RemoveRow(sheet1.GenerateRow(i));
                //RemoveMergedRegion(sheet1, i);
            }
            for (int i = 0; i < 6; i++)
            {
                RemoveMergedRegion(sheet1, 8 + i);
            }
            int index = 0;
            decimal totalCount = 0;
            decimal allTotalPrice = 0;
            ICellStyle style1 = SetCellStyle(work,fontHeightInPoints:10,horizontalAlignment:HorizontalAlignment.Left);
            ICellStyle style11 = SetCellStyle(work, boldWeight: (short)FontBoldWeight.Normal,fontHeightInPoints:10);
            foreach (var send in orderSends)
            {
                sheet1.GenerateCell(7 + index, 1).SetValue<int>(index+1, style1);
                sheet1.GenerateCell(7 + index, 2).SetValue(send.StockNo??"", style11);
                sheet1.GenerateCell(7 + index, 3).SetValue(send.PartNo??"", style11);
                sheet1.GenerateCell(7 + index, 4).SetValue(send.ProductName??"", style11);
                sheet1.GenerateCell(7 + index, 5).SetValue(send.SurfaceColor??"", style11);
                sheet1.GenerateCell(7 + index, 6).SetValue<decimal>("", style11);
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                sheet1.GenerateCell(7 + index, 7).SetValue<decimal>(sendQuantity * 1000, style11);
                totalCount += sendQuantity * 1000;
                sheet1.GenerateCell(7 + index, 8).SetValue<decimal>(send.Price / 10, style11);
                var afterTotalPrice = send.AfterTaxPrice * send.SendQuantity;
                sheet1.GenerateCell(7 + index, 9).SetValue<decimal>(afterTotalPrice, style11);
                allTotalPrice += afterTotalPrice;
                sheet1.GenerateCell(7 + index, 10).SetValue(send.ProductBatchNum??"", style11);
                index++;
            }
            
            ICellStyle style2 = SetCellStyle(work, fontHeightInPoints: 11);
            ICellStyle style3 = SetCellStyle(work,fillBackgroundColor: (short)ColorType.Yellow,borderStyle:BorderStyle.None, fontHeightInPoints: 9);
            ICellStyle style4 = SetCellStyle(work,fillBackgroundColor: (short)ColorType.Yellow, boldWeight: (short)FontBoldWeight.Normal, fontHeightInPoints: 9);
            ICellStyle style5 = SetCellStyle(work,fillBackgroundColor: (short)ColorType.Yellow, fontHeightInPoints: 11,horizontalAlignment: HorizontalAlignment.Left);

            sheet1.GenerateCell(7 + index, 1).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 2).SetValue("合计", style2);
            sheet1.GenerateCell(7 + index, 3).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 4).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 5).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 6).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 7).SetValue<decimal>(totalCount, style2);
            sheet1.GenerateCell(7 + index, 8).SetValue("", style2);
            sheet1.GenerateCell(7 + index, 9).SetValue<decimal>(allTotalPrice, style2);
            sheet1.GenerateCell(7 + index, 10).SetValue("", style2);

            for (int i =9; i <= 14; i++)
            {
                sheet1.GenerateCell(i + index, 1).SetValue("", style3);
                sheet1.GenerateCell(i + index, 2).SetValue("", style3);
                sheet1.GenerateCell(i + index, 3).SetValue("", style3);
                sheet1.GenerateCell(i + index, 4).SetValue("", style5);
                sheet1.GenerateCell(i + index, 5).SetValue("", style5);
                sheet1.GenerateCell(i + index, 6).SetValue("", style4);
                sheet1.MergedRegion(i + index, i + index, 7, 8);
                sheet1.GenerateCell(i + index, 7).SetValue("", style4);
                sheet1.GenerateCell(i + index, 8).SetValue("", style4);
                sheet1.GenerateCell(i + index, 9).SetValue("", style3);
            }
           
            sheet1.GenerateCell(9 + index, 4).SetValue("品名：");
            sheet1.GenerateCell(9 + index, 6).SetValue("重量 KG");
            sheet1.GenerateCell(9 + index, 7).SetValue("数量 PCS");

            sheet1.GenerateCell(10 + index, 4).SetValue("垫片类：");
            sheet1.GenerateCell(10 + index, 6).SetValue("垫片类：");

            sheet1.GenerateCell(11 + index, 4).SetValue("总数量");
            sheet1.GenerateCell(11 + index, 6).SetValue("总数量");
            sheet1.GenerateCell(11 + index,7).SetValue(totalCount+"PCS");
            
            sheet1.GenerateCell(12 + index, 4).SetValue("总重量：");
            sheet1.GenerateCell(12 + index, 6).SetValue("总重量公斤：");
        
            sheet1.GenerateCell(13 + index, 4).SetValue("总件数：");
            sheet1.GenerateCell(13 + index, 6).SetValue("总件数：");
           
            sheet1.GenerateCell(14 + index, 4).SetValue("送货司机：");
            sheet1.GenerateCell(14 + index, 6).SetValue("车牌号：");

            var fileName = $"柏中专用-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }

        /// <summary>
        /// 杭州西门子
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderSends"></param>
        /// <param name="customer"></param>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public string ExportTempHzxmz(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateInfo.FilePath;
            var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            sheet1.GenerateCell(5, 3).SetCellValue(bill.SendDate?.ToString("yyyy/MM/dd"));
            int index = 0;
           
            ICellStyle style1 = SetCellStyle(work, fontName: "Arial", horizontalAlignment: HorizontalAlignment.Left, boldWeight: (short)FontBoldWeight.Normal,borderColor:(short)ColorType.Black);
           // ICellStyle style11 = SetCellStyle(work, boldWeight: (short)FontBoldWeight.Normal, fontHeightInPoints: 10);
            foreach (var send in orderSends)
            {
                sheet1.GenerateCell(8 + index, 1).SetValue<int>(index + 1, style1);
                sheet1.GenerateCell(8 + index, 2).SetValue(send?.StockNo??"", style1);
                sheet1.GenerateCell(8 + index, 3).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 4).SetValue(send?.PartNo??"", style1);
                sheet1.GenerateCell(8 + index, 5).SetValue("", style1);
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                sheet1.GenerateCell(8 + index, 6).SetValue<decimal>(sendQuantity * 1000, style1);
                sheet1.GenerateCell(8 + index, 7).SetValue(send?.Model??"", style1);
                sheet1.GenerateCell(8 + index, 8).SetValue((send?.ProductName ?? "") + "," + (send?.SurfaceColor ?? "") + "," + (send?.Rigidity ?? "") + "", style1);
                sheet1.GenerateCell(8 + index, 9).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 10).SetValue(send?.Remark??"", style1);
                index++;
            }

            for (int i = index+1; i <= 13; i++)
            {
                sheet1.GenerateCell(8 + index, 1).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 2).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 3).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 4).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 5).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 6).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 7).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 8).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 9).SetValue("", style1);
                sheet1.GenerateCell(8 + index, 10).SetValue("", style1);
                index++;
            }
            ICellStyle style2 = SetCellStyle(work, fontName: "宋体", fontHeightInPoints: 11, horizontalAlignment: HorizontalAlignment.Center,borderColor:(short)ColorType.Grey25Percent);
            sheet1.GenerateCell(8 + index, 1).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 2).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 3).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 4).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 5).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 6).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 7).SetValue("", style2);
            sheet1.GenerateCell(8 + index, 8).SetValue("", style2);
            sheet1.MergedRegion(8 + index, 8 + index, 9, 10);
            sheet1.GenerateCell(8 + index, 9).SetValue("合计_箱", style2);
            ICellStyle style3 = SetCellStyle(work, fontName: "宋体", horizontalAlignment: HorizontalAlignment.Left, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Grey25Percent);
            sheet1.MergedRegion(9 + index, 9 + index, 1, 2);
            sheet1.GenerateCell(9 + index, 1).SetValue("  送货人:沈", style3);
            sheet1.GenerateCell(9 + index, 3).SetValue("", style3);
            sheet1.GenerateCell(9 + index, 4).SetValue("", style3);
            sheet1.GenerateCell(9 + index, 5).SetValue("签收人：", style3);
            sheet1.GenerateCell(9 + index, 6).SetValue("", style3);
            sheet1.GenerateCell(9 + index, 7).SetValue("", style3);
            sheet1.GenerateCell(9 + index, 8).SetValue("签收日期：", style3);
            sheet1.GenerateCell(9 + index, 9).SetValue("", style3);
            sheet1.GenerateCell(9 + index, 10).SetValue("", style3);
            ICellStyle style4 = SetCellStyle(work, fontName: "Arial",fontHeightInPoints: 10,horizontalAlignment: HorizontalAlignment.Left, borderStyle: BorderStyle.Thin, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Grey25Percent);
            sheet1.GenerateCell(10 + index, 1).SetValue("Comments:备注", style4);
            sheet1.GenerateCell(10 + index, 2).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 3).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 4).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 5).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 6).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 7).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 8).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 9).SetValue("", style4);
            sheet1.GenerateCell(10 + index, 10).SetValue("", style4);

            for (int i = 0; i < 5; i++)
            {
                sheet1.GenerateCell(11 +index+ i, 1).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 2).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 3).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 4).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 5).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 6).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 7).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 8).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 9).SetValue("", style4);
                sheet1.GenerateCell(11 +index+ i, 10).SetValue("", style4);
            }
            ICellStyle style5 = SetCellStyle(work, fontName: "Arial", fontHeightInPoints: 10, horizontalAlignment: HorizontalAlignment.Left, borderStyle: BorderStyle.Thin,borderColor: (short)ColorType.Grey25Percent);

            sheet1.GenerateCell(11 + index, 2).SetValue("1、The same Drawing No. are consider same Lot / Batch不同订单号但是相同零件图号的为一批；相同订单号，相同零件图号必须在同一行！", style4);
            sheet1.GenerateCell(12 + index, 2).SetValue("2、Only 5 Lots / Batch must be per page !. 一页最多只允许有5批或10行，否则拒收！", style5);
            sheet1.GenerateCell(13 + index, 2).SetValue("3、零件图号和版本号务必填写正确、完整；", style4);
            sheet1.GenerateCell(14 + index, 2).SetValue("4、供应商提供送检单须一式两份，一份给SAP操作员用于入录数据，一份给检验员用于检验记录；", style4);
            sheet1.GenerateCell(15 + index, 2).SetValue("5、检验完成后，一份交予仓库，一份质量部留档。", style4);

            var fileName = $"杭州西门子专用-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }

        /// <summary>
        /// 凯乐金霸
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderSends"></param>
        /// <param name="customer"></param>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public string ExportTempKlgb(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateInfo.FilePath;
            //var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            //int sendCount = orderSends.Count;
            //List<IRow> rows = sheet1.GetRows(91, 101);
            //for (int i = 0; i < rows.Count; i++)
            //{
            //    CopyRowEx(work, sheet1.GenerateRow(91+i), sheet1.GenerateRow(7+i+ sendCount), true);
            //}
            sheet1.GenerateCell(1, 10).SetValue(bill.Id);
            int index = 0;
            decimal totalCount = 0;
            ICellStyle style1 = SetCellStyle(work, fontName: "Arial", fontHeightInPoints: 10, horizontalAlignment: HorizontalAlignment.Center, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Black);
            foreach (var send in orderSends)
            {
                sheet1.GenerateCell(7 + index, 1).SetValue<int>(index + 1, style1);
                sheet1.GenerateCell(7 + index, 2).SetValue(send.StockNo, style1);
                sheet1.GenerateCell(7 + index, 3).SetValue(send.PartNo??"", style1);
                sheet1.GenerateCell(7 + index, 4).SetValue(send.Model??"", style1);
                sheet1.GenerateCell(7 + index, 5).SetValue( (send.ProductName??"") + "," + (send.SurfaceColor??"") + "," + (send.Rigidity??"") , style1);
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = send.QuantityPerPack ?? 0;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                sheet1.GenerateCell(7 + index, 6).SetValue<decimal>(sendQuantity * 1000, style1);
                totalCount += sendQuantity * 1000;
                sheet1.GenerateCell(7 + index, 7).SetValue("", style1);
                sheet1.GenerateCell(7 + index, 8).SetValue<decimal>(quantityPerPack*1000, style1);
                sheet1.GenerateCell(7 + index, 9).SetValue(packageCount+"箱"+"*"+ quantityPerPack+ (sysl>0?"+"+sysl:"")+"="+ sendQuantity, style1);
                sheet1.GenerateCell(7 + index, 10).SetValue("", style1);
                sheet1.GenerateCell(7 + index, 11).SetValue(send.Remark ?? "", style1);
                index++;
            }

            for (int i = 1; i <= 11; i++)
            {
                sheet1.GenerateCell(7 + index, i).SetValue("", style1);
            }
            ICellStyle style1_1 = SetCellStyle(work, fontName: "Arial", fontHeightInPoints: 10, horizontalAlignment: HorizontalAlignment.Left, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Black,wrapText:true);
            sheet1.GenerateCell(8 + index, 1).SetValue("收货验收确认Received Confirmation:", style1_1);
            sheet1.GenerateCell(8 + index, 2).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 3).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 4).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 5).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 6).SetValue<int>(totalCount, style1);
            sheet1.GenerateCell(8 + index, 7).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 8).SetValue("", style1);
            sheet1.MergedRegion(8 + index, 8 + index, 9, 10);
            sheet1.GenerateCell(8 + index, 9).SetValue("合计_箱，附检测报告 ", style1);
            sheet1.GenerateCell(8 + index, 10).SetValue("", style1);
            sheet1.GenerateCell(8 + index, 11).SetValue("", style1);
            ICellStyle style1_2 = SetCellStyle(work, fontName: "Arial", fontHeightInPoints: 10, horizontalAlignment: HorizontalAlignment.Center, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Black, wrapText: true,fillBackgroundColor:(short)ColorType.Grey25Percent);
            sheet1.GenerateCell(9 + index, 1).SetValue("日期及签字/Date & Signature:", style1_2);
            sheet1.GenerateCell(9 + index, 2).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 3).SetValue("___日期____沈再红", style1_2);
            sheet1.GenerateCell(9 + index, 4).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 5).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 6).SetValue("送货人：", style1_2);
            sheet1.MergedRegion(9 + index, 9 + index, 7, 8);
            sheet1.GenerateCell(9 + index, 7).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 8).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 9).SetValue("收货人", style1_2);
            sheet1.MergedRegion(9 + index, 9 + index, 10, 11);
            sheet1.GenerateCell(9 + index, 10).SetValue("", style1_2);
            sheet1.GenerateCell(9 + index, 11).SetValue("", style1_2);
            ICellStyle style2= SetCellStyle(work, fontName: "Arial", fontHeightInPoints: 10, horizontalAlignment: HorizontalAlignment.Left, boldWeight: (short)FontBoldWeight.Normal, borderColor: (short)ColorType.Grey25Percent);
           
            for (int i = 0; i < 8; i++)
            {
                sheet1.GenerateCell(10+i + index, 1).SetValue("", style2);
                sheet1.MergedRegion(10 + i + index, 10 + i + index, 2, 7);
                sheet1.GenerateCell(10+i + index, 2).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 3).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 4).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 5).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 6).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 7).SetValue("", style2);
                sheet1.GenerateCell(10+i + index, 8).SetValue("", style2);
                sheet1.GenerateCell(10+i + index, 9).SetValue("", style2);
                sheet1.GenerateCell(10 + i + index, 10).SetValue("", style2);
                sheet1.GenerateCell(10+i + index, 11).SetValue("", style2);
            }
            sheet1.GenerateCell(10 + index, 1).SetValue("送货及物品包装要求");
            sheet1.GenerateCell(10 + index, 2).SetValue("1: 物品包装箱子必须为纸箱，不允许麻布袋形式包装，含油性物料必须在包装箱内增加塑料袋");
            sheet1.GenerateCell(11 + index, 2).SetValue("2: 包装箱尺寸必须一致，推荐尺寸为320*220*120的包装箱");
            sheet1.GenerateCell(12 + index, 2).SetValue("3: 同一款物料每箱数量必须一致，只留一个尾箱，且必须在包装箱上标注尾数箱");
            sheet1.GenerateCell(13 + index, 2).SetValue("4: 每个物品独立包装箱，不可多个物料摆放在一个包装箱，避免造成混料");
            sheet1.GenerateCell(14 + index, 2).SetValue("5: 每箱物料重量必须控制在20KG以下");
            sheet1.GenerateCell(15 + index, 2).SetValue("6：物料箱上必须贴有物料标示，必须具备订单号/KK物料号/尺寸描述/装箱数量/最小包装量");
            sheet1.GenerateCell(16 + index, 2).SetValue("7: 严禁包装箱破损与无包装裸露送货，不满足的包装，仓库一律拒收");
            sheet1.GenerateCell(17 + index, 2).SetValue("8: 供应商送货数量必须与送货单数量一致");

            
            for (int i = 1; i <= 11; i++)
            {
                ICellStyle ss = work.CreateCellStyle();
                ss.BorderTop = BorderStyle.Thin;
                ss.TopBorderColor = (short)ColorType.Black;
                sheet1.GenerateCell(18 + index, i).CellStyle = ss;
                sheet1.GenerateCell(18 + index, i).SetValue("");
            }

          
            for (int i = 0; i < 8; i++)
            {
                ICellStyle ss = work.CreateCellStyle();
                ss.BorderLeft = BorderStyle.Thin;
                ss.LeftBorderColor = (short) ColorType.Black;
                sheet1.GenerateCell(10 + i + index, 12).CellStyle = ss;
                sheet1.GenerateCell(10 + i + index, 12).SetValue("");
            }
            var fileName = $"凯乐金霸专用-{Clock.Now:yyMMddHHmmss}.xlsx";
            //var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            //if (!result.IsNullOrEmpty())
            //{
            //    //CheckErrors(IwbIdentityResult.Failed(result));
            //    return null;
            //}

            //return $"/{savePath}/{fileName}";
            return Save(sheet1, fileName);
        }

        /// <summary>
        /// 杭州德特
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="orderSends"></param>
        /// <param name="customer"></param>
        /// <param name="templateInfo"></param>
        /// <returns></returns>
        public string ExportTempHzdt(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {
          
            var sheet1 = GetSheet1(templateInfo);
            int index = 0;

            sheet1.GenerateCell(3, 4).SetValue(DateTime.Now.ToString("yyyy.M.dd"));
            sheet1.InsertRows(6, orderSends.Count);
            var viewOrderSends = orderSends.OrderBy(i => i.PartNo);
            foreach (var send in viewOrderSends)
            {
                sheet1.GenerateCell(6 + index,1).SetValue<int>(index + 1);
                sheet1.GenerateCell(6 + index,2).SetValue(send.StockNo ?? "");
                sheet1.GenerateCell(6 + index,3).SetValue(send.PartNo ?? "");
                sheet1.GenerateCell(6 + index, 4).SetValue((send.Model ?? "") + "," + (send.ProductName ?? "") + "," +
                                                           (send.SurfaceColor ?? "") + "," + (send.Rigidity ?? ""));
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = Math.Floor((send.QuantityPerPack ?? 0)*1000)/1000;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                sheet1.GenerateCell(6 + index, 5).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(6 + index, 6).SetValue((packageCount  + (sysl > 0 ? 1 : 0)) + "箱" );
                sheet1.GenerateCell(6 + index, 7).SetValue(send.ProductBatchNum);
                index++;
            }
            var fileName = $"杭州德特专用-{Clock.Now:yyMMddHHmmss}.xlsx";
          
            return Save(sheet1, fileName);
        }

        //上海泛得五金配件厂

        public string ExportTempShFd(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {

            var sheet1 = GetSheet1(templateInfo);
            int index = 0;

            //sheet1.GenerateCell(3, 4).SetValue(DateTime.Now.ToString("yyyy.MM.dd"));
            var orderSendCount = orderSends.Count;
            if (orderSendCount < 20)
            {
                orderSendCount = 20;
            }
            sheet1.InsertRows(7, orderSendCount);
            var viewOrderSends = orderSends.OrderBy(i => i.PartNo);
            decimal allSendCount = 0;
            foreach (var send in viewOrderSends)
            {
                sheet1.GenerateCell(7 + index, 1).SetValue<int>(index + 1);
                sheet1.GenerateCell(7 + index, 2).SetValue(DateTime.Now.ToString("yyyy.MM.dd"));
                sheet1.GenerateCell(7 + index, 3).SetValue(send.StockNo ?? "");
                sheet1.GenerateCell(7 + index, 4).SetValue(send.PartNo ?? "");
                sheet1.GenerateCell(7 + index, 5).SetValue((send.Model ?? "") + "," + (send.ProductName ?? "") + "," +
                                                           (send.SurfaceColor ?? "") + "," + (send.Rigidity ?? ""));
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = Math.Floor((send.QuantityPerPack ?? 0) * 1000) / 1000;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                sheet1.GenerateCell(7 + index, 6).SetValue<decimal>(sendQuantity*1000);
                sheet1.GenerateCell(7 + index, 7).SetValue((packageCount + (sysl > 0 ? 1 : 0))+"");
                sheet1.GenerateCell(7 + index, 8).SetValue(send.ProductBatchNum);
                allSendCount += sendQuantity * 1000;
                index++;
            }
            sheet1.MergedRegion(7 + orderSendCount+2, 7 + orderSendCount+2, 7, 9);
            sheet1.GenerateCell(7 + orderSendCount+2, 7).SetValue("合计:"+ allSendCount);
            //sheet1.GenerateCell(7 + orderSendCount, 8).SetValue(send.ProductBatchNum);
            var fileName = $"上海泛得五金配件厂-{Clock.Now:yyMMddHHmmss}.xlsx";

            return Save(sheet1, fileName);
        }

        //太仓慧鱼

        public string ExportTempTCHY(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {
            var sheet1 = GetSheet1(templateInfo);
            int index = 0;
            //sheet1.GenerateCell(3, 4).SetValue(DateTime.Now.ToString("yyyy.MM.dd"));
            var orderSendCount = orderSends.Count;
            if (orderSendCount < 7)
            {
                orderSendCount = 7;
            }
            sheet1.InsertRows(14, orderSendCount);
            var viewOrderSends = orderSends.OrderBy(i => i.PartNo);
            //decimal allSendCount = 0;
            sheet1.GenerateCell(7, 3).SetValue(DateTime.Now.ToString("yyyy/MM/dd"));
            foreach (var send in viewOrderSends)
            {
                sheet1.GenerateCell(14 + index, 1).SetValue(send?.PartNo);

                sheet1.GenerateCell(14 + index, 2).SetValue(send.StockNo ?? "");
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = Math.Floor((send.QuantityPerPack ?? 0) * 1000) / 1000;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                sheet1.GenerateCell(14 + index, 3).SetValue<decimal>(sendQuantity * 1000);
                sheet1.GenerateCell(14 + index, 5).SetValue(send.ProductBatchNum);
                sheet1.GenerateCell(14 + index, 6).SetValue((send.ProductName ?? "") + " " + (send.Model ?? "") + " " +
                                                           (send.SurfaceColor ?? "") + " " + (send.Rigidity ?? ""));
                sheet1.GenerateCell(14 + index, 7).SetValue("太仓");
                sheet1.GenerateCell(14 + index, 8).SetValue(send.OrderDate.ToString("yyyy/MM/dd"));
                sheet1.GenerateCell(14 + index, 9).SetValue(packageCount + "箱*" + quantityPerPack + (sysl > 0 ? " + " + (sysl * 1000) : "") + "PCS");

                //allSendCount += sendQuantity * 1000;
                index++;
            }
            //sheet1.MergedRegion(7 + orderSendCount + 2, 7 + orderSendCount + 2, 7, 9);
            // sheet1.GenerateCell(7 + orderSendCount + 2, 7).SetValue("合计:" + allSendCount);
            //sheet1.GenerateCell(7 + orderSendCount, 8).SetValue(send.ProductBatchNum);
            var fileName = $"太仓慧鱼送货单-{Clock.Now:yyMMddHHmmss}.xlsx";

            return Save(sheet1, fileName);
        }

        //上海特强汽车紧固件有限公司

        public string ExportTempTq(OrderSendBill bill, List<ViewOrderSend> orderSends, Customer customer,
            TemplateInfo templateInfo)
        {
            var sheet1 = GetSheet1(templateInfo);
            int index = 0;
            //sheet1.GenerateCell(3, 4).SetValue(DateTime.Now.ToString("yyyy.MM.dd"));
            var orderSendCount = orderSends.Count;
            //if (orderSendCount < 7)
            //{
            //    orderSendCount = 7;
            //}
            sheet1.InsertRows(10, orderSendCount);
            var viewOrderSends = orderSends.OrderBy(i => i.PartNo);
            //decimal allSendCount = 0;
            sheet1.GenerateCell(2, 1).SetValue("单号：" + bill.Id);
            sheet1.GenerateCell(7, 1).SetValue("日期:"+DateTime.Now.ToString("yyyy年MM月dd日"));
            sheet1.GenerateCell(5, 2).SetValue("客户："+ customer.CustomerName);
            sheet1.GenerateCell(5, 9).SetValue("地址：" + customer.Address);
            sheet1.GenerateCell(6, 2).SetValue("联系电话：" + customer.Telephone);
            sheet1.GenerateCell(6, 9).SetValue("联系人：" + customer.LinkMan);
           
            foreach (var send in viewOrderSends)
            {
                sheet1.GenerateCell(10 + index, 1).SetValue($"{index+1}");

                sheet1.GenerateCell(10 + index, 2).SetValue(send.StockNo ?? "");
                var sendQuantity = Math.Round(Math.Floor(send.SendQuantity * 1000) / 1000, 3);//发货数量
                var quantityPerPack = Math.Floor((send.QuantityPerPack ?? 0) * 1000) / 1000;
                quantityPerPack = quantityPerPack == 0 ? sendQuantity : quantityPerPack;//每包数量，如果为0则等于发货数量
                decimal packageCount = sendQuantity == 0 ? 0 : (sendQuantity <= quantityPerPack ? 1 : Math.Floor(send.SendQuantity / quantityPerPack));
                var sysl = sendQuantity - quantityPerPack * packageCount;
                //sheet1.GenerateCell(10 + index, 3).SetValue<decimal>(sendQuantity * 1000);
                sheet1.GenerateCell(10 + index, 3).SetValue(send.ProductName??"");
                sheet1.GenerateCell(10 + index, 4).SetValue(send.Model ?? "");
                sheet1.GenerateCell(10 + index, 5).SetValue(send.SurfaceColor??"");
                sheet1.GenerateCell(10 + index, 6).SetValue(send.Material??"");
              
                sheet1.GenerateCell(10 + index, 7).SetValue(send.Rigidity??"");
                sheet1.GenerateCell(10 + index, 8).SetValue("千件"); 
                sheet1.GenerateCell(10 + index, 9).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(10 + index, 10).SetValue<decimal>(packageCount);
                sheet1.GenerateCell(10 + index, 11).SetValue<decimal>(quantityPerPack);
                sheet1.GenerateCell(10 + index, 12).SetValue<decimal>(sysl);
                sheet1.GenerateCell(10 + index, 13).SetValue<decimal>(sendQuantity);
                sheet1.GenerateCell(10 + index, 14).SetValue(send.ProductBatchNum??"");
               
                index++;
            }
            sheet1.GenerateCell(13 + index, 11).SetValue("送货日期：" + Clock.Now.ToString("yyyy-MM-dd"));
            //sheet1.MergedRegion(7 + orderSendCount + 2, 7 + orderSendCount + 2, 7, 9);
            // sheet1.GenerateCell(7 + orderSendCount + 2, 7).SetValue("合计:" + allSendCount);
            //sheet1.GenerateCell(7 + orderSendCount, 8).SetValue(send.ProductBatchNum);
            var fileName = $"上海特强送货单-{Clock.Now:yyMMddHHmmss}.xlsx";

            return Save(sheet1, fileName);
        }
        private ISheet GetSheet1(TemplateInfo templateInfo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateInfo.FilePath;
            //var savePath = "Download/Excel/OrderSendBill";
            var work = ExcelHelper.CreateWorkBook07(path);
            var sheet1 = work.GetSheet("Sheet1");
            return sheet1;
        }

        private string Save(ISheet sheet,string fileName)
        {
            IWorkbook work = sheet.Workbook;
            var savePath = "Download/Excel/OrderSendBill";
            //var fileName = $"杭州德特专用-{Clock.Now:yyMMddHHmmss}.xlsx";
            var result = work?.SaveWorkBook($"{AppDomain.CurrentDomain.BaseDirectory}{savePath}", fileName);
            if (!result.IsNullOrEmpty())
            {
                //CheckErrors(IwbIdentityResult.Failed(result));
                return null;
            }
            return $"/{savePath}/{fileName}";
        }

        #region ex
        private void RemoveMergedRegion(ISheet sheet, int rowIndex)
        {
            int mergedCount = sheet.NumMergedRegions;
            for (int i = mergedCount - 1; i >= 0; i--)
            {
                /**
CellRangeAddress对象属性有：FirstColumn，FirstRow，LastColumn，LastRow 进行操作 取消合并单元格
                **/
                var temp = sheet.GetMergedRegion(i);
                if (temp.FirstRow == rowIndex)
                {
                    sheet.RemoveMergedRegion(i);
                }
            }
        }

        public static ICellStyle SetCellStyle(IWorkbook work, BorderStyle borderStyle= BorderStyle.Thin, short borderColor = 0, short topBorderColor = 0, short rightBorderColor = 0, short leftBorderColor = 0, short bottomBorderColor = 0, short fillBackgroundColor = (short)ColorType.White,bool wrapText=false, VerticalAlignment verticalAlignment= VerticalAlignment.Center, HorizontalAlignment horizontalAlignment= HorizontalAlignment.Center,string fontName= "微软雅黑", short fontColor = (short)ColorType.Black, short boldWeight= (short)FontBoldWeight.Bold,int fontHeightInPoints=12,bool isItalic=false)
        {
            ExportTemplateFont fontStyle = new ExportTemplateFont()
            {
                FontName = fontName,
                BoldWeight = boldWeight,
                Color = fontColor,
                FontHeightInPoints = fontHeightInPoints,
                IsItalic = isItalic
            };
            ExportTemplateCss cssStyle = new ExportTemplateCss()
            {
                BorderStyle = borderStyle,
                FillBackgroundColor = fillBackgroundColor,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAlignment,
                WrapText = wrapText,
                FontStyle = fontStyle,
                BorderColor = borderColor,
                TopBorderColor = topBorderColor,
                BottomBorderColor = bottomBorderColor,
                LeftBorderColor = leftBorderColor,
                RightBorderColor = rightBorderColor
            };
            return SetCellStyle(work, cssStyle);
        }
        public static ICellStyle SetCellStyle(IWorkbook work, ExportTemplateCss cssStyle)
        {
            ICellStyle style = work.CreateCellStyle();
            style.BorderBottom = cssStyle.BorderStyle;
            style.BorderTop = cssStyle.BorderStyle;
            style.BorderLeft = cssStyle.BorderStyle;
            style.BorderRight = cssStyle.BorderStyle;
            if (cssStyle.BorderColor > 0)
            {
                style.BottomBorderColor = cssStyle.BorderColor;
                style.TopBorderColor = cssStyle.BorderColor;
                style.LeftBorderColor = cssStyle.BorderColor;
                style.RightBorderColor = cssStyle.BorderColor;
            }
            if (cssStyle.BottomBorderColor > 0)
            {
                style.BottomBorderColor = cssStyle.BottomBorderColor;
            }
            if (cssStyle.TopBorderColor > 0)
            {
                style.TopBorderColor = cssStyle.TopBorderColor;
            }
            if (cssStyle.RightBorderColor > 0)
            {
                style.RightBorderColor = cssStyle.RightBorderColor;
            }
            if (cssStyle.LeftBorderColor > 0)
            {
                style.LeftBorderColor = cssStyle.LeftBorderColor;
            }
            style.FillForegroundColor = cssStyle.FillBackgroundColor;
            style.FillPattern = FillPattern.Squares;
            style.FillBackgroundColor = cssStyle.FillBackgroundColor;
            style.WrapText = cssStyle.WrapText;
            style.VerticalAlignment = cssStyle.VerticalAlignment;
            style.Alignment = cssStyle.HorizontalAlignment;
            ExportTemplateFont fontStyle = cssStyle.FontStyle;
            IFont font = work.CreateFont();
            font.FontName = fontStyle.FontName;
            font.Color = fontStyle.Color;
            font.IsItalic = fontStyle.IsItalic;
            font.Boldweight = fontStyle.BoldWeight;
            font.FontHeightInPoints = fontStyle.FontHeightInPoints;
            style.SetFont(font);
            return style;
        }

        #region copy
        public static void CopyCellStyle(IWorkbook wb, ICellStyle fromStyle, ICellStyle toStyle)
        {
            toStyle.Alignment = fromStyle.Alignment;
            //边框和边框颜色
            toStyle.BorderBottom = fromStyle.BorderBottom;
            toStyle.BorderLeft = fromStyle.BorderLeft;
            toStyle.BorderRight = fromStyle.BorderRight;
            toStyle.BorderTop = fromStyle.BorderTop;
            toStyle.TopBorderColor = fromStyle.TopBorderColor;
            toStyle.BottomBorderColor = fromStyle.BottomBorderColor;
            toStyle.RightBorderColor = fromStyle.RightBorderColor;
            toStyle.LeftBorderColor = fromStyle.LeftBorderColor;
            //背景和前景
            toStyle.FillBackgroundColor = fromStyle.FillBackgroundColor;
            toStyle.FillForegroundColor = fromStyle.FillForegroundColor;
            toStyle.DataFormat = fromStyle.DataFormat;
            toStyle.FillPattern = fromStyle.FillPattern;
            //toStyle.Hidden=fromStyle.Hidden;
            toStyle.IsHidden = fromStyle.IsHidden;
            toStyle.Indention = fromStyle.Indention;//首行缩进
            toStyle.IsLocked = fromStyle.IsLocked;
            toStyle.Rotation = fromStyle.Rotation;//旋转
            toStyle.VerticalAlignment = fromStyle.VerticalAlignment;
            toStyle.WrapText = fromStyle.WrapText;
            toStyle.SetFont(fromStyle.GetFont(wb));
        }
        /// <summary>
        /// 复制行
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="fromRow"></param>
        /// <param name="toRow"></param>
        /// <param name="copyValueFlag"></param>
        public static void CopyRowEx(IWorkbook wb, IRow fromRow, IRow toRow, bool copyValueFlag)
        {
            System.Collections.IEnumerator cells = fromRow.GetEnumerator();//.GetRowEnumerator();
            toRow.Height = fromRow.Height;
            while (cells.MoveNext())
            {
                ICell cell = null;
                //ICell cell = (wb is HSSFWorkbook) ? cells.Current as HSSFCell : cells.Current as NPOI.XSSF.UserModel.XSSFCell;
                if (wb is HSSFWorkbook)
                    cell = cells.Current as HSSFCell;
                else
                    cell = cells.Current as NPOI.XSSF.UserModel.XSSFCell;
                ICell newCell = toRow.CreateCell(cell.ColumnIndex);
                CopyCell(wb, cell, newCell, copyValueFlag);
            }
        }
        /// <summary>
        /// 复制原有sheet的合并单元格到新创建的sheet
        /// </summary>
        /// <param name="fromSheet"></param>
        /// <param name="toSheet"></param>
        public static void MergerRegion(ISheet fromSheet, ISheet toSheet)
        {
            int sheetMergerCount = fromSheet.NumMergedRegions;
            for (int i = 0; i < sheetMergerCount; i++)
            {
                //Region mergedRegionAt = fromSheet.GetMergedRegion(i); //.MergedRegionAt(i);
                //CellRangeAddress[] cra = new CellRangeAddress[1];
                //cra[0] = fromSheet.GetMergedRegion(i);
                //Region[] rg = Region.ConvertCellRangesToRegions(cra);
                toSheet.AddMergedRegion(fromSheet.GetMergedRegion(i));
            }
        }

        /// <summary>
        /// 复制单元格
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="srcCell"></param>
        /// <param name="distCell"></param>
        /// <param name="copyValueFlag"></param>
        public static void CopyCell(IWorkbook wb, ICell srcCell, ICell distCell, bool copyValueFlag)
        {
            ICellStyle newstyle = wb.CreateCellStyle();
            CopyCellStyle(wb, srcCell.CellStyle, newstyle);
            //样式
            distCell.CellStyle = newstyle;
            //评论
            if (srcCell.CellComment != null)
            {
                distCell.CellComment = srcCell.CellComment;
            }

            // 不同数据类型处理
            CellType srcCellType = srcCell.CellType;
            distCell.SetCellType(srcCellType);
            if (copyValueFlag)
            {
                if (srcCellType == CellType.Numeric)
                {
                    if (DateUtil.IsCellDateFormatted(srcCell))
                    {
                        distCell.SetCellValue(srcCell.DateCellValue);
                    }
                    else
                    {
                        distCell.SetCellValue(srcCell.NumericCellValue);
                    }
                }
                else if (srcCellType == CellType.String)
                {
                    distCell.SetCellValue(srcCell.RichStringCellValue);
                }
                else if (srcCellType == CellType.Blank)
                {
                    // nothing21
                }
                else if (srcCellType == CellType.Boolean)
                {
                    distCell.SetCellValue(srcCell.BooleanCellValue);
                }
                else if (srcCellType == CellType.Error)
                {
                    distCell.SetCellErrorValue(srcCell.ErrorCellValue);
                }
                else if (srcCellType == CellType.Formula)
                {
                    distCell.SetCellFormula(srcCell.CellFormula);
                }
                else
                {
                    // nothing29
                }
            }
        }
        #endregion



        #endregion


    }
        public class ExportTemplateCss
    {
        /// <summary>
        /// 上下左右边框
        /// </summary>
        public BorderStyle BorderStyle { get; set; } = BorderStyle.Thin;
        public short TopBorderColor { get; set; } 
        public short RightBorderColor { get; set; }
        public short LeftBorderColor { get; set; }
        public short BottomBorderColor { get; set; }

        public short BorderColor { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public short FillBackgroundColor { get; set; } = (short) ColorType.White;
        /// <summary>
        /// 文字换行
        /// </summary>
        public bool WrapText { get; set; } = false;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        public ExportTemplateFont FontStyle { get; set; }

       

    }

    public class ExportTemplateFont
    {
        public string FontName { get; set; } = "微软雅黑";

        public bool IsItalic { get; set; } = false;

        public short Color { get; set; } = (short) ColorType.Black;

        public short BoldWeight { get; set; } = (short) FontBoldWeight.Bold;
        public int FontHeightInPoints { get; set; } = 12;

        //public bool IsBold { get; set; } = false;
    }
}

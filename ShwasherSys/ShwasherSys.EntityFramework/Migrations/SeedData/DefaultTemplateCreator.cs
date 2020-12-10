using System;
using System.Linq;
using Abp.Localization;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;
using IwbZero.Setting;
using ShwasherSys.Inspection;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultTemplateCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultTemplateCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            #region 检验报告模板
            string temp = @"<table style='width: 950px;border-spacing: 0;border-collapse: collapse;'><thead><tr><td colspan='11'style='text-align: center; padding: 15px'><img src='../../Content/Images/excle/report.png'style='width: 600px'></td></tr><tr><td colspan='11'style='font-size: 24px; font-weight: 600'><span>产品检验报告</span><br><span>Inspection Report</span></td></tr></thead><tbody><tr><td><span>客户名称</span><br><span>Customer Name</span></td><td colspan='2'class='td-input'id='tdCustomerName'></td><td><span>品名</span><br><span>Product Name</span></td><td colspan='2'class='td-input'id='tdProductName'></td><td><span>订单号</span><br><span>Order Number</span></td><td colspan='4'class='td-input'id='tdOrderNumber'></td></tr><tr><td><span>规格</span><br><span>Part Name</span></td><td colspan='2'class='td-input'id='tdPartName'></td><td><span>表面处理</span><br><span>Surface Treatment</span></td><td colspan='2'class='td-input'id='tdSurfaceTreatment'></td><td><span>批次号</span><br><span>Product Lot</span></td><td colspan='4'class='td-input'id='tdProductionLot'></td></tr><tr><td><span>材料牌号</span><br><span>Material grade</span></td><td colspan='2'class='td-input'id='tdMaterialGrade'></td><td><span>材料规格</span><br><span>Material size</span></td><td colspan='2'class='td-input'id='tdMaterialSize'></td><td><span>材料炉号</span><br><span>Material Lot No</span></td><td colspan='4'class='td-input'id='tdMaterialLotNo'></td></tr><tr><td><span>零件号</span><br><span>Part Number</span></td><td colspan='2'class='td-input'id='tdPartNumber'></td><td><span>检测件数</span><br><span>Test Lot</span></td><td colspan='2'><span class='td-count'style='display: inline;'>10</span><span>件/pcs</span></td><td><span>检测日期</span><br><span>Test Date</span></td><td colspan='4'class='td-input'id='tdCheckDate'></td></tr><tr><td style='width: 11%;'></td><td style='width: 6%;'><span>内径</span><br><span>Id</span></td><td style='width: 6%;'><span>外径</span><br><span>Od</span></td><td style='width: 11%;'><span>厚度</span><br><span>Th</span></td><td style='width: 6%;background: #f5f5f5;'class='td-input'></td><td style='width: 6%;'><span>硬度</span><br><span>Hardness</span></td><td style='width: 11%;'><span>镀层</span><br><span>Um</span></td><td style='width: 10%;'><span>盐雾试验</span><br><span>Salt Spray Test</span></td><td colspan='3'><span>氢脆试验</span><br><span>Hydrogen Embrittlement Tests</span></td></tr><tr class='td-data-head'><td><span>尺寸范围</span><br><span>Range</span></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td style='background: #f5f5f5;'rowspan='11'class='td-input'></td><td style='width: 10%;'><span>标准要求</span><br><span>Standards</span></td><td style='width: 10%;background: #f5f5f5;'class='td-input'></td><td style='width: 10%;background: #f5f5f5;'class='td-input'></td></tr><tr class='td-data'><td class='td-input'>1</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>2</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>3</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>4</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>5</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>6</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>7</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>8</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>9</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr class='td-data'><td class='td-input'>10</td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr><tr><td><span>检测结果</span><br><span>Inspect Result</span></td><td colspan='2'class='td-input'></td><td colspan='3'><span>检测人</span><br><span>Inspector</span></td><td colspan='5'class='td-input'></td></tr><tr><td colspan='11'style='font-size: 24px; font-weight: 600'><span>材料化学成分(Material chemical composition)%</span></td></tr><tr><td><span>C</span></td><td><span>Si</span></td><td><span>Mn</span></td><td><span>P</span></td><td><span>S</span></td><td><span>Cr</span></td><td><span>Ni</span></td><td><span>Cu</span></td><td><span></span></td><td><span></span></td><td><span></span></td></tr><tr><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td><td class='td-input'></td></tr></tbody>
</table>";

            #endregion
            //Languages
            AddTemplateIfNotExists( ShwasherConsts.InspectReportTemplateName, "检验报告模板",temp);

    }

        private void AddTemplateIfNotExists(string no, string name, string value, string desc = "",int type=1)
        {
            if (_context.TemplateInfos.Any(s => s.TemplateNo == no))
                return;
            _context.TemplateInfos.Add(new TemplateInfo()
            {
                TemplateNo = no,
                Name = name,
                Content = value,
                Description = desc,
                Type = type
            });
            _context.SaveChanges();
        }
    }
}
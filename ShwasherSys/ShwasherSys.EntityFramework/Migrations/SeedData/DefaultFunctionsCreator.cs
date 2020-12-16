using System;
using System.IO;
using System.Linq;
using System.Text;
using Abp.Extensions;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultFunctionsCreator
    {
        private readonly ShwasherDbContext _context;
        private string PermissionStr { get; set; }
        public DefaultFunctionsCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateFunctions("HTSystem", "后台管理系统", "0", 0, "Index", "Home", "icon-home", 0, 0, null, "", "", "Pages");

            OrderMenu1();
            FaHuo2();
            FaPiao3();

            ProductionOrder10();
            SemiProductStoreMenu11();
            JianYan12();
            Baozhuang13();
            Store14();
            InventoryCheck15();
            RawMaterialStore16();
            ScrapStore17();

            Company30();
            CustomerInfoMenu31();
            NotificationInfo32();
            ProductMenu33();

            Statistic40();

            B100();
            Sys101();
            Help102();
            WritePermission();
        }

        #region 订单，发货，发票
        public void OrderMenu1()
        {
            CreateFunctions("OrderInfo", "订单管理", "HTSystem", 0, "", "", "icon-system", 1, 1, "");
            CreateFunctions("OrderStatusMg", "订单状态", "OrderInfo", 1, "OrderStatusMg", "OrderInfo", "icon-menu", 1, 2);
            CreateFunctions("QueryPage", "查看页面", "OrderStatusMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Print", "打印订单", "OrderStatusMg", 2, "", "", "icon-folder", 1, 3, "", "btn btn-default", "btnPrint");
            CreateFunctions("Query", "查看订单", "OrderStatusMg", 2, "", "", "icon-search", 2, 3, "", "btn btn-default", "btnQuery");
            CreateFunctions("Audit", "审核", "OrderStatusMg", 2, "", "", "icon-edit", 3, 3, "/api/services/app/OrderHeaders/Audit", "btn btn-default", "btnAudit");
            CreateFunctions("AuditItem", "审核订单明细", "OrderStatusMg", 2, "", "", "icon-edit", 4, 3, "/api/services/app/OrderItems/Audit", "btn btn-default", "N_btnAudit");
            CreateFunctions("EndItem", "结束订单明细", "OrderStatusMg", 2, "", "", "icon-edit", 5, 3, "/api/services/app/OrderItems/End", "btn btn-default", "N_btnEnd");
            CreateFunctions("SendItem", "发货", "OrderStatusMg", 2, "", "", "icon-edit",6, 3, "/api/services/app/OrderItems/SendOrderAction", "btn btn-default", "N_btnSend");
            CreateFunctions("ChangeStatus", "订单明细状态变更", "OrderStatusMg", 2, "", "", "icon-edit", 7, 3, "/api/services/app/OrderItems/ChangeOrderItemStatus", "btn btn-default", "N_btnChangeStatus");
            CreateFunctions("QueryOrderPrice", "查看订单价格", "OrderStatusMg", 2, "", "", "", 8, 3, "", "btn btn-default", "None");
            CreateFunctions("ReplaceSendItem", "替代发货", "OrderStatusMg", 2, "", "", "icon-edit", 9, 3, "/api/services/app/OrderItems/SendOrderAction", "btn btn-default", "N_btnSend");

            CreateFunctions("OrderMg", "订单维护", "OrderInfo", 1, "OrderMg", "OrderInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "OrderMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建订单", "OrderMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/OrderHeaders/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改订单", "OrderMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/OrderHeaders/Update", "btn btn-default", "N_btnUpdate");
            CreateFunctions("Delete", "删除订单", "OrderMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/OrderHeaders/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ShowDetail", "查看订单", "OrderMg", 2, "", "", "icon-search", 4, 3, "", "btn btn-default", "btnShow");
            CreateFunctions("CreateOrderItem", "创建订单明细", "OrderMg", 2, "", "", "icon-add", 4, 3, "/api/services/app/OrderItems/Create", "btn btn-default", "N_btnCreate");
            CreateFunctions("UpdateOrderItem", "修改订单明细", "OrderMg", 2, "", "", "icon-edit", 5, 3, "/api/services/app/OrderItems/Update", "btn btn-default", "N_btnUpdate");
            CreateFunctions("DeleteOrderItem", "删除订单明细", "OrderMg", 2, "", "", "icon-delete", 6, 3, "/api/services/app/OrderItems/Delete", "btn btn-default", "N_btnDelete");
            CreateFunctions("QueryOrderPrice", "查看订单价格", "OrderMg", 2, "", "", "", 7, 3, "", "btn btn-default", "None");

            CreateFunctions("OrderQueryMg", "订单查询", "OrderInfo", 1, "OrderQueryMg", "OrderInfo", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "OrderQueryMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ExportExcel", "导出Excel", "OrderQueryMg", 2, "", "", "icon-download", 1, 3, "/api/services/app/OrderItems/ExportExcel", "btn btn-default", "_btnExportExcel");
            CreateFunctions("QueryOrderPrice", "查看订单价格", "OrderQueryMg", 2, "", "", "", 2, 3, "", "btn btn-default", "None");

            CreateFunctions("NotCompleteOrderItem", "未完成订单明细", "OrderInfo", 1, "NotCompleteOrderItem", "OrderInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "NotCompleteOrderItem", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ExportExcel", "导出Excel", "NotCompleteOrderItem", 2, "", "", "icon-download", 1, 3, "/api/services/app/OrderItems/ExportExcel", "btn btn-default", "_btnExportExcel");
            CreateFunctions("QueryOrderPrice", "查看订单价格", "NotCompleteOrderItem", 2, "", "", "", 2, 3, "", "btn btn-default", "None");

            CreateFunctions("OrderItemStatistics", "订单明细统计", "OrderInfo", 1, "OrderItemStatistics", "OrderInfo", "icon-menu", 5, 2);
            CreateFunctions("Query", "查看页面", "OrderItemStatistics", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
        }

        public void FaHuo2()
        {
            CreateFunctions("SendGoods", "发货管理", "HTSystem", 0, "", "", "icon-system", 2, 1, "");

            CreateFunctions("OrderSendBillCreate", "发货单生成", "SendGoods", 1, "OrderSendBillCreate", "SendGoods", "icon-menu", 1, 2);
            CreateFunctions("QueryPage", "查看页面", "OrderSendBillCreate", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "生成发货单", "OrderSendBillCreate", 2, "", "", "icon-add", 1, 3, "", "btn btn-default", "btnCreate");
            CreateFunctions("Delete", "撤销发货", "OrderSendBillCreate", 2, "", "", "icon-delete", 2, 3, "", "btn btn-default", "btnDelete");
            CreateFunctions("OrderSendBillMg", "发货单维护", "SendGoods", 1, "OrderSendBillMg", "SendGoods", "icon-menu", 2, 2);
            CreateFunctions("QueryPage", "查看页面", "OrderSendBillMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ShowSendBill", "查看发货单", "OrderSendBillMg", 2, "", "", "icon-search", 1, 3, "", "btn btn-default", "btnShow");
            CreateFunctions("Delete", "撤销发货单", "OrderSendBillMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/OrderSendBill/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("OrderSendQueryMg", "客户发货统计", "SendGoods", 1, "OrderSendQueryMg", "SendGoods", "icon-menu", 3, 2);
            CreateFunctions("QueryPage", "查看页面", "OrderSendQueryMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ExportExcel", "导出Excel", "OrderSendQueryMg", 2, "", "", "icon-download", 1, 3, "/api/services/app/OrderSend/ExportExcel", "btn btn-default", "_btnExportExcel");
            CreateFunctions("QueryPrice", "查看金额", "OrderSendQueryMg", 2, "", "", "icon-search", 1, 4, "", "btn btn-default", "N_btnQueryPrice");

            
            CreateFunctions("ReturnGoodMg", "退货退款信息", "SendGoods", 1, "ReturnGood", "SendGoods", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "ReturnGoodMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建退货记录", "ReturnGoodMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/ReturnGoodOrder/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改记录", "ReturnGoodMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ReturnGoodOrder/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除记录", "ReturnGoodMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ReturnGoodOrder/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ChangeState", "申请检验", "ReturnGoodMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/ReturnGoodOrder/ChangeState", "btn btn-default", "btnChangeState");
            CreateFunctions("ExportReturn", "导出退货单", "ReturnGoodMg", 2, "", "", "icon-delete", 5, 3, "/api/services/app/ReturnGoodOrder/ExportReturn", "btn btn-default", "btnExportReturn");
            CreateFunctions("RefundApply", "申请退款", "ReturnGoodMg", 2, "", "", "icon-edit", 6, 3, "/api/services/app/ReturnGoodOrder/RefundApply", "btn btn-default", "btnRefundApply");
            CreateFunctions("RefundConfirm", "确认退款", "ReturnGoodMg", 2, "", "", "icon-add", 7, 3, "/api/services/app/ReturnGoodOrder/RefundConfirm", "btn btn-default", "btnRefundConfirm");
        }
        public void FaPiao3()
        {
            CreateFunctions("InvoiceInfo", "发票管理", "HTSystem", 0, "", "", "icon-system", 3, 1, "");
            CreateFunctions("InvoiceCreate", "客户发票生成", "InvoiceInfo", 1, "InvoiceCreate", "InvoiceInfo", "icon-menu", 1, 2);
            CreateFunctions("QueryPage", "查看页面", "InvoiceCreate", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "生成发票", "InvoiceCreate", 2, "", "", "icon-add", 1, 3, "/api/services/app/OrderStickBill/Create", "btn btn-default", "btnCreate");

            CreateFunctions("InvoiceMg", "客户发票维护", "InvoiceInfo", 1, "InvoiceMg", "InvoiceInfo", "icon-menu", 2, 2);
            CreateFunctions("QueryPage", "查看页面", "InvoiceMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ShowStickBill", "查看发票", "InvoiceMg", 2, "", "", "icon-search", 1, 3, "", "btn btn-default", "btnShow");
            CreateFunctions("Delete", "作废发票", "InvoiceMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/OrderStickBill/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("CreateRed", "添加红冲发票", "InvoiceMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/OrderStickBill/CreateRed", "btn btn-default", "_btnCreateRed");
            CreateFunctions("Update", "发票号修改", "InvoiceMg", 2, "", "", "icon-edit", 5, 3, "/api/services/app/OrderStickBill/UpdateStickNum", "btn btn-default", "btnUpdate");
            CreateFunctions("UpdateState", "确认收(退)款", "InvoiceMg", 2, "", "", "icon-edit", 6, 3, "/api/services/app/OrderStickBill/UpdateState", "btn btn-default", "btnUpdateState");
            CreateFunctions("ExportInvoices", "导出发票", "InvoiceMg", 2, "", "", "icon-edit", 7, 3, "/api/services/app/OrderStickBill/ExportInvoices", "btn btn-default", "_btnExportInvoices");

            CreateFunctions("StatementBill", "客户对账单维护", "InvoiceInfo", 1, "StatementBill", "InvoiceInfo", "icon-menu", 1, 2);
            CreateFunctions("QueryPage", "查看页面", "StatementBill", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "生成对账单", "StatementBill", 2, "", "", "icon-add", 1, 3, "/api/services/app/StatementBill/Create", "btn btn-default", "N_btnCreate");
            CreateFunctions("ShowStickBill", "查看对账单", "StatementBill", 2, "", "", "icon-search", 1, 3, "", "btn btn-default", "btnShow");
            CreateFunctions("Delete", "撤销对账单", "StatementBill", 2, "", "", "icon-delete", 2, 3, "/api/services/app/StatementBill/Delete", "btn btn-default", "btnDelete");
            
        }

        #endregion

        #region 生产,仓库,检验

        public void ProductionOrder10()
        {
            CreateFunctions("ProductionInfo", "生产管理", "HTSystem", 0, "", "", "icon-system", 10, 1, "");
            CreateFunctions("ProductionOrderMg", "排产信息维护", "ProductionInfo", 1, "ProductionOrderMg", "ProductionInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "ProductionOrderMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建排产单", "ProductionOrderMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/ProductionOrders/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改排产单", "ProductionOrderMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductionOrders/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除排产单", "ProductionOrderMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/Delete", "btn btn-default", "btnDelete");
             CreateFunctions("Export", "导出流转单", "ProductionOrderMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/ProductionOrders/ExcelExport", "btn btn-default", "btnExport");

            CreateFunctions("OutProductionOrderMg", "外协信息维护", "ProductionInfo", 1, "OutProductionOrderMg", "ProductionInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "OutProductionOrderMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建外协单", "OutProductionOrderMg", 2, "", "", "icon-add", 2, 3, "/api/services/app/ProductionOrders/CreateOutProductionOrder", "btn btn-default", "N_btnCreate");
            CreateFunctions("Update", "修改外协单", "OutProductionOrderMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductionOrders/UpdateOutProductionOrder", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除外协单", "OutProductionOrderMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/DeleteOutProductionOrder", "btn btn-default", "btnDelete");
            CreateFunctions("ExportOut", "导出外协单", "OutProductionOrderMg", 2, "", "", "icon-add", 4, 3, "/api/services/app/ProductionOrders/ExcelExportOut", "btn btn-default", "_btnExportOut");
            CreateFunctions("ProductionEnterStoreApplyMg", "生产入库申请", "ProductionInfo", 1, "ProductionEnterStoreApplyMg", "ProductionInfo", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "ProductionEnterStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "修改入库申请", "ProductionEnterStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductionOrders/UpdateEnterStoreApply", "btn btn-default", "btnUpdate");
            CreateFunctions("Cancel", "取消入库申请", "ProductionEnterStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/CancelEnterStoreApply", "btn btn-default", "N_btnCancel");
            CreateFunctions("Recovery", "恢复入库申请", "ProductionEnterStoreApplyMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/ProductionOrders/RecoveryEnterStoreApply", "btn btn-default", "N_btnRecovery");
            CreateFunctions("Confirm", "确认入库申请", "ProductionEnterStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/ConfirmEnterStoreApply", "btn btn-default", "N_btnConfirm");
            CreateFunctions("Close", "取消入库申请", "ProductionEnterStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/CloseEnterStoreApply", "btn btn-default", "N_btnClose");
            CreateFunctions("ProductionOutStoreApplyMg", "外协出库申请", "ProductionInfo", 1, "ProductionOutStoreApplyMg", "ProductionInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "ProductionOutStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/ProductionOrders/CreateOutStoreApply", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductionOrders/UpdateOutStoreApply", "btn btn-default", "N_btnUpdate");
            CreateFunctions("QueryStroe", "实时库存", "ProductionOutStoreApplyMg", 2, "", "", "icon-list", 3, 3, "", "btn btn-default", "_btnQueryStore");
            CreateFunctions("Cancel", "取消出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/CancelOutStoreApply", "btn btn-default", "N_btnCancel");
            CreateFunctions("Recovery", "恢复出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/ProductionOrders/RecoveryOutStoreApply", "btn btn-default", "N_btnRecovery");
            CreateFunctions("Confirm", "确认出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/ConfirmOutStoreApply", "btn btn-default", "N_btnConfirm");
            CreateFunctions("Close", "关闭出库申请", "ProductionOutStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/CloseOutStoreApply", "btn btn-default", "N_btnClose");
            CreateFunctions("RePlatingOutStoreApplyMg", "改镀出库申请", "ProductionInfo", 1, "RePlatingOutStoreApplyMg", "ProductionInfo", "icon-menu", 5, 2);
            //add
            CreateFunctions("Query", "查看页面", "RePlatingOutStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Confirm", "确认出库申请", "RePlatingOutStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductionOrders/UpdateFinishOutStoreApply", "btn btn-default", "N_btnConfirm");
            CreateFunctions("Cancel", "取消出库申请", "RePlatingOutStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductionOrders/CancelFinishOutStoreApply", "btn btn-default", "N_btnCancel");
            CreateFunctions("Export", "导出改镀申请单", "RePlatingOutStoreApplyMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/ProductionOrders/RePlatingExportApply", "btn btn-default", "N_btnExport");
            //CreateFunctions("Create", "创建外协单", "RePlatingOutStoreApplyMg", 2, "", "", "icon-add", 4, 3, "/api/services/app/ProductionOrders/CreateOutProductionOrder", "btn btn-default", "N_btnCreate");

        }

        public void SemiProductStoreMenu11()
        {
            CreateFunctions("SemiProductStoreInfo", "半成品仓库管理", "HTSystem", 0, "", "", "icon-system", 11, 1, "");
            CreateFunctions("SemiEnterStoreApplyMg", "半成品入库申请审核", "SemiProductStoreInfo", 1, "SemiEnterStoreApplyMg", "SemiProductStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "SemiEnterStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "审核通过", "SemiEnterStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/SemiEnterStores/Audit", "btn btn-default", "btnUpdate");
            CreateFunctions("Refuse", "拒绝申请", "SemiEnterStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/SemiEnterStores/Refuse", "btn btn-default", "btnDelete");
            //CreateFunctions("QueryStore", "实时库存", "SemiEnterStoreApplyMg", 2, "", "", "icon-task", 4, 3, "", "btn btn-default", "_btnQueryStore");
            CreateFunctions("SemiEnterStoreMg", "半成品入库信息", "SemiProductStoreInfo", 1, "SemiEnterStoreMg", "SemiProductStoreInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "SemiEnterStoreMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("SemiOutStoreApplyMg", "半成品出库申请审核", "SemiProductStoreInfo", 1, "SemiOutStoreApplyMg", "SemiProductStoreInfo", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "SemiOutStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "审核通过", "SemiOutStoreApplyMg", 2, "", "", "icon-edit", 1, 3, "/api/services/app/SemiOutStore/Audit", "btn btn-default", "btnUpdate");
            CreateFunctions("Refuse", "拒绝申请", "SemiOutStoreApplyMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/SemiOutStore/Refuse", "btn btn-default", "btnDelete");
            //CreateFunctions("QueryStore", "实时库存", "SemiOutStoreApplyMg", 2, "", "", "icon-task", 4, 3, "", "btn btn-default", "_btnQueryStore");
            CreateFunctions("SemiOutStoreMg", "半成品出库信息", "SemiProductStoreInfo", 1, "SemiOutStoreMg", "SemiProductStoreInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "SemiOutStoreMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("CurrentSemiStoreHouseMg", "半成品库存信息", "SemiProductStoreInfo", 1, "CurrentSemiStoreHouseMg", "SemiProductStoreInfo", "icon-menu", 5, 2);
            CreateFunctions("Query", "查看页面", "CurrentSemiStoreHouseMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("PackageApply", "申请包装", "CurrentSemiStoreHouseMg", 2, "", "", "",1, 3, "/api/services/app/SemiOutStore/PackageApply", "btn btn-default", "btnPackage");
            CreateFunctions("UpdateKgWeight", "修改千斤重", "CurrentSemiStoreHouseMg", 2, "", "", "",2, 3, "/api/services/app/CurrentSemiStoreHouses/UpdateKgWeight", "btn btn-default", "N_btnUpdateKgWeight");
            CreateFunctions("AddEnter", "入库平衡", "CurrentSemiStoreHouseMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/CurrentSemiStoreHouses/AddEnter", "btn btn-default", "_btnAddEnter");
            CreateFunctions("AddOut", "出库平衡", "CurrentSemiStoreHouseMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/CurrentSemiStoreHouses/AddOut", "btn btn-default", "N_btnAddOut");
            CreateFunctions("ChangeStoreHouse", "仓库移库", "CurrentSemiStoreHouseMg", 2, "", "", "icon-edit", 5, 3, "/api/services/app/CurrentSemiStoreHouses/ChangeStoreHouse", "btn btn-default", "btnChangeStoreHouse");

            CreateFunctions("CurrentSemiStoreQueryMg", "半成品库存数量查询", "SemiProductStoreInfo", 1, "CurrentSemiStoreHouseQueryMg", "SemiProductStoreInfo", "icon-menu", 6, 2);
            CreateFunctions("Query", "查看页面", "CurrentSemiStoreQueryMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Print", "库存报表打印", "CurrentSemiStoreQueryMg", 2, "", "", "icon-search", 2, 3, "", "btn btn-default", "_btnPrint");
            CreateFunctions("QueryEnterOut", "查看进出货明细信息", "CurrentSemiStoreQueryMg", 2, "", "", "icon-search", 3, 3, "", "btn btn-default", "N_btnPrint");
        }
        
        public void JianYan12()
        {
            CreateFunctions("ProductInspect", "技术检验管理", "HTSystem", 0, "", "", "icon-system", 12, 1, "");

            CreateFunctions("ProductItemInspectMg", "半成品入库检验", "ProductInspect", 1, "ProductItem", "ProductInspect", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "ProductItemInspectMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Check", "检验合格", "ProductItemInspectMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/ProductInspect/Check", "btn btn-default", "btnCheck");
            CreateFunctions("UnCheck", "检验不合格", "ProductItemInspectMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/ProductInspect/UnCheck", "btn btn-default", "btnUnCheck");

            CreateFunctions("DisqualifiedProductMg", "不合格产品处理", "ProductInspect", 1, "DisqualifiedProduct", "ProductInspect", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "DisqualifiedProductMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("CheckDowngrade", "检验选项", "DisqualifiedProductMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/DisqualifiedProduct/CheckDowngrade", "btn btn-default", "btnCheckDowngrade");
            CreateFunctions("UseDowngrade", "降级使用", "DisqualifiedProductMg", 2, "", "", "icon-add", 2, 3, "/api/services/app/DisqualifiedProduct/UseDowngrade", "btn btn-default", "btnUseDowngrade");
            //CreateFunctions("Scrapped", "报废处理", "DisqualifiedProductMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/DisqualifiedProduct/Scrapped", "btn btn-default", "btnScrap"); 
            CreateFunctions("ConfirmScrapped", "确认报废", "DisqualifiedProductMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/DisqualifiedProduct/ConfirmScrapped", "btn btn-default", "btnConfirmScrapped");
            CreateFunctions("UnScrapped", "拒绝报废", "DisqualifiedProductMg", 2, "", "", "icon-delete", 5, 3, "/api/services/app/DisqualifiedProduct/UnScrapped", "btn btn-default", "btnUnScrapped");
            //CreateFunctions("AntiPlating", "确认反镀", "DisqualifiedProductMg", 2, "", "", "icon-delete", 6, 3, "/api/services/app/DisqualifiedProduct/AntiPlating", "btn btn-default", "btnAntiPlating");

            CreateFunctions("ProductInspectMg", "检验报告生成", "ProductInspect", 1, "Index", "ProductInspect", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "ProductInspectMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Template", "报告模板", "ProductInspectMg", 2, "", "", "icon-task", 1, 3, "/api/services/app/ProductInspect/Template", "btn btn-default", "_btnTemplate");
            CreateFunctions("Create", "添加报告", "ProductInspectMg", 2, "", "", "icon-add", 2, 3, "/api/services/app/ProductInspect/CreateInspect", "btn btn-default", "btnCreate");

            CreateFunctions("InspectReport", "检验报告确认", "ProductInspect", 1, "Report", "ProductInspect", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "InspectReport", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "修改报告", "InspectReport", 2, "", "", "icon-edit", 1, 3, "/api/services/app/ProductInspect/UpdateInspect", "btn btn-default", "N_btnUpdate");
            CreateFunctions("ConfirmReport", "确认最终报告", "InspectReport", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductInspect/ConfirmReport", "btn btn-default", "btnConfirmReport");
            CreateFunctions("QueryReport", "查看报告", "InspectReport", 2, "", "", "icon-task", 3, 3, "/api/services/app/ProductInspect/QueryReport", "btn btn-default", "btnQueryReport");


           

        }

        public void Baozhuang13()
        {
            CreateFunctions("PackInfo", "包装管理", "HTSystem", 0, "", "", "icon-system", 13, 1, "");
            //CreateFunctions("PackInfoApply", "包装申请", "PackInfo", 1, "PackInfoApply", "PackInfo", "icon-menu", 1, 2);
            //CreateFunctions("Query", "查看页面", "PackInfoApply", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            //CreateFunctions("Create", "发起出库申请", "PackInfoApply", 2, "", "", "icon-edit", 1, 3, "/api/services/app/PackInfoApply/CreatePackInfoApply", "btn btn-default", "_btnCreate");
            //CreateFunctions("Update", "修改出库申请", "PackInfoApply", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/UpdatePackInfoApply", "btn btn-default", "btnUpdate");
            //CreateFunctions("Delete", "取消出库申请", "PackInfoApply", 2, "", "", "icon-delete", 3, 3, "/api/services/app/PackInfoApply/DeletePackInfoApply", "btn btn-default", "btnDelete");

            CreateFunctions("PackageApplyInfo", "包装申请信息查询", "PackInfo", 1, "PackageApplyInfo", "PackInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "PackageApplyInfo", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("PackInfoMg", "包装信息维护", "PackInfo", 1, "Index", "PackInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "PackInfoMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "确认包装", "PackInfoMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/PackInfoApply/CreatePackInfo", "btn btn-default", "btnCreate");
            CreateFunctions("Refuse", "拒绝包装", "PackInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/RefusePackInfoApply", "btn btn-default", "btnRefuse");
            CreateFunctions("Close", "关闭申请", "PackInfoMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/PackInfoApply/ClosePackInfoApply", "btn btn-default", "btnClose");

            CreateFunctions("PackageInfoMg", "产品包装维护", "PackInfoMg", 4, "", "", "icon-edit", 3, 3, "", "btn btn-default", "NotShow");
            CreateFunctions("Query", "查看页面", "PackageInfoMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("EnterBatch", "一键入库", "PackageInfoMg", 2, "", "", "icon-edit", 1, 3, "/api/services/app/PackInfoApply/CreateProductApplyBatch", "btn btn-default", "_btnEnterBatch");
            CreateFunctions("Add", "增加包装规格", "PackageInfoMg", 2, "", "", "icon-add", 2, 3, "/api/services/app/PackInfoApply/AddPackInfo", "btn btn-default", "_btnAdd");
            CreateFunctions("Update", "修改包装规格", "PackageInfoMg", 2, "", "", "icon-edit", 3, 3, "/api/services/app/PackInfoApply/UpdatePackInfo", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除包装规格", "PackageInfoMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/PackInfoApply/DeletePackInfo", "btn btn-default", "btnDelete");
            CreateFunctions("Enter", "包装入库申请", "PackageInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/CreateProductApply", "btn btn-default", "N_btnEnter");
            //CreateFunctions("Delete", "删除入库申请", "PackageInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/DeleteProductApply", "btn btn-default", "N_btnDelete");
            CreateFunctions("Cancel", "取消入库申请", "PackageInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/CancelProductApply", "btn btn-default", "N_btnCancel");
            CreateFunctions("Recovery", "恢复申请", "PackageInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/RecoveryProductApply", "btn btn-default", "N_btnRecovery");
            CreateFunctions("Confirm", "确认入库申请", "PackageInfoMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/PackInfoApply/ConfirmProductApply", "btn btn-default", "N_btnConfirm");
            CreateFunctions("Close", "关闭入库申请", "PackageInfoMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/PackInfoApply/CloseProductApply", "btn btn-default", "N_btnClose");

        }

        public void Store14()
        {

            CreateFunctions("FinshedStoreInfo", "成品仓库管理", "HTSystem", 0, "", "", "icon-system", 14, 1, "");

            CreateFunctions("FinshedEnterStoreApplyMg", "成品入库申请审核", "FinshedStoreInfo", 1, "FinshedEnterStoreApplyMg", "FinshedStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "FinshedEnterStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "审核通过", "FinshedEnterStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/FinshedEnterStore/Audit", "btn btn-default", "btnUpdate");
           
            CreateFunctions("Refuse", "拒绝申请", "FinshedEnterStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/FinshedEnterStore/Refuse", "btn btn-default", "btnDelete");
            //CreateFunctions("QueryStore", "实时库存", "FinshedEnterStoreApplyMg", 2, "", "", "icon-task", 4, 3, "", "btn btn-default", "_btnQueryStore");

            CreateFunctions("FinshedEnterStoreMg", "成品入库信息", "FinshedStoreInfo", 1, "FinshedEnterStoreMg", "FinshedStoreInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "FinshedEnterStoreMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("FinshedOutStoreApplyMg", "成品出库申请审核", "FinshedStoreInfo", 1, "FinshedOutStoreApplyMg", "FinshedStoreInfo", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "FinshedOutStoreApplyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "审核通过", "FinshedOutStoreApplyMg", 2, "", "", "icon-edit", 1, 3, "/api/services/app/FinshedOutStore/Audit", "btn btn-default", "btnUpdate");
            CreateFunctions("BatchAudit", "批量审核", "FinshedOutStoreApplyMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/FinshedOutStore/AuditBatch", "btn btn-default", "a_btnAuditBatch");
            CreateFunctions("Refuse", "拒绝申请", "FinshedOutStoreApplyMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/FinshedOutStore/Refuse", "btn btn-default", "btnRefuse");
            CreateFunctions("Recovery", "恢复申请", "FinshedOutStoreApplyMg", 2, "", "", "icon-add", 4, 3, "/api/services/app/FinshedOutStore/Recovery", "btn btn-default", "btnRecovery");
            //CreateFunctions("QueryStore", "实时库存", "FinshedOutStoreApplyMg", 2, "", "", "icon-task", 4, 3, "", "btn btn-default", "_btnQueryStore");

            CreateFunctions("FinshedOutStoreMg", "成品出库信息", "FinshedStoreInfo", 1, "FinshedOutStoreMg", "FinshedStoreInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "FinshedOutStoreMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("CurrentFinshedStoreHouseMg", "成品库存信息", "FinshedStoreInfo", 1, "CurrentFinshedStoreHouseMg", "FinshedStoreInfo", "icon-menu", 5, 2);
            CreateFunctions("Query", "查看页面", "CurrentFinshedStoreHouseMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("PackageApply", "申请包装", "CurrentFinshedStoreHouseMg", 2, "", "", "", 1, 3, "/api/services/app/FinshedOutStore/PackageApply", "btn btn-default", "btnPackage");
            //CreateFunctions("AddVirtualStore", "添加临时库存", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-add", 2, 3, "/api/services/app/CurrentFinshedStoreHouse/AddVirtualStore", "btn btn-default", "_btnAddVirtualStore");
           
            CreateFunctions("ExportExcel", "导出Excel", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-download", 3, 3, "/api/services/app/CurrentFinshedStoreHouse/ExportExcel", "btn btn-default", "_btnExportExcel");
            CreateFunctions("AddEnter", "入库平衡", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-add", 4, 3, "/api/services/app/CurrentFinshedStoreHouse/AddEnter", "btn btn-default", "_btnAddEnter");
            CreateFunctions("AddOut", "出库平衡", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-delete", 5, 3, "/api/services/app/CurrentFinshedStoreHouse/AddOut", "btn btn-default", "N_btnAddOut");
            CreateFunctions("UpdateStoreLocation", "仓库移库", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-edit", 6, 3, "/api/services/app/CurrentFinshedStoreHouse/UpdateStoreLocation", "btn btn-default", "btnUpdateStoreLocation");
            //add
            CreateFunctions("RePlating", "改镀", "CurrentFinshedStoreHouseMg", 2, "", "", "icon-add", 7, 3, "/api/services/app/FinshedOutStore/RePlating", "btn btn-default", "btnRePlating");


            CreateFunctions("CurrentStoreHouseQueryMg", "库存数量查询", "FinshedStoreInfo", 1, "CurrentStoreHouseQueryMg", "FinshedStoreInfo", "icon-menu", 6, 2);
            CreateFunctions("Query", "查看页面", "CurrentStoreHouseQueryMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Print", "库存报表打印", "CurrentStoreHouseQueryMg", 2, "", "", "icon-search", 2, 3, "/api/services/app/CurrentStoreHouseQueryMg/Print", "btn btn-default", "_btnPrint");
            CreateFunctions("QueryEnterOut", "查看进出货明细信息", "CurrentStoreHouseQueryMg", 2, "", "", "icon-search", 3, 3, "/api/services/app/CurrentStoreHouseQueryMg/Print", "btn btn-default", "N_btnPrint");

            CreateFunctions("EnterOutStoreHouseQueryMg", "进出库数量查询", "FinshedStoreInfo", 1, "EnterOutStoreHouseQueryMg", "FinshedStoreInfo", "icon-menu", 7, 2);
            CreateFunctions("Query", "查看页面", "EnterOutStoreHouseQueryMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ExportExcel", "导出Excel", "EnterOutStoreHouseQueryMg", 2, "", "", "icon-download", 1, 3, "/api/services/app/CurrentFinshedStoreHouse/ExportExcel", "btn btn-default", "_btnExportExcel");


        }
        
        public void InventoryCheck15()
        {
            CreateFunctions("InventoryCheckInfo", "仓库盘点管理", "HTSystem", 0, "", "", "icon-system", 15, 1, "");
            CreateFunctions("InventoryCheckCreate", "库存创建盘点", "InventoryCheckInfo", 1, "InventoryCheckCreate", "FinshedStoreInfo", "icon-menu", 8, 2);
            CreateFunctions("Query", "查看页面", "InventoryCheckCreate", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "盘点任务创建", "InventoryCheckCreate", 2, "", "", "icon-add", 2, 3, "/api/services/app/InventoryCheck/Create", "btn btn-default", "N_btnCreate");
            CreateFunctions("Update", "修改盘点任务", "InventoryCheckCreate", 2, "", "", "icon-edit", 3, 3, "/api/services/app/InventoryCheck/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("ChangeState", "修改盘点任务状态", "InventoryCheckCreate", 2, "", "", "icon-edit", 3, 3, "/api/services/app/InventoryCheck/ChangeState", "btn btn-default", "N_btnChangeState");
            CreateFunctions("CheckData", "盘点库存", "InventoryCheckCreate", 2, "", "", "icon-edit", 3, 3, "/api/services/app/InventoryCheck/CheckData", "btn btn-default", "N_btnCheckData");

            CreateFunctions("InventoryCheck", "库存盘点", "InventoryCheckInfo", 1, "InventoryCheck", "FinshedStoreInfo", "icon-menu", 9, 2);
            CreateFunctions("Query", "查看页面", "InventoryCheck", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("ChangeState", "修改盘点任务状态", "InventoryCheck", 2, "", "", "icon-edit", 3, 3, "/api/services/app/InventoryCheck/UpdateState", "btn btn-default", "N_btnChangeState");
            CreateFunctions("CheckData", "盘点库存", "InventoryCheck", 2, "", "", "icon-edit", 3, 3, "/api/services/app/InventoryCheck/CheckData", "btn btn-default", "N_btnCheckData");
        }
        public void RawMaterialStore16()
        {
            CreateFunctions("RawMaterialStore", "原材料仓库管理", "HTSystem", 0, "", "", "icon-system", 16, 1, "");
            CreateFunctions("RmStoreEnterMg", "原材料入库信息维护", "RawMaterialStore", 1, "RmStoreEnterMg", "RmStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "RmStoreEnterMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "申请入库", "RmStoreEnterMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/RmEnterStore/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "审核入库", "RmStoreEnterMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/RmEnterStore/UpdateState", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "取消入库", "RmStoreEnterMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/RmEnterStore/UpdateState", "btn btn-default", "btnDelete");

            CreateFunctions("RmStoreOutMg", "原材料出库信息维护", "RawMaterialStore", 1, "RmStoreOutMg", "RmStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "RmStoreOutMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            //CreateFunctions("Create", "申请入库", "RmStoreEnterMg", 2, "", "", "icon-add", 1, 3, "/api/services/app/RawMaterialStore/CreateEnter", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "审核出库", "RmStoreOutMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/RmOutStore/UpdateState", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "取消出库", "RmStoreOutMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/RmOutStore/UpdateState", "btn btn-default", "btnDelete");

            CreateFunctions("RmCurrentStoreMg", "原材料库存信息维护", "RawMaterialStore", 1, "RmCurrentStoreMg", "RmStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "RmCurrentStoreMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("AddEnter", "入库平衡", "RmCurrentStoreMg", 2, "", "", "icon-add", 3, 3, "/api/services/app/CurrentRmStoreHouse/AddEnter", "btn btn-default", "N_btnAddEnter");
            CreateFunctions("AddOut", "出库平衡", "RmCurrentStoreMg", 2, "", "", "icon-delete", 4, 3, "/api/services/app/CurrentRmStoreHouse/AddOut", "btn btn-default", "N_btnAddOut");

        }
        public void ScrapStore17()
        {
            CreateFunctions("ScrapStore", "废品仓库管理", "HTSystem", 0, "", "", "icon-system", 17, 1, "");
            CreateFunctions("ScrapStoreEnterMg", "废品入库信息维护", "ScrapStore", 1, "ScrapStoreEnterMg", "ScrapStoreInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "ScrapStoreEnterMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Update", "审核入库", "ScrapStoreEnterMg", 2, "", "", "icon-edit", 2, 3, "/api/services/app/RmEnterStore/UpdateState", "btn btn-default", "N_btnUpdate");
            CreateFunctions("Delete", "取消入库", "ScrapStoreEnterMg", 2, "", "", "icon-delete", 3, 3, "/api/services/app/RmEnterStore/UpdateState", "btn btn-default", "N_btnDelete");

           

          

        }
        


        #endregion
        
        #region 公司基础信息
        
        private void Company30()
        {
            CreateFunctions("Company", "公司信息管理", "HTSystem", 0, "", "", "icon-system", 30, 1, "");

           

            CreateFunctions("Employee", "人员管理", "Company", 1, "Employee", "Company", "icon-menu", 2, 2);

            CreateFunctions("Query", "查看页面", "Employee", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建人员", "Employee", 2, "", "", "icon-add", 1, 3, "/api/services/app/Employee/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改人员", "Employee", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Employee/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除人员", "Employee", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Employee/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Bind", "绑定账号", "Employee", 2, "", "", "icon-add", 4, 3, "/api/services/app/Employee/Bind", "btn btn-default", "btnBind");
            CreateFunctions("UnBind", "解绑账号", "Employee", 2, "", "", "icon-delete", 5, 3, "/api/services/app/Employee/UnBind", "btn btn-default", "btnUnBind");

            CreateFunctions("EmployeePerformance", "绩效查询", "Company", 1, "Performance", "Company", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "EmployeePerformance", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("LicenseDocument", "证照信息维护", "Company", 1, "LicenseDocument", "Company", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "LicenseDocument", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建证照", "LicenseDocument", 2, "", "", "icon-add", 1, 3, "/api/services/app/LicenseDocument/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改证照", "LicenseDocument", 2, "", "", "icon-edit", 2, 3, "/api/services/app/LicenseDocument/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除证照", "LicenseDocument", 2, "", "", "icon-delete", 3, 3, "/api/services/app/LicenseDocument/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("DownLoad", "下载证照", "LicenseDocument", 2, "", "", "icon-delete", 3, 3, "/api/services/app/LicenseDocument/DownLoad", "btn btn-default", "btnDownLoad");
     
  
           
            CreateFunctions("DieMaintenance", "机模设备维护管理", "Company", 0, "", "", "icon-system", 5, 3, "");
            
            CreateFunctions("FixedAsset", "设备固定资产维护", "DieMaintenance", 1, "FixedAsset", "Company", "icon-menu", 1, 4);
            CreateFunctions("Query", "查看页面", "FixedAsset", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建资产", "FixedAsset", 2, "", "", "icon-add", 1, 3, "/api/services/app/FixedAsset/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改资产", "FixedAsset", 2, "", "", "icon-edit", 2, 3, "/api/services/app/FixedAsset/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除资产", "FixedAsset", 2, "", "", "icon-delete", 3, 3, "/api/services/app/FixedAsset/Delete", "btn btn-default", "btnDelete");

            CreateFunctions("Mold", "模具信息维护", "DieMaintenance", 1, "Mold", "Company", "icon-menu", 2, 4);
            CreateFunctions("Query", "查看页面", "Mold", 2, "", "", "", 0, 5, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建模具", "Mold", 2, "", "", "icon-add", 1, 5, "/api/services/app/Mold/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改模具", "Mold", 2, "", "", "icon-edit", 2, 5, "/api/services/app/Mold/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除模具", "Mold", 2, "", "", "icon-delete", 3, 5, "/api/services/app/Mold/Delete", "btn btn-default", "btnDelete");

            //CreateFunctions("MoldMg", "模具维护计划", "DieMaintenance", 1, "MoldMg", "Company", "icon-menu", 1, 4);
            //CreateFunctions("Query", "查看页面", "MoldMg", 2, "", "", "", 0, 5, "", "btn btn-default", "None");
            //CreateFunctions("Create", "创建模具", "MoldMg", 2, "", "", "icon-add", 1, 5, "/api/services/app/Mold/Create", "btn btn-default", "_btnCreate");
            //CreateFunctions("Update", "修改模具", "MoldMg", 2, "", "", "icon-edit", 2, 5, "/api/services/app/Mold/Update", "btn btn-default", "btnUpdate");
            //CreateFunctions("Delete", "删除模具", "MoldMg", 2, "", "", "icon-delete", 3, 5, "/api/services/app/Mold/Delete", "btn btn-default", "btnDelete");
            //CreateFunctions("Maintain", "添加维护记录", "MoldMg", 2, "", "", "icon-add", 4, 5, "/api/services/app/Mold/Maintain", "btn btn-default", "btnMaintain");

            CreateFunctions("DeviceMgPlan", "机模维护计划", "DieMaintenance", 1, "DeviceMgPlan", "Company", "icon-menu", 3, 4);
            CreateFunctions("Query", "查看页面", "DeviceMgPlan", 2, "", "", "", 0, 5, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建计划", "DeviceMgPlan", 2, "", "", "icon-add", 1, 5, "/api/services/app/DeviceMgPlan/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改计划", "DeviceMgPlan", 2, "", "", "icon-edit", 2, 5, "/api/services/app/DeviceMgPlan/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除计划", "DeviceMgPlan", 2, "", "", "icon-delete", 3, 5, "/api/services/app/DeviceMgPlan/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Maintain", "添加维护记录", "DeviceMgPlan", 2, "", "", "icon-add", 5, 5, "/api/services/app/DeviceMgPlan/Maintain", "btn btn-default", "btnMaintain");

            CreateFunctions("MaintainRecord", "机模维护记录", "DieMaintenance", 1, "MaintainRecord", "Company", "icon-menu", 4, 4);
            CreateFunctions("Query", "查看页面", "MaintainRecord", 2, "", "", "", 0, 5, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建维护记录", "MaintainRecord", 2, "", "", "icon-add", 1, 5, "/api/services/app/MaintainRecord/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改维护记录", "MaintainRecord", 2, "", "", "icon-edit", 2, 5, "/api/services/app/MaintainRecord/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除维护记录", "MaintainRecord", 2, "", "", "icon-delete", 3, 5, "/api/services/app/MaintainRecord/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("AddMember", "添加维护人员", "MaintainRecord", 2, "", "", "icon-delete", 3, 5, "/api/services/app/MaintainRecord/AddMember", "btn btn-default", "btnAddMember");
            CreateFunctions("Complete", "完成维护", "MaintainRecord", 2, "", "", "icon-delete", 3, 5, "/api/services/app/MaintainRecord/Complete", "btn btn-default", "btnComplete");

            CreateFunctions("MaintainMember", "维护人员", "DieMaintenance", 3, "MaintainRecord", "Company", "icon-menu", 5, 4);
            CreateFunctions("Update", "修改信息", "MaintainMember", 2, "", "", "icon-edit", 1, 5, "/api/services/app/MaintainRecord/UpdateMember", "btn btn-default", "btnUpdateMember");
            CreateFunctions("Delete", "删除信息", "MaintainMember", 2, "", "", "icon-delete", 1, 5, "/api/services/app/MaintainRecord/DeleteMember", "btn btn-default", "btnDeleteMember");
        }
        
        public void CustomerInfoMenu31()
        {
            CreateFunctions("CustomerInfo", "客户管理", "HTSystem", 0, "", "", "icon-system", 31, 1, "");

            CreateFunctions("Customers", "客户基础信息维护", "CustomerInfo", 1, "Customers", "CustomerInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "Customers", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建客户", "Customers", 2, "", "", "icon-add", 1, 3, "/api/services/app/Customers/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改客户", "Customers", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Customers/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除客户", "Customers", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Customers/Delete", "btn btn-default", "btnDelete");
            /*CreateFunctions("CustomerSends", "客户送货地址维护", "CustomerInfo", 1, "CustomerSends", "CustomerInfo", "icon-menu", 2, 2);
            CreateFunctions("Create", "添加", "CustomerSends", 2, "", "", "icon-add", 4, 3, "/api/services/app/CustomerSends/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改", "CustomerSends", 2, "", "", "icon-edit", 5, 3, "/api/services/app/CustomerSends/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除", "CustomerSends", 2, "", "", "icon-delete", 6, 3, "/api/services/app/CustomerSends/Delete", "btn btn-default", "btnDelete");*/
            CreateFunctions("CreateSend", "添加客户发货地址", "Customers", 2, "", "", "icon-add", 4, 3, "/api/services/app/CustomerSends/Create", "btn btn-default", "N_btnCreate");
            CreateFunctions("UpdateSend", "修改客户发货地址", "Customers", 2, "", "", "icon-edit", 5, 3, "/api/services/app/CustomerSends/Update", "btn btn-default", "N_btnUpdate");
            CreateFunctions("DeleteSend", "删除客户发货地址", "Customers", 2, "", "", "icon-delete", 6, 3, "/api/services/app/CustomerSends/Delete", "btn btn-default", "N_btnDelete");
          
            CreateFunctions("CreateDefaultProduct", "添加客户默认产品", "Customers", 2, "", "", "icon-add", 7, 3, "/api/services/app/CustomerDefaultProduct/Create", "btn btn-default", "N_btnCreate");
            CreateFunctions("UpdateDefaultProduct", "修改客户默认产品", "Customers", 2, "", "", "icon-edit", 8, 3, "/api/services/app/CustomerDefaultProduct/Update", "btn btn-default", "N_btnUpdate");
            CreateFunctions("DeleteDefaultProduct", "删除客户默认产品", "Customers", 2, "", "", "icon-delete", 9, 3, "/api/services/app/CustomerDefaultProduct/Delete", "btn btn-default", "N_btnDelete");
            /*CreateFunctions("QueryCustomerDefaultProduct", "查询客户默认产品", "Customers", 2, "", "", "icon-add", 10, 3, "/api/services/app/Products/GetQueryCustomerDefaultProduct", "btn btn-default", "N_btnQuery");*/

            CreateFunctions("CustomerInvoices", "客户发票地址维护", "CustomerInfo", 3, "", "", "icon-menu", 2, 2);
          CreateFunctions("Create", "添加发票地址", "CustomerInvoices", 2, "", "", "icon-add", 4, 3, "/api/services/app/CustomerInvoiceAddress/Create", "btn btn-default", "_btnCreate");
          CreateFunctions("Update", "修改发票地址", "CustomerInvoices", 2, "", "", "icon-edit", 5, 3, "/api/services/app/CustomerInvoiceAddress/Update", "btn btn-default", "btnUpdate");
          CreateFunctions("Delete", "删除发票地址", "CustomerInvoices", 2, "", "", "icon-delete", 6, 3, "/api/services/app/CustomerInvoiceAddress/Delete", "btn btn-default", "btnDelete");
        }


        public void NotificationInfo32()
        {
            CreateFunctions("NotificationInfo", "消息管理", "HTSystem", 0, "", "", "icon-system", 32, 1, "");

            CreateFunctions("BulletinInfos", "通告管理", "NotificationInfo", 1, "BulletinInfos", "NotificationInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "BulletinInfos", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建通知", "BulletinInfos", 2, "", "", "icon-add", 1, 3, "/api/services/app/BulletinInfos/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改通知", "BulletinInfos", 2, "", "", "icon-edit", 2, 3, "/api/services/app/BulletinInfos/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除通知", "BulletinInfos", 2, "", "", "icon-delete", 3, 3, "/api/services/app/BulletinInfos/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ShortMsgMg", "短消息管理", "NotificationInfo", 1, "ShortMsgMg", "NotificationInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "ShortMsgMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            /*CreateFunctions("Create", "创建通知", "BulletinInfos", 2, "", "", "icon-add", 1, 3, "/api/services/app/BulletinInfos/Create", "btn btn-default", "_btnCreate");*/
            CreateFunctions("SetRead", "已读", "ShortMsgMg", 2, "", "", "icon-edit", 1, 3, "/api/services/app/ShortMsgDetail/SetRead", "btn btn-default", "a_btnSetRead");
            CreateFunctions("Delete", "删除消息", "ShortMsgMg", 2, "", "", "icon-delete", 2, 3, "/api/services/app/ShortMsgDetail/BatchDelete", "btn btn-default", "a_btnDelete");
        }

         
        public void ProductMenu33()
        {
            CreateFunctions("ProductInfo", "产品管理", "HTSystem", 0, "", "", "icon-system", 33, 1, "");

            CreateFunctions("Products", "成品维护", "ProductInfo", 1, "Products", "ProductInfo", "icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "Products", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建成品", "Products", 2, "", "", "icon-add", 1, 3, "/api/services/app/Products/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改成品", "Products", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Products/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除成品", "Products", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Products/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ExportExcel", "Excel导出", "Products", 2, "", "", "icon-add", 4, 3, "/api/services/app/Products/ExportExcel", "btn btn-default", "_btnExport");
            CreateFunctions("SemiProducts", "半成品维护", "ProductInfo", 1, "SemiProducts", "ProductInfo", "icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "SemiProducts", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建半成品", "SemiProducts", 2, "", "", "icon-add", 1, 3, "/api/services/app/SemiProducts/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改半成品", "SemiProducts", 2, "", "", "icon-edit", 2, 3, "/api/services/app/SemiProducts/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除半成品", "SemiProducts", 2, "", "", "icon-delete", 3, 3, "/api/services/app/SemiProducts/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ImportExcel", "Excel导入", "SemiProducts", 2, "", "", "icon-add", 4, 3, "/api/services/app/SemiProducts/ImportExcel", "btn btn-default", "_btnImport");
            
            CreateFunctions("Standards", "标准维护", "ProductInfo", 1, "Standards", "ProductInfo", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "Standards", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建标准", "Standards", 2, "", "", "icon-add", 1, 3, "/api/services/app/Standards/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改标准", "Standards", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Standards/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除标准", "Standards", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Standards/Delete", "btn btn-default", "btnDelete");

            CreateFunctions("RmProduct", "原材料维护", "ProductInfo", 1, "RmProduct", "ProductInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "RmProduct", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建原材料", "RmProduct", 2, "", "", "icon-add", 1, 3, "/api/services/app/RmProduct/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改原材料", "RmProduct", 2, "", "", "icon-edit", 2, 3, "/api/services/app/RmProduct/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除原材料", "RmProduct", 2, "", "", "icon-delete", 3, 3, "/api/services/app/RmProduct/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ImportExcel", "Excel导入", "RmProduct", 2, "", "", "icon-add", 4, 3, "/api/services/app/RmProduct/ImportExcel", "btn btn-default", "_btnImport");

            CreateFunctions("ProductProperty", "产品属性维护", "ProductInfo", 1, "ProductProperty", "ProductInfo", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "ProductProperty", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建产品属性", "ProductProperty", 2, "", "", "icon-add", 1, 3, "/api/services/app/ProductProperty/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改产品属性", "ProductProperty", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ProductProperty/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除产品属性", "ProductProperty", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ProductProperty/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ImportExcel", "Excel导入", "ProductProperty", 2, "", "", "icon-add", 4, 3, "/api/services/app/ProductProperty/ImportExcel", "btn btn-default", "_btnImport");
        }

     


        #endregion
        
        #region 数据统计
        public void Statistic40()
        {
            CreateFunctions("StatisticMg", "数据统计报表", "HTSystem", 0, "", "", "icon-system", 40, 1, "");
            
            CreateFunctions("OutsourcingReportMg", "采购数据统计", "StatisticMg", 1, "OutsourcingReport",  "Statistic","icon-menu", 1, 2);
            CreateFunctions("Query", "查看页面", "OutsourcingReportMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Export", "导出Excel", "OutsourcingReportMg", 2, "", "", "", 1, 3, "", "btn btn-default", "None");
            
            CreateFunctions("ProductionReportMg", "车间生产数据统计", "StatisticMg", 1, "ProductionReport",  "Statistic","icon-menu", 2, 2);
            CreateFunctions("Query", "查看页面", "ProductionReportMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("PackageDailyMg", "车间包装数据统计", "StatisticMg", 1,  "PackageDaily","Statistic", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "PackageDailyMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

            CreateFunctions("InventoryCheckReportMg", "盘点数据统计", "StatisticMg", 1, "InventoryCheckReport", "Statistic", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "InventoryCheckReportMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("StatementBillReportMg", "对账单数据统计", "StatisticMg", 1, "StatementBillReport", "Statistic", "icon-menu", 4, 2);
            CreateFunctions("Query", "查看页面", "StatementBillReportMg", 2, "", "", "", 0, 3, "", "btn btn-default", "None");

        }
        #endregion
          
        #region 基础信息

        private void B100()
        {
            CreateFunctions("BasicInfo", "基础信息", "HTSystem", 0, "", "", "icon-system", 100, 1, "");
            CreateFunctions("Departments", "部门信息管理", "BasicInfo", 1, "Departments", "BasicInfo", "icon-menu", 1, 2);
            CreateFunctions("Create", "创建部门", "Departments", 2, "", "", "icon-add", 1, 3, "/api/services/app/Departments/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改部门", "Departments", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Departments/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除部门", "Departments", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Departments/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Dutys", "职务信息管理", "BasicInfo", 1, "Dutys", "BasicInfo", "icon-menu", 2, 2);
            CreateFunctions("Create", "创建职务", "Dutys", 2, "", "", "icon-add", 1, 3, "/api/services/app/Dutys/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改职务", "Dutys", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Dutys/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除职务", "Dutys", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Dutys/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("StoreHouses", "仓库信息管理", "BasicInfo", 1, "StoreHouses", "BasicInfo", "icon-menu", 3, 2);
            CreateFunctions("Create", "创建仓库", "StoreHouses", 2, "", "", "icon-add", 1, 3, "/api/services/app/StoreHouses/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改仓库", "StoreHouses", 2, "", "", "icon-edit", 2, 3, "/api/services/app/StoreHouses/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除仓库", "StoreHouses", 2, "", "", "icon-delete", 3, 3, "/api/services/app/StoreHouses/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("StoreHouseLocations", "仓库位置管理", "BasicInfo", 1, "StoreHouseLocations", "BasicInfo", "icon-menu", 4, 2);
            CreateFunctions("Create", "创建库位", "StoreHouseLocations", 2, "", "", "icon-add", 1, 3, "/api/services/app/StoreHouseLocations/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改库位", "StoreHouseLocations", 2, "", "", "icon-edit", 2, 3, "/api/services/app/StoreHouseLocations/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除库位", "StoreHouseLocations", 2, "", "", "icon-delete", 3, 3, "/api/services/app/StoreHouseLocations/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Regions", "区域管理", "BasicInfo", 1, "Regions", "BasicInfo", "icon-menu", 5, 2);
            CreateFunctions("Create", "创建区域", "Regions", 2, "", "", "icon-add", 1, 3, "/api/services/app/Regions/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改区域", "Regions", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Regions/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除区域", "Regions", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Regions/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Factories", "办公场所信息管理", "BasicInfo", 1, "Factories", "BasicInfo", "icon-menu", 6, 2);
            CreateFunctions("Create", "创建场所", "Factories", 2, "", "", "icon-add", 1, 3, "/api/services/app/Factories/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改场所", "Factories", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Factories/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除场所场所", "Factories", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Factories/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("OutFactory", "外协加工厂商管理", "BasicInfo", 1, "OutFactory", "BasicInfo", "icon-menu", 7, 2);
            CreateFunctions("Create", "创建外协厂商", "OutFactory", 2, "", "", "icon-add", 1, 3, "/api/services/app/OutFactory/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改外协厂商", "OutFactory", 2, "", "", "icon-edit", 2, 3, "/api/services/app/OutFactory/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除外协厂商", "OutFactory", 2, "", "", "icon-delete", 3, 3, "/api/services/app/OutFactory/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Currency", "货币汇率管理", "BasicInfo", 1, "Currency", "BasicInfo", "icon-menu", 8, 2);
            CreateFunctions("Create", "创建货币", "Currency", 2, "", "", "icon-add", 1, 3, "/api/services/app/Currency/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改货币", "Currency", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Currency/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除货币", "Currency", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Currency/Delete", "btn btn-default", "btnDelete");
            //CreateFunctions("", "编辑汇率", "Currency", 2, "", "", "icon-delete", 3, 3, "/api/services/app/OutFactory/Delete", "btn btn-default", "btnDelete");


            CreateFunctions("Express", "快递公司管理", "BasicInfo", 1, "Express", "BasicInfo", "icon-menu", 8, 2);
            CreateFunctions("Create", "创建快递公司", "Express", 2, "", "", "icon-add", 1, 3, "/api/services/app/Express/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改快递公司", "Express", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Express/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除快递公司", "Express", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Express/Delete", "btn btn-default", "btnDelete");

            
            CreateFunctions("LicenseType", "证照类型管理", "BasicInfo", 1, "LicenseType", "BasicInfo", "icon-menu", 9, 2);
            CreateFunctions("Query", "查看页面", "LicenseType", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建证照类型", "LicenseType", 2, "", "", "icon-add", 1, 3, "/api/services/app/LicenseType/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改证照类型", "LicenseType", 2, "", "", "icon-edit", 2, 3, "/api/services/app/LicenseType/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除证照类型", "LicenseType", 2, "", "", "icon-delete", 3, 3, "/api/services/app/LicenseType/Delete", "btn btn-default", "btnDelete");
            
            CreateFunctions("QualityIssueLabel", "质量问题标签管理", "BasicInfo", 1, "QualityIssueLabel", "BasicInfo", "icon-menu", 10, 2);
            CreateFunctions("Query", "查看页面", "QualityIssueLabel", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建标签", "QualityIssueLabel", 2, "", "", "icon-add", 1, 3, "/api/services/app/QualityIssueLabel/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改标签", "QualityIssueLabel", 2, "", "", "icon-edit", 2, 3, "/api/services/app/QualityIssueLabel/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除标签", "QualityIssueLabel", 2, "", "", "icon-delete", 3, 3, "/api/services/app/QualityIssueLabel/Delete", "btn btn-default", "btnDelete");  

            CreateFunctions("ScrapType", "报废类型管理", "BasicInfo", 1, "ScrapType", "BasicInfo", "icon-menu", 11, 2);
            CreateFunctions("Query", "查看页面", "ScrapType", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建报废类型", "ScrapType", 2, "", "", "icon-add", 1, 3, "/api/services/app/ScrapType/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改报废类型", "ScrapType", 2, "", "", "icon-edit", 2, 3, "/api/services/app/ScrapType/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除报废类型", "ScrapType", 2, "", "", "icon-delete", 3, 3, "/api/services/app/ScrapType/Delete", "btn btn-default", "btnDelete");

            CreateFunctions("FixedAssetType", "固定资产类型管理", "BasicInfo", 1, "FixedAssetType", "BasicInfo", "icon-menu", 12, 2);
            CreateFunctions("Query", "查看页面", "FixedAssetType", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建资产类型", "FixedAssetType", 2, "", "", "icon-add", 1, 3, "/api/services/app/FixedAssetType/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改资产类型", "FixedAssetType", 2, "", "", "icon-edit", 2, 3, "/api/services/app/FixedAssetType/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除资产类型", "FixedAssetType", 2, "", "", "icon-delete", 3, 3, "/api/services/app/FixedAssetType/Delete", "btn btn-default", "btnDelete");
        }
        private void Sys101()
        {
            CreateFunctions("System", "系统信息管理", "HTSystem", 0, "", "", "icon-system", 101, 1, "");

            CreateFunctions("Roles", "角色管理", "System", 1, "SysRoles", "System", "icon-menu", 1, 2);

            CreateFunctions("Query", "查看页面", "Roles", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建角色", "Roles", 2, "", "", "icon-add", 1, 3, "/api/services/app/Roles/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改角色", "Roles", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Roles/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除角色", "Roles", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Roles/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Auth", "角色权限", "Roles", 2, "", "", "icon-perm-list", 4, 3, "/api/services/app/Roles/Auth", "btn btn-default", "btnAuth");

            CreateFunctions("Users", "用户管理", "System", 1, "SysUsers", "System", "icon-menu", 2, 2);

            CreateFunctions("Query", "查看页面", "Users", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建用户", "Users", 2, "", "", "icon-add", 1, 3, "/api/services/app/Users/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改用户", "Users", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Users/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除用户", "Users", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Users/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("ResetPassword", "重置密码", "Users", 2, "", "", "icon-perm-list", 5, 3, "/api/services/app/Users/ResetPassword", "btn btn-default", "btnResetPwd");
            CreateFunctions("Auth", "用户权限", "Users", 2, "", "", "icon-perm-list", 6, 3, "/api/services/app/Users/Auth", "btn btn-default", "btnAuth");

            CreateFunctions("SysState", "系统字典管理", "System", 1, "SysStates", "System", "icon-menu", 3, 2);
            CreateFunctions("Query", "查看页面", "SysState", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建字典", "SysState", 2, "", "", "icon-add", 1, 3, "/api/services/app/States/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改字典", "SysState", 2, "", "", "icon-edit", 2, 3, "/api/services/app/States/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除字典", "SysState", 2, "", "", "icon-delete", 3, 3, "/api/services/app/States/Delete", "btn btn-default", "btnDelete");

            CreateFunctions("SysSetting", "系统配置管理", "System", 1, "SysSettings", "System", "icon-menu", 4, 2);

            CreateFunctions("Query", "查看页面", "SysSetting", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建配置", "SysSetting", 2, "", "", "icon-add", 1, 3, "/api/services/app/Settings/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改配置", "SysSetting", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Settings/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除配置", "SysSetting", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Settings/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("Refresh", "强制刷新", "SysSetting", 2, "", "", "icon-refresh", 4, 3, "/api/services/app/Settings/Refresh", "btn btn-default", "_btnRefresh");
            /*CreateFunctions("LoginSetting", "登陆页面图片配置", "SysSetting", 2, "", "", "icon-detail", 5, 3, "/Settings/LoginImage", "btn btn-default", "_btnLoginSetting");*/

            CreateFunctions("SysFunction", "功能菜单管理", "System", 1, "SysFunctions", "System", "icon-menu", 5, 2);

            CreateFunctions("Query", "查看页面", "SysFunction", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建菜单", "SysFunction", 2, "", "", "icon-add", 1, 3, "/api/services/app/Functions/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改菜单", "SysFunction", 2, "", "", "icon-edit", 2, 3, "/api/services/app/Functions/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除菜单", "SysFunction", 2, "", "", "icon-delete", 3, 3, "/api/services/app/Functions/Delete", "btn btn-default", "btnDelete");
            CreateFunctions("MoveUp", "上移菜单", "SysFunction", 2, "", "", "icon-moveup", 4, 3, "/api/services/app/Functions/MoveUp", "btn btn-default", "btnMoveUp");
            CreateFunctions("MoveDown", "下移菜单", "SysFunction", 2, "", "", "icon-movedown", 5, 3, "/api/services/app/Functions/MoveDown", "btn btn-default", "btnMoveDown");
            CreateFunctions("Refresh", "强制刷新", "SysFunction", 2, "", "", "icon-refresh", 6, 3, "/api/services/app/Functions/Refresh", "btn btn-default", "_btnRefresh");
            CreateFunctions("SysLog", "系统日志管理", "System", 1, "SysLogs", "System", "icon-menu", 6, 2);


            CreateFunctions("SysHelp", "系统帮助管理", "System", 1, "SysHelps", "System", "icon-menu", 7, 2);

            CreateFunctions("Query", "查看页面", "SysHelp", 2, "", "", "", 0, 3, "", "btn btn-default", "None");
            CreateFunctions("Create", "创建帮助", "SysHelp", 2, "", "", "icon-add", 1, 3, "/api/services/app/SysHelps/Create", "btn btn-default", "_btnCreate");
            CreateFunctions("Update", "修改帮助", "SysHelp", 2, "", "", "icon-edit", 2, 3, "/api/services/app/SysHelps/Update", "btn btn-default", "btnUpdate");
            CreateFunctions("Delete", "删除帮助", "SysHelp", 2, "", "", "icon-delete", 3, 3, "/api/services/app/SysHelps/Delete", "btn btn-default", "btnDelete");
        }

        private void Help102()
        {
            CreateFunctions("SysHelpPreview", "系统帮助", "HTSystem", 1, "SysHelpPreview", "System", "icon-menu", 102, 2);
        }

        #endregion



        private void CreateFunctions(string funNo, string funName, string parentNo, int funType,
            string action, string controller, string icon, int sort, int depth, string url = null, string c = "", string s = "", string path = "")
        {
            var fun = _context.Functions.FirstOrDefault(e => e.FunctionNo == funNo && e.ParentNo == parentNo);

            if (fun == null)
            {
                string permissionName = path;
                if (path.IsNullOrEmpty())
                {
                    var parentFun = _context.Functions.FirstOrDefault(f => f.FunctionNo == parentNo);
                    if (parentFun != null)
                    {
                        path = parentFun.FunctionPath + "," + funNo;
                        permissionName = parentFun.PermissionName + "." + funNo;
                    }
                }
                url = url ?? controller + "/" + action;
                fun = new SysFunction
                {
                    FunctionNo = funNo,
                    ParentNo = parentNo,
                    FunctionName = funName,
                    PermissionName = permissionName,
                    FunctionPath = path,
                    FunctionType = funType + 1,
                    Controller = controller,
                    Action = action,
                    Icon = icon,
                    Sort = sort,
                    Depth = depth,
                    Class = c,
                    Script = s,
                    Url = url
                };
                _context.Functions.Add(fun);
                _context.SaveChanges();
            }
            PermissionStr += $"        public const string {fun.FunctionPath.Replace(",", "")} = \"{fun.PermissionName}\";\r\n";
        }

        private void WritePermission()
        {
            if (string.IsNullOrEmpty(PermissionStr))
            {
                return;
            }
            try
            {

                string content = "namespace ShwasherSys.Authorization.Permissions\r\n";
                content += "{\r\n";
                content += "    public static class PermissionNames\r\n";
                content += "    {\r\n";
                content += PermissionStr;
                content += "    }\r\n";
                content += "}\r\n";
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var basePath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, path.IndexOf("\\ShwasherSys.EntityFramework", StringComparison.Ordinal));
                var filePath = Path.Combine(basePath, "ShwasherSys.Core\\Authorization\\Permissions\\PermissionNames.cs");
                var fs = new FileStream(filePath, FileMode.Truncate, FileAccess.ReadWrite);//清空文件内容
                fs.Close();
                using (FileStream fr = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    fr.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    fr.Write(bytes, 0, bytes.Length);
                    fr.Flush();
                }
            }
            catch (Exception e)
            {
                throw (new Exception("Permission:" + e.Message));
            }

           
        }
    }
}
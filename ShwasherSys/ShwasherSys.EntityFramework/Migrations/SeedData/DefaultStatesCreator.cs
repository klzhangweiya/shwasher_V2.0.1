using System;
using System.Linq;
using ShwasherSys.BaseSysInfo;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultStatesCreator
    {
        private readonly ShwasherDbContext _context;

        public DefaultStatesCreator(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.NoHandle, "未处理");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.Downgrade, "检验降级");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.Scrapped, "检验报废");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.ScrappedDowngrade, "拒绝报废后降级");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.AntiPlating, "返工反镀");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.NormalReturn, "正常退货");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.DowngradeHandled, "已降级");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.ScrappedHandled, "已报废");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.OutPurchaseReturnGood, "外购退货");
            CreateState("不合格产品处理类型", "DisProduct", "HandleType", DisProductStateDefinition.SpecialPurchase, "特采");

            CreateState("员工绩效类型", "Performance", "WorkType", WorkTypeDefinition.Product, "生产绩效");
            CreateState("员工绩效类型", "Performance", "WorkType", WorkTypeDefinition.Package, "包装负责人绩效");
            CreateState("员工绩效类型", "Performance", "WorkType", WorkTypeDefinition.VerifyPackage, "包装核件人绩效");
            CreateState("员工绩效类型", "Performance", "WorkType", WorkTypeDefinition.MoldMg, "模具维护绩效");
            CreateState("员工绩效类型", "Performance", "WorkType", WorkTypeDefinition.DeviceMg, "设备维护绩效");

            CreateState("证照类型", "LicenseType", "Type", LicenseTypeDefinition.Company, "公司证照");
            CreateState("证照类型", "LicenseType", "Type", LicenseTypeDefinition.Employee, "员工证照");
            CreateState("证照类型", "LicenseType", "Type", LicenseTypeDefinition.Device, "设备证照");
            CreateState("证照类型", "LicenseType", "Type", LicenseTypeDefinition.Document, "文书存档");
            CreateState("证照类型", "LicenseType", "Type", LicenseTypeDefinition.Other, "其他");

            CreateState("维护设备类型", "Maintain", "Type", MaintainTypeDefinition.Mold, "模具");
            CreateState("维护设备类型", "Maintain", "Type", MaintainTypeDefinition.Device, "设备");
            CreateState("维护设备类型", "Maintain", "Type", MaintainTypeDefinition.Other, "其他");

            CreateState("维护记录状态", "Maintain", "State", MaintainStateDefinition.New, "新建");
            CreateState("维护记录状态", "Maintain", "State", MaintainStateDefinition.Start, "维护中");
            CreateState("维护记录状态", "Maintain", "State", MaintainStateDefinition.Complete, "完成");
            CreateState("维护记录状态", "Maintain", "State", MaintainStateDefinition.End, "结束");

            CreateState("用户类型", "SysUser", "UserType", "1", "超级管理员");
            CreateState("用户类型", "SysUser", "UserType", "2", "系统用户");
            CreateState("用户类型", "SysUser", "UserType", "3", "高级用户");
            CreateState("用户类型", "SysUser", "UserType", "4", "普通用户");

            CreateState("用户状态", "SysUser", "IsActive", "false", "锁定");
            CreateState("用户状态", "SysUser", "IsActive", "true", "激活");

            CreateState("角色类型", "SysRole", "RoleType", "1", "超级管理员");
            CreateState("角色类型", "SysRole", "RoleType", "2", "系统用户角色");
            CreateState("角色类型", "SysRole", "RoleType", "3", "高级用户角色");
            CreateState("角色类型", "SysRole", "RoleType", "4", "普通用户角色");

            CreateState("角色状态", "SysRole", "IsActive", "false", "锁定");
            CreateState("角色状态", "SysRole", "IsActive", "true", "激活");

            CreateState("菜单类型", "SysFunction", "FunctionType", "0", "未知");
            CreateState("菜单类型", "SysFunction", "FunctionType", "1", "目录");
            CreateState("菜单类型", "SysFunction", "FunctionType", "2", "菜单");
            CreateState("菜单类型", "SysFunction", "FunctionType", "3", "按钮");

            CreateState("帮助类别", "SysHelp", "Classification", "0", "订单管理");
            CreateState("帮助类别", "SysHelp", "Classification", "1", "生产管理");
            CreateState("帮助类别", "SysHelp", "Classification", "2", "仓库管理");
            CreateState("帮助类别", "SysHelp", "Classification", "3", "基础信息管理");

            CreateState("仓库类型", "StoreHouse", "StoreHouseType", "1", "成品仓库");
            CreateState("仓库类型", "StoreHouse", "StoreHouseType", "2", "半成品仓库");
            CreateState("仓库类型", "StoreHouse", "StoreHouseType", "3", "原材料仓库");

            CreateState("通知类型", "BulletinInfo", "BulletinType", "1", "一般通知");
            CreateState("通知类型", "BulletinInfo", "BulletinType", "2", "重要通知");
            CreateState("通知类型", "BulletinInfo", "BulletinType", "3", "紧急通知");


            CreateState("订单状态", "OrderHeader", "OrderStatusId", "2", "新建");
            CreateState("订单状态", "OrderHeader", "OrderStatusId", "3", "已审核");
            CreateState("订单状态", "OrderHeader", "OrderStatusId", "12", "完成");
            CreateState("订单状态", "OrderHeader", "OrderStatusId", "23", "仓库");
            CreateState("订单状态", "OrderHeader", "OrderStatusId", "99", "取消");



            CreateState("订单销售类型", "OrderHeader", "SaleType", OrderTypeDefinition.InSale, "内销");
            CreateState("订单销售类型", "OrderHeader", "SaleType", OrderTypeDefinition.OutSale, "外销");
            CreateState("订单销售类型", "OrderHeader", "SaleType", OrderTypeDefinition.Exchange, "换货");

            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "2", "新建");
            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "9", "发货");
            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "11", "结束");
            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "15", "完成审核");
            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "20", "协商完成");
            CreateState("订单明细状态", "OrderItems", "OrderItemStatusId", "99", "取消");

            CreateState("订单明细状态", "OrderItems", "EmergencyLevel", "1", "正常");
            CreateState("订单明细状态", "OrderItems", "EmergencyLevel", "2", "紧急");
            CreateState("订单明细状态", "OrderItems", "EmergencyLevel", "3", "延期");

            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.New, "新建");
            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.Check, "检验中");
            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.HasChecked, "已检验");
            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.RefundApply, "申请退款");
            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.RefundConfirm, "已确认退款");
            CreateState("退货单状态", "ReturnGood", "StateType", ReturnGoodStateDefinition.End, "结束");

            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "1", "新建");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "2", "生产中");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "3", "入库中");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "4", "已入库");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "5", "已结束");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "6", "挂起");
            CreateState("排产单状态", "ProductionOrders", "ProductionOrderStatus", "7", "已审核");

            CreateState("排产单加工类型", "ProductionOrders", "ProcessingType", "1", "车间加工");
            CreateState("排产单加工类型", "ProductionOrders", "ProcessingType", "2", "表面处理");
            CreateState("排产单加工类型", "ProductionOrders", "ProcessingType", "3", "热处理");

            CreateState("排产单加工阶段", "ProductionOrders", "ProcessingLevel", "1", "车间生产");
            CreateState("排产单加工阶段", "ProductionOrders", "ProcessingLevel", "2", "外协加工");


            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "1", "申请中");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "2", "已审核");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "21", "已检验");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "22", "不合格");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "3", "已取消");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "4", "已拒绝");
            CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "5", "已入库");
            //CreateState("半成品入库状态", "SemiEnterStore", "ApplyStatus", "6", "已关闭");

            CreateState("半成品入库来源", "SemiEnterStore", "ApplySource", "1", "车间加工生产");
            CreateState("半成品入库来源", "SemiEnterStore", "ApplySource", "2", "外购单申请入库");

            CreateState("半成品出库状态", "SemiOutStore", "ApplyStatus", "1", "申请中");
            CreateState("半成品出库状态", "SemiOutStore", "ApplyStatus", "2", "已审核");
            CreateState("半成品出库状态", "SemiOutStore", "ApplyStatus", "3", "已取消");//ApplyOutStoreSource
            CreateState("半成品出库状态", "SemiOutStore", "ApplyStatus", "4", "已拒绝");//ApplyOutStoreSource
            CreateState("半成品出库状态", "SemiOutStore", "ApplyStatus", "5", "已出库");
            //CreateState("半成品入库状态", "SemiOutStore", "ApplyStatus", "6", "已关闭");


            CreateState("半成品出库来源", "SemiOutStore", "ApplyOutStoreSource", "1", "外协加工");
            CreateState("半成品出库来源", "SemiOutStore", "ApplyOutStoreSource", "2", "半成品包装");

            CreateState("半成品包装申请状态", "PackInfoApply", "ApplyStatus", "1", "申请中");
            CreateState("半成品包装申请状态", "PackInfoApply", "ApplyStatus", "2", "已审核");
            CreateState("半成品包装申请状态", "PackInfoApply", "ApplyStatus", "3", "已拒绝");

            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "0", "新建");
            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "1", "申请中");
            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "2", "已审核");
            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "3", "已取消");
            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "4", "已拒绝");
            CreateState("成品入库状态", "FinshedEnterStore", "ApplyStatus", "5", "已入库");

          
         

            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "0", "新建");
            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "1", "申请中");
            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "2", "已审核");
            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "3", "已取消");
            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "4", "已拒绝");
            CreateState("成品出库状态", "FinshedOutStore", "ApplyStatus", "5", "已出库");

            CreateState("创建出入库来源类型", "StoreHouse", "CreateSourceType", "1", "默认");
            CreateState("创建出入库来源类型", "StoreHouse", "CreateSourceType", "2", "手动添加");

            CreateState("客户销售类型", "Customer", "SaleType", "1", "内销");
            CreateState("客户销售类型", "Customer", "SaleType", "2", "外销");

            CreateState("客户发票状态", "Invoice", "State", "1", "未收款");
            CreateState("客户发票状态", "Invoice", "State", "2", "已收款");

            CreateState("原材料入库状态", "RmEnterStore", "ApplyStatus", "0", "新建");
            CreateState("原材料入库状态", "RmEnterStore", "ApplyStatus", "1", "申请中");
            CreateState("原材料入库状态", "RmEnterStore", "ApplyStatus", "2", "已入库");
            CreateState("原材料入库状态", "RmEnterStore", "ApplyStatus", "3", "已取消");

            CreateState("原材料出库状态", "RmOutStore", "ApplyStatus", "0", "新建");
            CreateState("原材料出库状态", "RmOutStore", "ApplyStatus", "1", "申请中");
            CreateState("原材料出库状态", "RmOutStore", "ApplyStatus", "2", "已出库");
            CreateState("原材料出库状态", "RmOutStore", "ApplyStatus", "3", "已取消");

            CreateState("盘点任务状态", "InventoryCheck", "CheckState", "1", "新建");
            CreateState("盘点任务状态", "InventoryCheck", "CheckState", "2", "盘点中");
            CreateState("盘点任务状态", "InventoryCheck", "CheckState", "3", "已完成");
            CreateState("盘点任务状态", "InventoryCheck", "CheckState", "4", "已关闭");

            CreateState("产品类型", "ScrapEnterStore", "ProductType", "1", "成品");
            CreateState("产品类型", "ScrapEnterStore", "ProductType", "2", "半成品");
            CreateState("产品类型", "ScrapEnterStore", "ProductType", "3", "原材料");

            CreateState("废品来源类型", "ScrapEnterStore", "ScrapSource", "1", "成品退货");
            CreateState("废品来源类型", "ScrapEnterStore", "ScrapSource", "2", "半成品检验报废");
            // CreateState("废品来源类型", "ScrapEnterStore", "ScrapSource", "3", "原材料");

            CreateState("不合格检验来源类别", "DisqualifiedProduct", "DisqualifiedType", "1", "生产检验");
            CreateState("不合格检验来源类别", "DisqualifiedProduct", "DisqualifiedType", "2", "退货检验");

            CreateState("成品入库状态","FinshedEnterStore", "CreateSourceType", "1", "正常包装入库");
            CreateState("成品入库状态","FinshedEnterStore", "CreateSourceType", "2", "手动平衡");
            CreateState("成品入库状态","FinshedEnterStore", "CreateSourceType",  "3", "正常退货入库");
            CreateState("成品入库状态","FinshedEnterStore", "CreateSourceType",  "4", "返工返镀");
            CreateState("成品入库状态","FinshedEnterStore", "CreateSourceType", "5", "降级使用入库");

            CreateState("产品属性类型", "ProductProperty", "ProductPropertyType", "1", "规格尺寸");
            CreateState("产品属性类型", "ProductProperty", "ProductPropertyType", "2", "材质");
            CreateState("产品属性类型", "ProductProperty", "ProductPropertyType", "3", "硬度");
            CreateState("产品属性类型", "ProductProperty", "ProductPropertyType", "4", "表色");
        }


        private void CreateState(string name, string t, string c, int v, string d)
        {
            CreateState(name, t, c, v + "", d);
        }
        private void CreateState(string name, string t, string c, string v, string d)
        {
            if (_context.SysStates.Any(s => s.TableName == t && s.ColumnName == c && s.CodeValue == v))
            {
                return;
            }

            _context.SysStates.Add(new SysState()
            {
                StateNo = Guid.NewGuid().ToString("N"),
                StateName = name,
                TableName = t,
                ColumnName = c,
                CodeValue = v,
                DisplayValue = d
            });
            _context.SaveChanges();
        } 
      
    }
}
namespace ShwasherSys
{
    public class ShwasherConsts
    {
        public const string SemiProductCache = "SemiProductCache";
        public const string FinshedProductCache = "FinshedProductCache";

        public const string LocalizationSourceName = "Language";
        //public const string SysFunctionCache = "IwbAdminSysFun";
        //public const string SysFunctionItemCache = "IwbAdminSysFunItem";
        //public const string SysStateCache = "IwbAdminSysState";
        //public const string SystemUserCache = "IwbAdminSysUserInfo";
        //public const string AuditLogDescCache = "IwbAdminAuditLogDesc";
        public const string UserExpireTimeCache = "IwbAdminUserExpireTime";

        public const string UserDepartmentIdClaimType = "UserDepartmentIdClaimType";
        public const string TemplateCache = "IwbAdminTemplateCache";
        public const string InspectReportTemplateName = "ReportTemplateName";

        public const string AuthenticationTypes = "ShwasherAuthenticationCookie";

        public const string StoreHouseCache = "StoreHouseCache";
        public const string EmployeeCache = "Employee";
        //public const string  ClaimTypesUserRoleInfos = "ClaimTypesUserRoleInfos";

    }

    public class ShwasherSettingNames
    {
        public const string DINGDANLRMSG = "DINGDANLRMSG";//订单创建后需要通知人员
        public const string DINGDANSHMSG = "DINGDANSHMSG";//订单审核后需要发送消息的用户
        public const string DINGDANXGMSG = "DINGDANXGMSG";//订单修改的时候需要发送消息的用户
        public const string DINGDANXGJEMSG = "DINGDANXGJEMSG";//订单修改金额的时候需要发送消息的用户
        public const string SENDADDRESS = "SENDADDRESS";//送货单上显示的公司地址
        public const string SENDBILLTITLE = "SENDBILLTITLE";//送货单上显示的大标题
        public const string SENDTELEPHONE = "SENDTELEPHONE";//送货单上显示的电话传真号码
        public const string SysUserCop = "SysUserCop";//送货单上显示的电话传真号码
        public const string CanShowOrderItemPrice = "CanShowOrderItemPrice";//查看订单明细价格
        public const string OrderItemPriceTaxRate = "OrderItemPriceTaxRate";//订单价格税率

        public const string DINGDANSLXGTOD = "DINGDANSLXGTOD";//订单数量修改发送消息到部门
        public const string DINGDANJEXGTOD = "DINGDANJEXGTOD";//订单金额修改发送部门消息
        public const string DINGDANXGTOD = "DINGDANXGTOD";//订单修改发送消息到部门
        public const string CKBZRY = "CKBZRY";//订单修改发送消息到部门

    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {
        //新建
        NewCreate=2,
        //已审核
        Audited = 3,
        //完成
        Completed=12,
        //仓库
        Stored=23,
        //删除
        Delete = 99
    }
    /// <summary>
    /// 订单明细状态
    /// </summary>
    public enum OrderItemStatusEnum
    {
        //新建
        NewCreate = 2,
        //已审核
        Audited = 15,
        //结束
        End=11,
        //发货
        Send=9,
        //协商完成
        NegotiationComplete = 20,
        //删除
        Delete = 99
    }


    /// <summary>
    /// 生产单状态
    /// </summary>
    public enum ProductionOrderStatusEnum
    {
        Start=1,
        Storeing=3,
        Producting=2,
        EnterStore = 4,
        End=5,
        HangUp=6, //挂起
        Audited=7, //已经审核
    }
    /// <summary>
    /// 生产环节
    /// </summary>
    public enum ProductionOrderProcessTypeEnum
    {
        CarMachining = 1,
        SurfaceTreatment = 2,
        HeatTreatment = 3,
    }
    /// <summary>
    /// 入库申请状态
    /// </summary>
    public enum EnterStoreApplyStatusEnum
    {
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 3,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = 4,
        /// <summary>
        /// 已入库
        /// </summary>
        EnterStored = 5,
        /// <summary>
        /// 已检验
        /// </summary>
        Checked = 21,
        /// <summary>
        /// 检验不合格
        /// </summary>
        UnChecked = 22,
       
       
    }
    /// <summary>
    /// 入库申请来源
    /// </summary>
    public enum EnterStoreApplySourceEnum
    {
        /// <summary>
        /// 车间加工
        /// </summary>
        InnerCar = 1,
        /// <summary>
        /// 外购
        /// </summary>
        Out = 2,
        /// <summary>
        /// 撤销发货
        /// </summary>
        CancelSend=3,
        /// <summary>
        /// 拒绝包装
        /// </summary>
        RefusePackage = 4,
        /// <summary>
        /// 入库平衡
        /// </summary>
        Balance = 5,
        /// <summary>
        /// 外协
        /// </summary>
        OutProduct = 6,
        /// <summary>
        /// 降级
        /// </summary>
        Downgrade = 7,
        /// <summary>
        /// 特采
        /// </summary>
        SpecialPurchase = 8,
        /// <summary>
        /// 退货
        /// </summary>
        NormalReturnGood = 9,
        //返工返镀
        AntiPlating = 10,

    }

    
    /// <summary>
    /// 出库申请状态
    /// </summary>
    public enum OutStoreApplyStatusEnum
    {
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 3,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = 4,
        /// <summary>
        /// 已出库
        /// </summary>
        OutStored = 5,
    }
    /// <summary>
    /// 出库申请类型
    /// </summary>
    public enum OutStoreApplyTypeEnum
    {
        /// <summary>
        /// 外协加工需要
        /// </summary>
        OutAssistant = 1,
        /// <summary>
        /// 包装
        /// </summary>
        Package = 2,
        /// <summary>
        /// 发货
        /// </summary>
        SendGood = 3,
        /// <summary>
        /// 出库平衡
        /// </summary>
        Balance = 4,
        /// <summary>
        /// 成品改镀
        /// </summary>
        RePlating = 5
    }
    /// <summary>
    /// 出库申请来源
    /// </summary>
    public enum OutStoreApplySourceTypeEnum
    {
        /// <summary>
        /// 半成品
        /// </summary>
        SemiProduct=1,
        /// <summary>
        /// 成品
        /// </summary>
        FinshedProduct=2
    }
    /// <summary>
    /// 包装出库申请状态
    /// </summary>
    public enum PackageApplyStatusEnum
    {
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = 3,
       
    }
    /// <summary>
    /// 包装完成入库申请状态
    /// </summary>
    public enum FinshedEnterStoreApplyStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        New = 0,
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 3,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = 4,
        /// <summary>
        /// 已入库
        /// </summary>
        EnterStored = 5,

    }
    /// <summary>
    /// 成品出库申请状态
    /// </summary>
    public enum FinshedOutStoreApplyStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        New = 0,
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 3,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = 4,
        /// <summary>
        /// 已入库
        /// </summary>
        OutStored = 5,

    }
    /// <summary>
    /// 原材料出入库申请来源
    /// </summary>
    public enum FinshedEnterSourceEnum
    {
        /// <summary>
        /// 生产
        /// </summary>
        Product = 1,
        /// <summary>
        /// 改包装
        /// </summary>
        ChangePackage = 2,
        /// <summary>
        /// 降级
        /// </summary>
        Downgrade = 3,

        /// <summary>
        /// 平衡
        /// </summary>
        Balance = 5,
     
      
    }  /// <summary>
    /// 原材料出入库申请来源
    /// </summary>
    public enum RmEnterOutStatusEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        New = 0,
        /// <summary>
        /// 申请中
        /// </summary>
        Applying = 1,
        /// <summary>
        /// 库存完成
        /// </summary>
        Stored = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 3,
     
      
    }
    public enum CreateSourceType
    {
        Normal=1,
        /// <summary>
        /// 人工手动
        /// </summary>
        Manual=2,
    }
    //发票状态
    public enum InvoiceState
    {
        NotPay=1,
        HasPay=2
    }

    public class CreateProductionType
    {
        public const string ReturnToSemiStore = "T";//成品退货到半成品仓库
        public const string ChangeProduction = "G";//改镀
    }

    /// <summary>
    /// 入库创建来源
    /// </summary>
    public enum EnterStoreCreateSourceEnum
    {

        NormalPackage = 1,
        /// <summary>
        /// 手动平衡
        /// </summary>
        Balance = 2,
        //正常退货入库
        NormalReturnGood = 3,
        //返工返镀
        AntiPlating = 4,
        //降级使用入库
        Downgrade = 5
    }

}
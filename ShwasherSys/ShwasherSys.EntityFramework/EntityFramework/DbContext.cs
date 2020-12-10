using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.UI.WebControls;
using ShwasherSys.Authorization.Roles;
using ShwasherSys.Authorization.Users;
using ShwasherSys.BaseSysInfo;
using IwbZero;
using ShwasherSys.BasicInfo;
using ShwasherSys.BasicInfo.OutFactory;
using ShwasherSys.CompanyInfo;
using ShwasherSys.CustomerInfo;
using ShwasherSys.Inspection;
using ShwasherSys.Invoice;
using ShwasherSys.NotificationInfo;
using ShwasherSys.Order;
using ShwasherSys.OrderSendInfo;
using ShwasherSys.PackageInfo;
using ShwasherSys.ProductionOrderInfo;
using ShwasherSys.ProductInfo;
using ShwasherSys.ProductStoreInfo;
using ShwasherSys.ReturnGoods;
using ShwasherSys.RmStore;
using ShwasherSys.ScrapStore;
using ShwasherSys.SemiProductStoreInfo;

namespace ShwasherSys.EntityFramework
{
    public class ShwasherDbContext : IwbDbContext<SysRole, SysUser>
    {
        //TODO: Define an IDbSet for each Entity...


        public IDbSet<SysAppGuid> AppGuids { get; set; }
        public IDbSet<SysLog> AuditLogs { get; set; }
        public IDbSet<SysFunction> Functions { get; set; }
        public IDbSet<SysSetting> Settings { get; set; }
        public IDbSet<SysState> SysStates { get; set; }
        public IDbSet<SysAttachFile> SysAttachFiles { get; set; }

        public IDbSet<BusinessLog> BusinessLogs { get; set; }

        #region New
        public IDbSet<Employee> EmployeeInfo { get; set; }
        public IDbSet<ViewEmployee> ViewEmployeeInfo { get; set; }
        public IDbSet<EmployeeWorkPerformance> EmployeeWorkPerformanceInfo { get; set; }
        public IDbSet<DisqualifiedProduct> DisqualifiedProductInfo { get; set; }
        public IDbSet<CustomerDisabledProduct> CustomerDisabledProductInfo { get; set; }
        public IDbSet<ProductionLog> ProductionOrderLogInfo { get; set; }
        public IDbSet<LicenseDocument> LicenseDocumentInfo { get; set; }
        public IDbSet<LicenseType> LicenseTypeInfo { get; set; }
        public IDbSet<FixedAsset> FixedAssetInfo { get; set; }
        public IDbSet<Mold> MoldInfo { get; set; }
        public IDbSet<DeviceMgPlan> DeviceMgInfo { get; set; }
        public IDbSet<MaintenanceRecord> MaintenanceRecordsInfo { get; set; }
        public IDbSet<MaintenanceMember> MaintenanceMemberInfo { get; set; }
        public IDbSet<QualityIssueLabel> QualityIssueLabelInfo { get; set; }
        public IDbSet<ScrapType> ScrapTypeInfo { get; set; }
        public IDbSet<FixedAssetType> FixedAssetTypeInfo { get; set; }
        public IDbSet<CustomerInvoiceAddress> CustomerInvoiceAddressInfo { get; set; }
        public IDbSet<ProductInspectReport> ProductInspectReportInfo { get; set; }

        #endregion


        #region ShwasherSys

        public IDbSet<OrderHeader> OrderHeaders { get; set; }
        public IDbSet<OrderItem> OrderItems { get; set; }
        public IDbSet<SysHelp> SysHelps { get; set; }
        public IDbSet<Department> Department { get; set; }
        public IDbSet<Duty> Dutys { get; set; }
        public IDbSet<StoreHouse> StoreHouses { get; set; }
        public IDbSet<StoreHouseLocation> StoreHouseLocations { get; set; }
        public IDbSet<Regions> Regions { get; set; }
        public IDbSet<Factories> Factories { get; set; }
        public IDbSet<OutFactory> OutFactory { get; set; }
        public IDbSet<BulletinInfo> BulletinInfos { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<CustomerDefaultProduct> CustomerDefaultProducts { get; set; }
        public IDbSet<CustomerSend> CustomerSends { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Standard> Standards { get; set; }

        public IDbSet<ShortMessage> ShortMessages { get; set; }
        public IDbSet<ShortMsgDetail> ShortMsgDetails { get; set; }
        public IDbSet<ViewOrderItems> ViewOrderItems { get; set; }

        public IDbSet<ViewOrderSend> ViewOrderSends { get; set; }
        public IDbSet<ViewOrderSendBill> ViewOrderSendBills { get; set; }

        public IDbSet<SemiProducts> SemiProducts { get; set; }

        public IDbSet<ProductionOrder> ProductionOrders { get; set; }


        public IDbSet<BackUpCurrentSemiStoreHouse> BackUpCurrentSemiStoreHouses { get; set; }

        public IDbSet<CurrentSemiStoreHouse> CurrentSemiStoreHouses { get; set; }

        public IDbSet<SemiEnterStore> SemiEnterStores { get; set; }

        public IDbSet<SemiOutStore> SemiOutStores { get; set; }

        public IDbSet<SemiProductStore> SemiProductStores { get; set; }

        public IDbSet<ProductInspectInfo> ProductInspectInfos { get; set; }
        public IDbSet<ProductInspectReportContent> PtoductInspectReports { get; set; }
        public IDbSet<TemplateInfo> TemplateInfos { get; set; }

        public IDbSet<PackageApply> PackageApply { get; set; }
        public IDbSet<FinshedEnterStore> FinshedEnterStores { get; set; }
        public IDbSet<ProductOutStore> ProductOutStores { get; set; }
        public IDbSet<CurrentProductStoreHouse> CurrentProductStoreHouses { get; set; }

        public IDbSet<OrderSend> OrderSends { get; set; }
        public IDbSet<OrderSendBill> OrderSendBills { get; set; }
        public IDbSet<OrderStickBill> OrderStickBills { get; set; }

        public IDbSet<ViewCurrentSemiStoreHouse> ViewCurrentSemiStoreHouses { get; set; }
        public IDbSet<ViewSemiEnterStore> ViewSemiEnterStores { get; set; }
        public IDbSet<ViewSemiOutStore> ViewSemiOutStores { get; set; }

        public IDbSet<ViewCustomerStick> ViewCustomerSticks { get; set; }
        public IDbSet<OrderUnit> OrderUnits { get; set; }

        public IDbSet<ViewOrderSendStickBill> ViewOrderSendStickBills { get; set; }
        public IDbSet<ViewQueryCurrentProductNum> ViewQueryCurrentProductNums { get; set; }
        public IDbSet<ViewBookedProductNum> ViewBookedProductNums { get; set; }
        public IDbSet<ViewCanProductStore> ViewCanProductStores { get; set; }

        public IDbSet<ViewProductEnterStore> ViewProductEnterStores { get; set; }
        public IDbSet<ViewProductOutStore> ViewProductOutStores { get; set; }
        public IDbSet<ViewCurrentProductStoreHouse> ViewCurrentProductStoreHouses { get; set; }
        public IDbSet<ViewEnterOutProductStore> ViewEnterOutProductStores { get; set; }
        public IDbSet<ViewEnterOutLogCus> ViewEnterOutLogCus { get; set; }
        public IDbSet<ViewCurrentStoreTotal> ViewCurrentStoreTotals { get; set; }
        public IDbSet<ViewPackageApply> ViewPackageApply { get; set; }
        public IDbSet<Currency> Currency { get; set; }
        public IDbSet<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }
        public IDbSet<ViewStickBill> ViewStickBill { get; set; }

        

        public IDbSet<OrderSendExceed> OrderSendExceed { get; set; }

        public IDbSet<StatementBill> StatementBill { get; set; }

        public IDbSet<RmProduct> RmProducts { get; set; }

        public IDbSet<RmEnterStore> RmEnterStores { get; set; }

        public IDbSet<RmOutStore> RmOutStores { get; set; }

        public IDbSet<CurrentRmStoreHouse> CurrentRmStoreHouses { get; set; }
        
        public IDbSet<ViewRmEnterStore> ViewRmEnterStores { get; set; }

        public IDbSet<ViewRmOutStore> ViewRmOutStores { get; set; }
        public IDbSet<ViewCurrentRmStoreHouse> ViewCurrentRmStoreHouse { get; set; }

        public IDbSet<ExpressLogistics> ExpressLogistics { get; set; }
        public IDbSet<ExpressProviderMapper> ExpressProviderMapper { get; set; }
        public IDbSet<ExpressServiceProvider> ExpressServiceProviders { get; set; }

        public IDbSet<InventoryCheckInfo> InventoryCheck { get; set; }
        public IDbSet<InventoryCheckRecord> InventoryCheckRecord { get; set; }

        public IDbSet<ViewInventoryCheckRecordProduct> ViewInventoryCheckRecordProduct { get; set; }
        public IDbSet<ViewInventoryCheckRecordSemi> ViewInventoryCheckRecordSemi { get; set; }

        public IDbSet<ViewEnterOutSemiProductStore> ViewEnterOutSemiProductStore { get; set; }
        public IDbSet<ViewCurrentSemiStoreTotal> ViewCurrentSemiStoreTotal { get; set; }

        public IDbSet<ScrapEnterStore> ScrapEnterStores { get; set; }

        public IDbSet<ViewScrapEnterStore> ViewScrapEnterStore { get; set; }

        public IDbSet<ReturnGoodOrder> ReturnGoodOrders { get; set; }

        public IDbSet<ViewStatementBill> ViewStatementBill { get; set; }
        #endregion

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public ShwasherDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in IwbYueDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of IwbYueDbContext since ABP automatically handles it.
         */
        public ShwasherDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public ShwasherDbContext(DbConnection existingConnection)
            : base(existingConnection, false)
        {

        }

        public ShwasherDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Add(new DecimalPropertyConvention(18,3));
            modelBuilder.Entity<OrderItem>().Property(t => t.Id)
                .HasColumnName("OrderItemId");
            modelBuilder.Entity<OrderItem>().Property(t => t.Quantity)
                .HasPrecision(18, 3);
            modelBuilder.Entity<OrderHeader>().Property(t => t.Id)
                .HasColumnName("OrderNo");
            modelBuilder.Entity<SysHelp>().Property(t => t.Id)
                .HasColumnName("HelpId");
            modelBuilder.Entity<Department>().Property(t => t.Id)
                .HasColumnName("DepartmentID");
            modelBuilder.Entity<Duty>().Property(t => t.Id)
                .HasColumnName("DutyID");
            modelBuilder.Entity<StoreHouse>().Property(t => t.Id)
                .HasColumnName("StoreHouseID");
            modelBuilder.Entity<Factories>().Property(t => t.Id)
                .HasColumnName("FactoryID");
            modelBuilder.Entity<OutFactory>().Property(t => t.Id)
                .HasColumnName("OutFactoryId");
            modelBuilder.Entity<Regions>().Property(t => t.Id)
                .HasColumnName("RegionID");
            modelBuilder.Entity<BulletinInfo>().Property(t => t.Id)
                .HasColumnName("BulletinInfoId");
            //BulletinInfoId  CustomerId
            modelBuilder.Entity<Customer>().Property(t => t.Id)
                .HasColumnName("CustomerId");
            modelBuilder.Entity<CustomerSend>().Property(t => t.Id)
                .HasColumnName("CustomerSendId");
            modelBuilder.Entity<Product>().Property(t => t.Id).HasColumnName("ProductNo");
            modelBuilder.Entity<Standard>().Property(t => t.Id).HasColumnName("StandardId");
            modelBuilder.Entity<ShortMessage>().Property(t => t.Id).HasColumnName("MsgID");
            modelBuilder.Entity<ShortMsgDetail>().Property(t => t.Id).HasColumnName("DetailID");
            modelBuilder.Entity<ViewOrderItems>().Property(t => t.Id).HasColumnName("OrderItemId");
            modelBuilder.Entity<SemiProducts>().Property(t => t.Id).HasColumnName("SemiProductNo");
            modelBuilder.Entity<OrderSend>().Property(t => t.Id).HasColumnName("OrderSendId");
            modelBuilder.Entity<OrderSendBill>().Property(t => t.Id).HasColumnName("OrderSendBillNo");
            modelBuilder.Entity<OrderStickBill>().Property(t => t.Id).HasColumnName("OrderStickBillNo");
            modelBuilder.Entity<ViewOrderSend>().Property(t => t.Id).HasColumnName("OrderSendId");
            modelBuilder.Entity<ViewOrderSendBill>().Property(t => t.Id).HasColumnName("OrderSendBillNo");
            modelBuilder.Entity<OrderUnit>().Property(t => t.Id).HasColumnName("OrderUnitId");
            modelBuilder.Entity<ViewCustomerStick>().Property(t => t.Id).HasColumnName("OrderSendId");
            modelBuilder.Entity<ViewOrderSendStickBill>().Property(t => t.Id).HasColumnName("OrderSendId");
            modelBuilder.Entity<ViewQueryCurrentProductNum>().Property(t => t.Id).HasColumnName("ProductNo");
            modelBuilder.Entity<ViewBookedProductNum>().Property(t => t.Id).HasColumnName("ProductNo");
            modelBuilder.Entity<ViewCanProductStore>().Property(t => t.Id).HasColumnName("ProductNo");
            modelBuilder.Entity<ViewCurrentStoreTotal>().Property(t => t.Id).HasColumnName("ProductNo");
            modelBuilder.Entity<ViewStickBill>().Property(t => t.Id).HasColumnName("OrderStickBillNo");
           
        }
    }

    /// <summary>
    /// 用于modelBuilder全局设置自定义精度属性
    /// </summary>
    public class DecimalPrecisionAttributeConvention
        : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
    {
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration,
            DecimalPrecisionAttribute attribute)
        {
            if (attribute.Precision < 1 || attribute.Precision > 38)
            {
                throw new InvalidOperationException("Precision must be between 1 and 38.");
            }

            if (attribute.Scale > attribute.Precision)
            {
                throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
            }

            configuration.HasPrecision(attribute.Precision, attribute.Scale);
            
        }
    }
   

}

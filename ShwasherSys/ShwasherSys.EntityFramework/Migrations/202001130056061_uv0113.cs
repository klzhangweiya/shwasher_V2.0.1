namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv0113 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpressLogistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpressName = c.String(maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Sort = c.Int(nullable: false),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 50),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExpressProviderMappers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpressId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        MapperCode = c.String(maxLength: 50),
                        QueryUrl = c.String(maxLength: 500),
                        ExtendInfo = c.String(maxLength: 500),
                        ActiveStatus = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpressLogistics", t => t.ExpressId, cascadeDelete: true)
                .ForeignKey("dbo.ExpressServiceProviders", t => t.ProviderId, cascadeDelete: true)
                .Index(t => t.ExpressId)
                .Index(t => t.ProviderId);
            
            CreateTable(
                "dbo.ExpressServiceProviders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProviderName = c.String(maxLength: 50),
                        QueryApiUrl = c.String(maxLength: 150),
                        CallBackUrl = c.String(maxLength: 150),
                        ExcuteNamespaceAndMethod = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 50),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderSendBills", "ExpressId", c => c.Int());
            AddColumn("dbo.OrderSendBills", "ExpressBillNo", c => c.String());
            //AddColumn("dbo.PackageApply", "KgWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            //AddColumn("dbo.RmEnterStore", "CreateSourceType", c => c.Int(nullable: false));
            //AddColumn("dbo.RmOutStore", "CreateSourceType", c => c.Int(nullable: false));
            //AddColumn("dbo.v_OrderSendBill", "ExpressId", c => c.Int());
            //AddColumn("dbo.v_OrderSendBill", "ExpressBillNo", c => c.String());
            //AddColumn("dbo.v_OrderSendBill", "ExpressName", c => c.String());
            //AddColumn("dbo.N_ViewPackageApply", "KgWeight", c => c.Decimal(precision: 18, scale: 2));
            //AddColumn("dbo.N_ViewRmEnterStore", "CreateSourceType", c => c.Int());
            //AddColumn("dbo.N_ViewRmOutStore", "CreateSourceType", c => c.Int());
            //AlterColumn("dbo.EmployeeWorkPerformanceInfo", "Performance", c => c.Decimal(precision: 18, scale: 3));
            //AlterColumn("dbo.RmEnterStore", "ProductionOrderNo", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpressProviderMappers", "ProviderId", "dbo.ExpressServiceProviders");
            DropForeignKey("dbo.ExpressProviderMappers", "ExpressId", "dbo.ExpressLogistics");
            DropIndex("dbo.ExpressProviderMappers", new[] { "ProviderId" });
            DropIndex("dbo.ExpressProviderMappers", new[] { "ExpressId" });
            AlterColumn("dbo.RmEnterStore", "ProductionOrderNo", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.EmployeeWorkPerformanceInfo", "Performance", c => c.Int(nullable: false));
            DropColumn("dbo.N_ViewRmOutStore", "CreateSourceType");
            DropColumn("dbo.N_ViewRmEnterStore", "CreateSourceType");
            DropColumn("dbo.N_ViewPackageApply", "KgWeight");
            DropColumn("dbo.v_OrderSendBill", "ExpressName");
            DropColumn("dbo.v_OrderSendBill", "ExpressBillNo");
            DropColumn("dbo.v_OrderSendBill", "ExpressId");
            DropColumn("dbo.RmOutStore", "CreateSourceType");
            DropColumn("dbo.RmEnterStore", "CreateSourceType");
            DropColumn("dbo.PackageApply", "KgWeight");
            DropColumn("dbo.OrderSendBills", "ExpressBillNo");
            DropColumn("dbo.OrderSendBills", "ExpressId");
            DropTable("dbo.ExpressServiceProviders");
            DropTable("dbo.ExpressProviderMappers");
            DropTable("dbo.ExpressLogistics");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0109 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeWorkPerformanceInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PerformanceNo = c.String(maxLength: 20),
                        EmployeeId = c.Int(nullable: false),
                        RelatedNo = c.String(maxLength: 20),
                        ProductOrderNo = c.String(maxLength: 11),
                        WorkType = c.Int(nullable: false),
                        Performance = c.Int(nullable: false),
                        PerformanceUnit = c.String(maxLength: 10),
                        PerformanceDesc = c.String(maxLength: 500),
                        Remark = c.String(maxLength: 500),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.EmployeeInfo", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.ProductionOrderLogInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionNo = c.String(maxLength: 20),
                        ProductOrderNo = c.String(maxLength: 11),
                        EmployeeId = c.Int(nullable: false),
                        CarNo = c.String(maxLength: 20),
                        QuantityWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        QuantityPcs = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.EmployeeInfo", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.CreatorUserId);
            
            AddColumn("dbo.FinshedEnterStore", "PackageUser", c => c.String(maxLength: 20));
            AddColumn("dbo.FinshedEnterStore", "VerifyUser", c => c.String(maxLength: 20));
            AddColumn("dbo.FinshedEnterStore", "ApplyQuantity2", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            //AddColumn("dbo.ProductionOrders", "HasExported", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SemiEnterStore", "ApplyStatus", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.SemiEnterStore", "ApplySource", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.SemiOutStores", "ApplyStatus", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.SemiOutStores", "ApplyOutStoreSource", c => c.String(nullable: false, maxLength: 5));
            DropTable("dbo.ProductionOrderLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductionOrderLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        OperatorTitle = c.String(maxLength: 150),
                        OperatorConent = c.String(),
                        TimeCreated = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ProductionOrderLogInfo", "EmployeeId", "dbo.EmployeeInfo");
            DropForeignKey("dbo.ProductionOrderLogInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.EmployeeWorkPerformanceInfo", "EmployeeId", "dbo.EmployeeInfo");
            DropForeignKey("dbo.EmployeeWorkPerformanceInfo", "CreatorUserId", "dbo.Sys_Users");
            DropIndex("dbo.ProductionOrderLogInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.ProductionOrderLogInfo", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeWorkPerformanceInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.EmployeeWorkPerformanceInfo", new[] { "EmployeeId" });
            AlterColumn("dbo.SemiOutStores", "ApplyOutStoreSource", c => c.String(nullable: false, maxLength: 1));
            AlterColumn("dbo.SemiOutStores", "ApplyStatus", c => c.String(nullable: false, maxLength: 1));
            AlterColumn("dbo.SemiEnterStore", "ApplySource", c => c.String(nullable: false, maxLength: 1));
            AlterColumn("dbo.SemiEnterStore", "ApplyStatus", c => c.String(nullable: false, maxLength: 1));
            DropColumn("dbo.ProductionOrders", "HasExported");
            DropColumn("dbo.FinshedEnterStore", "ApplyQuantity2");
            DropColumn("dbo.FinshedEnterStore", "VerifyUser");
            DropColumn("dbo.FinshedEnterStore", "PackageUser");
            DropTable("dbo.ProductionOrderLogInfo");
            DropTable("dbo.EmployeeWorkPerformanceInfo");
        }
    }
}

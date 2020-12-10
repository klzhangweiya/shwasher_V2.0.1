namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200301 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductInspectReports", newName: "ProductInspectReportContents");
            CreateTable(
                "dbo.ProductInspectReportInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductInspectReportNo = c.String(nullable: false, maxLength: 32),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        SemiProductNo = c.String(maxLength: 32),
                        ConfirmStatus = c.Int(nullable: false),
                        InspectCount = c.Int(nullable: false),
                        ConfirmDate = c.DateTime(),
                        ConfirmUser = c.String(maxLength: 100),
                        InspectContent = c.String(maxLength: 1000),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProductInspectInfos", "ProductInspectReportNo", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.ProductInspectInfos", "InspectSubject", c => c.String(maxLength: 50));
            AddColumn("dbo.ProductInspectReportContents", "ProductionOrderNo", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInspectReportContents", "ProductionOrderNo");
            DropColumn("dbo.ProductInspectInfos", "InspectSubject");
            DropColumn("dbo.ProductInspectInfos", "ProductInspectReportNo");
            DropTable("dbo.ProductInspectReportInfos");
            RenameTable(name: "dbo.ProductInspectReportContents", newName: "ProductInspectReports");
        }
    }
}

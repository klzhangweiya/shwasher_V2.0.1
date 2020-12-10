namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0108 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerDisProductInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductOrderNo = c.String(),
                        CustomerNo = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.DisqualifiedProductInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisqualifiedNo = c.String(maxLength: 20),
                        ProductOrderNo = c.String(maxLength: 11),
                        ProductNo = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 100),
                        ProductType = c.Int(nullable: false),
                        QuantityWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        QuantityPcs = c.Decimal(nullable: false, precision: 18, scale: 3),
                        HandleType = c.Int(nullable: false),
                        CheckUser = c.String(maxLength: 20),
                        CheckDate = c.DateTime(),
                        HandleUser = c.String(maxLength: 20),
                        HandleDate = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DisqualifiedProductInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.CustomerDisProductInfo", "CreatorUserId", "dbo.Sys_Users");
            DropIndex("dbo.DisqualifiedProductInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.CustomerDisProductInfo", new[] { "CreatorUserId" });
            DropTable("dbo.DisqualifiedProductInfo");
            DropTable("dbo.CustomerDisProductInfo");
        }
    }
}

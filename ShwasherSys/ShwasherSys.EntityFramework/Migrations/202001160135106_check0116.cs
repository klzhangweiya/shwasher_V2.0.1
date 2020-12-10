namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check0116 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.N_ViewInventoryCheckRecord_Product",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductNo = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            ProductDesc = c.String(),
            //            CheckNo = c.String(),
            //            CurrentStoreHouseNo = c.String(),
            //            CheckQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            StoreQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            StoreLocationNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            ProductionOrderNo = c.String(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewInventoryCheckRecord_Semi",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductNo = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            ProductDesc = c.String(),
            //            CheckNo = c.String(),
            //            CurrentStoreHouseNo = c.String(),
            //            CheckQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            StoreQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            StoreLocationNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            ProductionOrderNo = c.String(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.N_ViewInventoryCheckRecord_Semi");
            DropTable("dbo.N_ViewInventoryCheckRecord_Product");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv01131 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CurrentProductStoreHouse", "InventoryCheckState", c => c.Int());
            AddColumn("dbo.CurrentProductStoreHouse", "ReturnState", c => c.Int());
            AddColumn("dbo.CurrentSemiStoreHouse", "StoreLocationNo", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.CurrentSemiStoreHouse", "PreMonthQuantity", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.CurrentSemiStoreHouse", "InventoryCheckState", c => c.Int());
            AddColumn("dbo.CurrentSemiStoreHouse", "ReturnState", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CurrentSemiStoreHouse", "ReturnState");
            DropColumn("dbo.CurrentSemiStoreHouse", "InventoryCheckState");
            DropColumn("dbo.CurrentSemiStoreHouse", "PreMonthQuantity");
            DropColumn("dbo.CurrentSemiStoreHouse", "StoreLocationNo");
            DropColumn("dbo.CurrentProductStoreHouse", "ReturnState");
            DropColumn("dbo.CurrentProductStoreHouse", "InventoryCheckState");
        }
    }
}

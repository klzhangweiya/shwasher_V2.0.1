namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv01143 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryCheck", "ProductNo", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "CheckNo", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "StoreAreaCode", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "ShelfNumber", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "ShelfLevel", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "SequenceNo", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "CheckType", c => c.String(maxLength: 10));
            AlterColumn("dbo.InventoryCheck", "Remark", c => c.String(maxLength: 200));
            AlterColumn("dbo.InventoryCheck", "CheckUser", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheck", "PublishUser", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheckRecord", "CheckNo", c => c.String(maxLength: 32));
            AlterColumn("dbo.InventoryCheckRecord", "CurrentStoreHouseNo", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryCheckRecord", "CurrentStoreHouseNo", c => c.String());
            AlterColumn("dbo.InventoryCheckRecord", "CheckNo", c => c.String());
            AlterColumn("dbo.InventoryCheck", "PublishUser", c => c.String());
            AlterColumn("dbo.InventoryCheck", "CheckUser", c => c.String());
            AlterColumn("dbo.InventoryCheck", "Remark", c => c.String());
            AlterColumn("dbo.InventoryCheck", "CheckType", c => c.String());
            AlterColumn("dbo.InventoryCheck", "SequenceNo", c => c.String());
            AlterColumn("dbo.InventoryCheck", "ShelfLevel", c => c.String());
            AlterColumn("dbo.InventoryCheck", "ShelfNumber", c => c.String());
            AlterColumn("dbo.InventoryCheck", "StoreAreaCode", c => c.String());
            AlterColumn("dbo.InventoryCheck", "CheckNo", c => c.String());
            DropColumn("dbo.InventoryCheck", "ProductNo");
        }
    }
}

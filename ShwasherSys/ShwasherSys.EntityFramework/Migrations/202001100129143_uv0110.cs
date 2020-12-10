namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv0110 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CurrentRmStoreHouse", "ProductBatchNum", c => c.String(maxLength: 32));
            AddColumn("dbo.RmEnterStore", "ProductBatchNum", c => c.String(maxLength: 32));
            AddColumn("dbo.RmOutStore", "ProductBatchNum", c => c.String(maxLength: 32));
            //AddColumn("dbo.N_ViewCurrentRmStoreHouse", "ProductBatchNum", c => c.String());
            //AddColumn("dbo.N_ViewRmEnterStore", "ProductBatchNum", c => c.String());
            //AddColumn("dbo.N_ViewRmOutStore", "ProductBatchNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.N_ViewRmOutStore", "ProductBatchNum");
            DropColumn("dbo.N_ViewRmEnterStore", "ProductBatchNum");
            DropColumn("dbo.N_ViewCurrentRmStoreHouse", "ProductBatchNum");
            DropColumn("dbo.RmOutStore", "ProductBatchNum");
            DropColumn("dbo.RmEnterStore", "ProductBatchNum");
            DropColumn("dbo.CurrentRmStoreHouse", "ProductBatchNum");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0219 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "PartNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "PartNo");
        }
    }
}

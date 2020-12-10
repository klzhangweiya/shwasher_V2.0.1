namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228003 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DeviceMgInfo", newName: "DeviceMgPlanInfo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DeviceMgPlanInfo", newName: "DeviceMgInfo");
        }
    }
}

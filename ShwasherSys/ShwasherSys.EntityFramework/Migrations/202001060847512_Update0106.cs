namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0106 : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.ViewEmployeeInfo", newName: "NV_ViewEmployeeInfo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.NV_ViewEmployeeInfo", newName: "ViewEmployeeInfo");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceNo");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228004 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceMgPlanNo", c => c.String(maxLength: 50));
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceName", c => c.String(maxLength: 50));
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceMgNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceMgNo", c => c.String(maxLength: 50));
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceName");
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceMgPlanNo");
        }
    }
}

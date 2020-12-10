namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0115 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DeviceInfo", "NextMaintenanceDate", c => c.DateTime());
            AlterColumn("dbo.MaintenanceRecordInfo", "DeviceType", c => c.Int(nullable: false));
            AlterColumn("dbo.MoldInfo", "NextMaintenanceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MoldInfo", "NextMaintenanceDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MaintenanceRecordInfo", "DeviceType", c => c.String(maxLength: 50));
            AlterColumn("dbo.DeviceInfo", "NextMaintenanceDate", c => c.DateTime(nullable: false));
        }
    }
}

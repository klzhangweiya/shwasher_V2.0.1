namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228001 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DeviceInfo", newName: "DeviceMgInfo");
            AlterTableAnnotations(
                "dbo.DeviceMgInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 50),
                        DeviceNo = c.String(maxLength: 50),
                        MgType = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        ExpireDate = c.DateTime(nullable: false),
                        MaintenanceCycle = c.Int(nullable: false),
                        MaintenanceDate = c.DateTime(nullable: false),
                        NextMaintenanceDate = c.DateTime(),
                        Remark = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Device_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_DeviceMg_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.DeviceMgInfo", "DeviceNo", c => c.String(maxLength: 50));
            AddColumn("dbo.DeviceMgInfo", "MgType", c => c.Int(nullable: false));
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceMgNo", c => c.String(maxLength: 50));
            AddColumn("dbo.MaintenanceRecordInfo", "MgType", c => c.Int(nullable: false));
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceNo");
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceType");
            DropColumn("dbo.MoldInfo", "ExpireDate");
            DropColumn("dbo.MoldInfo", "MaintenanceCycle");
            DropColumn("dbo.MoldInfo", "MaintenanceDate");
            DropColumn("dbo.MoldInfo", "NextMaintenanceDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoldInfo", "NextMaintenanceDate", c => c.DateTime());
            AddColumn("dbo.MoldInfo", "MaintenanceDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.MoldInfo", "MaintenanceCycle", c => c.Int(nullable: false));
            AddColumn("dbo.MoldInfo", "ExpireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceType", c => c.Int(nullable: false));
            AddColumn("dbo.MaintenanceRecordInfo", "DeviceNo", c => c.String(maxLength: 50));
            DropColumn("dbo.MaintenanceRecordInfo", "MgType");
            DropColumn("dbo.MaintenanceRecordInfo", "DeviceMgNo");
            DropColumn("dbo.DeviceMgInfo", "MgType");
            DropColumn("dbo.DeviceMgInfo", "DeviceNo");
            AlterTableAnnotations(
                "dbo.DeviceMgInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 50),
                        DeviceNo = c.String(maxLength: 50),
                        MgType = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        ExpireDate = c.DateTime(nullable: false),
                        MaintenanceCycle = c.Int(nullable: false),
                        MaintenanceDate = c.DateTime(nullable: false),
                        NextMaintenanceDate = c.DateTime(),
                        Remark = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Device_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_DeviceMg_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            RenameTable(name: "dbo.DeviceMgInfo", newName: "DeviceInfo");
        }
    }
}

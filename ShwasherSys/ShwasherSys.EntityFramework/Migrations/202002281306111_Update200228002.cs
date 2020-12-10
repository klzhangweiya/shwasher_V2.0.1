namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228002 : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.DeviceMgInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 50),
                        DeviceNo = c.String(maxLength: 50),
                        PlanType = c.Int(nullable: false),
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
                        "DynamicFilter_DeviceMg_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_DeviceMgPlan_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.DeviceMgInfo", "PlanType", c => c.Int(nullable: false));
            DropColumn("dbo.DeviceMgInfo", "MgType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceMgInfo", "MgType", c => c.Int(nullable: false));
            DropColumn("dbo.DeviceMgInfo", "PlanType");
            AlterTableAnnotations(
                "dbo.DeviceMgInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 50),
                        DeviceNo = c.String(maxLength: 50),
                        PlanType = c.Int(nullable: false),
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
                        "DynamicFilter_DeviceMg_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_DeviceMgPlan_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}

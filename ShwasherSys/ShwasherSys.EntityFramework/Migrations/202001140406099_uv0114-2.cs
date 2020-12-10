namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv01142 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.DeviceInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            No = c.String(maxLength: 50),
            //            Name = c.String(maxLength: 50),
            //            Description = c.String(),
            //            ExpireDate = c.DateTime(nullable: false),
            //            MaintenanceCycle = c.Int(nullable: false),
            //            MaintenanceDate = c.DateTime(nullable: false),
            //            NextMaintenanceDate = c.DateTime(nullable: false),
            //            Remark = c.String(maxLength: 500),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_Device_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
            //    .Index(t => t.DeleterUserId)
            //    .Index(t => t.LastModifierUserId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.FixedAssetInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            No = c.String(maxLength: 50),
            //            Name = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 50),
            //            Description = c.String(maxLength: 500),
            //            Remark = c.String(maxLength: 500),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_FixedAsset_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
            //    .Index(t => t.DeleterUserId)
            //    .Index(t => t.LastModifierUserId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.LicenseDocumentInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            No = c.String(maxLength: 50),
            //            Name = c.String(maxLength: 50),
            //            Description = c.String(maxLength: 500),
            //            LicenseGroup = c.String(maxLength: 50),
            //            LicenseType = c.String(maxLength: 50),
            //            FilePath = c.String(maxLength: 300),
            //            ExpireDate = c.DateTime(nullable: false),
            //            Remark = c.String(maxLength: 500),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_LicenseDocument_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
            //    .Index(t => t.DeleterUserId)
            //    .Index(t => t.LastModifierUserId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.LicenseTypeInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 50),
            //            GroupName = c.String(maxLength: 50),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
            //    .Index(t => t.LastModifierUserId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.MaintenanceMemberInfo",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            MaintenanceNo = c.String(),
            //            EmployeeId = c.Int(nullable: false),
            //            Name = c.String(),
            //            StartDateTime = c.DateTime(),
            //            EndDateTime = c.DateTime(),
            //            WorkHour = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            WorkDesc = c.String(),
            //            Remark = c.String(maxLength: 500),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.EmployeeInfo", t => t.EmployeeId, cascadeDelete: true)
            //    .Index(t => t.EmployeeId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.MaintenanceRecordInfo",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            DeviceNo = c.String(maxLength: 50),
            //            DeviceType = c.String(maxLength: 50),
            //            Description = c.String(maxLength: 500),
            //            Address = c.String(maxLength: 200),
            //            PlanDate = c.DateTime(nullable: false),
            //            CompleteState = c.Int(nullable: false),
            //            CompleteDate = c.DateTime(),
            //            Remark = c.String(maxLength: 500),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .Index(t => t.CreatorUserId);
            
            //CreateTable(
            //    "dbo.MoldInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            No = c.String(maxLength: 50),
            //            Name = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 50),
            //            Material = c.String(maxLength: 50),
            //            Description = c.String(maxLength: 500),
            //            ExpireDate = c.DateTime(nullable: false),
            //            MaintenanceCycle = c.Int(nullable: false),
            //            MaintenanceDate = c.DateTime(nullable: false),
            //            NextMaintenanceDate = c.DateTime(nullable: false),
            //            Remark = c.String(maxLength: 500),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_Mold_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
            //    .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
            //    .Index(t => t.DeleterUserId)
            //    .Index(t => t.LastModifierUserId)
            //    .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MoldInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.MoldInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.MoldInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.MaintenanceRecordInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.MaintenanceMemberInfo", "EmployeeId", "dbo.EmployeeInfo");
            DropForeignKey("dbo.MaintenanceMemberInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.LicenseTypeInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.LicenseTypeInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.LicenseDocumentInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.LicenseDocumentInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.LicenseDocumentInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.DeviceInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.DeviceInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.DeviceInfo", "CreatorUserId", "dbo.Sys_Users");
            DropIndex("dbo.MoldInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.MoldInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.MoldInfo", new[] { "DeleterUserId" });
            DropIndex("dbo.MaintenanceRecordInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.MaintenanceMemberInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.MaintenanceMemberInfo", new[] { "EmployeeId" });
            DropIndex("dbo.LicenseTypeInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.LicenseTypeInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.LicenseDocumentInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.LicenseDocumentInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.LicenseDocumentInfo", new[] { "DeleterUserId" });
            DropIndex("dbo.FixedAssetInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.FixedAssetInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.FixedAssetInfo", new[] { "DeleterUserId" });
            DropIndex("dbo.DeviceInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.DeviceInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.DeviceInfo", new[] { "DeleterUserId" });
            DropTable("dbo.MoldInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Mold_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MaintenanceRecordInfo");
            DropTable("dbo.MaintenanceMemberInfo");
            DropTable("dbo.LicenseTypeInfo");
            DropTable("dbo.LicenseDocumentInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LicenseDocument_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FixedAssetInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FixedAsset_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.DeviceInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Device_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update0117 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FixedAssetTypeInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        Remark = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FixedAssetType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.QualityIssueLabelInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        Remark = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QualityIssueLabel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.ScrapTypeInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        Remark = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScrapType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScrapTypeInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.ScrapTypeInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.ScrapTypeInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.QualityIssueLabelInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.QualityIssueLabelInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.QualityIssueLabelInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetTypeInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetTypeInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.FixedAssetTypeInfo", "CreatorUserId", "dbo.Sys_Users");
            DropIndex("dbo.ScrapTypeInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.ScrapTypeInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.ScrapTypeInfo", new[] { "DeleterUserId" });
            DropIndex("dbo.QualityIssueLabelInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.QualityIssueLabelInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.QualityIssueLabelInfo", new[] { "DeleterUserId" });
            DropIndex("dbo.FixedAssetTypeInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.FixedAssetTypeInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.FixedAssetTypeInfo", new[] { "DeleterUserId" });
            DropTable("dbo.ScrapTypeInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScrapType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.QualityIssueLabelInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QualityIssueLabel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FixedAssetTypeInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FixedAssetType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

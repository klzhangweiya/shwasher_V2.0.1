namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class addProductProperty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyType = c.String(),
                        PropertyNo = c.String(),
                        PropertyValue = c.String(),
                        DisplayValue = c.String(),
                        ContentInfo = c.String(),
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
                    { "DynamicFilter_ProductProperty_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductProperties",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProductProperty_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

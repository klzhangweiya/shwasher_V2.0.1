namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv0215 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReturnGoodOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReturnOrderNo = c.String(maxLength: 32),
                        OrderNo = c.String(maxLength: 32),
                        ProductNo = c.String(maxLength: 32),
                        ProductionOrderNo = c.String(maxLength: 32),
                        Quantity = c.Decimal(precision: 18, scale: 2),
                        HandleUser = c.String(maxLength: 32),
                        ReturnDate = c.DateTime(),
                        ReturnState = c.Int(nullable: false),
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
                    { "DynamicFilter_ReturnGoodOrder_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ReturnOrderNo, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReturnGoodOrder", new[] { "ReturnOrderNo" });
            DropTable("dbo.ReturnGoodOrder",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ReturnGoodOrder_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

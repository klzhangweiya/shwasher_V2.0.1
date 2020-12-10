namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv01132 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryCheck",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CheckNo = c.String(),
                        StoreHouseTypeId = c.Int(),
                        StoreHouseId = c.Int(nullable: false),
                        StoreAreaCode = c.String(),
                        ShelfNumber = c.String(),
                        ShelfLevel = c.String(),
                        SequenceNo = c.String(),
                        CheckType = c.String(),
                        PlanStartDate = c.DateTime(),
                        PlanEndDate = c.DateTime(),
                        Remark = c.String(),
                        CheckUser = c.String(),
                        PublishUser = c.String(),
                        FinishDate = c.DateTime(),
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
                    { "DynamicFilter_InventoryCheck_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InventoryCheckRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CheckNo = c.String(),
                        CurrentStoreHouseNo = c.String(),
                        CheckQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StoreQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InventoryCheckRecord");
            DropTable("dbo.InventoryCheck",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_InventoryCheck_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

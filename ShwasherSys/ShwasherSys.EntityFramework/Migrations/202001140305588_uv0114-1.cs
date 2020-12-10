namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv01141 : DbMigration
    {
        public override void Up()
        {
            //AlterTableAnnotations(
            //    "dbo.InventoryCheck",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            CheckNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreAreaCode = c.String(),
            //            ShelfNumber = c.String(),
            //            ShelfLevel = c.String(),
            //            SequenceNo = c.String(),
            //            CheckType = c.String(),
            //            PlanStartDate = c.DateTime(),
            //            PlanEndDate = c.DateTime(),
            //            Remark = c.String(),
            //            CheckUser = c.String(),
            //            PublishUser = c.String(),
            //            FinishDate = c.DateTime(),
            //            CheckState = c.Int(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            DeleterUserId = c.Long(),
            //            DeletionTime = c.DateTime(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        },
            //    annotations: new Dictionary<string, AnnotationValues>
            //    {
            //        { 
            //            "DynamicFilter_InventoryCheck_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //        { 
            //            "DynamicFilter_InventoryCheckInfo_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //    });
            
            //AddColumn("dbo.InventoryCheck", "CheckState", c => c.Int(nullable: false));
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "PreMonthQuantity", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "InventoryCheckState", c => c.Int());
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "ReturnState", c => c.Int());
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "StoreAreaCode", c => c.String());
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "ShelfNumber", c => c.String());
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "ShelfLevel", c => c.String());
            //AddColumn("dbo.N_ViewCurrentProductStoreHouse", "SequenceNo", c => c.String());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "StoreLocationNo", c => c.String());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "PreMonthQuantity", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "InventoryCheckState", c => c.Int());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "ReturnState", c => c.Int());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "StoreAreaCode", c => c.String());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "ShelfNumber", c => c.String());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "ShelfLevel", c => c.String());
            //AddColumn("dbo.N_ViewCurrentSemiStoreHouse", "SequenceNo", c => c.String());
            //DropColumn("dbo.InventoryCheck", "StoreHouseTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InventoryCheck", "StoreHouseTypeId", c => c.Int());
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "SequenceNo");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "ShelfLevel");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "ShelfNumber");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "StoreAreaCode");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "ReturnState");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "InventoryCheckState");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "PreMonthQuantity");
            DropColumn("dbo.N_ViewCurrentSemiStoreHouse", "StoreLocationNo");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "SequenceNo");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "ShelfLevel");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "ShelfNumber");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "StoreAreaCode");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "ReturnState");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "InventoryCheckState");
            DropColumn("dbo.N_ViewCurrentProductStoreHouse", "PreMonthQuantity");
            DropColumn("dbo.InventoryCheck", "CheckState");
            AlterTableAnnotations(
                "dbo.InventoryCheck",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CheckNo = c.String(),
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
                        CheckState = c.Int(nullable: false),
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
                        "DynamicFilter_InventoryCheck_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_InventoryCheckInfo_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}

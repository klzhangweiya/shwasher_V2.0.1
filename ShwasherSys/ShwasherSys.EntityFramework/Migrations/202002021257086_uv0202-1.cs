namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv02021 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.N_ViewScrapEnterStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(nullable: false, maxLength: 11),
            //            ProductNo = c.String(maxLength: 50),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreLocationNo = c.String(maxLength: 32),
            //            ApplyStatus = c.Int(nullable: false),
            //            IsClose = c.Boolean(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            ApplyQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            AuditUser = c.String(maxLength: 32),
            //            AuditDate = c.DateTime(),
            //            ApplyEnterDate = c.DateTime(),
            //            EnterStoreUser = c.String(maxLength: 32),
            //            EnterStoreDate = c.DateTime(),
            //            Remark = c.String(maxLength: 150),
            //            ProductName = c.String(),
            //            Material = c.String(),
            //            Model = c.String(),
            //            ProductDesc = c.String(),
            //            StoreHouseName = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            ProductType = c.Int(),
            //            ScrapSource = c.Int(nullable: false),
            //            ScrapSourceNo = c.String(),
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
            //        { "DynamicFilter_ViewScrapEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ScrapEnterStore", "ScrapSource", c => c.Int(nullable: false));
            AddColumn("dbo.ScrapEnterStore", "ScrapSourceNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrapEnterStore", "ScrapSourceNo");
            DropColumn("dbo.ScrapEnterStore", "ScrapSource");
            DropTable("dbo.N_ViewScrapEnterStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ViewScrapEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

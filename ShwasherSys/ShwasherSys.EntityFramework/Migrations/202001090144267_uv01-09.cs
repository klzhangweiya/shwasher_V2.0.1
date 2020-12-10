namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv0109 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CurrentRmStoreHouse",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        StoreLocationNo = c.String(maxLength: 32),
                        RwProductNo = c.String(maxLength: 32),
                        FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Remark = c.String(maxLength: 150),
                        PreMonthQuantity = c.Decimal(precision: 18, scale: 3),
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
                    { "DynamicFilter_CurrentRwStoreHouse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RmEnterStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        RwProductNo = c.String(maxLength: 50),
                        StoreHouseId = c.Int(nullable: false),
                        StoreLocationNo = c.String(maxLength: 32),
                        ApplyStatus = c.Int(nullable: false),
                        IsClose = c.Boolean(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        AuditUser = c.String(maxLength: 32),
                        AuditDate = c.DateTime(),
                        ApplyEnterDate = c.DateTime(),
                        EnterStoreUser = c.String(maxLength: 32),
                        EnterStoreDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
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
                    { "DynamicFilter_RwEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RmOutStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(maxLength: 11),
                        CurrentRwStoreHouseNo = c.String(maxLength: 32),
                        RwProductNo = c.String(maxLength: 50),
                        StoreHouseId = c.Int(nullable: false),
                        ApplyStatus = c.Int(nullable: false),
                        IsClose = c.Boolean(nullable: false),
                        IsConfirm = c.Boolean(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        AuditUser = c.String(maxLength: 150),
                        AuditDate = c.DateTime(),
                        OutStoreUser = c.String(maxLength: 150),
                        OutStoreDate = c.DateTime(),
                        ApplyOutDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
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
                    { "DynamicFilter_RwOutStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RmProduct",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(maxLength: 50),
                        Material = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        ProductDesc = c.String(maxLength: 200),
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
                    { "DynamicFilter_RwProduct_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.OrderStickBills", "InvoiceState", c => c.Int());
            //AddColumn("dbo.StatementBill", "StatementState", c => c.Int());
            //AddColumn("dbo.N_ViewOrderSends", "StatementBillNo", c => c.String());
            //DropColumn("dbo.OrderStickBills", "State");
            //DropColumn("dbo.StatementBill", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StatementBill", "State", c => c.Int());
            AddColumn("dbo.OrderStickBills", "State", c => c.Int());
            DropColumn("dbo.N_ViewOrderSends", "StatementBillNo");
            DropColumn("dbo.StatementBill", "StatementState");
            DropColumn("dbo.OrderStickBills", "InvoiceState");
            DropTable("dbo.RmProduct",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RwProduct_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RmOutStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RwOutStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RmEnterStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RwEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CurrentRmStoreHouse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CurrentRwStoreHouse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

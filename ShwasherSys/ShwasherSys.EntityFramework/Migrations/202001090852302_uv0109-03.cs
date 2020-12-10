namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv010903 : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CurrentRwStoreHouse", newName: "CurrentRmStoreHouse");
            //RenameTable(name: "dbo.RwEnterStore", newName: "RmEnterStore");
            //RenameTable(name: "dbo.RwOutStore", newName: "RmOutStore");
            //RenameTable(name: "dbo.RwProduct", newName: "RmProduct");
            //CreateTable(
            //    "dbo.N_ViewCurrentRmStoreHouse",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(maxLength: 11),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreLocationNo = c.String(maxLength: 32),
            //            RmProductNo = c.String(maxLength: 32),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            Remark = c.String(maxLength: 150),
            //            PreMonthQuantity = c.Decimal(precision: 18, scale: 3),
            //            ProductName = c.String(),
            //            Material = c.String(),
            //            Model = c.String(),
            //            ProductDesc = c.String(),
            //            StoreHouseName = c.String(),
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
            //        { "DynamicFilter_ViewCurrentRmStoreHouse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewRmEnterStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(nullable: false, maxLength: 11),
            //            RmProductNo = c.String(maxLength: 50),
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
            //        { "DynamicFilter_ViewRmEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewRmOutStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(maxLength: 11),
            //            CurrentRmStoreHouseNo = c.String(maxLength: 32),
            //            RmProductNo = c.String(maxLength: 50),
            //            StoreHouseId = c.Int(nullable: false),
            //            ApplyStatus = c.Int(nullable: false),
            //            IsClose = c.Boolean(nullable: false),
            //            IsConfirm = c.Boolean(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            AuditUser = c.String(maxLength: 150),
            //            AuditDate = c.DateTime(),
            //            OutStoreUser = c.String(maxLength: 150),
            //            OutStoreDate = c.DateTime(),
            //            ApplyOutDate = c.DateTime(),
            //            Remark = c.String(maxLength: 150),
            //            ProductName = c.String(),
            //            Material = c.String(),
            //            Model = c.String(),
            //            ProductDesc = c.String(),
            //            StoreHouseName = c.String(),
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
            //        { "DynamicFilter_ViewRmOutStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    })
            //    .PrimaryKey(t => t.Id);
            
            //AlterTableAnnotations(
            //    "dbo.CurrentRmStoreHouse",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(maxLength: 11),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreLocationNo = c.String(maxLength: 32),
            //            RmProductNo = c.String(maxLength: 32),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            Remark = c.String(maxLength: 150),
            //            PreMonthQuantity = c.Decimal(precision: 18, scale: 3),
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
            //            "DynamicFilter_CurrentRmStoreHouse_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //        { 
            //            "DynamicFilter_CurrentRwStoreHouse_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //    });
            
            //AlterTableAnnotations(
            //    "dbo.RmEnterStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(nullable: false, maxLength: 11),
            //            RmProductNo = c.String(maxLength: 50),
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
            //            "DynamicFilter_RmEnterStore_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //        { 
            //            "DynamicFilter_RwEnterStore_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //    });
            
            //AlterTableAnnotations(
            //    "dbo.RmOutStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductionOrderNo = c.String(maxLength: 11),
            //            CurrentRmStoreHouseNo = c.String(maxLength: 32),
            //            RmProductNo = c.String(maxLength: 50),
            //            StoreHouseId = c.Int(nullable: false),
            //            ApplyStatus = c.Int(nullable: false),
            //            IsClose = c.Boolean(nullable: false),
            //            IsConfirm = c.Boolean(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            AuditUser = c.String(maxLength: 150),
            //            AuditDate = c.DateTime(),
            //            OutStoreUser = c.String(maxLength: 150),
            //            OutStoreDate = c.DateTime(),
            //            ApplyOutDate = c.DateTime(),
            //            Remark = c.String(maxLength: 150),
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
            //            "DynamicFilter_RmOutStore_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //        { 
            //            "DynamicFilter_RwOutStore_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //    });
            
            //AlterTableAnnotations(
            //    "dbo.RmProduct",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ProductName = c.String(maxLength: 50),
            //            Material = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 50),
            //            ProductDesc = c.String(maxLength: 200),
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
            //            "DynamicFilter_RmProduct_SoftDelete",
            //            new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
            //        },
            //        { 
            //            "DynamicFilter_RwProduct_SoftDelete",
            //            new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
            //        },
            //    });
            
            //AddColumn("dbo.CurrentRmStoreHouse", "RmProductNo", c => c.String(maxLength: 32));
            //AddColumn("dbo.RmEnterStore", "RmProductNo", c => c.String(maxLength: 50));
            //AddColumn("dbo.RmOutStore", "CurrentRmStoreHouseNo", c => c.String(maxLength: 32));
            //AddColumn("dbo.RmOutStore", "RmProductNo", c => c.String(maxLength: 50));
            //DropColumn("dbo.CurrentRmStoreHouse", "RwProductNo");
            //DropColumn("dbo.RmEnterStore", "RwProductNo");
            //DropColumn("dbo.RmOutStore", "CurrentRwStoreHouseNo");
            //DropColumn("dbo.RmOutStore", "RwProductNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RmOutStore", "RwProductNo", c => c.String(maxLength: 50));
            AddColumn("dbo.RmOutStore", "CurrentRwStoreHouseNo", c => c.String(maxLength: 32));
            AddColumn("dbo.RmEnterStore", "RwProductNo", c => c.String(maxLength: 50));
            AddColumn("dbo.CurrentRmStoreHouse", "RwProductNo", c => c.String(maxLength: 32));
            DropColumn("dbo.RmOutStore", "RmProductNo");
            DropColumn("dbo.RmOutStore", "CurrentRmStoreHouseNo");
            DropColumn("dbo.RmEnterStore", "RmProductNo");
            DropColumn("dbo.CurrentRmStoreHouse", "RmProductNo");
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_RmProduct_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_RwProduct_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.RmOutStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(maxLength: 11),
                        CurrentRmStoreHouseNo = c.String(maxLength: 32),
                        RmProductNo = c.String(maxLength: 50),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_RmOutStore_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_RwOutStore_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.RmEnterStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        RmProductNo = c.String(maxLength: 50),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_RmEnterStore_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_RwEnterStore_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.CurrentRmStoreHouse",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        StoreLocationNo = c.String(maxLength: 32),
                        RmProductNo = c.String(maxLength: 32),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_CurrentRmStoreHouse_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_CurrentRwStoreHouse_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            DropTable("dbo.N_ViewRmOutStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ViewRmOutStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.N_ViewRmEnterStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ViewRmEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.N_ViewCurrentRmStoreHouse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ViewCurrentRmStoreHouse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameTable(name: "dbo.RmProduct", newName: "RwProduct");
            RenameTable(name: "dbo.RmOutStore", newName: "RwOutStore");
            RenameTable(name: "dbo.RmEnterStore", newName: "RwEnterStore");
            RenameTable(name: "dbo.CurrentRmStoreHouse", newName: "CurrentRwStoreHouse");
        }
    }
}

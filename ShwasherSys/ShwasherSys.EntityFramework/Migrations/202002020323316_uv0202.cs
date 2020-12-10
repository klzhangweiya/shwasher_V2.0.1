namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class uv0202 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScrapEnterStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductionOrderNo = c.String(maxLength: 11),
                        ProductNo = c.String(maxLength: 50),
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
                        ProductType = c.Int(nullable: false),
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
                    { "DynamicFilter_ScrapEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_SemiProductStoreInfo",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            AllQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AllFreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AllPreMonthQuantity = c.Decimal(precision: 18, scale: 2),
            //            SemiProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            PartNo = c.String(),
            //            ProductDesc = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_EnterOutSemiProductStore",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ActualId = c.Int(nullable: false),
            //            EnterOutFlag = c.Int(nullable: false),
            //            SemiProductNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateTiem = c.DateTime(),
            //            UserIDLastMod = c.String(),
            //            Remark = c.String(),
            //            SemiProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            PartNo = c.String(),
            //            ProductDesc = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.OrderSend", "CreatorUserId", c => c.String(maxLength: 20));
            //AddColumn("dbo.v_OrderSendBill", "CreatorUserId", c => c.String());
            //AlterColumn("dbo.OrderSendBills", "ExpressBillNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderSendBills", "ExpressBillNo", c => c.String());
            DropColumn("dbo.v_OrderSendBill", "CreatorUserId");
            DropColumn("dbo.OrderSend", "CreatorUserId");
            DropTable("dbo.v_EnterOutSemiProductStore");
            DropTable("dbo.v_SemiProductStoreInfo");
            DropTable("dbo.ScrapEnterStore",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScrapEnterStore_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sys_AppGuids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Step = c.Short(nullable: false),
                        LastId = c.Int(nullable: false),
                        IdType = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_AuditLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        UserName = c.String(),
                        ServiceName = c.String(maxLength: 256),
                        MethodName = c.String(maxLength: 256),
                        Parameters = c.String(maxLength: 1024),
                        ExecutionTime = c.DateTime(nullable: false),
                        ExecutionDuration = c.Int(nullable: false),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        BrowserInfo = c.String(maxLength: 512),
                        Exception = c.String(maxLength: 2000),
                        ImpersonatorUserId = c.Long(),
                        ImpersonatorTenantId = c.Int(),
                        CustomData = c.String(maxLength: 2000),
                        LogType = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BackUpCurrentSemiStoreHouse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentSemiStoreHouseNo = c.String(nullable: false, maxLength: 32),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        SemiProductNo = c.String(maxLength: 32),
                        FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyEnterDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BulletinInfo",
                c => new
                    {
                        BulletinInfoId = c.Int(nullable: false, identity: true),
                        BulletinType = c.String(nullable: false, maxLength: 1),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        Promulgator = c.String(maxLength: 50),
                        PromulgatTime = c.DateTime(storeType: "smalldatetime"),
                        ExpirationDate = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.BulletinInfoId);
            
            CreateTable(
                "dbo.BusinessLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogType = c.Int(nullable: false),
                        LogCommand = c.String(),
                        LogMessage = c.String(),
                        LogErrorMessage = c.String(),
                        LogDate = c.DateTime(),
                        UserHostAddress = c.String(),
                        Extend1Log = c.String(),
                        Extend2Log = c.String(),
                        Extend3Log = c.String(),
                        Extend4Log = c.String(),
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
                    { "DynamicFilter_BusinessLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CurrencyName = c.String(maxLength: 20),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CurrencyExchangeRate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromCurrencyId = c.String(maxLength: 20),
                        ToCurrencyId = c.String(maxLength: 20),
                        ExchangeRate = c.Decimal(precision: 18, scale: 4),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CurrentProductStoreHouse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentProductStoreHouseNo = c.String(nullable: false, maxLength: 32),
                        ProductionOrderNo = c.String(maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        StoreLocationNo = c.String(maxLength: 32),
                        ProductNo = c.String(maxLength: 32),
                        FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PreMonthQuantity = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CurrentSemiStoreHouse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentSemiStoreHouseNo = c.String(nullable: false, maxLength: 32),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        SemiProductNo = c.String(maxLength: 32),
                        FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyEnterDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerDefaultProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(maxLength: 30),
                        ProductNo = c.String(maxLength: 30),
                        CustomerProductName = c.String(maxLength: 30),
                        Sequence = c.Int(nullable: false),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        CustomerName = c.String(nullable: false, maxLength: 50),
                        LinkMan = c.String(maxLength: 20),
                        Address = c.String(maxLength: 150),
                        WebSite = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                        Telephone = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        zip = c.String(maxLength: 6),
                        Email = c.String(maxLength: 200),
                        SaleType = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerSend",
                c => new
                    {
                        CustomerSendId = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        CustomerSendName = c.String(nullable: false, maxLength: 150),
                        SendAdress = c.String(nullable: false, maxLength: 250),
                        LinkMan = c.String(maxLength: 30),
                        Telephone = c.String(maxLength: 50),
                        Zip = c.String(maxLength: 8),
                        Email = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.CustomerSendId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.String(nullable: false, maxLength: 128),
                        DepartmentName = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                        OrderStatusList = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Dutys",
                c => new
                    {
                        DutyID = c.Int(nullable: false, identity: true),
                        DutyName = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.DutyID);
            
            CreateTable(
                "dbo.EmployeeInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 20),
                        Name = c.String(maxLength: 50),
                        DepartmentNo = c.String(maxLength: 20),
                        DutyNo = c.String(maxLength: 20),
                        Description = c.String(maxLength: 500),
                        UserName = c.String(maxLength: 32),
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
                    { "DynamicFilter_Employee_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Sys_Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FactoryID = c.String(maxLength: 20),
                        DepartmentID = c.String(maxLength: 20),
                        DutyID = c.Int(),
                        AccountNo = c.String(maxLength: 20),
                        Birthday = c.DateTime(),
                        Address = c.String(maxLength: 200),
                        ZIP = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 500),
                        UserName = c.String(nullable: false, maxLength: 32),
                        UserType = c.Int(nullable: false),
                        AuthenticationSource = c.String(maxLength: 64),
                        EmailAddress = c.String(nullable: false, maxLength: 256),
                        RealName = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 128),
                        EmailConfirmationCode = c.String(maxLength: 328),
                        PasswordResetCode = c.String(maxLength: 328),
                        LockoutEndDateUtc = c.DateTime(),
                        AccessFailedCount = c.Int(nullable: false),
                        IsLockoutEnabled = c.Boolean(nullable: false),
                        PhoneNumber = c.String(maxLength: 32),
                        IsPhoneNumberConfirmed = c.Boolean(nullable: false),
                        SecurityStamp = c.String(maxLength: 128),
                        IsTwoFactorEnabled = c.Boolean(nullable: false),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastLoginTime = c.DateTime(),
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
                    { "DynamicFilter_SysUser_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.Sys_UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Factories",
                c => new
                    {
                        FactoryID = c.String(nullable: false, maxLength: 128),
                        FactoryName = c.String(nullable: false, maxLength: 100),
                        ShortNames = c.String(nullable: false, maxLength: 20),
                        RegionID = c.String(nullable: false, maxLength: 50),
                        FactoryURL = c.String(maxLength: 80),
                        Address = c.String(maxLength: 100),
                        ZIP = c.String(maxLength: 50),
                        LinkMan = c.String(maxLength: 50),
                        Telephone = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.FactoryID);
            
            CreateTable(
                "dbo.FinshedEnterStore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        PackageApplyNo = c.String(nullable: false, maxLength: 32),
                        PackageProductNo = c.String(maxLength: 50),
                        ProductNo = c.String(maxLength: 50),
                        StoreHouseId = c.Int(nullable: false),
                        SourceStoreHouseId = c.Int(nullable: false),
                        StoreLocationNo = c.String(maxLength: 32),
                        ApplyStatus = c.Int(nullable: false),
                        IsClose = c.Boolean(nullable: false),
                        ApplySourceType = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PackageSpecification = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PackageCount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualPackageCount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        AuditUser = c.String(maxLength: 20),
                        AuditDate = c.DateTime(),
                        ApplyEnterDate = c.DateTime(),
                        EnterStoreUser = c.String(maxLength: 20),
                        EnterStoreDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreateSourceType = c.Int(),
                        PackageEnterNum = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FunctionNo = c.String(maxLength: 100),
                        ParentNo = c.String(maxLength: 100),
                        FunctionName = c.String(maxLength: 100),
                        PermissionName = c.String(maxLength: 500),
                        FunctionType = c.Int(nullable: false),
                        FunctionPath = c.String(maxLength: 500),
                        Action = c.String(maxLength: 50),
                        Controller = c.String(maxLength: 50),
                        Url = c.String(),
                        Icon = c.String(maxLength: 20),
                        Class = c.String(maxLength: 100),
                        Script = c.String(),
                        Sort = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        IsLeaf = c.Boolean(),
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
                    { "DynamicFilter_SysFunction_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.OrderHeader",
                c => new
                    {
                        OrderNo = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        LinkName = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Fax = c.String(maxLength: 50),
                        Telephone = c.String(maxLength: 50),
                        CustomerSendId = c.Int(nullable: false),
                        StockNo = c.String(maxLength: 50),
                        OrderStatusId = c.Int(nullable: false),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 30),
                        SaleType = c.Int(),
                    })
                .PrimaryKey(t => t.OrderNo);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(nullable: false, maxLength: 30),
                        ProductNo = c.String(nullable: false, maxLength: 30),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        AfterTaxPrice = c.Decimal(nullable: false, storeType: "money"),
                        CurrencyId = c.String(nullable: false, maxLength: 10),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3, storeType: "numeric"),
                        OrderUnitId = c.Int(nullable: false),
                        SendDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        IsReport = c.String(nullable: false, maxLength: 1),
                        IsPartSend = c.String(nullable: false, maxLength: 1),
                        OrderItemStatusId = c.Int(),
                        WareHouse = c.String(maxLength: 50),
                        OrderItemDesc = c.String(maxLength: 500),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        PartNo = c.String(maxLength: 30),
                        ToCnyRate = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.OrderItemId);
            
            CreateTable(
                "dbo.OrderSendBills",
                c => new
                    {
                        OrderSendBillNo = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        SendDate = c.DateTime(storeType: "smalldatetime"),
                        SendAddress = c.String(maxLength: 250),
                        ContactTels = c.String(maxLength: 50),
                        ContactMan = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsDoBill = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.OrderSendBillNo);
            
            CreateTable(
                "dbo.OrderSend",
                c => new
                    {
                        OrderSendId = c.Int(nullable: false, identity: true),
                        OrderItemId = c.Int(nullable: false),
                        SendDate = c.DateTime(storeType: "smalldatetime"),
                        SendQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        OrderUnitId = c.Int(),
                        Remark = c.String(maxLength: 250),
                        OrderSendBillNo = c.String(maxLength: 20),
                        OrderStickBillNo = c.String(maxLength: 40),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        QuantityPerPack = c.Decimal(precision: 18, scale: 3),
                        PackageCount = c.Decimal(precision: 18, scale: 3),
                        ProductBatchNum = c.String(),
                        StoreLocationNo = c.String(maxLength: 32),
                        CurrentProductStoreHouseNo = c.String(),
                    })
                .PrimaryKey(t => t.OrderSendId);
            
            CreateTable(
                "dbo.OrderStickBills",
                c => new
                    {
                        OrderStickBillNo = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        CreatDate = c.DateTime(storeType: "smalldatetime"),
                        StickNum = c.String(maxLength: 30),
                        StickMan = c.String(maxLength: 20),
                        Description = c.String(maxLength: 4000),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.OrderStickBillNo);
            
            CreateTable(
                "dbo.OrderUnit",
                c => new
                    {
                        OrderUnitId = c.Int(nullable: false, identity: true),
                        OrderUnitName = c.String(maxLength: 50),
                        ProductNum = c.Int(),
                        OrderUnitDesc = c.String(maxLength: 250),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(),
                    })
                .PrimaryKey(t => t.OrderUnitId);
            
            CreateTable(
                "dbo.OutFactory",
                c => new
                    {
                        OutFactoryId = c.String(nullable: false, maxLength: 128),
                        OutFactoryName = c.String(nullable: false, maxLength: 50),
                        LinkMan = c.String(maxLength: 20),
                        Address = c.String(maxLength: 150),
                        WebSite = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                        Telephone = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        zip = c.String(maxLength: 6),
                        Email = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.OutFactoryId);
            
            CreateTable(
                "dbo.PackageApply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageApplyNo = c.String(nullable: false, maxLength: 32),
                        CurrentStoreHouseNo = c.String(maxLength: 32),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        SemiProductNo = c.String(maxLength: 32),
                        ProductNo = c.String(maxLength: 32),
                        ApplyQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SourceStore = c.Int(nullable: false),
                        ApplyStatus = c.String(nullable: false, maxLength: 1),
                        IsClose = c.Boolean(nullable: false),
                        ApplyDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PermissionNo = c.String(maxLength: 32),
                        PermissionName = c.String(maxLength: 500),
                        Master = c.Int(),
                        MasterValue = c.String(),
                        Access = c.Int(),
                        AccessValue = c.String(),
                        IsGranted = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductInspectInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductInspectNo = c.String(nullable: false, maxLength: 32),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        SemiProductNo = c.String(maxLength: 32),
                        InspectStatus = c.Int(nullable: false),
                        InspectResult = c.Int(nullable: false),
                        InspectDate = c.DateTime(),
                        InspectMember = c.String(maxLength: 100),
                        IsLock = c.String(maxLength: 1),
                        InspectContent = c.String(maxLength: 1000),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductionOrderLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        OperatorTitle = c.String(maxLength: 150),
                        OperatorConent = c.String(),
                        TimeCreated = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductionOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        ProductionOrderStatus = c.Int(nullable: false),
                        StoveNo = c.String(maxLength: 20),
                        CarNo = c.String(maxLength: 20),
                        SemiProductNo = c.String(maxLength: 32),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        RawMaterials = c.String(maxLength: 100),
                        Material = c.String(maxLength: 50),
                        PartNo = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        SurfaceColor = c.String(maxLength: 50),
                        Rigidity = c.String(maxLength: 50),
                        Size = c.String(maxLength: 50),
                        ProcessingType = c.String(maxLength: 1),
                        ProcessingLevel = c.String(maxLength: 1),
                        Remark = c.String(maxLength: 150),
                        IsChecked = c.Int(),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(maxLength: 1),
                        SourceProductionOrderNo = c.String(maxLength: 11),
                        PlanProduceDate = c.DateTime(),
                        EnterQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ProductionType = c.String(maxLength: 1),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        OutsourcingFactory = c.String(),
                        EnterDate = c.DateTime(),
                        InspectDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductOutStore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(maxLength: 11),
                        CurrentProductStoreHouseNo = c.String(maxLength: 32),
                        ProductNo = c.String(maxLength: 50),
                        StoreHouseId = c.Int(nullable: false),
                        ApplyStatus = c.Int(nullable: false),
                        IsClose = c.Boolean(nullable: false),
                        IsConfirm = c.Boolean(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        AuditUser = c.String(maxLength: 20),
                        AuditDate = c.DateTime(),
                        OutStoreUser = c.String(maxLength: 20),
                        OutStoreDate = c.DateTime(),
                        ApplyOutDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        OrderSendId = c.Int(nullable: false),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyOutStoreSourceType = c.Int(nullable: false),
                        CreateSourceType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductNo = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        Model = c.String(maxLength: 50),
                        StandardId = c.Int(),
                        Material = c.String(maxLength: 50),
                        ProductDesc = c.String(maxLength: 200),
                        SurfaceColor = c.String(maxLength: 50),
                        Rigidity = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        Sequence = c.Int(nullable: false),
                        IsStandard = c.String(nullable: false, maxLength: 1),
                        IsLock = c.String(nullable: false, maxLength: 1),
                        Defprice = c.Decimal(storeType: "money"),
                        TranUnitValue = c.Decimal(precision: 18, scale: 3),
                        PartNo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductNo);
            
            CreateTable(
                "dbo.ProductInspectReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PtoductInspectNo = c.String(maxLength: 32),
                        ReportContent = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        RegionID = c.String(nullable: false, maxLength: 128),
                        RegionName = c.String(nullable: false, maxLength: 50),
                        FatherRegionID = c.String(nullable: false, maxLength: 50),
                        URL = c.String(maxLength: 80),
                        Depth = c.Int(nullable: false),
                        IsLeaf = c.String(nullable: false, maxLength: 1),
                        Sort = c.Int(nullable: false),
                        Path = c.String(nullable: false, maxLength: 220),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsLock = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.RegionID);
            
            CreateTable(
                "dbo.Sys_Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                        RoleDisplayName = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 1000),
                        RoleType = c.Int(nullable: false),
                        IsStatic = c.Boolean(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SysRole_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.SemiEnterStore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        StoreHouseId = c.Int(nullable: false),
                        ApplyStatus = c.String(nullable: false, maxLength: 1),
                        IsClose = c.Boolean(nullable: false),
                        ApplySource = c.String(nullable: false, maxLength: 1),
                        SemiProductNo = c.String(maxLength: 32),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        AuditUser = c.String(maxLength: 20),
                        AuditDate = c.DateTime(),
                        EnterStoreUser = c.String(maxLength: 20),
                        EnterStoreDate = c.DateTime(),
                        ApplyEnterDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreateSourceType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SemiOutStores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionOrderNo = c.String(nullable: false, maxLength: 11),
                        CurrentSemiStoreHouseNo = c.String(maxLength: 32),
                        StoreHouseId = c.Int(nullable: false),
                        ApplyStatus = c.String(nullable: false, maxLength: 1),
                        ApplyTypes = c.Int(nullable: false),
                        IsClose = c.Boolean(nullable: false),
                        ApplyOutStoreSource = c.String(nullable: false, maxLength: 1),
                        IsConfirm = c.Boolean(),
                        SemiProductNo = c.String(maxLength: 32),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ApplyOutDate = c.DateTime(),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                        AuditUser = c.String(maxLength: 20),
                        AuditDate = c.DateTime(),
                        OutStoreUser = c.String(maxLength: 20),
                        OutStoreDate = c.DateTime(),
                        KgWeight = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreateSourceType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SemiProducts",
                c => new
                    {
                        SemiProductNo = c.String(nullable: false, maxLength: 128),
                        SemiProductName = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        Material = c.String(maxLength: 50),
                        ProductDesc = c.String(maxLength: 200),
                        SurfaceColor = c.String(maxLength: 50),
                        Rigidity = c.String(maxLength: 50),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        IsStandard = c.String(nullable: false, maxLength: 1),
                        Sequence = c.Int(nullable: false),
                        IsLock = c.String(nullable: false, maxLength: 1),
                        PartNo = c.String(maxLength: 50),
                        TranUnitValue = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.SemiProductNo);
            
            CreateTable(
                "dbo.SemiProductStore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreHouseId = c.Int(nullable: false),
                        SemiProductNo = c.String(maxLength: 32),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        StoreYear = c.String(maxLength: 5),
                        StoreMonth = c.String(maxLength: 4),
                        Active = c.String(nullable: false, maxLength: 1),
                        Remark = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(),
                        TimeLastMod = c.DateTime(),
                        CreatorUserId = c.String(maxLength: 20),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingNo = c.String(maxLength: 32),
                        SettingName = c.String(maxLength: 50),
                        SettingType = c.Int(nullable: false),
                        Code = c.String(maxLength: 100),
                        Value = c.String(maxLength: 500),
                        Description = c.String(maxLength: 1000),
                        Remark = c.String(maxLength: 1000),
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
                    { "DynamicFilter_SysSetting_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.ShortMessage",
                c => new
                    {
                        MsgID = c.Int(nullable: false, identity: true),
                        SendUserID = c.String(nullable: false, maxLength: 20),
                        Title = c.String(maxLength: 100),
                        Content = c.String(maxLength: 3000),
                        SendTime = c.DateTime(storeType: "smalldatetime"),
                        IsDelete = c.String(nullable: false, maxLength: 1),
                        RecieveUserIds = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.MsgID);
            
            CreateTable(
                "dbo.ShortMsgDetail",
                c => new
                    {
                        DetailID = c.Int(nullable: false, identity: true),
                        MsgID = c.Int(nullable: false),
                        RecvUserID = c.String(nullable: false, maxLength: 20),
                        IsRead = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.DetailID);
            
            CreateTable(
                "dbo.Standard",
                c => new
                    {
                        StandardId = c.Int(nullable: false, identity: true),
                        StandardName = c.String(nullable: false, maxLength: 50),
                        StandardDesc = c.String(maxLength: 150),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.StandardId);
            
            CreateTable(
                "dbo.StoreHouseLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreLocationNo = c.String(maxLength: 32),
                        StoreAreaCode = c.String(maxLength: 32),
                        ShelfNumber = c.String(maxLength: 50),
                        ShelfLevel = c.String(maxLength: 10),
                        SequenceNo = c.String(maxLength: 10),
                        Remark = c.String(maxLength: 250),
                        StoreHouseId = c.Int(),
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
                    { "DynamicFilter_StoreHouseLocation_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreHouse",
                c => new
                    {
                        StoreHouseID = c.Int(nullable: false, identity: true),
                        StoreHouseName = c.String(nullable: false, maxLength: 50),
                        StoreHouseTypeId = c.Int(),
                        Address = c.String(maxLength: 250),
                        Tel = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        ContactMan = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 500),
                        IsLock = c.String(maxLength: 1),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                        StoreHouseNo = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.StoreHouseID);
            
            CreateTable(
                "dbo.Sys_AttachFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttachNo = c.String(maxLength: 32),
                        TableName = c.String(maxLength: 50),
                        ColumnName = c.String(maxLength: 50),
                        SourceKey = c.String(maxLength: 50),
                        FileTitle = c.String(maxLength: 50),
                        FileName = c.String(maxLength: 50),
                        FilePath = c.String(maxLength: 500),
                        FileType = c.String(maxLength: 20),
                        FileExt = c.String(maxLength: 10),
                        Description = c.String(maxLength: 500),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_Helps",
                c => new
                    {
                        HelpId = c.Int(nullable: false, identity: true),
                        Classification = c.String(maxLength: 40),
                        HelpTitle = c.String(nullable: false, maxLength: 20),
                        HelpKeyWords = c.String(maxLength: 20),
                        HelpContent = c.String(storeType: "ntext"),
                        Sequence = c.Int(nullable: false),
                        TimeCreated = c.DateTime(storeType: "smalldatetime"),
                        TimeLastMod = c.DateTime(storeType: "smalldatetime"),
                        UserIDLastMod = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.HelpId);
            
            CreateTable(
                "dbo.Sys_States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateNo = c.String(maxLength: 32),
                        StateName = c.String(maxLength: 50),
                        TableName = c.String(nullable: false, maxLength: 50),
                        ColumnName = c.String(nullable: false, maxLength: 50),
                        CodeValue = c.String(nullable: false, maxLength: 100),
                        DisplayValue = c.String(nullable: false, maxLength: 100),
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
                    { "DynamicFilter_SysState_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sys_Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Sys_Users", t => t.DeleterUserId)
                .ForeignKey("dbo.Sys_Users", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.TemplateInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateNo = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 500),
                        Content = c.String(),
                        Type = c.Int(nullable: false),
                        TempKey = c.String(maxLength: 50),
                        FilePath = c.String(maxLength: 300),
                        FileExt = c.String(maxLength: 50),
                        ClassPath = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_UserLoginLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        UserNameOrEmailAddress = c.String(maxLength: 255),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        BrowserInfo = c.String(maxLength: 512),
                        Result = c.Byte(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sys_UserLogins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
     Sql(@"


/****** Object:  View [dbo].[N_ViewSemiOutStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewSemiOutStore]
AS
SELECT     so.Id, so.ProductionOrderNo, so.CurrentSemiStoreHouseNo, so.ApplyStatus, so.StoreHouseId, so.SemiProductNo, so.Quantity, so.ActualQuantity, 
                      so.ApplyOutStoreSource, so.Remark, so.TimeCreated, so.TimeLastMod, so.CreatorUserId, so.ApplyOutDate, so.UserIDLastMod, so.AuditUser, so.AuditDate, 
                      sp.SemiProductName, sp.Model, sp.Material, sp.SurfaceColor, sp.Rigidity, sp.IsStandard, so.IsConfirm, so.IsClose, so.ApplyTypes, sp.PartNo, sp.TranUnitValue, 
                      so.KgWeight, so.CreateSourceType
FROM         dbo.SemiOutStores AS so LEFT OUTER JOIN
                      dbo.SemiProducts AS sp ON so.SemiProductNo = sp.SemiProductNo
GO

/****** Object:  View [dbo].[N_ViewSemiEnterStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewSemiEnterStore]
AS
SELECT     se.Id, se.ProductionOrderNo, se.StoreHouseId, se.ApplyStatus, se.ApplySource, se.SemiProductNo, se.Quantity, se.ActualQuantity, se.ApplyEnterDate, se.Remark, 
                      se.TimeCreated, se.TimeLastMod, se.CreatorUserId, se.UserIDLastMod, se.AuditUser, se.AuditDate, sp.SemiProductName, sp.Model, sp.Material, sp.SurfaceColor, 
                      sp.Rigidity, sp.IsStandard, se.IsClose, sp.PartNo, sp.TranUnitValue, se.KgWeight, se.CreateSourceType
FROM         dbo.SemiEnterStore AS se LEFT OUTER JOIN
                      dbo.SemiProducts AS sp ON se.SemiProductNo = sp.SemiProductNo
GO

/****** Object:  View [dbo].[N_ViewProductOutStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewProductOutStore]
AS
SELECT     po.ProductionOrderNo, po.Id, po.CurrentProductStoreHouseNo, po.ProductNo, po.StoreHouseId, po.ApplyStatus, po.IsClose, po.Quantity, po.ActualQuantity, 
                      po.AuditUser, po.AuditDate, po.Remark, po.IsConfirm, po.TimeCreated, po.TimeLastMod, po.CreatorUserId, po.UserIDLastMod, po.ApplyOutDate, p.ProductName, 
                      p.Model, p.Material, p.SurfaceColor, p.Rigidity, p.IsStandard, dbo.OrderHeader.CustomerId, dbo.Customers.CustomerName, dbo.OrderSend.OrderSendBillNo, 
                      po.KgWeight, po.ApplyOutStoreSourceType, po.CreateSourceType
FROM         dbo.Customers RIGHT OUTER JOIN
                      dbo.OrderHeader ON dbo.Customers.CustomerId = dbo.OrderHeader.CustomerId RIGHT OUTER JOIN
                      dbo.OrderItems ON dbo.OrderHeader.OrderNo = dbo.OrderItems.OrderNo RIGHT OUTER JOIN
                      dbo.OrderSend ON dbo.OrderItems.OrderItemId = dbo.OrderSend.OrderItemId RIGHT OUTER JOIN
                      dbo.ProductOutStore AS po ON dbo.OrderSend.OrderSendId = po.OrderSendId LEFT OUTER JOIN
                      dbo.Products AS p ON po.ProductNo = p.ProductNo
GO

/****** Object:  View [dbo].[N_ViewProductEnterStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewProductEnterStore]
AS
SELECT     e.Id, e.ProductionOrderNo, e.PackageApplyNo, e.PackageProductNo, e.ProductNo, e.StoreLocationNo, e.UserIDLastMod, e.CreatorUserId, e.TimeLastMod, 
                      e.TimeCreated, e.Remark, e.ApplyEnterDate, e.AuditDate, e.AuditUser, e.ActualPackageCount, e.PackageCount, e.PackageSpecification, e.Quantity, e.ApplyStatus, 
                      e.IsClose, e.StoreHouseId, p.ProductName, p.Model, p.Material, p.SurfaceColor, p.Rigidity, p.IsStandard, e.KgWeight, e.ApplySourceType, e.SourceStoreHouseId, 
                      e.CreateSourceType, e.PackageEnterNum
FROM         dbo.FinshedEnterStore AS e LEFT OUTER JOIN
                      dbo.Products AS p ON e.ProductNo = p.ProductNo
GO

/****** Object:  View [dbo].[N_ViewPackageApply]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewPackageApply]
AS
SELECT     Id, PackageApplyNo, CurrentStoreHouseNo, ProductionOrderNo, SemiProductNo, ApplyQuantity, ActualQuantity, ApplyStatus, IsClose, ApplyDate, Remark, 
                      TimeCreated, TimeLastMod, CreatorUserId, UserIDLastMod, ProductNo, SourceStore,
                          (SELECT     COUNT(*) AS Expr1
                            FROM          dbo.FinshedEnterStore
                            WHERE      (ApplyStatus = 2 OR
                                                   ApplyStatus = 4) AND (PackageApplyNo = p.PackageApplyNo)) AS ProcessingNum,
                          (SELECT     SUM(ApplyQuantity) AS Expr1
                            FROM          dbo.FinshedEnterStore AS fe
                            WHERE      (PackageApplyNo = p.PackageApplyNo)) AS IsApplyEnterQuantity
FROM         dbo.PackageApply AS p
GO

/****** Object:  View [dbo].[N_ViewOrderSends]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewOrderSends]
AS
SELECT     s.OrderItemId, s.SendDate, s.SendQuantity, s.OrderSendBillNo, s.QuantityPerPack, s.PackageCount, s.ProductBatchNum, i.OrderNo, i.ProductNo, p.ProductName, 
                      p.Model, p.StandardId, p.Material, p.SurfaceColor, p.Rigidity, p.ProductDesc, i.IsReport, i.PartNo, h.LinkName, h.OrderDate, h.CustomerSendId, h.CustomerId, 
                      s.Remark, h.StockNo, s.OrderSendId, s.UserIDLastMod, i.Price, CONVERT(money, i.Price * s.SendQuantity) AS TotalPrice, s.OrderStickBillNo, u.OrderUnitName, 
                      h.SaleType, i.AfterTaxPrice, c.CustomerName
FROM         dbo.Customers AS c RIGHT OUTER JOIN
                      dbo.OrderHeader AS h ON c.CustomerId = h.CustomerId RIGHT OUTER JOIN
                      dbo.OrderSend AS s LEFT OUTER JOIN
                      dbo.OrderItems AS i LEFT OUTER JOIN
                      dbo.OrderUnit AS u ON i.OrderUnitId = u.OrderUnitId LEFT OUTER JOIN
                      dbo.Products AS p ON i.ProductNo = p.ProductNo ON s.OrderItemId = i.OrderItemId ON h.OrderNo = i.OrderNo
GO

/****** Object:  View [dbo].[N_ViewOrderItems_Old]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewOrderItems_Old]
AS
SELECT     i.OrderItemId, i.OrderNo, i.ProductNo, i.Price, i.CurrencyId, i.Quantity, i.OrderUnitId, i.SendDate, i.IsReport, i.IsPartSend, i.OrderItemStatusId, i.WareHouse, 
                      i.OrderItemDesc, i.PartNo, CONVERT(money, i.Price * i.Quantity) AS TotalPrice, h.CustomerId, h.LinkName, h.OrderDate, h.OrderStatusId, h.Fax, h.Telephone, 
                      p.ProductName, p.Model, p.StandardId, p.Material, p.SurfaceColor, p.Rigidity, p.IsStandard,
                          (SELECT     SUM(SendQuantity) AS a
                            FROM          dbo.OrderSend
                            WHERE      (OrderItemId = i.OrderItemId)) AS IsSendQuantity, u.OrderUnitName, h.StockNo, c.CustomerName, i.AfterTaxPrice, CONVERT(money, 
                      i.AfterTaxPrice * i.Quantity) AS AfterTaxTotalPrice, h.SaleType
FROM         dbo.Customers AS c RIGHT OUTER JOIN
                      dbo.OrderHeader AS h ON c.CustomerId = h.CustomerId RIGHT OUTER JOIN
                      dbo.OrderItems AS i LEFT OUTER JOIN
                      dbo.OrderUnit AS u ON i.OrderUnitId = u.OrderUnitId ON h.OrderNo = i.OrderNo LEFT OUTER JOIN
                      dbo.Products AS p ON i.ProductNo = p.ProductNo
GO

/****** Object:  View [dbo].[N_ViewOrderItems]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewOrderItems]
AS
SELECT     i.OrderItemId, i.OrderNo, i.ProductNo, i.Price, i.CurrencyId, i.Quantity, i.OrderUnitId, i.SendDate, i.IsReport, i.IsPartSend, i.OrderItemStatusId, i.WareHouse, 
                      i.OrderItemDesc, i.PartNo, CONVERT(money, i.Price * i.Quantity) AS TotalPrice, h.CustomerId, h.LinkName, h.OrderStatusId, h.Fax, h.Telephone, p.ProductName, 
                      p.Model, p.StandardId, p.Material, p.SurfaceColor, p.Rigidity, p.IsStandard,
                          (SELECT     SUM(SendQuantity) AS a
                            FROM          dbo.OrderSend
                            WHERE      (OrderItemId = i.OrderItemId)) AS IsSendQuantity, u.OrderUnitName, h.StockNo, c.CustomerName, i.AfterTaxPrice, CONVERT(money, 
                      i.AfterTaxPrice * i.Quantity) AS AfterTaxTotalPrice, h.SaleType, h.OrderDate
FROM         dbo.Customers AS c RIGHT OUTER JOIN
                      dbo.OrderHeader AS h ON c.CustomerId = h.CustomerId RIGHT OUTER JOIN
                      dbo.OrderItems AS i LEFT OUTER JOIN
                      dbo.OrderUnit AS u ON i.OrderUnitId = u.OrderUnitId ON h.OrderNo = i.OrderNo LEFT OUTER JOIN
                      dbo.Products AS p ON i.ProductNo = p.ProductNo
GO

/****** Object:  View [dbo].[N_ViewCurrentSemiStoreHouse]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewCurrentSemiStoreHouse]
AS
SELECT     c.Id, c.CurrentSemiStoreHouseNo, c.ProductionOrderNo, c.StoreHouseId, c.SemiProductNo, c.FreezeQuantity, c.ActualQuantity, c.ApplyEnterDate, c.Remark, 
                      c.TimeCreated, c.TimeLastMod, c.CreatorUserId, c.UserIDLastMod, s.SemiProductName, s.Model, s.Material, s.ProductDesc, s.SurfaceColor, s.Rigidity, s.IsStandard, 
                      c.KgWeight, s.PartNo
FROM         dbo.CurrentSemiStoreHouse AS c LEFT OUTER JOIN
                      dbo.SemiProducts AS s ON c.SemiProductNo = s.SemiProductNo
GO

/****** Object:  View [dbo].[N_ViewCurrentProductStoreHouse]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[N_ViewCurrentProductStoreHouse]
AS
SELECT     p.Material, p.Model, p.ProductName, p.Rigidity, p.SurfaceColor, p.IsStandard, c.Id, c.CurrentProductStoreHouseNo, c.ProductionOrderNo, c.StoreHouseId, 
                      c.StoreLocationNo, c.ProductNo, c.FreezeQuantity, c.Quantity, c.Quantity - c.FreezeQuantity AS ActualQuantity, c.Remark, c.TimeCreated, c.TimeLastMod, 
                      c.CreatorUserId, c.UserIDLastMod, c.KgWeight, p.ProductDesc
FROM         dbo.CurrentProductStoreHouse AS c LEFT OUTER JOIN
                      dbo.Products AS p ON c.ProductNo = p.ProductNo
GO

/****** Object:  View [dbo].[vEnterOutLogDetail]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vEnterOutLogDetail]
AS
SELECT   lower(replace(NEWID(), '-', '')) AS Id, Id AS ActualId, 1 AS EnterOutFlag, ProductNo, StoreHouseId, Quantity AS Quantity, 
                EnterStoreDate AS DateTiem, UserIDLastMod, Remark
FROM      FinshedEnterStore
WHERE   ApplyStatus = 5
UNION ALL
SELECT   lower(replace(NEWID(), '-', '')) AS Id, Id AS ActualId, - 1 AS EnterOutFlag, ProductNo, StoreHouseId, 
                ActualQuantity AS Quantity, OutStoreDate AS DateTiem, UserIDLastMod, Remark
FROM      ProductOutStore
WHERE   ApplyStatus = 5
GO

/****** Object:  View [dbo].[vwOrderSendBill]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwOrderSendBill]
AS
SELECT     os.OrderItemId, os.SendDate, os.SendQuantity, os.OrderSendBillNo, i.OrderNo, i.ProductNo, p.ProductName, p.Model, p.StandardId, p.Material, p.SurfaceColor, 
                      p.Rigidity, p.ProductDesc, i.IsReport, i.PartNo, h.LinkName, h.OrderDate, h.CustomerSendId, h.CustomerId, u.OrderUnitName, os.Remark, h.StockNo, os.OrderSendId, 
                      os.UserIDLastMod, i.Price, CONVERT(money, i.Price * os.SendQuantity) AS totalprice, os.OrderStickBillNo, i.AfterTaxPrice, i.CurrencyId, CONVERT(money, 
                      i.AfterTaxPrice * os.SendQuantity) AS AfterTaxTotalprice
FROM         dbo.OrderUnit AS u RIGHT OUTER JOIN
                      dbo.OrderItems AS i ON u.OrderUnitId = i.OrderUnitId LEFT OUTER JOIN
                      dbo.Products AS p ON i.ProductNo = p.ProductNo RIGHT OUTER JOIN
                      dbo.OrderSend AS os ON i.OrderItemId = os.OrderItemId LEFT OUTER JOIN
                      dbo.OrderHeader AS h ON i.OrderNo = h.OrderNo
GO

/****** Object:  View [dbo].[vProductStoreStatistics]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vProductStoreStatistics]
AS
SELECT     ProductNo, SUM(Quantity) AS AllQuantity, SUM(FreezeQuantity) AS AllFreezeQuantity, SUM(PreMonthQuantity) AS AllPreMonthQuantity
FROM         dbo.CurrentProductStoreHouse
GROUP BY ProductNo
GO

/****** Object:  View [dbo].[v_customerstick]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view[dbo].[v_customerstick] as SELECT h.OrderNo, h.StockNo, os.OrderSendBillNo, os.SendDate, i.PartNo, p.Model, p.SurfaceColor, p.Rigidity, os.SendQuantity, os.OrderUnitId, i.Price, os.SendQuantity* i.Price AS total, h.CustomerId, os.OrderSendId, u.OrderUnitName, osb.IsDoBill, p.ProductName AS Remark, os.OrderStickBillNo FROM      OrderSendBills as osb INNER JOIN OrderSend as os ON osb.OrderSendBillNo = os.OrderSendBillNo LEFT OUTER JOIN OrderHeader as h INNER JOIN OrderItems as i ON h.OrderNo = i.OrderNo INNER JOIN   Products as p ON i.ProductNo = p.ProductNo ON os.OrderItemId = i.OrderItemId LEFT OUTER JOIN OrderUnit as u ON os.OrderUnitId = u.OrderUnitId WHERE   (os.OrderStickBillNo IS NULL OR os.OrderStickBillNo = '') AND(p.IsLock = 'N')
GO
/****** Object:  View [dbo].[v_CanProductStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view[dbo].[v_CanProductStore] as SELECT SUM(Quantity - FreezeQuantity) AS CanUserQuantity, ProductNo FROM dbo.CurrentProductStoreHouse GROUP BY ProductNo
GO
/****** Object:  View [dbo].[v_BookedProductNum]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_BookedProductNum]
AS
SELECT   ProductNo, SUM(totalnum - ISNULL(IsSendQuantity, 0)) AS BookedQuantity
FROM      (SELECT   SUM(i.Quantity) AS totalnum, p.ProductNo, i.OrderItemId,
                                     (SELECT   SUM(SendQuantity) AS a
                                      FROM      dbo.OrderSend AS s
                                      WHERE   (OrderItemId = i.OrderItemId)) AS IsSendQuantity
                 FROM      dbo.OrderItems AS i LEFT OUTER JOIN
                                 dbo.Products AS p ON i.ProductNo = p.ProductNo AND p.IsLock = 'N' AND (i.OrderItemStatusId = 15 OR
                                 i.OrderItemStatusId IS NULL)
                 GROUP BY p.ProductNo, i.OrderItemId) AS t
GROUP BY ProductNo
GO
/****** Object:  View [dbo].[v_orderbillandstick_m]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW[dbo].[v_orderbillandstick_m]
AS
SELECT   OrderSendBillNo, OrderStickBillNo
FROM      dbo.OrderSend
GROUP BY OrderSendBillNo, OrderStickBillNo
GO
/****** Object:  View [dbo].[v_EnterOutProductStore]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_EnterOutProductStore]
AS
SELECT   dbo.vEnterOutLogDetail.Id, dbo.vEnterOutLogDetail.ActualId, dbo.vEnterOutLogDetail.EnterOutFlag, 
                dbo.vEnterOutLogDetail.ProductNo, dbo.vEnterOutLogDetail.StoreHouseId, dbo.vEnterOutLogDetail.Quantity, 
                dbo.vEnterOutLogDetail.DateTiem, dbo.vEnterOutLogDetail.UserIDLastMod, dbo.vEnterOutLogDetail.Remark, 
                dbo.Products.ProductName, dbo.Products.Model, dbo.Products.Material, dbo.Products.SurfaceColor, 
                dbo.Products.Rigidity, dbo.Products.PartNo
FROM      dbo.Products RIGHT OUTER JOIN
                dbo.vEnterOutLogDetail ON dbo.Products.ProductNo = dbo.vEnterOutLogDetail.ProductNo
GO
/****** Object:  View [dbo].[v_billAndStick_StickNum_m]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_billAndStick_StickNum_m]
AS
SELECT     dbo.OrderStickBills.StickNum, dbo.v_orderbillandstick_m.OrderSendBillNo, dbo.OrderStickBills.CreatDate, dbo.OrderStickBills.StickMan
FROM         dbo.v_orderbillandstick_m LEFT OUTER JOIN
                      dbo.OrderStickBills ON dbo.v_orderbillandstick_m.OrderStickBillNo = dbo.OrderStickBills.OrderStickBillNo
WHERE     (dbo.v_orderbillandstick_m.OrderStickBillNo IS NOT NULL) AND (dbo.v_orderbillandstick_m.OrderStickBillNo <> '')
GO

/****** Object:  View [dbo].[vEnterOutLogDetail_c]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vEnterOutLogDetail_c]
AS
SELECT     t.Id, t.ActualId, t.EnterOutFlag, t.ProductNo, t.StoreHouseId, t.Quantity, t.DateTiem, t.UserIDLastMod, t.Remark, (CASE WHEN t .EnterOutFlag = - 1 THEN
                          (SELECT     CustomerId
                            FROM          N_ViewProductOutStore
                            WHERE      Id = t .ActualId) ELSE '' END) AS CustomerId, (CASE WHEN t .EnterOutFlag = - 1 THEN
                          (SELECT     CustomerName
                            FROM          N_ViewProductOutStore
                            WHERE      Id = t .ActualId) ELSE '' END) AS CustomerName, dbo.Products.ProductName, dbo.Products.Model, dbo.Products.StandardId, dbo.Products.Material, 
                      dbo.Products.SurfaceColor, dbo.Products.Rigidity, dbo.Products.PartNo
FROM         dbo.vEnterOutLogDetail AS t LEFT OUTER JOIN
                      dbo.Products ON t.ProductNo = dbo.Products.ProductNo
GO

/****** Object:  View [dbo].[v_QueryCurrentProductNum]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[v_QueryCurrentProductNum] as SELECT b.ProductNo, b.BookedQuantity, c.CanUserQuantity FROM      v_CanProductStore as c RIGHT OUTER JOIN v_BookedProductNum as b ON c.ProductNo = b.ProductNo
GO
/****** Object:  View [dbo].[v_ProductStoreInfo]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_ProductStoreInfo]
AS
SELECT     dbo.vProductStoreStatistics.ProductNo, dbo.vProductStoreStatistics.AllQuantity, dbo.vProductStoreStatistics.AllFreezeQuantity, dbo.Products.ProductName, 
                      dbo.Products.Model, dbo.Products.Material, dbo.Products.SurfaceColor, dbo.Products.Rigidity, dbo.Products.PartNo, dbo.vProductStoreStatistics.AllPreMonthQuantity, 
                      dbo.Products.Defprice
FROM         dbo.vProductStoreStatistics LEFT OUTER JOIN
                      dbo.Products ON dbo.vProductStoreStatistics.ProductNo = dbo.Products.ProductNo
GO

/****** Object:  View [dbo].[v_OrderStickBill]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_OrderStickBill]
AS
SELECT     s.OrderStickBillNo, t.TotalPrice, t.AfterTaxTotalPrice, c.CustomerName, c.Address, c.LinkMan, s.CustomerId, s.CreatDate, s.StickNum, s.StickMan, s.Description, 
                      s.TimeCreated, t.CurrencyId
FROM         dbo.OrderStickBills AS s LEFT OUTER JOIN
                      dbo.Customers AS c ON s.CustomerId = c.CustomerId LEFT OUTER JOIN
                          (SELECT     SUM(totalprice) AS TotalPrice, SUM(AfterTaxTotalprice) AS AfterTaxTotalPrice, OrderStickBillNo, CurrencyId
                            FROM          dbo.vwOrderSendBill
                            GROUP BY OrderStickBillNo, CurrencyId) AS t ON s.OrderStickBillNo = t.OrderStickBillNo
GO

/****** Object:  View [dbo].[v_Store_Query]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_Store_Query]
AS
SELECT     dbo.v_EnterOutProductStore.Id, dbo.v_EnterOutProductStore.ActualId, dbo.v_EnterOutProductStore.EnterOutFlag, dbo.v_EnterOutProductStore.ProductNo, 
                      dbo.v_EnterOutProductStore.StoreHouseId, dbo.v_EnterOutProductStore.Quantity, dbo.v_EnterOutProductStore.DateTiem, dbo.v_EnterOutProductStore.UserIDLastMod,
                       dbo.v_EnterOutProductStore.Remark, dbo.v_EnterOutProductStore.ProductName, dbo.v_EnterOutProductStore.Model, dbo.v_EnterOutProductStore.Material, 
                      dbo.v_EnterOutProductStore.SurfaceColor, dbo.v_EnterOutProductStore.Rigidity, dbo.v_EnterOutProductStore.PartNo, dbo.vProductStoreStatistics.AllQuantity, 
                      dbo.vProductStoreStatistics.AllFreezeQuantity, dbo.vProductStoreStatistics.AllPreMonthQuantity
FROM         dbo.v_EnterOutProductStore LEFT OUTER JOIN
                      dbo.vProductStoreStatistics ON dbo.v_EnterOutProductStore.ProductNo = dbo.vProductStoreStatistics.ProductNo
GO

/****** Object:  View [dbo].[v_OrderSendBill]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view[dbo].[v_OrderSendBill] as SELECT   b.OrderSendBillNo, b.CustomerId, b.SendDate, b.SendAddress, b.ContactTels, b.ContactMan, b.TimeCreated, b.TimeLastMod, b.UserIDLastMod, b.IsDoBill, vbsm.StickNum, vbsm.CreatDate, vbsm.StickMan, (CASE WHEN StickNum IS NULL THEN 'Î´¿ªÆ±' ELSE 'ÒÑ¿ªÆ±' END)  AS isbill FROM v_billAndStick_StickNum_m as vbsm RIGHT OUTER JOIN OrderSendBills as b ON vbsm.OrderSendBillNo = b.OrderSendBillNo
GO



/****** Object:  View [dbo].[NV_ViewEmployeeInfo]    Script Date: 01/06/2020 16:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view[dbo].[NV_ViewEmployeeInfo] as SELECT   E.Id, E.No, E.Name, E.DepartmentNo, E.DutyNo, E.Description, E.UserName, E.Remark, DU.DutyName, 
                DE.DepartmentName
FROM      dbo.EmployeeInfo AS E LEFT OUTER JOIN
                dbo.Departments AS DE ON E.DepartmentNo = DE.DepartmentID LEFT OUTER JOIN
                dbo.Dutys AS DU ON E.DutyNo = DU.DutyID
WHERE   (E.IsDeleted = 0)
GO



");       
            //CreateTable(
            //    "dbo.v_BookedProductNum",
            //    c => new
            //        {
            //            ProductNo = c.String(nullable: false, maxLength: 128),
            //            BookedQuantity = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.ProductNo);
            
            //CreateTable(
            //    "dbo.v_CanProductStore",
            //    c => new
            //        {
            //            ProductNo = c.String(nullable: false, maxLength: 128),
            //            CanUserQuantity = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.ProductNo);
            
            //CreateTable(
            //    "dbo.N_ViewCurrentProductStoreHouse",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CurrentProductStoreHouseNo = c.String(nullable: false),
            //            ProductionOrderNo = c.String(nullable: false),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreLocationNo = c.String(),
            //            ProductNo = c.String(),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Remark = c.String(maxLength: 150),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(maxLength: 20),
            //            UserIDLastMod = c.String(maxLength: 20),
            //            ProductName = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 20),
            //            Material = c.String(maxLength: 50),
            //            SurfaceColor = c.String(maxLength: 50),
            //            Rigidity = c.String(maxLength: 50),
            //            IsStandard = c.String(maxLength: 1),
            //            ProductDesc = c.String(),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewCurrentSemiStoreHouse",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CurrentSemiStoreHouseNo = c.String(nullable: false, maxLength: 32),
            //            ProductionOrderNo = c.String(nullable: false, maxLength: 11),
            //            StoreHouseId = c.Int(nullable: false),
            //            SemiProductNo = c.String(maxLength: 32),
            //            FreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ApplyEnterDate = c.DateTime(),
            //            Remark = c.String(maxLength: 150),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(maxLength: 20),
            //            UserIDLastMod = c.String(maxLength: 20),
            //            SemiProductName = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 20),
            //            PartNo = c.String(),
            //            Material = c.String(maxLength: 50),
            //            SurfaceColor = c.String(maxLength: 50),
            //            Rigidity = c.String(maxLength: 50),
            //            IsStandard = c.String(maxLength: 1),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_ProductStoreInfo",
            //    c => new
            //        {
            //            ProductNo = c.String(nullable: false, maxLength: 128),
            //            AllQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AllFreezeQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AllPreMonthQuantity = c.Decimal(precision: 18, scale: 2),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            PartNo = c.String(),
            //            Defprice = c.Decimal(precision: 18, scale: 3),
            //        })
            //    .PrimaryKey(t => t.ProductNo);
            
            //CreateTable(
            //    "dbo.v_customerstick",
            //    c => new
            //        {
            //            OrderSendId = c.Int(nullable: false, identity: true),
            //            OrderNo = c.String(),
            //            StockNo = c.String(),
            //            OrderSendBillNo = c.String(),
            //            SendDate = c.DateTime(),
            //            PartNo = c.String(),
            //            Model = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            SendQuantity = c.Decimal(precision: 18, scale: 2),
            //            OrderUnitId = c.Int(),
            //            Price = c.Decimal(precision: 18, scale: 2),
            //            total = c.Decimal(precision: 18, scale: 2),
            //            CustomerId = c.String(),
            //            OrderUnitName = c.String(),
            //            IsDoBill = c.String(),
            //            Remark = c.String(),
            //            OrderStickBillNo = c.String(),
            //        })
            //    .PrimaryKey(t => t.OrderSendId);
            
            //CreateTable(
            //    "dbo.ViewEmployeeInfo",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            No = c.String(),
            //            Name = c.String(),
            //            DepartmentNo = c.String(),
            //            DepartmentName = c.String(),
            //            DutyNo = c.String(),
            //            DutyName = c.String(),
            //            Description = c.String(),
            //            UserName = c.String(),
            //            Remark = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.vEnterOutLogDetail_c",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ActualId = c.Int(nullable: false),
            //            EnterOutFlag = c.Int(nullable: false),
            //            ProductNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateTiem = c.DateTime(),
            //            UserIDLastMod = c.String(),
            //            Remark = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            PartNo = c.String(),
            //            CustomerId = c.String(),
            //            CustomerName = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_Store_Query",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            ActualId = c.Int(nullable: false),
            //            EnterOutFlag = c.Int(nullable: false),
            //            ProductNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateTiem = c.DateTime(),
            //            UserIDLastMod = c.String(),
            //            Remark = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            PartNo = c.String(),
            //            AllQuantity = c.Decimal(precision: 18, scale: 2),
            //            AllFreezeQuantity = c.Decimal(precision: 18, scale: 2),
            //            AllPreMonthQuantity = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewOrderItems",
            //    c => new
            //        {
            //            OrderItemId = c.Int(nullable: false, identity: true),
            //            OrderNo = c.String(),
            //            ProductNo = c.String(),
            //            Price = c.Decimal(nullable: false, storeType: "money"),
            //            AfterTaxPrice = c.Decimal(nullable: false, storeType: "money"),
            //            CurrencyId = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
            //            OrderUnitId = c.Int(nullable: false),
            //            SendDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
            //            IsReport = c.String(),
            //            IsPartSend = c.String(),
            //            OrderItemStatusId = c.Int(),
            //            WareHouse = c.String(),
            //            OrderItemDesc = c.String(),
            //            PartNo = c.String(),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            CustomerId = c.String(),
            //            LinkName = c.String(),
            //            OrderDate = c.DateTime(storeType: "smalldatetime"),
            //            Fax = c.String(),
            //            Telephone = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            StandardId = c.Int(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            IsStandard = c.String(),
            //            IsSendQuantity = c.Decimal(precision: 18, scale: 2),
            //            OrderUnitName = c.String(),
            //            StockNo = c.String(),
            //            CustomerName = c.String(),
            //            AfterTaxTotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SaleType = c.Int(),
            //        })
            //    .PrimaryKey(t => t.OrderItemId);
            
            //CreateTable(
            //    "dbo.v_OrderSendBill",
            //    c => new
            //        {
            //            OrderSendBillNo = c.String(nullable: false, maxLength: 128),
            //            CustomerId = c.String(),
            //            SendDate = c.DateTime(),
            //            SendAddress = c.String(),
            //            ContactTels = c.String(),
            //            ContactMan = c.String(),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            UserIDLastMod = c.String(),
            //            IsDoBill = c.String(),
            //            StickNum = c.String(),
            //            StickMan = c.String(),
            //            CreatDate = c.DateTime(),
            //            isbill = c.String(),
            //        })
            //    .PrimaryKey(t => t.OrderSendBillNo);
            
            //CreateTable(
            //    "dbo.N_ViewOrderSends",
            //    c => new
            //        {
            //            OrderSendId = c.Int(nullable: false, identity: true),
            //            OrderItemId = c.Int(nullable: false),
            //            SendDate = c.DateTime(),
            //            SendQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Remark = c.String(),
            //            OrderSendBillNo = c.String(),
            //            OrderStickBillNo = c.String(),
            //            QuantityPerPack = c.Decimal(precision: 18, scale: 2),
            //            PackageCount = c.Decimal(precision: 18, scale: 2),
            //            ProductBatchNum = c.String(),
            //            UserIDLastMod = c.String(),
            //            OrderNo = c.String(),
            //            ProductNo = c.String(),
            //            Price = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            IsReport = c.String(),
            //            PartNo = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            StandardId = c.Int(),
            //            Material = c.String(),
            //            ProductDesc = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            CustomerId = c.String(),
            //            CustomerName = c.String(),
            //            LinkName = c.String(),
            //            OrderDate = c.DateTime(nullable: false),
            //            CustomerSendId = c.Int(nullable: false),
            //            StockNo = c.String(),
            //            OrderUnitName = c.String(),
            //            SaleType = c.Int(),
            //            AfterTaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.OrderSendId);
            
            //CreateTable(
            //    "dbo.vwOrderSendBill",
            //    c => new
            //        {
            //            OrderSendId = c.Int(nullable: false, identity: true),
            //            OrderItemId = c.Int(nullable: false),
            //            SendDate = c.DateTime(),
            //            SendQuantity = c.Decimal(precision: 18, scale: 2),
            //            Remark = c.String(),
            //            OrderSendBillNo = c.String(),
            //            OrderStickBillNo = c.String(),
            //            UserIDLastMod = c.String(),
            //            OrderNo = c.String(),
            //            ProductNo = c.String(),
            //            Price = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            totalprice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            IsReport = c.String(),
            //            PartNo = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            StandardId = c.Int(),
            //            Material = c.String(),
            //            ProductDesc = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            CustomerId = c.String(),
            //            LinkName = c.String(),
            //            OrderDate = c.DateTime(),
            //            CustomerSendId = c.Int(nullable: false),
            //            StockNo = c.String(),
            //            OrderUnitName = c.String(),
            //            CurrencyId = c.String(),
            //            AfterTaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.OrderSendId);
            
            //CreateTable(
            //    "dbo.N_ViewPackageApply",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            PackageApplyNo = c.String(),
            //            CurrentStoreHouseNo = c.String(),
            //            ProductionOrderNo = c.String(),
            //            SemiProductNo = c.String(),
            //            ProductNo = c.String(),
            //            ApplyQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
            //            SourceStore = c.Int(nullable: false),
            //            ApplyStatus = c.String(),
            //            IsClose = c.Boolean(nullable: false),
            //            ApplyDate = c.DateTime(),
            //            Remark = c.String(),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(),
            //            UserIDLastMod = c.String(),
            //            ProcessingNum = c.Int(nullable: false),
            //            IsApplyEnterQuantity = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewProductEnterStore",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductionOrderNo = c.String(maxLength: 11),
            //            PackageApplyNo = c.String(nullable: false),
            //            PackageProductNo = c.String(maxLength: 32),
            //            ProductNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            StoreLocationNo = c.String(),
            //            ApplyStatus = c.Int(nullable: false),
            //            IsClose = c.Boolean(nullable: false),
            //            ApplySourceType = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PackageSpecification = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PackageCount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualPackageCount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AuditUser = c.String(),
            //            AuditDate = c.DateTime(),
            //            ApplyEnterDate = c.DateTime(),
            //            Remark = c.String(),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(),
            //            UserIDLastMod = c.String(),
            //            ProductName = c.String(),
            //            Model = c.String(maxLength: 20),
            //            Material = c.String(maxLength: 50),
            //            SurfaceColor = c.String(maxLength: 50),
            //            Rigidity = c.String(maxLength: 50),
            //            IsStandard = c.String(maxLength: 1),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SourceStoreHouseId = c.Int(nullable: false),
            //            CreateSourceType = c.Int(),
            //            PackageEnterNum = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewProductOutStore",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductionOrderNo = c.String(),
            //            CurrentProductStoreHouseNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            ApplyStatus = c.Int(nullable: false),
            //            IsClose = c.Boolean(),
            //            IsConfirm = c.Boolean(),
            //            ProductNo = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ApplyOutDate = c.DateTime(),
            //            Remark = c.String(),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(),
            //            UserIDLastMod = c.String(),
            //            AuditUser = c.String(),
            //            AuditDate = c.DateTime(),
            //            ProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            IsStandard = c.String(),
            //            CustomerId = c.String(),
            //            CustomerName = c.String(),
            //            OrderSendBillNo = c.String(),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ApplyOutStoreSourceType = c.Int(nullable: false),
            //            CreateSourceType = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_QueryCurrentProductNum",
            //    c => new
            //        {
            //            ProductNo = c.String(nullable: false, maxLength: 128),
            //            BookedQuantity = c.Decimal(precision: 18, scale: 2),
            //            CanUserQuantity = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.ProductNo);
            
            //CreateTable(
            //    "dbo.N_ViewSemiEnterStore",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductionOrderNo = c.String(nullable: false, maxLength: 11),
            //            StoreHouseId = c.Int(nullable: false),
            //            ApplyStatus = c.String(nullable: false, maxLength: 1),
            //            IsClose = c.Boolean(),
            //            ApplySource = c.String(nullable: false, maxLength: 1),
            //            SemiProductNo = c.String(maxLength: 32),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AuditUser = c.String(maxLength: 20),
            //            AuditDate = c.DateTime(),
            //            ApplyEnterDate = c.DateTime(),
            //            Remark = c.String(maxLength: 150),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(maxLength: 20),
            //            UserIDLastMod = c.String(maxLength: 20),
            //            SemiProductName = c.String(maxLength: 50),
            //            Model = c.String(maxLength: 20),
            //            Material = c.String(maxLength: 50),
            //            SurfaceColor = c.String(maxLength: 50),
            //            Rigidity = c.String(maxLength: 50),
            //            IsStandard = c.String(maxLength: 1),
            //            PartNo = c.String(maxLength: 50),
            //            TranUnitValue = c.Decimal(precision: 18, scale: 2),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            CreateSourceType = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.N_ViewSemiOutStore",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductionOrderNo = c.String(),
            //            CurrentSemiStoreHouseNo = c.String(),
            //            StoreHouseId = c.Int(nullable: false),
            //            ApplyStatus = c.String(),
            //            ApplyTypes = c.Int(nullable: false),
            //            IsClose = c.Boolean(),
            //            IsConfirm = c.Boolean(),
            //            ApplyOutStoreSource = c.String(),
            //            SemiProductNo = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ActualQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ApplyOutDate = c.DateTime(),
            //            Remark = c.String(),
            //            TimeCreated = c.DateTime(),
            //            TimeLastMod = c.DateTime(),
            //            CreatorUserId = c.String(),
            //            UserIDLastMod = c.String(),
            //            AuditUser = c.String(),
            //            AuditDate = c.DateTime(),
            //            SemiProductName = c.String(),
            //            Model = c.String(),
            //            Material = c.String(),
            //            SurfaceColor = c.String(),
            //            Rigidity = c.String(),
            //            IsStandard = c.String(),
            //            PartNo = c.String(),
            //            TranUnitValue = c.Decimal(precision: 18, scale: 2),
            //            KgWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            CreateSourceType = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.v_OrderStickBill",
            //    c => new
            //        {
            //            OrderStickBillNo = c.String(nullable: false, maxLength: 128),
            //            CustomerId = c.String(),
            //            CreatDate = c.DateTime(),
            //            StickNum = c.String(),
            //            StickMan = c.String(),
            //            Description = c.String(),
            //            TimeCreated = c.DateTime(),
            //            CustomerName = c.String(),
            //            Address = c.String(),
            //            LinkMan = c.String(),
            //            TotalPrice = c.Decimal(precision: 18, scale: 3),
            //            AfterTaxTotalPrice = c.Decimal(precision: 18, scale: 3),
            //            CurrencyId = c.String(),
            //        })
            //    .PrimaryKey(t => t.OrderStickBillNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sys_States", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_States", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_States", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Settings", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Settings", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Settings", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Roles", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Roles", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Roles", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Functions", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Functions", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Functions", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.EmployeeInfo", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.EmployeeInfo", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.EmployeeInfo", "CreatorUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_UserRoles", "UserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Users", "LastModifierUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Users", "DeleterUserId", "dbo.Sys_Users");
            DropForeignKey("dbo.Sys_Users", "CreatorUserId", "dbo.Sys_Users");
            DropIndex("dbo.Sys_States", new[] { "CreatorUserId" });
            DropIndex("dbo.Sys_States", new[] { "LastModifierUserId" });
            DropIndex("dbo.Sys_States", new[] { "DeleterUserId" });
            DropIndex("dbo.Sys_Settings", new[] { "CreatorUserId" });
            DropIndex("dbo.Sys_Settings", new[] { "LastModifierUserId" });
            DropIndex("dbo.Sys_Settings", new[] { "DeleterUserId" });
            DropIndex("dbo.Sys_Roles", new[] { "CreatorUserId" });
            DropIndex("dbo.Sys_Roles", new[] { "LastModifierUserId" });
            DropIndex("dbo.Sys_Roles", new[] { "DeleterUserId" });
            DropIndex("dbo.Sys_Functions", new[] { "CreatorUserId" });
            DropIndex("dbo.Sys_Functions", new[] { "LastModifierUserId" });
            DropIndex("dbo.Sys_Functions", new[] { "DeleterUserId" });
            DropIndex("dbo.Sys_UserRoles", new[] { "UserId" });
            DropIndex("dbo.Sys_Users", new[] { "CreatorUserId" });
            DropIndex("dbo.Sys_Users", new[] { "LastModifierUserId" });
            DropIndex("dbo.Sys_Users", new[] { "DeleterUserId" });
            DropIndex("dbo.EmployeeInfo", new[] { "CreatorUserId" });
            DropIndex("dbo.EmployeeInfo", new[] { "LastModifierUserId" });
            DropIndex("dbo.EmployeeInfo", new[] { "DeleterUserId" });
            DropTable("dbo.v_OrderStickBill");
            DropTable("dbo.N_ViewSemiOutStore");
            DropTable("dbo.N_ViewSemiEnterStore");
            DropTable("dbo.v_QueryCurrentProductNum");
            DropTable("dbo.N_ViewProductOutStore");
            DropTable("dbo.N_ViewProductEnterStore");
            DropTable("dbo.N_ViewPackageApply");
            DropTable("dbo.vwOrderSendBill");
            DropTable("dbo.N_ViewOrderSends");
            DropTable("dbo.v_OrderSendBill");
            DropTable("dbo.N_ViewOrderItems");
            DropTable("dbo.v_Store_Query");
            DropTable("dbo.vEnterOutLogDetail_c");
            DropTable("dbo.ViewEmployeeInfo");
            DropTable("dbo.v_customerstick");
            DropTable("dbo.v_ProductStoreInfo");
            DropTable("dbo.N_ViewCurrentSemiStoreHouse");
            DropTable("dbo.N_ViewCurrentProductStoreHouse");
            DropTable("dbo.v_CanProductStore");
            DropTable("dbo.v_BookedProductNum");
            DropTable("dbo.Sys_UserLogins");
            DropTable("dbo.Sys_UserLoginLogs");
            DropTable("dbo.TemplateInfos");
            DropTable("dbo.Sys_States",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SysState_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Sys_Helps");
            DropTable("dbo.Sys_AttachFiles");
            DropTable("dbo.StoreHouse");
            DropTable("dbo.StoreHouseLocation",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_StoreHouseLocation_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Standard");
            DropTable("dbo.ShortMsgDetail");
            DropTable("dbo.ShortMessage");
            DropTable("dbo.Sys_Settings",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SysSetting_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SemiProductStore");
            DropTable("dbo.SemiProducts");
            DropTable("dbo.SemiOutStores");
            DropTable("dbo.SemiEnterStore");
            DropTable("dbo.Sys_Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SysRole_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Regions");
            DropTable("dbo.ProductInspectReports");
            DropTable("dbo.Products");
            DropTable("dbo.ProductOutStore");
            DropTable("dbo.ProductionOrders");
            DropTable("dbo.ProductionOrderLogs");
            DropTable("dbo.ProductInspectInfos");
            DropTable("dbo.Sys_Permissions");
            DropTable("dbo.PackageApply");
            DropTable("dbo.OutFactory");
            DropTable("dbo.OrderUnit");
            DropTable("dbo.OrderStickBills");
            DropTable("dbo.OrderSend");
            DropTable("dbo.OrderSendBills");
            DropTable("dbo.OrderItems");
            DropTable("dbo.OrderHeader");
            DropTable("dbo.Sys_Functions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SysFunction_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinshedEnterStore");
            DropTable("dbo.Factories");
            DropTable("dbo.Sys_UserRoles");
            DropTable("dbo.Sys_Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SysUser_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EmployeeInfo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Employee_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Dutys");
            DropTable("dbo.Departments");
            DropTable("dbo.CustomerSend");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerDefaultProduct");
            DropTable("dbo.CurrentSemiStoreHouse");
            DropTable("dbo.CurrentProductStoreHouse");
            DropTable("dbo.CurrencyExchangeRate");
            DropTable("dbo.Currency");
            DropTable("dbo.BusinessLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BusinessLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.BulletinInfo");
            DropTable("dbo.BackUpCurrentSemiStoreHouse");
            DropTable("dbo.Sys_AuditLogs");
            DropTable("dbo.Sys_AppGuids");
        }
    }
}

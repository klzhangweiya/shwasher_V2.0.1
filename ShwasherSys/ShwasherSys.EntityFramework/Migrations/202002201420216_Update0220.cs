namespace ShwasherSys.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update0220 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerInvoiceAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        InvoiceAddressName = c.String(nullable: false, maxLength: 150),
                        InvoiceAddress = c.String(nullable: false, maxLength: 250),
                        LinkMan = c.String(maxLength: 30),
                        Telephone = c.String(maxLength: 50),
                        Zip = c.String(maxLength: 8),
                        Email = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
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
                    { "DynamicFilter_CustomerInvoiceAddress_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerInvoiceAddress",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CustomerInvoiceAddress_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200227 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnGoodOrder", "CustomerId", c => c.String(maxLength: 32));
            AddColumn("dbo.ReturnGoodOrder", "Reason", c => c.String(maxLength: 500));
            AddColumn("dbo.ReturnGoodOrder", "Amount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ReturnGoodOrder", "AuditAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReturnGoodOrder", "AuditAmount");
            DropColumn("dbo.ReturnGoodOrder", "Amount");
            DropColumn("dbo.ReturnGoodOrder", "Reason");
            DropColumn("dbo.ReturnGoodOrder", "CustomerId");
        }
    }
}

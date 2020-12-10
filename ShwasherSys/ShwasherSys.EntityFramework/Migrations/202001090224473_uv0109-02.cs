namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv010902 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.v_OrderStickBill", "InvoiceState", c => c.Int());
           // AddColumn("dbo.v_OrderStickBill", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.v_OrderStickBill", "Amount");
            DropColumn("dbo.v_OrderStickBill", "InvoiceState");
        }
    }
}

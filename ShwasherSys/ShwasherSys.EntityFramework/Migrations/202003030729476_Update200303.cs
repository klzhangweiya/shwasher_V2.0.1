namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200303 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStickBills", "OriginalStickNum", c => c.String(maxLength: 30));
            AddColumn("dbo.OrderStickBills", "ReturnOrderNo", c => c.String(maxLength: 30));
            AddColumn("dbo.OrderStickBills", "OrderNo", c => c.String(maxLength: 500));
            AddColumn("dbo.OrderStickBills", "InvoiceType", c => c.Int(nullable: false));
            //AddColumn("dbo.v_OrderStickBill", "OriginalStickNum", c => c.String());
            //AddColumn("dbo.v_OrderStickBill", "ReturnOrderNo", c => c.String());
            //AddColumn("dbo.v_OrderStickBill", "OrderNo", c => c.String());
            //AddColumn("dbo.v_OrderStickBill", "InvoiceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.v_OrderStickBill", "InvoiceType");
            DropColumn("dbo.v_OrderStickBill", "OrderNo");
            DropColumn("dbo.v_OrderStickBill", "ReturnOrderNo");
            DropColumn("dbo.v_OrderStickBill", "OriginalStickNum");
            DropColumn("dbo.OrderStickBills", "InvoiceType");
            DropColumn("dbo.OrderStickBills", "OrderNo");
            DropColumn("dbo.OrderStickBills", "ReturnOrderNo");
            DropColumn("dbo.OrderStickBills", "OriginalStickNum");
        }
    }
}

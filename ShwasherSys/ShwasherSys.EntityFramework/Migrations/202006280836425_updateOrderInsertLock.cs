namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderInsertLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeader", "IsLock", c => c.String(maxLength: 1));
            AddColumn("dbo.OrderItems", "IsLock", c => c.String(maxLength: 1));
            //AddColumn("dbo.v_customerstick", "StatementBillNo", c => c.String());
            //AddColumn("dbo.N_ViewOrderItems", "EmergencyLevel", c => c.Int(nullable: false));
            //AddColumn("dbo.v_OrderSendBill", "TotalPrice", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.v_OrderSendBill", "AfterTaxTotalPrice", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.v_OrderSendBill", "CurrencyId", c => c.String());
            //DropColumn("dbo.v_OrderSendBill", "StickNum");
            //DropColumn("dbo.v_OrderSendBill", "StickMan");
            //DropColumn("dbo.v_OrderSendBill", "CreatDate");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.v_OrderSendBill", "CreatDate", c => c.DateTime());
            //AddColumn("dbo.v_OrderSendBill", "StickMan", c => c.String());
            //AddColumn("dbo.v_OrderSendBill", "StickNum", c => c.String());
            //DropColumn("dbo.v_OrderSendBill", "CurrencyId");
            //DropColumn("dbo.v_OrderSendBill", "AfterTaxTotalPrice");
            //DropColumn("dbo.v_OrderSendBill", "TotalPrice");
            //DropColumn("dbo.N_ViewOrderItems", "EmergencyLevel");
            //DropColumn("dbo.v_customerstick", "StatementBillNo");
            DropColumn("dbo.OrderItems", "IsLock");
            DropColumn("dbo.OrderHeader", "IsLock");
        }
    }
}

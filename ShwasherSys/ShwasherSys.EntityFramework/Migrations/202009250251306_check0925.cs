namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check0925 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.v_OrderSendBill", "OrderSendCount", c => c.Int(nullable: false));
            //AddColumn("dbo.v_OrderSendBill", "StatementCount", c => c.Int(nullable: false));
            //DropColumn("dbo.v_OrderSendBill", "isbill");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.v_OrderSendBill", "isbill", c => c.String());
            //DropColumn("dbo.v_OrderSendBill", "StatementCount");
            //DropColumn("dbo.v_OrderSendBill", "OrderSendCount");
        }
    }
}

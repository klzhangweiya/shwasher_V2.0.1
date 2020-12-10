namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200224 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnGoodOrder", "SendOrderNo", c => c.String(maxLength: 32));
            AddColumn("dbo.ReturnGoodOrder", "ReturnType", c => c.Int(nullable: false));
            //AddColumn("dbo.vwOrderSendBill", "StatementBillNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.vwOrderSendBill", "StatementBillNo");
            DropColumn("dbo.ReturnGoodOrder", "ReturnType");
            DropColumn("dbo.ReturnGoodOrder", "SendOrderNo");
        }
    }
}

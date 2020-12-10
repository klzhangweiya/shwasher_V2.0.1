namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVersion01082 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatementBill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatementBillNo = c.String(nullable: false, maxLength: 30),
                        CustomerId = c.String(nullable: false, maxLength: 30),
                        BillMan = c.String(maxLength: 20),
                        Description = c.String(maxLength: 4000),
                        State = c.Int(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StatementBillNo, unique: true);
            
            AddColumn("dbo.OrderSendExceed", "OrderNo", c => c.String());
            AddColumn("dbo.OrderSendExceed", "ProductNo", c => c.String());
            AddColumn("dbo.OrderSend", "StatementBillNo", c => c.String(maxLength: 32));
            AddColumn("dbo.OrderStickBills", "State", c => c.Int());
            AddColumn("dbo.OrderStickBills", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropIndex("dbo.StatementBill", new[] { "StatementBillNo" });
            DropColumn("dbo.OrderStickBills", "Amount");
            DropColumn("dbo.OrderStickBills", "State");
            DropColumn("dbo.OrderSend", "StatementBillNo");
            DropColumn("dbo.OrderSendExceed", "ProductNo");
            DropColumn("dbo.OrderSendExceed", "OrderNo");
            DropTable("dbo.StatementBill");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStatement : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.N_ViewStatementBill",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StatementBillNo = c.String(nullable: false, maxLength: 30),
            //            CustomerId = c.String(nullable: false, maxLength: 30),
            //            BillMan = c.String(maxLength: 20),
            //            Description = c.String(maxLength: 4000),
            //            StatementState = c.Int(),
            //            CustomerName = c.String(),
            //            OrderStickBillNo = c.String(),
            //            LastModificationTime = c.DateTime(),
            //            LastModifierUserId = c.Long(),
            //            CreationTime = c.DateTime(nullable: false),
            //            CreatorUserId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.StatementBillNo, unique: true);
            
            AddColumn("dbo.StatementBill", "OrderStickBillNo", c => c.String(maxLength: 30));
            //AddColumn("dbo.N_ViewOrderItems", "IsLock", c => c.String());
        }
        
        public override void Down()
        {
            //DropIndex("dbo.N_ViewStatementBill", new[] { "StatementBillNo" });
            //DropColumn("dbo.N_ViewOrderItems", "IsLock");
            DropColumn("dbo.StatementBill", "OrderStickBillNo");
            //DropTable("dbo.N_ViewStatementBill");
        }
    }
}

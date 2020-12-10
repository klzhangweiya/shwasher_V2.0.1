namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check07021 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.N_ViewStatementBill", "TotalPrice", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.N_ViewStatementBill", "AfterTaxTotalPrice", c => c.Decimal(precision: 18, scale: 3));
            //AddColumn("dbo.N_ViewStatementBill", "CurrencyId", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.N_ViewStatementBill", "CurrencyId");
            //DropColumn("dbo.N_ViewStatementBill", "AfterTaxTotalPrice");
            //DropColumn("dbo.N_ViewStatementBill", "TotalPrice");
        }
    }
}

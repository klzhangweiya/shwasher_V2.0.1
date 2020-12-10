namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200228 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.N_ViewOrderItems", "SaleMan", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.N_ViewOrderItems", "SaleMan");
        }
    }
}

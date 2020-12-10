namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0107 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeInfo", "CardId", c => c.String(maxLength: 18));
            AddColumn("dbo.EmployeeInfo", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeInfo", "PhoneNumber", c => c.String(maxLength: 18));
            AddColumn("dbo.OrderHeader", "SaleMan", c => c.String(maxLength: 50));
            //AddColumn("dbo.NV_ViewEmployeeInfo", "CardId", c => c.String());
            //AddColumn("dbo.NV_ViewEmployeeInfo", "Gender", c => c.Int(nullable: false));
            //AddColumn("dbo.NV_ViewEmployeeInfo", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NV_ViewEmployeeInfo", "PhoneNumber");
            DropColumn("dbo.NV_ViewEmployeeInfo", "Gender");
            DropColumn("dbo.NV_ViewEmployeeInfo", "CardId");
            DropColumn("dbo.OrderHeader", "SaleMan");
            DropColumn("dbo.EmployeeInfo", "PhoneNumber");
            DropColumn("dbo.EmployeeInfo", "Gender");
            DropColumn("dbo.EmployeeInfo", "CardId");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "EmergencyLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "EmergencyLevel");
        }
    }
}

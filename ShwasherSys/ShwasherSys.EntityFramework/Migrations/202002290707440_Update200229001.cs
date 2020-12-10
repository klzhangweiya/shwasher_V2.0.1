namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200229001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnGoodOrder", "OrderItemNo", c => c.String(maxLength: 32));
            AddColumn("dbo.ReturnGoodOrder", "ApplyUser", c => c.String(maxLength: 32));
            AddColumn("dbo.ReturnGoodOrder", "ConfirmUser", c => c.String(maxLength: 32));
            AddColumn("dbo.ReturnGoodOrder", "ApplyDate", c => c.DateTime());
            AddColumn("dbo.ReturnGoodOrder", "ConfirmDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReturnGoodOrder", "ConfirmDate");
            DropColumn("dbo.ReturnGoodOrder", "ApplyDate");
            DropColumn("dbo.ReturnGoodOrder", "ConfirmUser");
            DropColumn("dbo.ReturnGoodOrder", "ApplyUser");
            DropColumn("dbo.ReturnGoodOrder", "OrderItemNo");
        }
    }
}

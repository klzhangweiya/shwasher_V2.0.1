namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200229 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisqualifiedProductInfo", "ReturnOrderNo", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisqualifiedProductInfo", "ReturnOrderNo");
        }
    }
}

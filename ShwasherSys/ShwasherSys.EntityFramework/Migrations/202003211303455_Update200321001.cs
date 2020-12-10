namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200321001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnGoodOrder", "LinkName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReturnGoodOrder", "LinkName");
        }
    }
}

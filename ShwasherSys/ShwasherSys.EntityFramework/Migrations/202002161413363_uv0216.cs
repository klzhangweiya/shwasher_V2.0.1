namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv0216 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisqualifiedProductInfo", "DisqualifiedType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisqualifiedProductInfo", "DisqualifiedType");
        }
    }
}

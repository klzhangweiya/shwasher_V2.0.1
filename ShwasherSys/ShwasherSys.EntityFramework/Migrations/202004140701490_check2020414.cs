namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check2020414 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.vEnterOutLogDetail_c", "ProductionOrderNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.vEnterOutLogDetail_c", "ProductionOrderNo");
        }
    }
}

namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMoldInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoldInfo", "CustomerName", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "ShelfNum", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "OuterDiameter", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "InsideDiameter", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "Thickness", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "Height", c => c.String(maxLength: 50));
            AddColumn("dbo.MoldInfo", "Rigidity", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MoldInfo", "Rigidity");
            DropColumn("dbo.MoldInfo", "Height");
            DropColumn("dbo.MoldInfo", "Thickness");
            DropColumn("dbo.MoldInfo", "InsideDiameter");
            DropColumn("dbo.MoldInfo", "OuterDiameter");
            DropColumn("dbo.MoldInfo", "ShelfNum");
            DropColumn("dbo.MoldInfo", "CustomerName");
        }
    }
}

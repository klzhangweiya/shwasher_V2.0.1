namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update200321 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnGoodOrder", "SurveyReason", c => c.String(maxLength: 500));
            AddColumn("dbo.ReturnGoodOrder", "SurveyDetail", c => c.String(maxLength: 500));
            AddColumn("dbo.ReturnGoodOrder", "Solution", c => c.String(maxLength: 500));
            //AddColumn("dbo.N_ViewPackageApply", "IsApplyEnterQuantity2", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.N_ViewPackageApply", "IsApplyEnterQuantity2");
            DropColumn("dbo.ReturnGoodOrder", "Solution");
            DropColumn("dbo.ReturnGoodOrder", "SurveyDetail");
            DropColumn("dbo.ReturnGoodOrder", "SurveyReason");
        }
    }
}

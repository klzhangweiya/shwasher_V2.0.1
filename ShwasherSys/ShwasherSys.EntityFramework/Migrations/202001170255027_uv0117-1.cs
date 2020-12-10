namespace ShwasherSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uv01171 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.MaintenanceMemberInfo", "IsConfirm", c => c.Boolean(nullable: false));
            //AddColumn("dbo.SemiEnterStore", "StoreLocationNo", c => c.String(maxLength: 32));
            //AddColumn("dbo.N_ViewSemiEnterStore", "StoreLocationNo", c => c.String());
            //AlterColumn("dbo.CurrentSemiStoreHouse", "StoreLocationNo", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CurrentSemiStoreHouse", "StoreLocationNo", c => c.String(nullable: false, maxLength: 32));
            DropColumn("dbo.N_ViewSemiEnterStore", "StoreLocationNo");
            DropColumn("dbo.SemiEnterStore", "StoreLocationNo");
            DropColumn("dbo.MaintenanceMemberInfo", "IsConfirm");
        }
    }
}

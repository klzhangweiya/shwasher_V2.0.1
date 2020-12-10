using System.Data.Entity.Migrations;
using EntityFramework.DynamicFilters;
using ShwasherSys.EntityFramework;
using ShwasherSys.Migrations.SeedData;

namespace ShwasherSys.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ShwasherDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "IwbAdminSystem";
        }

        protected override void Seed(ShwasherDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...

            context.DisableAllFilters();
            new InitialHostDbBuilder(context).Create();

        }
    }
}

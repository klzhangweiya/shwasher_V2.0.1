using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class DefaultTruncateTableSql
    {
        private readonly ShwasherDbContext _context;

        public DefaultTruncateTableSql(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            Sql("TRUNCATE TABLE [dbo].[Sys_Functions]");
            Sql("TRUNCATE TABLE [dbo].[Sys_Permissions]");

            //Sql("TRUNCATE TABLE [dbo].[Sys_AppGuids]");

            //Sql("TRUNCATE TABLE [dbo].[Sys_Settings]");
            //Sql("TRUNCATE TABLE [dbo].[Sys_States]");

        }

        private void Sql(string sql)
        {
            _context.Database.ExecuteSqlCommand(sql);
        }
    }
}
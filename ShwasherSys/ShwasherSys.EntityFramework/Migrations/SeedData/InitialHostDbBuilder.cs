using EntityFramework.DynamicFilters;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly ShwasherDbContext _context;

        public InitialHostDbBuilder(ShwasherDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();
            //new DefaultDataCreator(_context).Create();

            //new DefaultTruncateTableSql(_context).Create();
            new DefaultFunctionsCreator(_context).Create();
            new DefaultRoleAndUserCreator(_context).Create();

            //new DefaultSettingsCreator(_context).Create();
            //new DefaultStatesCreator(_context).Create();
            //new DefaultTemplateCreator(_context).Create();
            //new DefaultStoreLocationCreator(_context).Create();
            //new DefaultAppGuidsCreator(_context).Create();
        }
    }


    internal static class ExcuteSql
    {
        public static void DeleteTable(this ShwasherDbContext context, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return;
            }
            var sql = $"TRUNCATE TABLE {tableName}";
            context.Sql(sql);
        }
        public static void Sql(this ShwasherDbContext context, string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            context.Database.ExecuteSqlCommand(sql);
        }

    }
}

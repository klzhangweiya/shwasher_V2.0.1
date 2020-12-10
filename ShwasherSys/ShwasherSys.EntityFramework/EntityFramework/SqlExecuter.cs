using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.EntityFramework;
using ShwasherSys.EntityFramework;

namespace ShwasherSys.EntityFramework
{
    public class SqlExecuter : ISqlExecuter, ITransientDependency
    {
        private readonly IDbContextProvider<ShwasherDbContext> _dbContextProvider;

        public SqlExecuter(IDbContextProvider<ShwasherDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        /// <summary>
        /// 执行给定的命令
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        /// <returns>执行命令后由数据库返回的结果</returns>
        public int Execute(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql, parameters);
        }
        /// <summary>
        /// 执行给定的命令
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        /// <returns>执行命令后由数据库返回的结果</returns>
        public async Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            try
            {
                var context = _dbContextProvider.GetDbContext();
                return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }
        ///// <summary>
        ///// 执行给定的命令
        ///// </summary>
        ///// <param name="sql">命令字符串</param>
        ///// <param name="parameters">要应用于命令字符串的参数</param>
        ///// <returns>执行命令后由数据库返回的结果</returns>
        //public async Task<int> ExecuteAsync(string sql, params object[] parameters)
        //{
        //    var context = new ShwasherDbContext();
        //    return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
        //}
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。
        /// </summary>
        /// <typeparam name="T">查询所返回对象的类型</typeparam>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
        /// <returns></returns>
        public IQueryable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return _dbContextProvider.GetDbContext().Database.SqlQuery<T>(sql, parameters).AsQueryable();
        }
    }
}

using LY.Common.Helper;
using SqlSugar;

namespace LY.Repository.DbContext
{
    public sealed class SQLSugarDbContext
    {
        private static SqlSugarScope instance = null;

        private SQLSugarDbContext()
        {

        }

        private static void InitSqlSugar()
        {
            AppsettingsHelper appsettingsHelper = new AppsettingsHelper();
            string connStr = appsettingsHelper.Get<string>("ConnectionStrings");

            instance = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = connStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true, //不设成true要手动close
                InitKeyType = InitKeyType.Attribute
            });

            instance.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql,查看执行sql
            };
        }

        /// <summary>
        /// SqlSugarScope是线程安全的，不用加锁，单例模式足矣
        /// </summary>
        public static SqlSugarScope Instance
        {
            get
            {
                if (instance == null)
                {
                    InitSqlSugar();
                }

                return instance;
            }
        }
    }
}

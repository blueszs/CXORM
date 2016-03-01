using System.Data.Common;
using System.Data.SqlClient;

namespace CXData.ADO
{
    /// <summary>
    /// Sql Server 提供程序
    /// 20150625-周盛-添加
    /// </summary>
    public class SqlDataProviders : IDataProviders
    {
        public string ConnectionString
        {
            get;
            set;
        }

        public DatabaseType GetDatabaseType()
        {
            return DatabaseType.SqlServer;
        }

        public DbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new SqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new SqlParameter();
        }

        public DbParameter GetDbParameter(string dbParaName,object oVal)
        {
            return new SqlParameter(dbParaName, oVal);
        }
    }
}

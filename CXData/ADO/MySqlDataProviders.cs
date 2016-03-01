using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CXData.ADO
{
    /// <summary>
    /// MySql 提供程序
    /// 20150625-周盛-添加
    /// </summary>
    public class MySqlDataProviders :  IDataProviders
    {
        public string ConnectionString
        {
            get;
            set;
        }

        public DatabaseType GetDatabaseType()
        {
            return DatabaseType.MySql;
        }

        public DbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new MySqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new MySqlParameter();
        }

        public DbParameter GetDbParameter(string dbParaName, object oVal)
        {
            return new MySqlParameter(dbParaName, oVal);
        }
    }
}

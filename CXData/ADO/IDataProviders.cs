using System.Data.Common;

namespace CXData.ADO
{
    /// <summary>
    /// 数据库提供程序接口
    /// 20150625-周盛-添加
    /// </summary>
    public interface IDataProviders
    {
        string ConnectionString { get; set; }

        DatabaseType GetDatabaseType();

        DbConnection GetDbConnection(string connectionString);

        DbCommand GetDbCommand(string cmdText);

        DbDataAdapter GetDbDataAdappter();

        DbParameter GetDbParameter();
    }
}

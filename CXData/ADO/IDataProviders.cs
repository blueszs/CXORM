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

        DbParameter GetDbParameter(string dbParaName, object oVal);

        string GetSelectLimitSql(string tableName, string strColumns, string whereStr, string orderBystr, int limit);

        string GetJoinLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType, string strColumns, string whereStr, string orderBystr, int limit);

        string GetJoinLimitSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB, string keyB1, string keyC, string joinType1, string joinType2, string strColumns, string whereStr, string orderBystr, int limit);

        string GetGroupLimitSql(string tableName, string strColumns, string whereStr, string keystr, string orderBystr, int limit);

        string GetJoinGroupLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType, string strColumns, string whereStr, string keystr, string orderBystr, int limit);

        string GetPageSql(string tableName, string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex);

        string GetJoinGroupPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType, string strColumns, string whereStr, string keystr, string orderBystr, int pageSize, int pageIndex);

        string GetJoinPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType, string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex);

        string GetJoinPageSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB, string joinType1,string keyA1,string keyC, string joinType2, string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex);

        string GetRowCoutSql();

        string GetIDENTITYSql();
    }
}

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

        public DbParameter GetDbParameter(string dbParaName, object oVal)
        {
            return new SqlParameter(dbParaName, oVal);
        }

        public string GetSelectLimitSql(string tableName, string strColumns, string whereStr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} {4} ", limit > 0 ? "TOP " + limit : "", strColumns, tableName, whereStr, orderBystr);
        }

        public string GetJoinLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} JOIN {4} ON {5}={6} {7} {8}", limit > 0 ? "TOP " + limit : "", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, orderBystr);
        }

        public string GetJoinLimitSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB,
            string keyB1, string keyC, string joinType1, string joinType2, string strColumns, string whereStr,
            string orderBystr, int limit)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} JOIN {4} ON {5}={6} {7} JOIN {8} ON {9}={10} {11} {12}", limit > 0 ? "TOP " + limit : "", strColumns, tableNameA, joinType1, tableNameB, keyA, keyB,joinType2,tableNameC,keyB1,keyC, whereStr, orderBystr);
        }

        public string GetGroupLimitSql(string tableName, string strColumns, string whereStr, string keystr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} GROUP BY {4} {5} ", limit > 0 ? "TOP " + limit : "", strColumns, tableName, whereStr, keystr, orderBystr);
        }

        public string GetJoinGroupLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} {1} FROM {2} {3} JOIN {4} ON {5}={6} {7} GROUP BY {8} {9} ", limit > 0 ? "TOP " + limit : "", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, keystr, orderBystr);
        }

        public string GetPageSql(string tableName, string strColumns, string whereStr, string orderBystr, int pageSize,
            int pageIndex)
        {
            return string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} ) AS T WHERE ROWID BETWEEN {4} AND {5} ",
                                orderBystr, strColumns, tableName, whereStr, (pageIndex - 1) * pageSize + 1,
                                pageIndex * pageSize);
        }

        public string GetJoinGroupPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int pageSize,int pageIndex)
        {
            return string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7} GROUP BY {8})AS T WHERE ROWID BETWEEN {9} AND {10}", orderBystr, strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, keystr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        }

        public string GetJoinPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex)
        {
            return string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7})AS T WHERE ROWID BETWEEN {8} AND {9}", orderBystr, strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        }

        public string GetJoinPageSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB, string joinType1,
            string keyA1, string keyC, string joinType2, string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex)
        {
            return string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7} JOIN {8} {9}={10} {11})AS T WHERE ROWID BETWEEN {12} AND {13}", orderBystr, strColumns, tableNameA, joinType1, tableNameB, keyA, keyB,joinType2,tableNameC,keyA1,keyC, whereStr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        }

        public string GetRowCoutSql()
        {
            return "SELECT @@ROWCOUNT;";
        }

        public string GetIDENTITYSql()
        {
            return "SELECT SCOPE_IDENTITY();";
        }
    }
}

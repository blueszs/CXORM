using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CXData.ADO
{
    /// <summary>
    /// MySql 提供程序
    /// 20150625-周盛-添加
    /// </summary>
    public class MySqlDataProviders : IDataProviders
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

        public string GetSelectLimitSql(string tableName, string strColumns, string whereStr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} {4} ", strColumns, tableName, whereStr, orderBystr, limit > 0 ? "LIMT " + limit : "");
        }

        public string GetJoinLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7} {8}", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, orderBystr, limit > 0 ? "LIMIT " + limit : "");
        }

        public string GetJoinLimitSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB,
            string keyB1, string keyC, string joinType1, string joinType2, string strColumns, string whereStr,
            string orderBystr, int limit)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} {8}={9} {10} {11} {12}", strColumns, tableNameA, joinType1, tableNameB, keyA, keyB, joinType2, tableNameC, keyB1, keyC, whereStr, orderBystr, limit > 0 ? "LIMIT " + limit : "");
        }

        public string GetGroupLimitSql(string tableName, string strColumns, string whereStr, string keystr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} FROM {1} {2} GROUP BY {3} {4} {5} ", strColumns, tableName, whereStr, keystr, orderBystr, limit > 0 ? "LIMT " + limit : "");
        }

        public string GetJoinGroupLimitSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int limit)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7} {8} {9}", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, keystr, orderBystr, limit > 0 ? "LIMIT " + limit : "");
        }

        public string GetPageSql(string tableName, string strColumns, string whereStr, string orderBystr, int pageSize,
            int pageIndex)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4},{5} ",
                            strColumns, tableName, whereStr, orderBystr, (pageIndex - 1) * pageSize, pageSize);
        }

        public string GetJoinGroupPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int pageSize, int pageIndex)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7} {8} {9},{10}", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, keystr, orderBystr, (pageIndex - 1) * pageSize, pageSize);
        }

        public string GetJoinPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7} {8},{9}", strColumns, tableNameA, joinType, tableNameB, keyA, keyB, whereStr, orderBystr, (pageIndex - 1) * pageSize, pageSize);
        }

        public string GetJoinPageSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB, string joinType1,
            string keyA1, string keyC, string joinType2, string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex)
        {
            return string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} {12},{13}", strColumns, tableNameA, joinType1, tableNameB, keyA, keyB, joinType2, tableNameC, keyA1, keyC, whereStr, orderBystr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        }

        public string GetRowCoutSql()
        {
            return "SELECT ROW_COUNT();";
        }

        public string GetIDENTITYSql()
        {
            return "SELECT LAST_INSERT_ID();";
        }
    }
}

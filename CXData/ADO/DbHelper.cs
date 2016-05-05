using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace CXData.ADO
{
    /// <summary>
    /// 数据库操作类
    /// 20150625-周盛-添加
    /// </summary>
    public class DbHelper
    {
        #region Constuctor
        [ThreadStatic]
        private static IDataProviders _dataProviders;

        /// <summary>
        /// 设置数据访问的数据源
        /// </summary>
        /// <param name="dataProviders">数据源提供程序名称</param>
        /// <param name="conn">数据源连接字符串</param>
        public static void SetDataProviders(string dataProviders, string conn)
        {
            _dataProviders = DataProvidersFactory.GetDataProviders(dataProviders, conn);
        }

        [ThreadStatic]
        private static TransConnection _transConnectionObj;
        #endregion

        #region GetDatabaseType
        public static DatabaseType GetDatabaseType()
        {
            return _dataProviders.GetDatabaseType();
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            bool mustCloseConn;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            var result = cmd.ExecuteNonQuery();

            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            bool mustCloseConn;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            var result = cmd.ExecuteScalar();

            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }
        #endregion ExecuteScalar

        #region ExecuteCount
        public static int ExecuteCount(string cmdText, params DbParameter[] parameterValues)
        {
            int result;
            bool mustCloseConn;
            DbCommand cmd = PrepareCmd(CommandType.Text, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            object obj = cmd.ExecuteScalar();
            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();
            int.TryParse(obj.ToString(), out result);
            return result;
        }
        #endregion ExecuteScalar

        #region ExecuteReader
        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            bool mustCloseConn;
            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            try
            {
                OpenConn(cmd.Connection);
                var result = mustCloseConn ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader();
                ClearCmdParameters(cmd);
                return result;
            }
            catch
            {
                if (mustCloseConn) CloseConn(cmd.Connection);
                ClearCmdParameters(cmd);
                cmd.Dispose();
                throw;
            }
        }
        #endregion ExecuteReader

        #region ExecuteDataset
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataSet result = null;
            bool mustCloseConn;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            try
            {
                using (DbDataAdapter da = _dataProviders.GetDbDataAdappter())
                {
                    da.SelectCommand = cmd;
                    result = new DataSet();

                    da.Fill(result);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            if (mustCloseConn) CloseConn(cmd.Connection);
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }
        #endregion ExecuteDataset

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataSet ds = ExecuteDataSet(cmdType, cmdText, parameterValues);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        #endregion

        #region Transaction
        public static void BeginTransaction()
        {
            if (_transConnectionObj == null)
            {
                DbConnection conn = _dataProviders.GetDbConnection(_dataProviders.ConnectionString);
                OpenConn(conn);
                DbTransaction trans = conn.BeginTransaction();
                _transConnectionObj = new TransConnection { MyDbTransaction = trans };
            }
            else
            {
                _transConnectionObj.Deeps += 1;
            }
        }

        public static void CommitTransaction()
        {
            if (_transConnectionObj == null) return;
            if (_transConnectionObj.Deeps > 0)
            {
                _transConnectionObj.Deeps -= 1;
            }
            else
            {
                _transConnectionObj.MyDbTransaction.Commit();
                ReleaseTransaction();
            }
        }

        public static void RollbackTransaction()
        {
            if (_transConnectionObj == null) return;
            if (_transConnectionObj.Deeps > 0)
            {
                _transConnectionObj.Deeps -= 1;
            }
            else
            {
                _transConnectionObj.MyDbTransaction.Rollback();
                ReleaseTransaction();
            }
        }

        private static void ReleaseTransaction()
        {
            if (_transConnectionObj == null) return;
            DbConnection conn = _transConnectionObj.MyDbTransaction.Connection;
            _transConnectionObj.MyDbTransaction.Dispose();
            _transConnectionObj = null;
            CloseConn(conn);
        }

        #endregion

        #region Connection
        private static void OpenConn(DbConnection conn)
        {
            if (conn == null) conn = _dataProviders.GetDbConnection(_dataProviders.ConnectionString);
            if (conn.State == ConnectionState.Closed) conn.Open();
        }

        private static void CloseConn(DbConnection conn)
        {
            if (conn == null) return;
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Dispose();
        }
        #endregion

        #region Create DbParameter

        public static DbParameter CreateInDbParameter(string paraName, DbType type, int size, object value)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.Input);
        }

        public static DbParameter CreateInDbParameter(string paraName, int size, object value)
        {
            return CreateDbParameter(paraName, size, value, ParameterDirection.Input);
        }

        public static DbParameter CreateInDbParameter(string paraName, DbType type, object value)
        {
            return CreateDbParameter(paraName, type, 0, value, ParameterDirection.Input);
        }

        public static DbParameter CreateInDbParameter(string paraName, object value)
        {
            return CreateDbParameter(paraName, 0, value, ParameterDirection.Input);
        }

        public static KeyValuePair<string, DbParameter> CreateKeyValInDbParameter(string dataField, string paraName, DbType type, object value)
        {
            return new KeyValuePair<string, DbParameter>(dataField, CreateDbParameter(paraName, type, 0, value, ParameterDirection.Input));
        }

        public static KeyValuePair<string, DbParameter> CreateKeyValInDbParameter(string dataField, string paraName, object value)
        {
            return new KeyValuePair<string, DbParameter>(dataField, CreateDbParameter(paraName, 0, value, ParameterDirection.Input));
        }

        public static DbParameter CreateOutDbParameter(string paraName, DbType type, int size)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.Output);
        }

        public static DbParameter CreateOutDbParameter(string paraName, int size)
        {
            return CreateDbParameter(paraName, size, null, ParameterDirection.Output);
        }

        public static DbParameter CreateOutDbParameter(string paraName, DbType type)
        {
            return CreateDbParameter(paraName, type, 0, null, ParameterDirection.Output);
        }

        public static DbParameter CreateOutDbParameter(string paraName)
        {
            return CreateDbParameter(paraName, 0, null, ParameterDirection.Output);
        }

        public static DbParameter CreateReturnDbParameter(string paraName, DbType type, int size)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateReturnDbParameter(string paraName, int size)
        {
            return CreateDbParameter(paraName, size, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateReturnDbParameter(string paraName, DbType type)
        {
            return CreateDbParameter(paraName, type, 0, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateReturnDbParameter(string paraName)
        {
            return CreateDbParameter(paraName, 0, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateDbParameter(string paraName, DbType type, int size, object value, ParameterDirection direction)
        {
            DbParameter para = _dataProviders.GetDbParameter(paraName, value ?? DBNull.Value);
            if (size != 0)
            {
                para.Size = size;
            }
            para.DbType = type;
            para.Direction = direction;
            return para;
        }

        public static DbParameter CreateDbParameter(string paraName, int size, object value, ParameterDirection direction)
        {
            DbParameter para = _dataProviders.GetDbParameter(paraName, value ?? DBNull.Value);
            if (size != 0)
            {
                para.Size = size;
            }
            para.Direction = direction;
            return para;
        }

        #endregion

        #region Command and Parameter

        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param>要处理的DbCommand</param>
        /// <param>数据库连接</param>
        /// <param>一个有效的事务或者是null值</param>
        /// <param>命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param>存储过程名或都T-SQL命令文本</param>
        /// <param>和命令相关联的DbParameter参数数组,如果没有参数为'null'</param>
        /// <param><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParams"></param>
        /// <param name="mustCloseConn"></param>
        private static DbCommand PrepareCmd(CommandType cmdType, string cmdText, DbParameter[] cmdParams, out bool mustCloseConn)
        {
            DbCommand cmd = _dataProviders.GetDbCommand(cmdText);

            DbConnection conn;
            if (_transConnectionObj != null)
            {
                conn = _transConnectionObj.MyDbTransaction.Connection;
                cmd.Transaction = _transConnectionObj.MyDbTransaction;
                mustCloseConn = false;
            }
            else
            {
                conn = _dataProviders.GetDbConnection(_dataProviders.ConnectionString);
                mustCloseConn = true;
            }
            cmd.Connection = conn;

            cmd.CommandType = cmdType;

            AttachParameters(cmd, cmdParams);

            return cmd;
        }

        /// <summary>
        /// 将DbParameter参数数组(参数值)分配给DbCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param>命令名</param>
        /// <param>SqlParameters数组</param>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        private static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command is null");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void ClearCmdParameters(DbCommand cmd)
        {
            bool canClear = true;
            if (cmd.Connection != null && cmd.Connection.State != ConnectionState.Open)
            {
                if (cmd.Parameters.Cast<DbParameter>().Any(commandParameter => commandParameter.Direction != ParameterDirection.Input))
                {
                    canClear = false;
                }
            }
            if (canClear)
            {
                cmd.Parameters.Clear();
            }
        }
        #endregion
        #region Get Limit Sql
        public static string GetSelectLimitSql(string tableName, string strColumns, string whereStr, string orderBystr, int limit)
        {
            return _dataProviders.GetSelectLimitSql(tableName, strColumns, whereStr, orderBystr, limit);
        }

        public static string GetJoinLimitSql(string tableNameA, string tableNameB, string keyA, string keyB,
            string joinType,
            string strColumns, string whereStr, string orderBystr, int limit)
        {
            return _dataProviders.GetJoinLimitSql(tableNameA, tableNameB, keyA, keyB, joinType, strColumns, whereStr, orderBystr, limit);
        }

        public static string GetJoinLimitSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB,
            string keyB1, string keyC, string joinType1, string joinType2, string strColumns, string whereStr,
            string orderBystr, int limit)
        {
            return _dataProviders.GetJoinLimitSql(tableNameA, tableNameB, tableNameC, keyA, keyB,
            keyB1, keyC, joinType1, joinType2, strColumns, whereStr, orderBystr, limit);
        }

        public static string GetGroupLimitSql(string tableName, string strColumns, string whereStr, string keystr,
            string orderBystr, int limit)
        {
            return _dataProviders.GetGroupLimitSql(tableName, strColumns, whereStr, keystr, orderBystr, limit);
        }

        public static string GetJoinGroupLimitSql(string tableNameA, string tableNameB, string keyA, string keyB,
            string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int limit)
        {
            return _dataProviders.GetJoinGroupLimitSql(tableNameA, tableNameB, keyA, keyB, joinType, strColumns, whereStr, keystr, orderBystr, limit);
        }

        #endregion

        #region Get Page Sql
        public static string GetPageSql(string tableName, string strColumns, string whereStr, string orderBystr, int pageSize,
            int pageIndex)
        {
            return _dataProviders.GetPageSql(tableName, strColumns, whereStr, orderBystr, pageSize, pageIndex);
        }

        public static string GetJoinGroupPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string keystr, string orderBystr, int pageSize, int pageIndex)
        {
            return _dataProviders.GetJoinGroupPageSql(tableNameA, tableNameB, keyA, keyB, joinType,
            strColumns, whereStr, keystr, orderBystr, pageSize, pageIndex);
        }

        public static string GetJoinPageSql(string tableNameA, string tableNameB, string keyA, string keyB, string joinType,
            string strColumns, string whereStr, string orderBystr, int pageSize, int pageIndex)
        {
            return _dataProviders.GetJoinPageSql(tableNameA, tableNameB, keyA, keyB, joinType,
            strColumns, whereStr, orderBystr, pageSize, pageIndex);
        }

        public static string GetJoinPageSql(string tableNameA, string tableNameB, string tableNameC, string keyA, string keyB,
            string joinType1, string keyA1, string keyC, string joinType2, string strColumns, string whereStr, string orderBystr,
            int pageSize, int pageIndex)
        {
            return _dataProviders.GetJoinPageSql(tableNameA, tableNameB, tableNameC, keyA, keyB,
                joinType1, keyA1, keyC, joinType2, strColumns, whereStr, orderBystr, pageSize, pageIndex);
        }

        public static string GetRowCoutSql()
        {
            return _dataProviders.GetRowCoutSql();
        }
        public static string GetIDENTITYSql()
        {
            return _dataProviders.GetIDENTITYSql();
        }
        #endregion
    }
}

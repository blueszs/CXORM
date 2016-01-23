using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using CXData.ADO;
using CXData.Helper;

namespace CXData.ORM
{
    /// <summary>
    /// ORM扩展类
    /// 20150625-周盛-添加
    /// </summary>
    public static class OrmExtension
    {
        #region Entity By ORM
        /// <summary>
        /// 根据条件删除数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool DeleteFrom<T>(this T entity, Expression<Func<T, bool>> func) where T : class, new()
        {
            List<DbParameter> dbparaList = new List<DbParameter>();
            string sql = string.Format("DELETE FROM {0}", LambdaExtension.GetTabName(entity));
            if (func != null)
            {
                string whereStr;
                var body = func.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        sql += string.Format(" WHERE {0}", whereStr);
                    }
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, false);
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        sql += string.Format(" WHERE {0}", whereStr);
                    }
                }
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparaList.ToArray()) > 0;
        }

        /// <summary>
        /// 根据条件更新数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="updateEntity"></param>
        /// <param name="funColumns"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool UpdateSet<T>(this T entity, T updateEntity, Expression<Func<T, object[]>> funColumns, Expression<Func<T, bool>> func) where T : class
        {
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (updateEntity != null && funColumns != null)
            {
                string whereStr = string.Empty;
                if (func != null)
                {
                    var body = func.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                    }
                    else
                    {
                        var expression = func.Body as MethodCallExpression;
                        if (expression != null)
                        {
                            whereStr = LambdaExtension.CallExpression(expression, dbparaList, AnalyType.Param, false);
                        }
                    }
                }
                var updateColumns = LambdaExtension.GetUpdateColumns(funColumns);
                string sql = LambdaExtension.GetUpdateSql(updateEntity, updateColumns, whereStr, dbparaList);
                if (string.IsNullOrEmpty(sql))
                {
                    return DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparaList.ToArray()) > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据条件更新数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="funColumns"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool UpdateSet<T>(this T entity, Expression<Func<T, object[]>> funColumns, Expression<Func<T, bool>> func) where T : class
        {
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (entity != null && funColumns != null)
            {
                string whereStr = string.Empty;
                if (func != null)
                {
                    var body = func.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                    }
                    else
                    {
                        var expression = func.Body as MethodCallExpression;
                        if (expression != null)
                        {
                            whereStr = LambdaExtension.CallExpression(expression, dbparaList, AnalyType.Param, false);
                        }
                    }
                }
                var updateColumns = LambdaExtension.GetUpdateColumns(funColumns);
                string sql = LambdaExtension.GetUpdateSql(entity, updateColumns, whereStr, dbparaList);
                if (string.IsNullOrEmpty(sql))
                {
                    return DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparaList.ToArray()) > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 向数据库中新增对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int InsertInto<T>(this T entity) where T : class
        {
            int index = 0;
            if (entity != null)
            {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string sql = LambdaExtension.GetInsertSql(entity, dbparaList);
                if (!string.IsNullOrEmpty(sql))
                {
                    object indexobj = DbHelper.ExecuteScalar(CommandType.Text, sql, dbparaList.ToArray());
                    int.TryParse(indexobj.ToString(), out index);
                }
            }
            return index;
        }

        /// <summary>
        /// 向数据库中新增对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="insertEntity"></param>
        /// <returns></returns>
        public static int InsertInto<T>(this T entity, T insertEntity) where T : class
        {
            int index = 0;
            if (insertEntity != null)
            {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string sql = LambdaExtension.GetInsertSql(insertEntity, dbparaList);
                if (!string.IsNullOrEmpty(sql))
                {
                    object indexobj = DbHelper.ExecuteScalar(CommandType.Text, sql, dbparaList.ToArray());
                    int.TryParse(indexobj.ToString(), out index);
                }
            }
            return index;
        }

        /// <summary>
        /// 根据条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="funColumns"></param>
        /// <returns></returns>
        public static T SelectFirst<T>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, object[]>> funColumns = null) where T : class, new()
        {
            string saName = "";
            string whereStr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            if (func != null)
            {
                saName = func.Parameters[0].Name;
                var body = func.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, true);
                }
            }
            string tablename = LambdaExtension.GetTabName(entity);
            if (!string.IsNullOrEmpty(saName))
            {
                tablename = string.Format("{0} AS {1}", tablename, saName);
            }
            string strColumns = LambdaExtension.GetColumns(funColumns, saName, ref dbparaList);
            string sql = string.Format("SELECT {0} {1} FROM {2}", databaseType == DatabaseType.SqlServer ? "TOP 1" : "", strColumns, tablename);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sql = sql + " WHERE " + whereStr;
            }
            if (databaseType != DatabaseType.SqlServer)
            {
                sql += " LIMIT 1";
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntity<T>();
            }
            return default(T);
        }

        /// <summary>
        /// 根据条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static TResult SelectFirst<T, TResult>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new()
        {
            string saName = "";
            string whereStr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            if (func != null)
            {
                saName = func.Parameters[0].Name;
                var body = func.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, true);
                }
            }
            string tablename = LambdaExtension.GetTabName(entity);
            if (!string.IsNullOrEmpty(saName))
            {
                tablename = string.Format("{0} AS {1}", tablename, saName);
            }
            string columnStr = "";
            var expression = resultSelector.Body as ParameterExpression;
            if (expression != null)
            {
                ParameterExpression para = expression;
                columnStr += para.Name;
                columnStr += ".*";
            }
            else
            {
                var iniexp = resultSelector.Body as MemberInitExpression;
                if (iniexp != null)
                {
                    MemberInitExpression memIniexp = iniexp;
                    foreach (var item in memIniexp.Bindings)
                    {
                        MemberAssignment memaig = item as MemberAssignment;
                        if (memaig != null)
                        {
                            var constantExpression = memaig.Expression as ConstantExpression;
                            if (constantExpression != null)
                            {
                                ConstantExpression ce = constantExpression;
                                if (ce.Value == null)
                                {
                                    columnStr += "NULL";
                                }
                                else if (ce.Value is ValueType)
                                {
                                    columnStr += string.Format("{0}", ce.Value);
                                }
                                else
                                {
                                    columnStr += string.Format("'{0}'", ce.Value);
                                }
                            }
                            else
                            {
                                columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                            }
                            columnStr += string.Format(" AS [{0}]", item.Member.Name);
                            columnStr += ",";
                        }
                    }
                    columnStr = columnStr.TrimEnd(',');
                }
            }
            string sql = string.Format("SELECT {0} {1} FROM {2}", databaseType == DatabaseType.SqlServer ? "TOP 1" : "", columnStr, tablename);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sql = sql + " WHERE " + whereStr;
            }
            if (databaseType != DatabaseType.SqlServer)
            {
                sql += " LIMIT 1";
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntity<TResult>();
            }
            return default(TResult);
        }

        /// <summary>
        /// 根据条件和分组返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static TResult GroupByFirst<T, TKey, TResult>(this T source, Expression<Func<T, TKey>> keySelector, Expression<Func<T, TResult>> resultSelector, Expression<Func<T, bool>> whereFunc)
            where T : class
            where TResult : class, new()
        {
            if (keySelector != null)
            {
                string tablename = LambdaExtension.GetTabName(source);
                string keystr = string.Empty;
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                NewExpression expkey = keySelector.Body as NewExpression;
                if (expkey != null)
                {
                    foreach (var argu in expkey.Arguments)
                    {
                        string groupkey = LambdaExtension.ExpressionRouter(argu, dbparaList, AnalyType.Column, true, OperandType.Left);
                        if (!string.IsNullOrEmpty(groupkey))
                        {
                            keystr += groupkey;
                            keystr += ",";
                        }
                    }
                    keystr = keystr.TrimEnd(',');
                }
                else
                {
                    keystr = LambdaExtension.ExpressionRouter(keySelector.Body, dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (resultSelector != null)
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, dbparaList, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                else
                {
                    columnStr = keystr;
                }
                if (whereFunc != null)
                {
                    var body = whereFunc.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereFunc.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereFunc.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }
                if (!string.IsNullOrEmpty(keystr) && !string.IsNullOrEmpty(tablename))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            sql = string.Format("SELECT TOP 1 {0} FROM {1} {2} GROUP BY {3};", columnStr, tablename, whereStr, keystr);
                            break;
                        case DatabaseType.MySql:
                            sql = string.Format("SELECT {0} FROM {1} {2} GROUP BY {3} LIMIT 1;", columnStr, tablename, whereStr, keystr);
                            break;
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntity<TResult>();
                    }
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static List<TResult> GroupByList<T, TKey, TResult>(this T source, Expression<Func<T, TKey>> keySelector, Expression<Func<T, TResult>> resultSelector, Expression<Func<T, bool>> whereFunc)
            where T : class
            where TResult : class, new()
        {
            if (keySelector != null)
            {
                string tablename = LambdaExtension.GetTabName(source);
                string keystr = string.Empty;
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                NewExpression expkey = keySelector.Body as NewExpression;
                if (expkey != null)
                {
                    foreach (var argu in expkey.Arguments)
                    {
                        string groupkey = LambdaExtension.ExpressionRouter(argu, dbparaList, AnalyType.Column, true, OperandType.Left);
                        if (!string.IsNullOrEmpty(groupkey))
                        {
                            keystr += groupkey;
                            keystr += ",";
                        }
                    }
                    keystr = keystr.TrimEnd(',');
                }
                else
                {
                    keystr = LambdaExtension.ExpressionRouter(keySelector.Body, dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (resultSelector != null)
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, dbparaList, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                else
                {
                    columnStr = keystr;
                }
                if (whereFunc != null)
                {
                    var body = whereFunc.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList,
                            AnalyType.Param, true);
                    }
                    else if (whereFunc.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereFunc.Body, dbparaList,
                            AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }
                if (!string.IsNullOrEmpty(keystr) && !string.IsNullOrEmpty(tablename))
                {
                    string sql = string.Format("SELECT {0} FROM {1} {2} GROUP BY {3}", columnStr, tablename, whereStr, keystr);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据条件和分组返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<T> SelectList<T>(this T entity, Expression<Func<T, bool>> func)
            where T : class, new()
        {
            return SelectList(entity, func, 0, null);
        }

        /// <summary>
        /// 根据条件和分组返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static List<TResult> SelectList<T, TResult>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new()
        {
            return SelectList(entity, func, 0, null, resultSelector);
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="rowNum"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static List<T> SelectList<T>(this T entity, Expression<Func<T, bool>> func, int rowNum, Expression<Func<T, bool>> funOrder)
            where T : class, new()
        {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null)
            {
                var expression = func.Body as BinaryExpression;
                if (expression != null)
                {
                    BinaryExpression be = expression;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, false);
                }
            }
            if (funOrder != null)
            {
                var body = funOrder.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, false);
                }
                else if (funOrder.Body is MethodCallExpression)
                {
                    orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, false);
                }
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            string sql = string.Format("SELECT {0} * FROM {1} {2} {3}", rowNum > 0 && databaseType == DatabaseType.SqlServer ? string.Format("TOP {0} ", rowNum) : "", tableName, whereStr, orderBystr);
            if (rowNum > 0 && databaseType != DatabaseType.SqlServer)
            {
                sql += string.Format(" LIMIT {0}", rowNum);
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntityList<T>();
            }
            return new List<T>();
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="rowNum"></param>
        /// <param name="funOrder"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static List<TResult> SelectList<T, TResult>(this T entity, Expression<Func<T, bool>> func, int rowNum, Expression<Func<T, bool>> funOrder, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new()
        {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null)
            {
                var expression = func.Body as BinaryExpression;
                if (expression != null)
                {
                    BinaryExpression be = expression;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, false);
                }
            }
            if (funOrder != null)
            {
                var body = funOrder.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, false);
                }
                else if (funOrder.Body is MethodCallExpression)
                {
                    orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, false);
                }
            }
            string columnStr = "";
            var expressionResult = resultSelector.Body as ParameterExpression;
            if (expressionResult != null)
            {
                columnStr += "*";
            }
            else
            {
                var iniexp = resultSelector.Body as MemberInitExpression;
                if (iniexp != null)
                {
                    MemberInitExpression memIniexp = iniexp;
                    foreach (var item in memIniexp.Bindings)
                    {
                        MemberAssignment memaig = item as MemberAssignment;
                        if (memaig != null)
                        {
                            var constantExpression = memaig.Expression as ConstantExpression;
                            if (constantExpression != null)
                            {
                                ConstantExpression ce = constantExpression;
                                if (ce.Value == null)
                                {
                                    columnStr += "NULL";
                                }
                                else if (ce.Value is ValueType)
                                {
                                    columnStr += string.Format("{0}", ce.Value);
                                }
                                else
                                {
                                    columnStr += string.Format("'{0}'", ce.Value);
                                }
                            }
                            else
                            {
                                columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, false, OperandType.Left);
                            }
                            columnStr += string.Format(" AS [{0}]", item.Member.Name);
                            columnStr += ",";
                        }
                    }
                    columnStr = columnStr.TrimEnd(',');
                }
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            string sql = string.Format("SELECT {0} {1} FROM {2} {3} {4}", rowNum > 0 && databaseType == DatabaseType.SqlServer ? string.Format("TOP {0} ", rowNum) : "", columnStr, tableName, whereStr, orderBystr);
            if (rowNum > 0 && databaseType != DatabaseType.SqlServer)
            {
                sql += string.Format(" LIMIT {0}", rowNum);
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntityList<TResult>();
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="funColumns"></param>
        /// <returns></returns>
        public static List<T> SelectList<T>(this T entity, Expression<Func<T, bool>> func,
            Expression<Func<T, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord,
            Expression<Func<T, object[]>> funColumns = null)
            where T : class, new()
        {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            string saName = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null)
            {
                saName = func.Parameters[0].Name;
                var expression = func.Body as BinaryExpression;
                if (expression != null)
                {
                    BinaryExpression be = expression;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, false);
                }
            }
            if (funOrder != null)
            {
                var body = funOrder.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null,
                        AnalyType.Order, false);
                }
                else if (funOrder.Body is MethodCallExpression)
                {
                    orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order,
                        false);
                }
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = "";
            string strColumns = LambdaExtension.GetColumns(funColumns, saName, ref dbparaList);
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            if (pageSize > 0)
            {
                sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM {0} {1};", tableName, whereStr);
                DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                dbparaList.Add(param);
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        sql +=
                            string.Format(
                                "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} ) AS T WHERE ROWID BETWEEN {4} AND {5} ",
                                orderBystr, strColumns, tableName, whereStr, (pageIndex - 1) * pageSize + 1,
                                pageIndex * pageSize);
                        break;
                    case DatabaseType.MySql:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4},{5} ",
                            strColumns, tableName, whereStr, orderBystr, (pageIndex - 1) * pageSize, pageSize);
                        break;
                }
            }
            else
            {
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3}", strColumns, tableName, whereStr, orderBystr);
                        break;
                    case DatabaseType.MySql:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3}", strColumns, tableName, whereStr, orderBystr);
                        break;
                }
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (pageSize > 0)
            {
                var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                if (paraOut != null)
                {
                    totalRecord = Convert.ToInt32(paraOut.Value);
                }
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntityList<T>();
            }
            return new List<T>();
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static List<TResult> SelectList<T, TResult>(this T entity, Expression<Func<T, bool>> func,
            Expression<Func<T, bool>> funOrder, Expression<Func<T, TResult>> resultSelector, int pageSize, int pageIndex, ref int totalRecord)
            where T : class
            where TResult : class, new()
        {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null)
            {
                var expression = func.Body as BinaryExpression;
                if (expression != null)
                {
                    BinaryExpression be = expression;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param, false);
                }
            }
            if (funOrder != null)
            {
                var body = funOrder.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null,
                        AnalyType.Order, false);
                }
                else if (funOrder.Body is MethodCallExpression)
                {
                    orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order,
                        false);
                }
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = "";
            string columnStr = "";
            var expressionResult = resultSelector.Body as ParameterExpression;
            if (expressionResult != null)
            {
                ParameterExpression para = expressionResult;
                columnStr += para.Name;
                columnStr += ".*";
            }
            else
            {
                var iniexp = resultSelector.Body as MemberInitExpression;
                if (iniexp != null)
                {
                    MemberInitExpression memIniexp = iniexp;
                    foreach (var item in memIniexp.Bindings)
                    {
                        MemberAssignment memaig = item as MemberAssignment;
                        if (memaig != null)
                        {
                            var constantExpression = memaig.Expression as ConstantExpression;
                            if (constantExpression != null)
                            {
                                ConstantExpression ce = constantExpression;
                                if (ce.Value == null)
                                {
                                    columnStr += "NULL";
                                }
                                else if (ce.Value is ValueType)
                                {
                                    columnStr += string.Format("{0}", ce.Value);
                                }
                                else
                                {
                                    columnStr += string.Format("'{0}'", ce.Value);
                                }
                            }
                            else
                            {
                                columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                            }
                            columnStr += string.Format(" AS [{0}]", item.Member.Name);
                            columnStr += ",";
                        }
                    }
                    columnStr = columnStr.TrimEnd(',');
                }
            }
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            if (pageSize > 0)
            {
                sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM {0} {1};", tableName, whereStr);
                DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                dbparaList.Add(param);
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        sql +=
                            string.Format(
                                "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} ) AS T WHERE ROWID BETWEEN {4} AND {5} ",
                                orderBystr, columnStr, tableName, whereStr, (pageIndex - 1) * pageSize + 1,
                                pageIndex * pageSize);
                        break;
                    case DatabaseType.MySql:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3} LIMIT {4},{5} ",
                            columnStr, tableName, whereStr, orderBystr, (pageIndex - 1) * pageSize, pageSize);
                        break;
                }
            }
            else
            {
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3}", columnStr, tableName, whereStr, orderBystr);
                        break;
                    case DatabaseType.MySql:
                        sql += string.Format("SELECT {0} FROM {1} {2} {3}", columnStr, tableName, whereStr, orderBystr);
                        break;
                }
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (pageSize > 0)
            {
                var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                if (paraOut != null)
                {
                    totalRecord = Convert.ToInt32(paraOut.Value);
                }
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].ToEntityList<TResult>();
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据连接表分组按条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TGroup"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="joinType"></param>
        /// <param name="groupkeyFunc"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <returns></returns>
        public static TResult JoinGroupByFirst<TOuter, TInner, TKey, TGroup, TResult>(this TOuter outer, TInner inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, JoinType joinType, Expression<Func<TOuter, TInner, TGroup>> groupkeyFunc, Expression<Func<TGroup, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new()
        {
            string tablenameA = LambdaExtension.GetTabName(outer);
            string tablenameB = LambdaExtension.GetTabName(inner);
            List<DbParameter> dbparaList = new List<DbParameter>();
            string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
            if (!string.IsNullOrEmpty(keystrA))
            {
                tablenameA += " AS ";
                tablenameA += outerKeySelector.Parameters[0].Name;
            }
            string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
            if (!string.IsNullOrEmpty(keystrB))
            {
                tablenameB += " AS ";
                tablenameB += innerKeySelector.Parameters[0].Name;
            }
            string whereStr = string.Empty;
            Dictionary<string, string> groupColumnDit = new Dictionary<string, string>();
            string groupkeystr = GetGroupStr(groupkeyFunc, dbparaList, groupColumnDit);
            string columnStr = GetGroupColumnStr(resultSelector, dbparaList, groupColumnDit, groupkeystr);
            if (whereSelector != null)
            {
                var body = whereSelector.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                }
                else if (whereSelector.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                }
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr = string.Format(" WHERE {0}", whereStr);
                }

            }
            if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
            {
                string sql = "";
                DatabaseType databaseType = DbHelper.GetDatabaseType();
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        sql = string.Format("SELECT TOP 1 {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7};",
                            columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr,
                            groupkeystr);
                        break;
                    case DatabaseType.MySql:
                        sql = string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7} LIMIT 1;",
                            columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr,
                            groupkeystr);
                        break;
                }
                DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0].ToEntity<TResult>();
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TGroup"></typeparam>
        /// <param name="groupkeyFunc"></param>
        /// <param name="dbparaList"></param>
        /// <param name="groupColumnDit"></param>
        /// <returns></returns>
        private static string GetGroupStr<TOuter, TInner, TGroup>(Expression<Func<TOuter, TInner, TGroup>> groupkeyFunc, List<DbParameter> dbparaList,
                    Dictionary<string, string> groupColumnDit) where TOuter : class, new() where TInner : class, new()
        {
            string groupkeystr = "";
            NewExpression expkey = groupkeyFunc.Body as NewExpression;
            if (expkey != null)
            {
                for (int i = 0; i < expkey.Members.Count; i++)
                {
                    string groupkey = expkey.Members[i].Name;
                    var argu = expkey.Arguments[i];
                    string val = LambdaExtension.ExpressionRouter(argu, dbparaList, AnalyType.Column, true, OperandType.Left);
                    if (!string.IsNullOrEmpty(groupkey) && !string.IsNullOrEmpty(val))
                    {
                        if (!string.IsNullOrEmpty(groupkeystr))
                        {
                            groupkeystr += ",";
                        }
                        groupkeystr += val;
                        groupColumnDit.Add(groupkey, val);
                    }
                }
            }
            else
            {
                groupkeystr = LambdaExtension.ExpressionRouter(groupkeyFunc.Body, dbparaList, AnalyType.Column, true, OperandType.Left);
                groupkeystr += groupkeystr;
                string[] groupkeyArray = groupkeystr.Split('.');
                if (!groupColumnDit.ContainsKey(groupkeyArray[1]))
                {
                    groupColumnDit.Add(groupkeyArray[1], groupkeyArray[0]);
                }
            }
            return groupkeystr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TGroup"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="resultSelector"></param>
        /// <param name="dbparaList"></param>
        /// <param name="groupColumnDit"></param>
        /// <param name="groupkeystr"></param>
        /// <returns></returns>
        private static string GetGroupColumnStr<TGroup, TResult>(Expression<Func<TGroup, TResult>> resultSelector,
             List<DbParameter> dbparaList, Dictionary<string, string> groupColumnDit, string groupkeystr) where TResult : class, new()
        {
            string columnStr = string.Empty;
            if (resultSelector != null)
            {
                var iniexp = resultSelector.Body as MemberInitExpression;
                if (iniexp != null)
                {
                    MemberInitExpression memIniexp = iniexp;
                    foreach (var item in memIniexp.Bindings)
                    {
                        string strcolumn = "";
                        MemberAssignment memaig = item as MemberAssignment;
                        if (memaig != null)
                        {
                            if (!string.IsNullOrEmpty(columnStr))
                            {
                                columnStr += ",";
                            }
                            var constantExpression = memaig.Expression as ConstantExpression;
                            if (constantExpression != null)
                            {
                                ConstantExpression ce = constantExpression;
                                if (ce.Value == null)
                                {
                                    strcolumn = "NULL";
                                }
                                else if (ce.Value is ValueType)
                                {
                                    strcolumn = string.Format("{0}", ce.Value);
                                }
                                else
                                {
                                    strcolumn = string.Format("'{0}'", ce.Value);
                                }
                            }
                            else
                            {
                                string xcolumn = LambdaExtension.ExpressionRouter(memaig.Expression, dbparaList,
                                    AnalyType.Column, false, OperandType.Left);
                                switch (memaig.Expression.NodeType)
                                {
                                    case ExpressionType.Call:
                                        MethodCallExpression mce = memaig.Expression as MethodCallExpression;
                                        if (mce != null)
                                        {
                                            strcolumn = LambdaExtension.ExpressionRouter(mce.Arguments[0], dbparaList, AnalyType.Column, false, OperandType.Left);
                                            strcolumn = xcolumn.Replace(strcolumn, groupColumnDit[strcolumn]);
                                        }
                                        break;
                                    case ExpressionType.Convert:
                                    case ExpressionType.ConvertChecked:
                                        var convertExpression = memaig.Expression as UnaryExpression;
                                        if (convertExpression != null &&
                                            convertExpression.Operand.NodeType == ExpressionType.Call)
                                        {
                                            string[] methodArray = { "ROWCOUNT", "LEN" };
                                            var callExpression = convertExpression.Operand as MethodCallExpression;
                                            if (callExpression != null && methodArray.Any(x => x == callExpression.Method.Name.ToUpper()))
                                            {
                                                strcolumn = LambdaExtension.ExpressionRouter(callExpression.Arguments[0], dbparaList, AnalyType.Column, false, OperandType.Left);
                                                strcolumn = xcolumn.Replace(strcolumn, groupColumnDit[strcolumn]);
                                            }
                                            else
                                            {
                                                strcolumn = groupColumnDit[xcolumn];
                                            }
                                        }
                                        break;
                                    default:
                                        strcolumn = groupColumnDit[xcolumn];
                                        break;
                                }
                            }
                            if (!string.IsNullOrEmpty(strcolumn))
                            {
                                columnStr += strcolumn;
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                            }
                        }
                    }
                }
            }
            else
            {
                columnStr = groupkeystr;
            }
            return columnStr;
        }

        /// <summary>
        /// 根据连接表分组按条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TGroup"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="groupkeyFunc"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<TResult> JoinGroupByList<TOuter, TInner, TKey, TGroup, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TGroup>> groupkeyFunc, Expression<Func<TGroup, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector, Expression<Func<TOuter, TInner, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new()
        {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && groupkeyFunc != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                Dictionary<string, string> groupColumnDit = new Dictionary<string, string>();
                string groupkeystr = GetGroupStr(groupkeyFunc, dbparaList, groupColumnDit);
                string columnStr = GetGroupColumnStr(resultSelector, dbparaList, groupColumnDit, groupkeystr);
                string whereStr = string.Empty;
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList,
                            AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList,
                            AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }
                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null,
                            AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    if (pageSize > 0)
                    {
                        sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5};",
                            tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql +=
                                    string.Format(
                                        "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7} GROUP BY {8})AS T WHERE ROWID BETWEEN {9} AND {10} ",
                                        orderBystr, columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, whereStr,
                                        groupkeystr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                                break;
                            case DatabaseType.MySql:
                                sql +=
                                    string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6}  GROUP BY {7} {8} LIMIT {9},{10} ",
                                        columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA,
                                        keystrB, whereStr, groupkeystr,
                                        orderBystr, (pageIndex - 1) * pageSize, pageSize);
                                break;
                        }
                    }
                    else
                    {
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql +=
                                    string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7} {8}",
                                        columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, whereStr, groupkeystr, orderBystr);
                                break;
                            case DatabaseType.MySql:
                                sql +=
                                    string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6}  GROUP BY {7} {8} ",
                                        columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA,
                                        keystrB, whereStr, groupkeystr, orderBystr);
                                break;
                        }
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0)
                    {
                        var paraOut =
                            dbparaList.First(
                                x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null)
                        {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }
            }
            return new List<TResult>();
        }



        /// <summary>
        /// 根据连接表按条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static TResult JoinOnFirst<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereFunc)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new()
        {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null)
            {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereFunc != null)
                {
                    var body = whereFunc.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereFunc.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereFunc.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            sql = string.Format("SELECT TOP 1 {0} FROM {1} {2} JOIN {3} ON {4}={5} {6};", columnStr, tablenameA,
                                joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                            break;
                        case DatabaseType.MySql:
                            sql += string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} LIMIT 1;",
                                columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                            break;
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntity<TResult>();
                    }
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TInner2"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType1"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="inner1"></param>
        /// <param name="inner2"></param>
        /// <param name="joinType2"></param>
        /// <param name="innerKeySelector1"></param>
        /// <param name="innerKeySelector2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static TResult JoinOnFirst<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TInner inner1, TInner2 inner2, JoinType joinType2, Expression<Func<TInner, TKey2>> innerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new()
        {
            if (inner != null && inner1 != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && innerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrB1 = LambdaExtension.ExpressionRouter(innerKeySelector1.Body, null, AnalyType.Column, true, OperandType.Left);
                string keystrC = LambdaExtension.ExpressionRouter(innerKeySelector2.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC))
                {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }

                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            sql +=
                                string.Format(
                                    "SELECT TOP 1 {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11};",
                                    columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                    keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC,
                                    whereStr, orderBystr);
                            break;
                        case DatabaseType.MySql:
                            sql +=
                                string.Format(
                                    "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} LIMIT 1;",
                                    columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                    keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC, whereStr,
                                    orderBystr);
                            break;
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntity<TResult>();
                    }
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TInner2"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType1"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="outer1"></param>
        /// <param name="inner2"></param>
        /// <param name="joinType2"></param>
        /// <param name="outerKeySelector1"></param>
        /// <param name="innerKeySelector2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static TResult JoinOnFirst<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TOuter outer1, TInner2 inner2, JoinType joinType2, Expression<Func<TOuter, TKey2>> outerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new()
        {
            if (inner != null && outer1 != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && outerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrA1 = LambdaExtension.ExpressionRouter(outerKeySelector1.Body, null, AnalyType.Column, true, OperandType.Left);
                string keystrC = LambdaExtension.ExpressionRouter(innerKeySelector2.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC))
                {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }

                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();

                    switch (databaseType)
                    {
                        case DatabaseType.SqlServer:
                            sql += string.Format(
                                "SELECT TOP 1 {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11};",
                                columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                                orderBystr);
                            break;
                        case DatabaseType.MySql:
                            sql += string.Format(
                                "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} LIMIT 1;",
                                columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                                orderBystr);
                            break;
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntity<TResult>();
                    }
                }
            }
            return default(TResult);
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static List<TResult> JoinOnList<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereFunc)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new()
        {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null)
            {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }

                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereFunc != null)
                {
                    var body = whereFunc.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereFunc.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereFunc.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6}", columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<TResult> JoinOnList<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector, Expression<Func<TOuter, TInner, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new()
        {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }

                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    if (pageSize > 0)
                    {
                        sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5};",
                            tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql +=
                                    string.Format(
                                        "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7})AS T WHERE ROWID BETWEEN {8} AND {9} ",
                                        orderBystr, columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, whereStr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                                break;
                            case DatabaseType.MySql:
                                sql += string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7} LIMIT {8},{9} ",
                                    columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr, orderBystr,
                                    (pageIndex - 1) * pageSize, pageSize);
                                break;
                        }
                    }
                    else
                    {
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql += string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7}",
                                        columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, whereStr, orderBystr);
                                break;
                            case DatabaseType.MySql:
                                sql += string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7}",
                                        columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, whereStr, orderBystr);
                                break;
                        }
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0)
                    {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null)
                        {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }

            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TInner2"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType1"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="inner1"></param>
        /// <param name="inner2"></param>
        /// <param name="joinType2"></param>
        /// <param name="innerKeySelector1"></param>
        /// <param name="innerKeySelector2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<TResult> JoinOnList<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TInner inner1, TInner2 inner2, JoinType joinType2, Expression<Func<TInner, TKey2>> innerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new()
        {
            if (inner != null && inner1 != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && innerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrB1 = LambdaExtension.ExpressionRouter(innerKeySelector1.Body, null, AnalyType.Column, true, OperandType.Left);
                string keystrC = LambdaExtension.ExpressionRouter(innerKeySelector2.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC))
                {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }

                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    if (pageSize > 0)
                    {
                        sql =
                            string.Format(
                                "SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5} JOIN {6} ON {7}={8} {9};",
                                tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA, keystrB,
                                joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql +=
                                    string.Format(
                                        "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7} JOIN {8} ON {9}={10} {11})AS T WHERE ROWID BETWEEN {12} AND {13}",
                                        orderBystr, columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC,
                                        whereStr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                                break;
                            case DatabaseType.MySql:
                                sql +=
                                    string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} LIMIT {12},{13}",
                                        columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                        keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC, whereStr,
                                        orderBystr, (pageIndex - 1) * pageSize, pageSize);
                                break;
                        }
                    }
                    else
                    {
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql +=
                                    string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                                        columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                        keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC,
                                        whereStr, orderBystr);
                                break;
                            case DatabaseType.MySql:
                                sql +=
                                    string.Format(
                                        "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} ",
                                        columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                        keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC, whereStr,
                                        orderBystr);
                                break;
                        }
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0)
                    {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null)
                        {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据连接表按条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TInner2"></typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="joinType1"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="outer1"></param>
        /// <param name="inner2"></param>
        /// <param name="joinType2"></param>
        /// <param name="outerKeySelector1"></param>
        /// <param name="innerKeySelector2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<TResult> JoinOnList<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TOuter outer1, TInner2 inner2, JoinType joinType2, Expression<Func<TOuter, TKey2>> outerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new()
        {
            if (inner != null && outer1 != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && outerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null)
            {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = LambdaExtension.ExpressionRouter(outerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA))
                {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = LambdaExtension.ExpressionRouter(innerKeySelector.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB))
                {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrA1 = LambdaExtension.ExpressionRouter(outerKeySelector1.Body, null, AnalyType.Column, true, OperandType.Left);
                string keystrC = LambdaExtension.ExpressionRouter(innerKeySelector2.Body, null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC))
                {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null)
                {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else
                {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null)
                    {
                        MemberInitExpression memIniexp = iniexp;
                        foreach (var item in memIniexp.Bindings)
                        {
                            MemberAssignment memaig = item as MemberAssignment;
                            if (memaig != null)
                            {
                                var constantExpression = memaig.Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    ConstantExpression ce = constantExpression;
                                    if (ce.Value == null)
                                    {
                                        columnStr += "NULL";
                                    }
                                    else if (ce.Value is ValueType)
                                    {
                                        columnStr += string.Format("{0}", ce.Value);
                                    }
                                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char || ce.Value is Guid)
                                    {
                                        columnStr += string.Format("'{0}'", ce.Value);
                                    }
                                }
                                else
                                {
                                    columnStr += LambdaExtension.ExpressionRouter(memaig.Expression, null, AnalyType.Column, true, OperandType.Left);
                                }
                                columnStr += string.Format(" AS [{0}]", item.Member.Name);
                                columnStr += ",";
                            }
                        }
                        columnStr = columnStr.TrimEnd(',');
                    }
                }
                if (whereSelector != null)
                {
                    var body = whereSelector.Body as BinaryExpression;
                    if (body != null)
                    {
                        BinaryExpression be = body;
                        whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, AnalyType.Param, true);
                    }
                    else if (whereSelector.Body is MethodCallExpression)
                    {
                        whereStr = LambdaExtension.CallExpression(whereSelector.Body, dbparaList, AnalyType.Param, true);
                    }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                }

                if (funOrder != null)
                {
                    var fbody = funOrder.Body as BinaryExpression;
                    if (fbody != null)
                    {
                        BinaryExpression be = fbody;
                        orderBystr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, null, AnalyType.Order, true);
                    }
                    else if (funOrder.Body is MethodCallExpression)
                    {
                        orderBystr = LambdaExtension.CallExpression(funOrder.Body, null, AnalyType.Order, true);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB))
                {
                    string sql = "";
                    DatabaseType databaseType = DbHelper.GetDatabaseType();
                    if (pageSize > 0)
                    {
                        sql = string.Format(
                            "SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5} JOIN {6} ON {7}={8} {9};",
                            tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA, keystrB,
                            joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql += string.Format(
                                    "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) AS ROWID ,{1} FROM {2} {3} JOIN {4} ON {5}={6} {7} JOIN {8} ON {9}={10} {11})AS T WHERE ROWID BETWEEN {12} AND {13} ",
                                    orderBystr, columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                    keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC,
                                    whereStr, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                                break;
                            case DatabaseType.MySql:
                                sql += string.Format(
                                    "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11} LIMIT {12},{13}",
                                    columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                    keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                                    orderBystr, (pageIndex - 1) * pageSize, pageSize);
                                break;
                        }
                    }
                    else
                    {
                        switch (databaseType)
                        {
                            case DatabaseType.SqlServer:
                                sql += string.Format(
                                    "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                                    columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                    keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                                    orderBystr);
                                break;
                            case DatabaseType.MySql:
                                sql += string.Format(
                                    "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                                    columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                                    keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                                    orderBystr);
                                break;
                        }
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0)
                    {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null)
                        {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ToEntityList<TResult>();
                    }
                }
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体数据条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int GetRowCount<T>(this T entity, Expression<Func<T, bool>> func) where T : class
        {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null)
            {
                var body = func.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList,
                        AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param,
                        false);
                }
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr = string.Format(" WHERE {0}", whereStr);
                }
            }
            string sql = string.Format("SELECT COUNT(1) FROM {0} {1}", tableName, whereStr);
            return DbHelper.ExecuteCount(sql, dbparaList.ToArray());
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool GetExists<T>(this T entity, Expression<Func<T, bool>> func) where T : class
        {
            List<DbParameter> dbparaList = new List<DbParameter>();
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            if (func != null)
            {
                var body = func.Body as BinaryExpression;
                if (body != null)
                {
                    BinaryExpression be = body;
                    whereStr = LambdaExtension.BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList,
                        AnalyType.Param, false);
                }
                else if (func.Body is MethodCallExpression)
                {
                    whereStr = LambdaExtension.CallExpression(func.Body, dbparaList, AnalyType.Param,
                        false);
                }
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr = string.Format(" WHERE {0}", whereStr);
                }
            }
            string sql = "";
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    sql = string.Format(@"IF EXISTS(SELECT 1 FROM {0} {1})
                        BEGIN
	                        select 1
                        END
                        ELSE
                        BEGIN
	                        select 0
                        end
                        ", tableName, whereStr);
                    break;
                case DatabaseType.MySql:
                    sql = string.Format("SELECT COUNT(1) FROM {0} {1}", tableName, whereStr);
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                object obj = DbHelper.ExecuteScalar(CommandType.Text, sql, dbparaList.ToArray());
                if (obj != null)
                {
                    return Convert.ToInt32(obj) > 0;
                }
            }
            return false;
        }
        #endregion
    }
}

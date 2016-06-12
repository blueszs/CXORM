using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using CXData.ADO;
using CXData.Helper;

namespace CXData.ORM {
    /// <summary>
    /// ORM扩展类
    /// 20150625-周盛-添加
    /// </summary>
    public static class OrmExtension {
        #region Entity By ORM
        /// <summary>
        /// 根据条件删除数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Delete<T>(this T entity, Expression<Func<T, bool>> func) where T : class, new() {
            List<DbParameter> dbparaList = new List<DbParameter>();
            string sql = string.Format("DELETE FROM {0}", LambdaExtension.GetTabName(entity));
            string whereStr = string.Empty;
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                sql += string.Format(" WHERE {0}", whereStr);
            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparaList.ToArray()) > 0;
        }

        /// <summary>
        /// 根据条件更新数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="updateEntity"></param>
        /// <param name="funColumns"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Update<T, TKey>(this T entity, T updateEntity, Expression<Func<T, TKey>> funColumns, Expression<Func<T, bool>> func) where T : class {
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (updateEntity != null && funColumns != null) {
                string whereStr = string.Empty;
                if (func != null) {
                    whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
                }
                var updateColumns = LambdaExtension.GetUpdateColumns(funColumns);
                string sql = LambdaExtension.GetUpdateSql(updateEntity, updateColumns, whereStr, dbparaList);
                if (!string.IsNullOrEmpty(sql)) {
                    return DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparaList.ToArray()) > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据条件更新数据库中对应的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="funColumns"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Update<T, TKey>(this T entity, Expression<Func<T, TKey>> funColumns, Expression<Func<T, bool>> func) where T : class {
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (entity != null && funColumns != null) {
                string whereStr = string.Empty;
                if (func != null) {
                    whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
                }
                var updateColumns = LambdaExtension.GetUpdateColumns(funColumns);
                string sql = LambdaExtension.GetUpdateSql(entity, updateColumns, whereStr, dbparaList);
                if (!string.IsNullOrEmpty(sql)) {
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
        public static int Insert<T>(this T entity) where T : class {
            int index = 0;
            if (entity != null) {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string sql = LambdaExtension.GetInsertSql(entity, dbparaList);
                if (!string.IsNullOrEmpty(sql)) {
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
        public static int Insert<T>(this T entity, T insertEntity) where T : class {
            int index = 0;
            if (insertEntity != null) {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string sql = LambdaExtension.GetInsertSql(insertEntity, dbparaList);
                if (!string.IsNullOrEmpty(sql)) {
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
        /// <returns></returns>
        public static T Find<T>(this T entity, Expression<Func<T, bool>> func) where T : class, new() {
            return entity.Find(func, null);
        }

        /// <summary>
        /// 根据条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static T Find<T>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, bool>> funOrder) where T : class, new() {
            string saName = "";
            string whereStr = string.Empty;
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                saName = func.Parameters[0].Name;
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
            }
            string tablename = LambdaExtension.GetTabName(entity);
            if (!string.IsNullOrEmpty(saName)) {
                tablename = string.Format("{0} AS {1}", tablename, saName);
            }
            string strColumns = "*";
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = " WHERE " + whereStr;
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, false, OperandType.Left);
            }
            string sql = DbHelper.GetSelectLimitSql(tablename, strColumns, whereStr, orderBystr, 1);
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0) {
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
        public static TResult Find<T, TResult>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new() {
            return entity.Find(func, resultSelector, null);
        }


        /// <summary>
        /// 根据条件返回数据库中对应的第一个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="resultSelector"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static TResult Find<T, TResult>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, TResult>> resultSelector, Expression<Func<T, bool>> funOrder)
            where T : class
            where TResult : class, new() {
            string saName = "";
            string whereStr = string.Empty;
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                saName = func.Parameters[0].Name;
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
            }
            string tablename = LambdaExtension.GetTabName(entity);
            if (!string.IsNullOrEmpty(saName)) {
                tablename = string.Format("{0} AS {1}", tablename, saName);
            }
            string columnStr = "";
            var expression = resultSelector.Body as ParameterExpression;
            if (expression != null) {
                ParameterExpression para = expression;
                columnStr += para.Name;
                columnStr += ".*";
            }
            else {
                columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = " WHERE " + whereStr;
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, false, OperandType.Left);
            }
            string sql = DbHelper.GetSelectLimitSql(tablename, columnStr, whereStr, orderBystr, 1);
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0) {
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
        public static TResult Find<T, TKey, TResult>(this T source, Expression<Func<T, TKey>> keySelector, Expression<Func<T, TResult>> resultSelector, Expression<Func<T, bool>> whereFunc)
            where T : class
            where TResult : class, new() {
            if (keySelector != null) {
                string tablename = LambdaExtension.GetTabName(source);
                string keystr = string.Empty;
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                NewExpression expkey = keySelector.Body as NewExpression;
                if (expkey != null) {
                    foreach (var argu in expkey.Arguments) {
                        string groupkey = argu.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                        if (!string.IsNullOrEmpty(groupkey)) {
                            keystr += groupkey;
                            keystr += ",";
                        }
                    }
                    keystr = keystr.TrimEnd(',');
                }
                else {
                    keystr = keySelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (resultSelector != null) {
                    var iniexp = resultSelector.Body as MemberInitExpression;
                    if (iniexp != null) {
                        columnStr = iniexp.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                    }
                }
                else {
                    columnStr = keystr;
                }
                if (whereFunc != null) {
                    whereStr = whereFunc.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(keystr) && !string.IsNullOrEmpty(tablename)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = DbHelper.GetGroupLimitSql(tablename, columnStr, whereStr, keystr, "", 1);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<T, TKey, TResult>(this T source, Expression<Func<T, TKey>> keySelector, Expression<Func<T, TResult>> resultSelector, Expression<Func<T, bool>> whereFunc)
            where T : class
            where TResult : class, new() {
            if (keySelector != null) {
                string tablename = LambdaExtension.GetTabName(source);
                string keystr = string.Empty;
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                NewExpression expkey = keySelector.Body as NewExpression;
                if (expkey != null) {
                    foreach (var argu in expkey.Arguments) {
                        string groupkey = argu.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                        if (!string.IsNullOrEmpty(groupkey)) {
                            keystr += groupkey;
                            keystr += ",";
                        }
                    }
                    keystr = keystr.TrimEnd(',');
                }
                else {
                    keystr = keySelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (resultSelector != null) {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                else {
                    columnStr = keystr;
                }
                if (whereFunc != null) {
                    whereStr = whereFunc.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(keystr) && !string.IsNullOrEmpty(tablename)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = string.Format("SELECT {0} FROM {1} {2} GROUP BY {3}", columnStr, tablename, whereStr, keystr);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static List<T> Where<T>(this T entity, Expression<Func<T, bool>> func)
            where T : class, new() {
            return Where(entity, func, 0, null);
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
        public static List<TResult> Where<T, TResult>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new() {
            return Where(entity, func, 0, null, resultSelector);
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
        public static List<T> Where<T>(this T entity, Expression<Func<T, bool>> func, int rowNum, Expression<Func<T, bool>> funOrder)
            where T : class, new() {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = DbHelper.GetSelectLimitSql(tableName, "*", whereStr, orderBystr, rowNum);
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<T, TResult>(this T entity, Expression<Func<T, bool>> func, int rowNum, Expression<Func<T, bool>> funOrder, Expression<Func<T, TResult>> resultSelector)
            where T : class
            where TResult : class, new() {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, false, OperandType.Left);
            }
            string columnStr = "";
            var expressionResult = resultSelector.Body as ParameterExpression;
            if (expressionResult != null) {
                columnStr += "*";
            }
            else {
                columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = DbHelper.GetSelectLimitSql(tableName, columnStr, whereStr, orderBystr, rowNum);
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (ds != null && ds.Tables.Count > 0) {
                return ds.Tables[0].ToEntityList<TResult>();
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 根据条件返回数据库中对应的实体列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="func"></param>
        /// <param name="funOrder"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="funColumns"></param>
        /// <returns></returns>
        public static List<T> Where<T, TKey>(this T entity, Expression<Func<T, bool>> func,
            Expression<Func<T, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord,
            Expression<Func<T, TKey>> funColumns = null)
            where T : class, new() {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = "";
            string columnStr = "";
            if (funColumns != null) {
                var expressionResult = funColumns.Body as ParameterExpression;
                if (expressionResult != null) {
                    columnStr += "*";
                }
                else {
                    columnStr = funColumns.Body.ExpressionRouter(dbparaList, AnalyType.Column, false, OperandType.Left);
                }
            }
            if (pageSize > 0) {
                sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM {0} {1};", tableName, whereStr);
                DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                dbparaList.Add(param);
                sql += DbHelper.GetPageSql(tableName, columnStr, whereStr, orderBystr, pageSize, pageIndex);
            }
            else {
                sql += string.Format("SELECT {0} FROM {1} {2} {3}", columnStr, tableName, whereStr, orderBystr);
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (pageSize > 0) {
                var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                if (paraOut != null) {
                    totalRecord = Convert.ToInt32(paraOut.Value);
                }
            }
            if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<T, TResult>(this T entity, Expression<Func<T, bool>> func,
            Expression<Func<T, bool>> funOrder, Expression<Func<T, TResult>> resultSelector, int pageSize, int pageIndex, ref int totalRecord)
            where T : class
            where TResult : class, new() {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            string orderBystr = string.Empty;
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (funOrder != null) {
                orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = "";
            string columnStr = "";
            var expressionResult = resultSelector.Body as ParameterExpression;
            if (expressionResult != null) {
                ParameterExpression para = expressionResult;
                columnStr += para.Name;
                columnStr += ".*";
            }
            else {
                columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
            }
            if (pageSize > 0) {
                sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM {0} {1};", tableName, whereStr);
                DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                dbparaList.Add(param);
                sql += DbHelper.GetPageSql(tableName, columnStr, whereStr, orderBystr, pageSize, pageIndex);
            }
            else {
                sql += string.Format("SELECT {0} FROM {1} {2} {3}", columnStr, tableName, whereStr, orderBystr);
            }
            DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
            if (pageSize > 0) {
                var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                if (paraOut != null) {
                    totalRecord = Convert.ToInt32(paraOut.Value);
                }
            }
            if (ds != null && ds.Tables.Count > 0) {
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
        public static TResult Find<TOuter, TInner, TKey, TGroup, TResult>(this TOuter outer, TInner inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, JoinType joinType, Expression<Func<TOuter, TInner, TGroup>> groupkeyFunc, Expression<Func<TGroup, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new() {
            string tablenameA = LambdaExtension.GetTabName(outer);
            string tablenameB = LambdaExtension.GetTabName(inner);
            List<DbParameter> dbparaList = new List<DbParameter>();
            string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
            if (!string.IsNullOrEmpty(keystrA)) {
                tablenameA += " AS ";
                tablenameA += outerKeySelector.Parameters[0].Name;
            }
            string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
            if (!string.IsNullOrEmpty(keystrB)) {
                tablenameB += " AS ";
                tablenameB += innerKeySelector.Parameters[0].Name;
            }
            string whereStr = string.Empty;
            Dictionary<string, string> groupColumnDit = new Dictionary<string, string>();
            string groupkeystr = GetGroupStr(groupkeyFunc, dbparaList, groupColumnDit);
            string columnStr = GetGroupColumnStr(resultSelector, dbparaList, groupColumnDit, groupkeystr);
            if (whereSelector != null) {
                whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                if (!string.IsNullOrEmpty(whereStr)) {
                    whereStr = string.Format(" WHERE {0}", whereStr);
                }
                string sql = DbHelper.GetJoinGroupLimitSql(tablenameA, tablenameB, keystrA, keystrB, joinType.ToString().ToUpper(), columnStr, whereStr, groupkeystr, "", 1);
                DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                if (ds != null && ds.Tables.Count > 0) {
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
                    Dictionary<string, string> groupColumnDit) where TOuter : class, new() where TInner : class, new() {
            string groupkeystr = "";
            if (groupkeyFunc != null) {
                groupkeystr = groupkeyFunc.Body.ExpressionRouter(dbparaList, AnalyType.Group, true, OperandType.Left, groupColumnDit);
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
             List<DbParameter> dbparaList, Dictionary<string, string> groupColumnDit, string groupkeystr) where TResult : class, new() {
            string columnStr;
            if (resultSelector != null) {
                columnStr = resultSelector.Body.ExpressionRouter(dbparaList,
                                    AnalyType.Column, false, OperandType.Left, groupColumnDit);
            }
            else {
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
        public static List<TResult> Where<TOuter, TInner, TKey, TGroup, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TGroup>> groupkeyFunc, Expression<Func<TGroup, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector, Expression<Func<TGroup, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new() {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && groupkeyFunc != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                Dictionary<string, string> groupColumnDit = new Dictionary<string, string>();
                string groupkeystr = GetGroupStr(groupkeyFunc, dbparaList, groupColumnDit);
                string columnStr = GetGroupColumnStr(resultSelector, dbparaList, groupColumnDit, groupkeystr);
                string whereStr = string.Empty;
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }
                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left, groupColumnDit);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = "";
                    if (pageSize > 0) {
                        sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5};",
                            tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        sql += DbHelper.GetJoinGroupPageSql(tablenameA, tablenameB, keystrA, keystrB,
                            joinType.ToString().ToUpper(), columnStr, whereStr, groupkeystr, orderBystr, pageSize,
                            pageIndex);
                    }
                    else {
                        sql += string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} GROUP BY {7} {8}",
                                columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                keystrA, keystrB, whereStr, groupkeystr, orderBystr);
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0) {
                        var paraOut =
                            dbparaList.First(
                                x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null) {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static TResult Find<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereFunc)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new() {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null) {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereFunc != null) {
                    whereStr = whereFunc.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = DbHelper.GetJoinLimitSql(tablenameA, tablenameB, keystrA, keystrB, joinType.ToString().ToUpper(),
                        columnStr, whereStr, "", 1);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static TResult Find<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TInner inner1, TInner2 inner2, JoinType joinType2, Expression<Func<TInner, TKey2>> innerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new() {
            if (inner != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && innerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrB1 = innerKeySelector1.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                string keystrC = innerKeySelector2.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC)) {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }

                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = DbHelper.GetJoinLimitSql(tablenameA, tablenameB, tablenameC, keystrA, keystrB, keystrB1, keystrC, joinType1.ToString().ToUpper()
                        , joinType2.ToString().ToUpper(), columnStr, whereStr, orderBystr, 1);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        /// <param name="outer2"></param>
        /// <param name="inner2"></param>
        /// <param name="joinType2"></param>
        /// <param name="outerKeySelector1"></param>
        /// <param name="innerKeySelector2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="whereSelector"></param>
        /// <param name="funOrder"></param>
        /// <returns></returns>
        public static TResult Find<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TOuter outer2, TInner2 inner2, JoinType joinType2, Expression<Func<TOuter, TKey2>> outerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new() {
            if (inner != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && outerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrA1 = outerKeySelector1.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                string keystrC = innerKeySelector2.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC)) {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }
                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = DbHelper.GetJoinLimitSql(tablenameA, tablenameB, tablenameC, keystrA, keystrB, keystrA1, keystrC, joinType1.ToString().ToUpper()
                        , joinType2.ToString().ToUpper(), columnStr, whereStr, orderBystr, 1);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereFunc)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new() {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null) {
                List<DbParameter> dbparaList = new List<DbParameter>();
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string whereStr = string.Empty;
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }

                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereFunc != null) {
                    whereStr = whereFunc.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);

                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = string.Format("SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6}", columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<TOuter, TInner, TKey, TResult>(this TOuter outer, TInner inner, JoinType joinType, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Expression<Func<TOuter, TInner, bool>> whereSelector, Expression<Func<TOuter, TInner, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TResult : class, new() {
            if (inner != null && outerKeySelector != null && innerKeySelector != null && resultSelector != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true,
                    OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }

                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = "";
                    if (pageSize > 0) {
                        sql = string.Format("SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5};",
                            tablenameA, joinType.ToString().ToUpper(), tablenameB, keystrA, keystrB, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        sql += DbHelper.GetJoinPageSql(tablenameA, tablenameB, keystrA, keystrB,
                            joinType.ToString().ToUpper(), columnStr, whereStr, orderBystr, pageSize, pageIndex);
                    }
                    else {
                        sql += string.Format(
                                "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} {7}",
                                columnStr, tablenameA, joinType.ToString().ToUpper(), tablenameB,
                                keystrA, keystrB, whereStr, orderBystr);
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0) {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null) {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static List<TResult> Where<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TInner inner1, TInner2 inner2, JoinType joinType2, Expression<Func<TInner, TKey2>> innerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new() {
            if (inner != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && innerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrB1 = innerKeySelector1.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                string keystrC = innerKeySelector2.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC)) {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }

                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = "";
                    if (pageSize > 0) {
                        sql =
                            string.Format(
                                "SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5} JOIN {6} ON {7}={8} {9};",
                                tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA, keystrB,
                                joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        sql += DbHelper.GetJoinPageSql(tablenameA, tablenameB, tablenameC, keystrA, keystrB, joinType1.ToString().ToUpper(),
                            keystrB1, keystrC, joinType2.ToString().ToUpper(), columnStr, whereStr, orderBystr, pageSize, pageIndex);
                    }
                    else {
                        sql += string.Format(
                                "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                                columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrB1, keystrC,
                                whereStr, orderBystr);

                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0) {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null) {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0) {
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
        /// <param name="outer2"></param>
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
        public static List<TResult> Where<TOuter, TInner, TInner2, TKey1, TKey2, TResult>(this TOuter outer, TInner inner, JoinType joinType1, Expression<Func<TOuter, TKey1>> outerKeySelector, Expression<Func<TInner, TKey1>> innerKeySelector, TOuter outer2, TInner2 inner2, JoinType joinType2, Expression<Func<TOuter, TKey2>> outerKeySelector1, Expression<Func<TInner2, TKey2>> innerKeySelector2, Expression<Func<TOuter, TInner, TInner2, TResult>> resultSelector, Expression<Func<TOuter, TInner, TInner2, bool>> whereSelector, Expression<Func<TOuter, TInner, TInner2, bool>> funOrder, int pageSize, int pageIndex, ref int totalRecord)
            where TOuter : class, new()
            where TInner : class, new()
            where TInner2 : class, new()
            where TResult : class, new() {
            if (inner != null && inner2 != null && outerKeySelector != null && innerKeySelector != null && outerKeySelector1 != null && innerKeySelector2 != null && resultSelector != null) {
                string tablenameA = LambdaExtension.GetTabName(outer);
                string tablenameB = LambdaExtension.GetTabName(inner);
                string tablenameC = LambdaExtension.GetTabName(inner2);
                string columnStr = string.Empty;
                string orderBystr = string.Empty;
                List<DbParameter> dbparaList = new List<DbParameter>();
                string keystrA = outerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrA)) {
                    tablenameA += " AS ";
                    tablenameA += outerKeySelector.Parameters[0].Name;
                }
                string keystrB = innerKeySelector.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrB)) {
                    tablenameB += " AS ";
                    tablenameB += innerKeySelector.Parameters[0].Name;
                }
                string keystrA1 = outerKeySelector1.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                string keystrC = innerKeySelector2.Body.ExpressionRouter(null, AnalyType.Column, true, OperandType.Left);
                if (!string.IsNullOrEmpty(keystrC)) {
                    tablenameC += " AS ";
                    tablenameC += innerKeySelector2.Parameters[0].Name;
                }
                string whereStr = string.Empty;
                var expression = resultSelector.Body as ParameterExpression;
                if (expression != null) {
                    ParameterExpression para = expression;
                    columnStr += para.Name;
                    columnStr += ".*";
                }
                else {
                    columnStr = resultSelector.Body.ExpressionRouter(dbparaList, AnalyType.Column, true, OperandType.Left);
                }
                if (whereSelector != null) {
                    whereStr = whereSelector.Body.ExpressionRouter(dbparaList, AnalyType.Param, true, OperandType.Left);
                }

                if (funOrder != null) {
                    orderBystr = funOrder.Body.ExpressionRouter(null, AnalyType.Order, true, OperandType.Left);
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(keystrA) && !string.IsNullOrEmpty(keystrB)) {
                    if (!string.IsNullOrEmpty(whereStr)) {
                        whereStr = string.Format(" WHERE {0}", whereStr);
                    }
                    string sql = "";
                    if (pageSize > 0) {
                        sql = string.Format(
                            "SELECT @TotalRecord = COUNT(1) FROM  {0} {1} JOIN {2} ON {3}={4} {5} JOIN {6} ON {7}={8} {9};",
                            tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA, keystrB,
                            joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr);
                        DbParameter param = DbHelper.CreateOutDbParameter("@TotalRecord", DbType.Int32);
                        dbparaList.Add(param);
                        sql += string.Format(
                                "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                                columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB,
                                keystrA, keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC,
                                whereStr, orderBystr);
                    }
                    else {
                        sql += string.Format(
                            "SELECT {0} FROM {1} {2} JOIN {3} ON {4}={5} {6} JOIN {7} ON {8}={9} {10} {11}",
                            columnStr, tablenameA, joinType1.ToString().ToUpper(), tablenameB, keystrA,
                            keystrB, joinType2.ToString().ToUpper(), tablenameC, keystrA1, keystrC, whereStr,
                            orderBystr);
                    }
                    DataSet ds = DbHelper.ExecuteDataSet(CommandType.Text, sql, dbparaList.ToArray());
                    if (pageSize > 0) {
                        var paraOut = dbparaList.First(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output);
                        if (paraOut != null) {
                            totalRecord = Convert.ToInt32(paraOut.Value);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0) {
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
        public static int RowCount<T>(this T entity, Expression<Func<T, bool>> func) where T : class {
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            List<DbParameter> dbparaList = new List<DbParameter>();
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
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
        public static bool Exists<T>(this T entity, Expression<Func<T, bool>> func) where T : class {
            List<DbParameter> dbparaList = new List<DbParameter>();
            string whereStr = string.Empty;
            string tableName = LambdaExtension.GetTabName(entity);
            if (func != null) {
                whereStr = func.Body.ExpressionRouter(dbparaList, AnalyType.Param, false, OperandType.Left);
            }
            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = string.Format(" WHERE {0}", whereStr);
            }
            string sql = "";
            DatabaseType databaseType = DbHelper.GetDatabaseType();
            switch (databaseType) {
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
            if (!string.IsNullOrEmpty(sql)) {
                object obj = DbHelper.ExecuteScalar(CommandType.Text, sql, dbparaList.ToArray());
                if (obj != null) {
                    return Convert.ToInt32(obj) > 0;
                }
            }
            return false;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CXData.ADO;
using CXData.Helper;

namespace CXData.ORM
{
    /// <summary>
    /// Lambda表达式解析类
    /// 20150625-周盛-添加
    /// </summary>
    public static class LambdaExtension
    {
        #region Lambda to SQL

        #region 数据库关键字
        /// <summary>
        /// In 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool In<T>(this T obj, params T[] array)
        {
            return true;
        }

        /// <summary>
        /// Not In 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T obj, params T[] array)
        {
            return true;
        }

        /// <summary>
        /// Like 查询操作
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr"></param>
        /// <returns></returns>
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }

        /// <summary>
        /// NotLike 查询操作
        /// </summary>
        /// <param name="str"></param>
        /// <param name="likeStr"></param>
        /// <returns></returns>
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }

        /// <summary>
        /// 升序查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Asc<T>(this T obj)
        {
            return true;
        }

        /// <summary>
        /// 降序查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Desc<T>(this T obj)
        {
            return true;
        }

        /// <summary>
        /// 随机产生Guid随机排序查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool RandSort<T>(this T obj)
        {
            return true;
        }

        /// <summary>
        /// As别名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T As<T>(this T obj, string name)
        {
            return obj;
        }

        /// <summary>
        /// 字段数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static object[] Columns<T>(this T obj, params object[] array)
        {
            return array;
        }

        /// <summary>
        /// 获取行记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int RowCount<T>(this T obj)
        {
            return 1;
        }

        /// <summary>
        /// Sum 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T SumValue<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        /// Max 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T MaxValue<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        ///  Min 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T MinValue<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        /// Avg 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T AvgValue<T>(this T obj)
        {
            return obj;
        }

        /// <summary>
        /// 数据库Len函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int Len<T>(this T obj)
        {
            return 0;
        }

        /// <summary>
        /// 数据库Left函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="len">返回的左边长度</param>
        /// <returns></returns>
        public static T Left<T>(this T obj, int len)
        {
            return obj;
        }

        /// <summary>
        /// 数据库Right函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="len">返回的右边长度</param>
        /// <returns></returns>
        public static T Right<T>(this T obj, int len)
        {
            return obj;
        }

        /// <summary>
        /// 数据库Convert函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRt"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TRt ConvertType<T, TRt>(this T obj)
        {
            return default(TRt);
        }

        /// <summary>
        /// 数据库判断Name=''
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T obj)
        {
            return true;
        }

        /// <summary>
        /// 数据库判断不为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this T obj)
        {
            return true;
        }

        #endregion

        #region 表达式拼接
        /// <summary>
        /// 合并表达式，它表示仅在第一个操作数解析为 true 时才计算第二个操作数的条件 AND 运算。 可指定实现方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlsoExp<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        /// <summary>
        /// 合并表达式，它表示仅在第一个操作数解析为 true 时才计算第二个操作数的条件 AND 运算。 可指定实现方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, T2, bool>> AndAlsoExp<T, T2>(this Expression<Func<T, T2, bool>> exp, Expression<Func<T, T2, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, T2, bool>>(Expression.AndAlso(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        /// <summary>
        ///  合并表达式，它表示仅在第一个操作数解析为 true 时才计算第二个操作数的条件 AND 运算。 可指定实现方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, T2, T3, bool>> AndAlsoExp<T, T2, T3>(this Expression<Func<T, T2, T3, bool>> exp, Expression<Func<T, T2, T3, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, T2, T3, bool>>(Expression.AndAlso(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        /// <summary>
        /// 表达式合并，它表示仅在第一个操作数的计算结果为 false 时才计算第二个操作数的条件 OR 运算。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElseExp<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        /// <summary>
        /// 表达式合并，它表示仅在第一个操作数的计算结果为 false 时才计算第二个操作数的条件 OR 运算。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, T2, bool>> OrElseExp<T, T2>(this Expression<Func<T, T2, bool>> exp, Expression<Func<T, T2, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, T2, bool>>(Expression.OrElse(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        /// <summary>
        /// 表达式合并，它表示仅在第一个操作数的计算结果为 false 时才计算第二个操作数的条件 OR 运算。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="exp"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression<Func<T, T2, T3, bool>> OrElseExp<T, T2, T3>(this Expression<Func<T, T2, T3, bool>> exp, Expression<Func<T, T2, T3, bool>> right)
        {
            if (exp != null)
            {
                return Expression.Lambda<Func<T, T2, T3, bool>>(Expression.OrElse(exp.Body, right.Body), exp.Parameters);
            }
            return right;
        }

        #endregion

        /// <summary>
        /// 表达式方法解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases"></param>
        /// <returns></returns>
        public static string CallExpression(Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases)
        {
            string ruesltStr = string.Empty;
            MethodCallExpression mce = exp as MethodCallExpression;
            if (mce != null)
            {
                switch (mce.Method.Name.ToUpper())
                {
                    case "LIKE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("({0} LIKE {1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "NOTLIKE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("({0} NOT LIKE {1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "IN":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("{0} IN({1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "NOTIN":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("{0} NOT IN ({1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "ASC":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], null, analyType, isAliases, OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" ORDER BY {0} ASC", leftStr);
                            }
                        }
                        break;
                    case "DESC":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], null, analyType, isAliases, OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" ORDER BY {0} DESC", leftStr);
                            }
                        }
                        break;
                    case "AS":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = ExpressionRouter(mce.Arguments[1], null, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format(" {0} AS {1}", leftStr, rightStr);
                            }
                        }
                        break;
                    case "ROWCOUNT":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" COUNT({0})", leftStr);
                            }
                        }
                        break;
                    case "SUMVALUE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" SUM({0})", leftStr);
                            }
                        }
                        break;
                    case "MAXVALUE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" MAX({0})", leftStr);
                            }
                        }
                        break;
                    case "MINVALUE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" MIN({0})", leftStr);
                            }
                        }
                        break;
                    case "AVGVALUE":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" AVG({0})", leftStr);
                            }
                        }
                        break;
                    case "ISEMPTY":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], null, analyType, isAliases, OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" {0} IS NULL", leftStr);
                            }
                        }
                        break;
                    case "ISNOTEMPTY":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], null, analyType, isAliases, OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" {0} IS NOT NULL AND {0} <> ''", leftStr);
                            }
                        }
                        break;
                    case "LEN":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                ruesltStr = string.Format(" LEN ({0})", leftStr);
                            }
                        }
                        break;
                    case "LEFT":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format(" LEFT ({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "RIGHT":
                        {
                            string leftStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            string rightStr = ExpressionRouter(mce.Arguments[1], dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format(" RIGHT ({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "CONVERTTYPE":
                        {
                            string leftStr = mce.Method.ReturnType.ConvertDbType();
                            string rightStr = ExpressionRouter(mce.Arguments[0], dbparaList, analyType, isAliases,
                                OperandType.Left);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format(" CONVERT({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    default:
                        {
                            ruesltStr = DynamicInvokeExpression(exp, mce.Method.Name, analyType, dbparaList);
                        }
                        break;
                }
            }
            return ruesltStr;
        }

        /// <summary>
        /// 表达式动态编译调用
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dataFliex"></param>
        /// <param name="analyType"></param>
        /// <param name="dbparaList"></param>
        /// <returns></returns>
        public static string DynamicInvokeExpression(Expression exp, string dataFliex, AnalyType analyType, List<DbParameter> dbparaList)
        {
            string ruesltStr = string.Empty;
            var result = Expression.Lambda(exp).Compile().DynamicInvoke();
            if (result is Array)
            {
                Array resulitArray = result as Array;
                for (int i = 0; i < resulitArray.Length; i++)
                {
                    object objitem = resulitArray.GetValue(i);
                    if (objitem == null)
                    {
                        ruesltStr += "NULL";
                        ruesltStr += ",";
                    }
                    else
                    {
                        if (dbparaList != null && analyType != AnalyType.Order)
                        {
                            DbParameter para = DbHelper.CreateInDbParameter("@" + dataFliex, objitem.GetType().GetDbType(), objitem);
                            if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.DbType == para.DbType && x.Value.Equals(para.Value)))
                            {
                                if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                {
                                    para.ParameterName = "@" + dataFliex + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                                }
                                dbparaList.Add(para);
                            }
                            ruesltStr += para.ParameterName;
                            ruesltStr += ",";
                        }
                        else
                        {
                            ruesltStr = objitem.ToString();
                            ruesltStr += ",";
                        }
                    }
                }
                ruesltStr = ruesltStr.TrimEnd(',');
            }
            else
            {
                if (result == null)
                {
                    ruesltStr = "NULL";
                }
                else
                {
                    if (dbparaList != null && analyType != AnalyType.Order)
                    {
                        DbParameter para = DbHelper.CreateInDbParameter("@" + dataFliex, result.GetType().GetDbType(), result);
                        if (!dbparaList.IsAny(x => x.ParameterName == para.ParameterName && x.DbType == para.DbType && x.Value.Equals(para.Value)))
                        {
                            if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                            {
                                para.ParameterName = "@" + dataFliex + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                            }
                            dbparaList.Add(para);
                        }
                        return para.ParameterName;
                    }
                    ruesltStr = result.GetType() == typeof(ValueType) ? result.ToString() : string.Format("'{0}'", result);
                }
            }
            return ruesltStr;
        }

        /// <summary>
        /// 二元表达式解析
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="type"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases"></param>
        /// <returns></returns>
        public static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases)
        {
            StringBuilder sb = new StringBuilder();
            string tmpStrLen;
            switch (analyType)
            {
                case AnalyType.Param:
                    sb.Append("(");
                    sb.Append(ExpressionRouter(left, dbparaList, analyType, isAliases, OperandType.Left));
                    tmpStrLen = ExpressionRouter(right, dbparaList, analyType, isAliases, OperandType.Right);
                    if (tmpStrLen.ToUpper() == "NULL")
                    {
                        switch (type)
                        {
                            case ExpressionType.Equal:
                                sb.Append(" IS NULL");
                                break;
                            case ExpressionType.NotEqual:
                                sb.Append(" IS NOT NULL");
                                break;
                            default:
                                sb.Append(ExpressionTypeCast(type));
                                sb.Append(tmpStrLen);
                                break;
                        }
                    }
                    else if (!string.IsNullOrEmpty(tmpStrLen))
                    {
                        sb.Append(ExpressionTypeCast(type));
                        sb.Append(tmpStrLen);
                    }
                    sb.Append(")");
                    break;
                case AnalyType.Order:
                    sb.Append(ExpressionRouter(left, null, analyType, isAliases, OperandType.Left));
                    tmpStrLen = ExpressionRouter(right, null, analyType, isAliases, OperandType.Left);
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                        tmpStrLen = tmpStrLen.Replace("ORDER BY ", "");
                    }
                    sb.Append(tmpStrLen);
                    break;
                case AnalyType.Column:
                    sb.Append(ExpressionRouter(left, null, analyType, isAliases, OperandType.Left));
                    tmpStrLen = ExpressionRouter(right, null, analyType, isAliases, OperandType.Left);
                    if (tmpStrLen.ToUpper() == "NULL")
                    {
                        switch (type)
                        {
                            case ExpressionType.Equal:
                                sb.Append(" IS NULL");
                                break;
                            case ExpressionType.NotEqual:
                                sb.Append(" IS NOT NULL");
                                break;
                            default:
                                sb.Append(ExpressionTypeCast(type));
                                sb.Append(tmpStrLen);
                                break;
                        }
                    }
                    else
                    {
                        sb.Append(ExpressionTypeCast(type));
                        sb.Append(tmpStrLen);
                    }
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 表达式解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases">是否别名</param>
        /// <param name="operaType"></param>
        /// <returns></returns>
        public static string ExpressionRouter(Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, OperandType operaType)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.NotEqual:
                case ExpressionType.OrElse:
                    {
                        BinaryExpression be = exp as BinaryExpression;
                        if (be != null)
                        {
                            return BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, analyType, isAliases);
                        }
                        return DynamicInvokeExpression(exp, exp.NodeType.ToString(), analyType, dbparaList);
                    }
                case ExpressionType.MemberAccess:
                    {
                        var expression = exp as MemberExpression;
                        if (expression != null)
                        {
                            if (expression.Expression.NodeType == ExpressionType.Parameter)
                            {
                                if (isAliases)
                                {
                                    return expression.ToString();
                                }
                                return expression.Member.Name;
                            }
                            MemberExpression mes = expression.Expression as MemberExpression;
                            if (mes != null && operaType == OperandType.Left)
                            {
                                return ExpressionRouter(mes, dbparaList, analyType, isAliases, operaType);
                            }
                        }
                        return DynamicInvokeExpression(exp, exp.NodeType.ToString(), analyType, dbparaList);
                    }
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    {
                        var arrayExpression = exp as NewArrayExpression;
                        StringBuilder tmpstr = new StringBuilder();
                        if (arrayExpression != null)
                        {
                            tmpstr.Append(string.Join(",", arrayExpression.Expressions.Select(x => ExpressionRouter(x, dbparaList, analyType, isAliases, operaType)).ToArray()));
                        }
                        return tmpstr.ToString();
                    }
                case ExpressionType.Call:
                    {
                        var callExpression = exp as MethodCallExpression;
                        return CallExpression(callExpression, dbparaList, analyType, isAliases);
                    }
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    {
                        var convertExpression = exp as UnaryExpression;
                        if (convertExpression != null && convertExpression.Operand.NodeType == ExpressionType.Call)
                        {
                            string[] methodArray = { "ROWCOUNT", "LEN" };
                            var callExpression = convertExpression.Operand as MethodCallExpression;
                            if (callExpression != null && methodArray.Any(x => x == callExpression.Method.Name.ToUpper()))
                            {
                                return CallExpression(callExpression, dbparaList, analyType, isAliases);
                            }
                        }
                        return DynamicInvokeExpression(exp, exp.NodeType.ToString(), analyType, dbparaList);
                    }
                case ExpressionType.Constant:
                    {
                        var ce = exp as ConstantExpression;
                        if (ce != null)
                        {
                            if (ce.Value == null)
                            {
                                return "null";
                            }
                            if (dbparaList != null && analyType != AnalyType.Order)
                            {
                                DbParameter para = DbHelper.CreateInDbParameter("@" + ce.NodeType, ce.Value.GetType().GetDbType(), ce.Value);
                                if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.DbType == para.DbType && x.Value.Equals(para.Value)))
                                {
                                    if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                    {
                                        para.ParameterName = "@" + ce.NodeType + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                                    }
                                    dbparaList.Add(para);
                                }
                                return para.ParameterName;
                            }
                        }
                        return DynamicInvokeExpression(exp, exp.NodeType.ToString(), analyType, dbparaList);
                    }
                default:
                    {
                        return DynamicInvokeExpression(exp, exp.NodeType.ToString(), analyType, dbparaList);
                    }
            }
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funColumns"></param>
        /// <param name="saName"></param>
        /// <param name="dbparaList"></param>
        /// <returns></returns>
        public static string GetColumns<T>(Expression<Func<T, object[]>> funColumns, string saName, ref List<DbParameter> dbparaList) where T : class, new()
        {
            string strColumns = "*";
            if (funColumns != null)
            {
                MethodCallExpression methodCall = funColumns.Body as MethodCallExpression;
                if (methodCall != null && methodCall.Method.Name.ToUpper() == "COLUMNS")
                {
                    NewArrayExpression expressionParams = methodCall.Arguments[1] as NewArrayExpression;
                    if (expressionParams != null && expressionParams.Expressions.Any())
                    {
                        strColumns = "";
                        foreach (Expression ex in expressionParams.Expressions)
                        {
                            if (!string.IsNullOrEmpty(strColumns))
                            {
                                strColumns += ",";
                            }
                            UnaryExpression ue = ex as UnaryExpression;
                            if (ue != null)
                            {
                                MethodCallExpression met = ue.Operand as MethodCallExpression;
                                if (met != null)
                                {
                                    switch (met.Method.Name.ToUpper())
                                    {
                                        case "AS":
                                            strColumns += ExpressionRouter(ex, dbparaList, AnalyType.Column, false, OperandType.Left);
                                            break;
                                        case "SUMVALUE":
                                            strColumns += string.Format("{0}", ExpressionRouter(ex, null, AnalyType.Column, true, OperandType.Left));
                                            break;
                                        default:
                                            if (string.IsNullOrEmpty(saName))
                                            {
                                                strColumns += ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left);
                                            }
                                            else
                                            {
                                                strColumns += string.Format("{0}.{1}", saName, ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left));
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(saName))
                                    {
                                        strColumns += ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left);
                                    }
                                    else
                                    {
                                        strColumns += string.Format("{0}.{1}", saName, ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left));
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(saName))
                                {
                                    strColumns += ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left);
                                }
                                else
                                {
                                    strColumns += string.Format("{0}.{1}", saName, ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left));
                                }
                            }
                        }
                    }
                }
            }
            return strColumns;
        }

        /// <summary>
        /// 获取实体新增的sql语句并返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obje"></param>
        /// <param name="dbparaList"></param>
        /// <returns></returns>
        public static string GetInsertSql<T>(T obje, List<DbParameter> dbparaList)
        {
            Type type = obje.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            if (ps.Length > 0)
            {
                string tableName = type.Name;
                StringBuilder columnNamestr = new StringBuilder();
                StringBuilder columnValuestr = new StringBuilder();
                var queryps = ps.Where(x => x.CanRead);
                foreach (System.Reflection.PropertyInfo i in queryps)
                {
                    try
                    {
                        TableAttribute[] cusAttrs = i.GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];
                        if (cusAttrs != null && cusAttrs.Length > 0)
                        {
                            if (cusAttrs[0].Identity)
                            {
                                continue;
                            }
                        }
                        string name = i.Name;
                        object obj = i.GetValue(obje, null);
                        if (obj != null)
                        {
                            if (columnNamestr.Length > 0)
                            {
                                columnNamestr.Append(",");
                                columnValuestr.Append(",");
                            }
                            columnNamestr.Append(name);
                            Type attrValType = i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(i.PropertyType) : i.PropertyType;
                            DbParameter para = DbHelper.CreateInDbParameter("@" + name, attrValType.GetDbType(), obj);
                            if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.DbType == para.DbType && x.Value.Equals(para.Value)))
                            {
                                if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                {
                                    para.ParameterName = "@" + name + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                                }
                                dbparaList.Add(para);
                            }
                            columnValuestr.Append(para.ParameterName);

                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
                if (columnNamestr.Length > 0)
                {
                    string sql = string.Format("INSERT INTO {0}({1})VALUES({2}); ", tableName, columnNamestr, columnValuestr);
                    switch (DbHelper.GetDatabaseType())
                    {
                        case DatabaseType.SqlServer:
                            sql += "SELECT SCOPE_IDENTITY();";
                            break;
                        case DatabaseType.MySql:
                            sql += "SELECT LAST_INSERT_ID();";
                            break;
                    }
                    return sql;
                }
            }
            return "";
        }

        /// <summary>
        /// 获取实体修改的sql语句并返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obje"></param>
        /// <param name="dataField"></param>
        /// <param name="strWhere"></param>
        /// <param name="dbparaList"></param>
        /// <returns></returns>
        public static string GetUpdateSql<T>(T obje, string[] dataField, string strWhere, List<DbParameter> dbparaList)
        {
            Type type = obje.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            if (ps.Length > 0)
            {
                string sql = string.Empty;
                var query = ps.Where(a => ((dataField == null || dataField.Length == 0) || dataField.Contains(a.Name)) && a.CanRead);
                var propertyInfos = query as System.Reflection.PropertyInfo[] ?? query.ToArray();
                if (propertyInfos.Any())
                {
                    foreach (System.Reflection.PropertyInfo i in propertyInfos)
                    {
                        string name = i.Name;
                        object obj = i.GetValue(obje, null);
                        if (obj != null)
                        {
                            if (string.IsNullOrEmpty(sql))
                            {
                                sql = string.Format("UPDATE {0} SET ", type.Name);
                            }
                            else
                            {
                                sql += ",";
                            }
                            Type attrValType = i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(i.PropertyType) : i.PropertyType;
                            DbParameter para = DbHelper.CreateInDbParameter("@" + name, attrValType.GetDbType(), obj);
                            if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.DbType == para.DbType && x.Value.Equals(para.Value)))
                            {
                                if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                {
                                    para.ParameterName = "@" + name + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                                }
                                dbparaList.Add(para);
                            }
                            sql += string.Format("{0}={1}", name, para.ParameterName);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sql))
                            {
                                sql = string.Format("UPDATE {0} SET ", type.Name);
                            }
                            else
                            {
                                sql += ",";
                            }
                            sql += string.Format("{0}= NULL", name);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(strWhere))
                {
                    sql += strWhere.Trim().StartsWith("WHERE", StringComparison.OrdinalIgnoreCase) ? " " + strWhere : " WHERE " + strWhere;
                }
                return sql;
            }
            return "";
        }

        /// <summary>
        /// 获取实体更新的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funColumns"></param>
        /// <returns></returns>
        public static string[] GetUpdateColumns<T>(Expression<Func<T, object[]>> funColumns) where T : new()
        {
            List<string> strColumns = new List<string>();
            if (funColumns != null)
            {
                MethodCallExpression methodCall = funColumns.Body as MethodCallExpression;
                if (methodCall != null && methodCall.Method.Name.ToUpper() == "COLUMNS")
                {
                    NewArrayExpression expressionParams = methodCall.Arguments[1] as NewArrayExpression;
                    if (expressionParams != null && expressionParams.Expressions.Count > 0)
                    {
                        strColumns.AddRange(expressionParams.Expressions.Select(ex => ExpressionRouter(ex, null, AnalyType.Column, false, OperandType.Left)));
                    }
                }
            }
            string[] updateColumns = strColumns.ToArray();
            return updateColumns;
        }

        /// <summary>
        /// 表达式解析的sql关键字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " =";
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " Or ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 表达式解析调用
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static object InternalCall(Expression exp)
        {
            ConstantExpression cexp = exp as ConstantExpression;
            if (cexp != null)
            {
                return cexp.Value;
            }
            ParameterExpression pexp = exp as ParameterExpression;
            if (pexp != null)
            {
                return pexp;
            }
            return Expression.Lambda(exp).Compile().DynamicInvoke();
        }

        #endregion
    }
}

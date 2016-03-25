using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        /// In 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool In<T>(this T obj, IList<T> array)
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
        ///  Not In 查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T obj, IList<T> array)
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
        /// 字段数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="obj"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static TKey Columns<T, TKey>(this T obj, TKey array)
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
        /// <param name="leftname"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string CallExpression(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType,
            bool isAliases, string leftname = "", Dictionary<string, string> groupDic = null)
        {
            string outParaName = string.Empty;
            return exp.CallExpression(dbparaList, analyType, isAliases, ref outParaName, leftname, groupDic);
        }

        /// <summary>
        /// 表达式方法解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases"></param>
        /// <param name="leftname"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string CallExpression(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, ref string outParaName, string leftname = "", Dictionary<string, string> groupDic = null)
        {
            string ruesltStr = string.Empty;
            MethodCallExpression mce = exp as MethodCallExpression;
            if (mce != null)
            {
                switch (mce.Method.Name.ToUpper())
                {
                    case "LIKE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, outParaName);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("({0} LIKE {1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "NOTLIKE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, outParaName);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format("({0} NOT LIKE {1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "IN":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, outParaName);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                if (string.IsNullOrEmpty(rightStr))
                                {
                                    rightStr = "NULL";
                                }
                                ruesltStr = string.Format("{0} IN({1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "NOTIN":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, outParaName);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                if (string.IsNullOrEmpty(rightStr))
                                {
                                    rightStr = "NULL";
                                }
                                ruesltStr = string.Format("{0} NOT IN ({1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "ASC":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(null, analyType, isAliases, OperandType.Left, ref outParaName);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                if (groupDic != null && groupDic.ContainsKey(outParaName))
                                {
                                    ruesltStr = string.Format(" ORDER BY {0} ASC", groupDic[outParaName]);
                                }
                                else
                                {
                                    ruesltStr = string.Format(" ORDER BY {0} ASC", leftStr);
                                }
                            }
                        }
                        break;
                    case "DESC":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(null, analyType, isAliases, OperandType.Left, ref outParaName);
                            if (!string.IsNullOrEmpty(leftStr))
                            {
                                if (groupDic != null && groupDic.ContainsKey(outParaName))
                                {
                                    ruesltStr = string.Format(" ORDER BY {0} DESC", groupDic[outParaName]);
                                }
                                else
                                {
                                    ruesltStr = string.Format(" ORDER BY {0} DESC", leftStr);
                                }
                            }
                        }
                        break;
                    case "AS":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left);
                            string rightStr = mce.Arguments[1].ExpressionRouter(null, analyType, isAliases,
                                OperandType.Right);
                            if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr))
                            {
                                ruesltStr = string.Format(" {0} AS {1}", leftStr, rightStr);
                            }
                        }
                        break;
                    case "ROWCOUNT":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                    OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" COUNT({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" COUNT({0})", leftStr);
                            }
                        }
                        break;
                    case "SUMVALUE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases, OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" SUM({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" SUM({0})", leftStr);
                            }
                        }
                        break;
                    case "MAXVALUE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases, OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" MAX({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" MAX({0})", leftStr);
                            }
                        }
                        break;
                    case "MINVALUE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" MIN({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" MIN({0})", leftStr);
                            }
                        }
                        break;
                    case "AVGVALUE":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" AVG({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" AVG({0})", leftStr);
                            }
                        }
                        break;
                    case "LEN":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" LEN({0})", leftStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" LEN({0})", leftStr);
                            }
                        }
                        break;
                    case "LEFT":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right);

                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" LEFT ({0},{1})", leftStr, rightStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" LEFT ({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "RIGHT":
                        {
                            string leftStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right, ref outParaName);
                            string rightStr = mce.Arguments[1].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Right);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" RIGHT ({0},{1})", leftStr, rightStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" RIGHT ({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    case "CONVERTTYPE":
                        {
                            string leftStr = mce.Method.ReturnType.ConvertDbType();
                            string rightStr = mce.Arguments[0].ExpressionRouter(dbparaList, analyType, isAliases,
                                OperandType.Left, ref outParaName);
                            if (analyType == AnalyType.Group)
                            {
                                if (!string.IsNullOrEmpty(leftStr) && !string.IsNullOrEmpty(rightStr) && groupDic != null)
                                {
                                    groupDic.Add(leftStr, string.Format(" CONVERT ({0},{1})", leftStr, rightStr));
                                    ruesltStr = leftStr;
                                }
                            }
                            else
                            {
                                ruesltStr = string.Format(" CONVERT ({0},{1})", leftStr, rightStr);
                            }
                        }
                        break;
                    default:
                        {
                            ruesltStr = exp.DynamicInvokeExpression(mce.Method.Name, analyType, dbparaList, leftname);
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
        /// <param name="leftname"></param>
        /// <returns></returns>
        internal static string DynamicInvokeExpression(this Expression exp, string dataFliex, AnalyType analyType, List<DbParameter> dbparaList, string leftname = "")
        {
            string ruesltStr = string.Empty;
            string paraName = string.IsNullOrEmpty(leftname) ? dataFliex : leftname;
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
                            DbParameter para = DbHelper.CreateInDbParameter("@" + paraName, objitem);
                            if (
                                !dbparaList.Any(
                                    x =>
                                        x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
                            {
                                if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                {
                                    para.ParameterName = "@" + paraName +
                                                         dbparaList.Count(
                                                             x => x.ParameterName.StartsWith(para.ParameterName));
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
            else if (result is IList)
            {
                IList resulitList = result as IList;
                foreach (var item in resulitList)
                {
                    if (item == null)
                    {
                        ruesltStr += "NULL";
                        ruesltStr += ",";
                    }
                    else
                    {
                        if (dbparaList != null && analyType != AnalyType.Order)
                        {
                            DbParameter para = DbHelper.CreateInDbParameter("@" + paraName, item);
                            if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
                            {
                                if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                                {
                                    para.ParameterName = "@" + paraName +
                                                         dbparaList.Count(
                                                             x => x.ParameterName.StartsWith(para.ParameterName));
                                }
                                dbparaList.Add(para);
                            }
                            ruesltStr += para.ParameterName;
                            ruesltStr += ",";
                        }
                        else
                        {
                            ruesltStr = item.ToString();
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
                        DbParameter para = DbHelper.CreateInDbParameter("@" + paraName, result);
                        if (!dbparaList.IsAny(x => x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
                        {
                            if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                            {
                                para.ParameterName = "@" + paraName +
                                                     dbparaList.Count(
                                                         x => x.ParameterName.StartsWith(para.ParameterName));
                            }
                            dbparaList.Add(para);
                        }
                        return para.ParameterName;
                    }
                    ruesltStr = result.GetType() == typeof(ValueType)
                        ? result.ToString()
                        : string.Format("'{0}'", result);
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
        /// <param name="outParaName"></param>
        /// <param name="leftname"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, ref string outParaName, string leftname, Dictionary<string, string> groupDic)
        {
            StringBuilder sb = new StringBuilder();
            string tmpStrLen;
            switch (analyType)
            {
                case AnalyType.Param:
                    {
                        string paraName = string.Empty;
                        sb.Append("(");
                        sb.Append(left.ExpressionRouter(dbparaList, analyType, isAliases, OperandType.Left, ref paraName));
                        tmpStrLen = right.ExpressionRouter(dbparaList, analyType, isAliases, OperandType.Right, paraName);
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
                    }
                case AnalyType.Order:
                    {
                        sb.Append(left.ExpressionRouter(null, analyType, isAliases, OperandType.Left, ref outParaName, leftname, groupDic));
                        tmpStrLen = right.ExpressionRouter(null, analyType, isAliases, OperandType.Left, ref outParaName, leftname, groupDic);
                        if (sb.Length > 0)
                        {
                            sb.Append(",");
                            tmpStrLen = tmpStrLen.Replace("ORDER BY ", "");
                        }
                        sb.Append(tmpStrLen);
                        break;
                    }
                case AnalyType.Column:
                    {
                        sb.Append(left.ExpressionRouter(null, analyType, isAliases, OperandType.Left));
                        tmpStrLen = right.ExpressionRouter(null, analyType, isAliases, OperandType.Left);
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
        /// <param name="leftname"></param>
        /// <returns></returns>
        internal static string ExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, OperandType operaType, string leftname = "")
        {
            string outParaName = string.Empty;
            return exp.ExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, leftname);
        }

        /// <summary>
        /// 表达式解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases">是否别名</param>
        /// <param name="operaType"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string ExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, OperandType operaType, Dictionary<string, string> groupDic)
        {
            string outParaName = string.Empty;
            return exp.ExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, null, groupDic);
        }

        /// <summary>
        /// 表达式解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases">是否别名</param>
        /// <param name="operaType"></param>
        /// <param name="outParaName"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string ExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, OperandType operaType, ref string outParaName, Dictionary<string, string> groupDic)
        {
            return exp.ExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, null, groupDic);
        }

        /// <summary>
        /// 表达式解析
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="dbparaList"></param>
        /// <param name="analyType"></param>
        /// <param name="isAliases">是否别名</param>
        /// <param name="operaType"></param>
        /// <param name="outParaName"></param>
        /// <param name="leftname"></param>
        /// <param name="groupDic"></param>
        /// <returns></returns>
        internal static string ExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases, OperandType operaType, ref string outParaName, string leftname = null, Dictionary<string, string> groupDic = null)
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
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    {
                        BinaryExpression be = exp as BinaryExpression;
                        if (be != null)
                        {
                            return BinarExpressionProvider(be.Left, be.Right, be.NodeType, dbparaList, analyType, isAliases, ref outParaName, leftname, groupDic);
                        }
                        return exp.DynamicInvokeExpression(exp.NodeType.ToString(), analyType, dbparaList, leftname);
                    }
                case ExpressionType.MemberAccess:
                    {
                        return exp.MemberDynamicInvokeExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, leftname);
                    }
                case ExpressionType.MemberInit:
                    {
                        return exp.MemberDynamicExpressionInitRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, leftname, groupDic);
                    }
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    {
                        return exp.NewArrayDynamicExpressionInitRouter(dbparaList, analyType, isAliases, operaType, leftname);
                    }
                case ExpressionType.Call:
                    {
                        return exp.CallExpression(dbparaList, analyType, isAliases, ref outParaName, leftname, groupDic);
                    }
                case ExpressionType.New:
                    {
                        return exp.NewDynamicInvokeExpressionRouter(dbparaList, analyType, isAliases, ref outParaName, leftname, groupDic);
                    }
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    {
                        return exp.ConvertDynamicInvokeExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, leftname, groupDic);
                    }
                case ExpressionType.Constant:
                    {
                        return exp.ConstantDynamicInvokeExpressionRouter(dbparaList, analyType, leftname);
                    }
                default:
                    {
                        return exp.DynamicInvokeExpression(exp.NodeType.ToString(), analyType, dbparaList, leftname);
                    }
            }
        }

        internal static string NewArrayDynamicExpressionInitRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType,
            bool isAliases, OperandType operaType, string leftname)
        {
            var arrayExpression = exp as NewArrayExpression;
            StringBuilder tmpstr = new StringBuilder();
            if (arrayExpression != null)
            {
                tmpstr.Append(string.Join(",",
                    arrayExpression.Expressions.Select(
                        x => x.ExpressionRouter(dbparaList, analyType, isAliases, operaType, leftname)).ToArray()));
            }
            return tmpstr.ToString();
        }

        internal static string NewDynamicInvokeExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType
            , bool isAliases, ref string outParaName, string leftname, Dictionary<string, string> groupDic)
        {
            string tmpstr = "";
            var newExpression = exp as NewExpression;
            if (newExpression != null)
            {
                for (int n = 0; n < newExpression.Arguments.Count; n++)
                {
                    string attrstr = newExpression.Arguments[n].ExpressionRouter(dbparaList, AnalyType.Group, isAliases,
                        OperandType.Left, ref outParaName, leftname, groupDic);
                    if (!string.IsNullOrEmpty(attrstr))
                    {
                        if (analyType == AnalyType.Group)
                        {
                            if (groupDic != null && groupDic.ContainsKey(attrstr))
                            {
                                groupDic.Add(newExpression.Members[n].Name, groupDic[attrstr]);
                                groupDic.Remove(attrstr);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(tmpstr))
                                {
                                    tmpstr += ",";
                                }
                                if (groupDic != null)
                                {
                                    groupDic.Add(newExpression.Members[n].Name, attrstr);
                                }
                                tmpstr += attrstr;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(tmpstr))
                            {
                                tmpstr += ",";
                            }
                            tmpstr += attrstr;
                        }
                    }
                }
            }
            return tmpstr;
        }

        internal static string ConstantDynamicInvokeExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType,
            string leftname)
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
                    string paraName = string.IsNullOrEmpty(leftname) ? ce.NodeType.ToString() : leftname;
                    DbParameter para = DbHelper.CreateInDbParameter("@" + paraName, ce.Value);
                    if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
                    {
                        if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                        {
                            para.ParameterName = "@" + paraName +
                                                 dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                        }
                        dbparaList.Add(para);
                    }
                    return para.ParameterName;
                }
            }
            return exp.DynamicInvokeExpression(exp.NodeType.ToString(), analyType, dbparaList, leftname);
        }

        internal static string ConvertDynamicInvokeExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType,
            bool isAliases, OperandType operaType, ref string outParaName, string leftname, Dictionary<string, string> groupDic)
        {
            var convertExpression = exp as UnaryExpression;
            if (analyType != AnalyType.Column)
            {
                if (convertExpression != null && convertExpression.Operand.NodeType == ExpressionType.Call)
                {
                    string[] methodArray = { "ROWCOUNT", "LEN" };
                    var callExpression = convertExpression.Operand as MethodCallExpression;
                    if (callExpression != null &&
                        methodArray.Any(x => x == callExpression.Method.Name.ToUpper()))
                    {
                        return callExpression.CallExpression(dbparaList, analyType, isAliases, leftname, groupDic);
                    }
                }
                return exp.DynamicInvokeExpression(exp.NodeType.ToString(), analyType, dbparaList, leftname);
            }
            if (convertExpression != null)
            {
                return convertExpression.Operand.ExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName, leftname);
            }
            return "";
        }

        internal static string MemberDynamicExpressionInitRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases,
            OperandType operaType, ref string outParaName, string leftname, Dictionary<string, string> groupDic)
        {
            string strExp = "";
            var expression = exp as MemberInitExpression;
            if (expression != null)
            {
                foreach (var item in expression.Bindings)
                {
                    outParaName = "";
                    MemberAssignment memaig = item as MemberAssignment;
                    if (memaig != null)
                    {
                        if (!string.IsNullOrEmpty(strExp) && analyType == AnalyType.Column)
                        {
                            strExp += ",";
                        }
                        var constantExpression = memaig.Expression as ConstantExpression;
                        if (constantExpression != null)
                        {
                            ConstantExpression ce = constantExpression;
                            if (ce.Value == null)
                            {
                                strExp += "NULL";
                            }
                            else if (ce.Value is ValueType)
                            {
                                strExp += string.Format("{0}", ce.Value);
                            }
                            else
                            {
                                strExp += string.Format("'{0}'", ce.Value);
                            }
                        }
                        else
                        {
                            if (groupDic != null && groupDic.Any())
                            {
                                string attrName = memaig.Expression.ExpressionRouter(dbparaList
                                    , analyType, false, operaType, ref outParaName, leftname);
                                if (groupDic.ContainsKey(outParaName))
                                {
                                    strExp += attrName.Replace(outParaName, groupDic[outParaName]);
                                }
                                else
                                {
                                    strExp += attrName;
                                }
                            }
                            else
                            {
                                strExp += memaig.Expression.ExpressionRouter(dbparaList
                                    , analyType, isAliases, operaType, ref outParaName, leftname);
                            }
                        }
                        strExp += string.Format(" AS [{0}]", item.Member.Name);
                    }
                }
            }
            return strExp;
        }

        internal static string MemberDynamicInvokeExpressionRouter(this Expression exp, List<DbParameter> dbparaList, AnalyType analyType, bool isAliases,
            OperandType operaType, ref string outParaName, string leftname)
        {
            var expression = exp as MemberExpression;
            if (expression != null)
            {
                outParaName = expression.Member.Name;
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
                    return mes.ExpressionRouter(dbparaList, analyType, isAliases, operaType, ref outParaName);
                }
            }
            return exp.DynamicInvokeExpression(exp.NodeType.ToString(), analyType, dbparaList, leftname);
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="funColumns"></param>
        /// <param name="saName"></param>
        /// <param name="dbparaList"></param>
        /// <returns></returns>
        internal static string GetColumns<T, TKey>(this Expression<Func<T, TKey>> funColumns, string saName, ref List<DbParameter> dbparaList) where T : class, new()
        {
            string strColumns = "*";
            if (funColumns != null)
            {
                MethodCallExpression methodCall = funColumns.Body as MethodCallExpression;
                if (methodCall != null && methodCall.Method.Name.ToUpper() == "COLUMNS")
                {
                    switch (methodCall.Arguments[1].NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            {
                                strColumns = methodCall.Arguments[1].ExpressionRouter(null, AnalyType.Column, false, OperandType.Left);
                                break;
                            }
                        case ExpressionType.NewArrayBounds:
                        case ExpressionType.NewArrayInit:
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
                                                        strColumns += ex.ExpressionRouter(dbparaList, AnalyType.Column,
                                                            false, OperandType.Left);
                                                        break;
                                                    case "SUMVALUE":
                                                        strColumns += string.Format("{0}",
                                                            ex.ExpressionRouter(null, AnalyType.Column, true,
                                                                OperandType.Left));
                                                        break;
                                                    default:
                                                        if (string.IsNullOrEmpty(saName))
                                                        {
                                                            strColumns += ex.ExpressionRouter(null, AnalyType.Column, false,
                                                                OperandType.Left);
                                                        }
                                                        else
                                                        {
                                                            strColumns += string.Format("{0}.{1}", saName,
                                                                ex.ExpressionRouter(null, AnalyType.Column, false,
                                                                    OperandType.Left));
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(saName))
                                                {
                                                    strColumns += ex.ExpressionRouter(null, AnalyType.Column, false,
                                                        OperandType.Left);
                                                }
                                                else
                                                {
                                                    strColumns += string.Format("{0}.{1}", saName,
                                                        ex.ExpressionRouter(null, AnalyType.Column, false, OperandType.Left));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(saName))
                                            {
                                                strColumns += ex.ExpressionRouter(null, AnalyType.Column, false,
                                                    OperandType.Left);
                                            }
                                            else
                                            {
                                                strColumns += string.Format("{0}.{1}", saName,
                                                    ex.ExpressionRouter(null, AnalyType.Column, false, OperandType.Left));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
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
        internal static string GetInsertSql<T>(T obje, List<DbParameter> dbparaList) where T : class
        {
            Type type = obje.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            if (ps.Length > 0)
            {
                string tableName = GetTabName(obje);
                StringBuilder columnNamestr = new StringBuilder();
                StringBuilder columnValuestr = new StringBuilder();
                var queryps = ps.Where(x => x.CanRead);
                bool identity = false;
                foreach (System.Reflection.PropertyInfo i in queryps)
                {
                    try
                    {
                        IdentityAttribute[] cusAttrs = i.GetCustomAttributes(typeof(IdentityAttribute), true) as IdentityAttribute[];
                        if (cusAttrs != null && cusAttrs.Any())
                        {
                            identity = true;
                            continue;
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
                            DbParameter para = DbHelper.CreateInDbParameter("@" + name, obj);
                            if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
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
                    sql += identity ? DbHelper.GetIDENTITYSql() : DbHelper.GetRowCoutSql();
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
        internal static string GetUpdateSql<T>(T obje, string[] dataField, string strWhere, List<DbParameter> dbparaList) where T : class
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
                    string tabName = GetTabName(obje);
                    foreach (System.Reflection.PropertyInfo i in propertyInfos)
                    {
                        string name = i.Name;
                        object obj = i.GetValue(obje, null);
                        if (string.IsNullOrEmpty(sql))
                        {
                            sql = string.Format("UPDATE {0} SET ", tabName);
                        }
                        else
                        {
                            sql += ",";
                        }
                        DbParameter para = DbHelper.CreateInDbParameter("@" + name, obj);
                        if (!dbparaList.Any(x => x.ParameterName == para.ParameterName && x.Value.Equals(para.Value)))
                        {
                            if (dbparaList.Any(x => x.ParameterName.StartsWith(para.ParameterName)))
                            {
                                para.ParameterName = "@" + name + dbparaList.Count(x => x.ParameterName.StartsWith(para.ParameterName));
                            }
                            dbparaList.Add(para);
                        }
                        sql += string.Format("{0}={1}", name, para.ParameterName);
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
        /// <typeparam name="TKey"></typeparam>
        /// <param name="funColumns"></param>
        /// <returns></returns>
        internal static string[] GetUpdateColumns<T, TKey>(Expression<Func<T, TKey>> funColumns) where T : class
        {
            List<string> strColumns = new List<string>();
            MethodCallExpression methodCall = funColumns != null ? funColumns.Body as MethodCallExpression : null;
            if (methodCall != null && methodCall.Method.Name.ToUpper() == "COLUMNS")
            {
                switch (methodCall.Arguments[1].NodeType)
                {
                    case ExpressionType.MemberAccess:
                        {
                            var expression = methodCall.Arguments[1] as MemberExpression;
                            if (expression != null)
                            {
                                strColumns.Add(expression.ExpressionRouter( null, AnalyType.Column, false, OperandType.Left));
                            }
                            break;
                        }
                    case ExpressionType.NewArrayBounds:
                    case ExpressionType.NewArrayInit:
                        {
                            NewArrayExpression expressionParams = methodCall.Arguments[1] as NewArrayExpression;
                            if (expressionParams != null && expressionParams.Expressions.Count > 0)
                            {
                                strColumns.AddRange(expressionParams.Expressions.Select(ex => ex.ExpressionRouter(null, AnalyType.Column, false, OperandType.Left)));
                            }
                            break;
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
        internal static string ExpressionTypeCast(ExpressionType type)
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
        /// 获取实体映射的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obje"></param>
        /// <returns></returns>
        internal static string GetTabName<T>(T obje) where T : class
        {
            string tabName = "";
            TableAttribute[] cusAttrs = typeof(T).GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];
            if (cusAttrs != null && cusAttrs.Length > 0)
            {
                if (!string.IsNullOrEmpty(cusAttrs[0].Name))
                {
                    tabName = cusAttrs[0].Name;
                }
            }
            else
            {
                tabName = typeof(T).Name;
            }
            return tabName;
        }
        #endregion
    }
}

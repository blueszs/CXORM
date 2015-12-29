using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CXData.Helper
{
    /// <summary>
    /// 实体工具类
    /// 20150625-周盛-添加
    /// </summary>
    public static class EntityHelper
    {
        #region Entity

        /// <summary>
        /// 根据某个数据表赋值返回新的实体对象
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable dt) where T : class , new()
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                T cloneobj = new T();
                Type type = cloneobj.GetType();
                PropertyInfo[] ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                var queryps = ps.Where(x => x.CanWrite);
                var propertyInfos = queryps as PropertyInfo[] ?? queryps.ToArray();
                if (propertyInfos.IsAny())
                {
                    DataColumn[] arraydc = new DataColumn[dt.Columns.Count];
                    dt.Columns.CopyTo(arraydc, 0);
                    cloneobj = ToEntity<T>(arraydc, propertyInfos, dt.Rows[0]);
                    return cloneobj;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 根据某个数据表赋值返回新的实体泛型对象
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <returns></returns>
        public static List<T> ToEntityList<T>(this DataTable dt) where T : class , new()
        {
            List<T> cloneList = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                T gtype = new T();
                Type type = gtype.GetType();
                PropertyInfo[] ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                var queryps = ps.Where(x => x.CanWrite);
                var propertyInfos = queryps as PropertyInfo[] ?? queryps.ToArray();
                if (propertyInfos.IsAny())
                {
                    DataColumn[] arraydc = new DataColumn[dt.Columns.Count];
                    dt.Columns.CopyTo(arraydc, 0);
                    cloneList.AddRange(from DataRow dr in dt.Rows select ToEntity<T>(arraydc, propertyInfos, dr));
                }
            }
            return cloneList;
        }

        /// <summary>
        /// 将DataRow转为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="arraydc">数据库字段数组</param>
        /// <param name="propertyInfos">属性数组</param>
        /// <param name="dr">DataRow对象</param>
        /// <returns></returns>
        private static T ToEntity<T>(DataColumn[] arraydc, PropertyInfo[] propertyInfos, DataRow dr) where T : class, new()
        {
            T cloneobj = new T();
            var querydc = arraydc.Where(x => propertyInfos.Select(y => y.Name.ToUpper()).Contains(x.ColumnName.ToUpper()));
            var dataColumns = querydc as DataColumn[] ?? querydc.ToArray();
            if (dataColumns.IsAny())
            {
                var savaqueryps = propertyInfos.Where(x => dataColumns.Select(y => y.ColumnName.ToUpper()).Contains(x.Name.ToUpper()) && x.CanRead);
                foreach (PropertyInfo i in savaqueryps)
                {
                    object objc = dr[i.Name];
                    Type dbValType = objc.GetType();
                    Type attrValType = i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(i.PropertyType) : i.PropertyType;
                    if (dbValType != typeof(DBNull))
                    {
                        if (dbValType == attrValType)
                        {
                            i.SetValue(cloneobj, objc, null);
                        }
                        else
                        {
                            object cobject = null;
                            try
                            {
                                cobject = Convert.ChangeType(objc, attrValType);
                            }
                            catch
                            {
                                // ignored
                            }
                            if (cobject != null)
                            {
                                i.SetValue(cloneobj, cobject, null);
                            }
                        }
                    }
                }
            }
            return cloneobj;
        }

        #endregion
    }
}

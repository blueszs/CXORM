using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CXData.Helper
{
    /// <summary>
    /// 扩展方法类
    /// 20150625-周盛-添加
    /// </summary>
    public static class ExtensionHelper
    {
        /// <summary>
        /// 判断类型是否为Null
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <returns></returns>
        public static bool IsNull<T>(this T source)
        {
            return source == null;
        }

        /// <summary>
        /// 对象中是否存在元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return false;
            }
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据表达式条件查询是否存在符合条件的元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                return false;
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate is null");
            }
            return source.Any(predicate);
        }

        /// <summary>
        /// 根据表达式条件查询所有的元素是否都符合条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool IsAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                return false;
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate is null");
            }
            return source.All(predicate);
        }

        /// <summary>
        /// 对象转化为Josn字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T source) where T : class
        {
            if (source != null)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, source);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray());
                    return szJson;
                }
            }
            return "";
        }

        /// <summary>
        /// 获取Json的T类型的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string source) where T : class
        {
            if (!string.IsNullOrEmpty(source))
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据某个对象动态赋值返回新的对象
        /// </summary>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <returns></returns>
        public static T CloneEntity<T>(this T source) where T : new()
        {
            T cloneobj = new T();
            Type type = source.GetType();
            PropertyInfo[] ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (ps.Length > 0)
            {
                Type ts = cloneobj.GetType();
                foreach (PropertyInfo i in ps)
                {
                    if (i.CanRead)
                    {
                        object obj = i.GetValue(source, null);
                        if (obj != null)
                        {
                            string name = i.Name;
                            PropertyInfo setprop = ts.GetProperty(name);
                            if (setprop != null && setprop.CanWrite)
                            {
                                try
                                {
                                    setprop.SetValue(cloneobj, obj, null);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }
                    }
                }
            }
            return cloneobj;
        }

        /// <summary>
        /// 根据某个对象动态赋值返回新的对象
        /// </summary>
        /// <typeparam name="T">原始类型</typeparam>
        /// <typeparam name="TResult">需要返回的类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <param name="target">返回类型的对象实例</param>
        public static void CopyEntity<T, TResult>(this T source, TResult target)
        {
            Type type = source.GetType();
            PropertyInfo[] ps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (ps.Length > 0)
            {
                PropertyInfo[] psCopy = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (PropertyInfo i in ps)
                {
                    if (i.CanRead)
                    {
                        object obj = i.GetValue(source, null);
                        if (obj != null)
                        {
                            string name = i.Name;
                            var query = psCopy.Where(x => (x.Name == name || x.Name.ToUpper() == name.ToUpper()) && x.CanWrite);
                            var propertyInfos = query as PropertyInfo[] ?? query.ToArray();
                            if (propertyInfos.Any())
                            {
                                PropertyInfo setprop = propertyInfos.First();
                                Type setType = setprop.PropertyType.IsGenericType && setprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(setprop.PropertyType) : setprop.PropertyType;
                                Type valType = i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(i.PropertyType) : i.PropertyType;
                                try
                                {
                                    if (setType == valType)
                                    {
                                        setprop.SetValue(target, obj, null);
                                    }
                                    else
                                    {
                                        object objeChangeType = Convert.ChangeType(obj, setType);
                                        setprop.SetValue(target, objeChangeType, null);
                                    }
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

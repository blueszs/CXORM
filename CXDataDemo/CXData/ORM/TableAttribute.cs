using System;

namespace CXData.ORM
{
    /// <summary>
    /// 字段自增长属性
    /// 20150625-周盛-添加
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IdentityAttribute : Attribute
    {
    }

    /// <summary>
    /// 表属性
    /// 20150625-周盛-添加
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}

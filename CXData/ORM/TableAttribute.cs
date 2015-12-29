using System;

namespace CXData.ORM
{
    /// <summary>
    /// 表属性
    /// 20150625-周盛-添加
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 是否为自增列
        /// </summary>
        public bool Identity
        {
            get;
            set;
        }
    }
}

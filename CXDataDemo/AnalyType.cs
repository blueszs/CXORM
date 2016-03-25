namespace CXData.ORM
{
    /// <summary>
    /// 表达式解析类型
    /// 20150625-周盛-添加
    /// </summary>
    public enum AnalyType
    {
        /// <summary>
        /// 字段
        /// </summary>
        Column,
        /// <summary>
        /// 条件参数
        /// </summary>
        Param,
        /// <summary>
        /// 排序
        /// </summary>
        Order,
        /// <summary>
        /// 分组
        /// </summary>
        Group
    }
}

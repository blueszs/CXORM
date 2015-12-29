using System.Data.Common;

namespace CXData.ADO
{
    /// <summary>
    /// 事务类
    /// 20150625-周盛-添加
    /// </summary>
    internal class TransConnection
    {
        public TransConnection()
        {
            Deeps = 0;
        }

        public DbTransaction MyDbTransaction { get; set; }

        public int Deeps { get; set; }
    }
}

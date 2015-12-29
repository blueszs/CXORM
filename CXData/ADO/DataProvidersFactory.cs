using System.Reflection;

namespace CXData.ADO
{
    /// <summary>
    /// 数据库提供程序工厂类
    /// 20150625-周盛-添加
    /// </summary>
    public class DataProvidersFactory
    {
        public static IDataProviders GetDataProviders(string dataProviderName, string conn)
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
            {
                string className = string.Format("{0}.{1}", declaringType.Namespace, dataProviderName);
                IDataProviders dataProviders = (IDataProviders)Assembly.GetExecutingAssembly().CreateInstance(className);
                if (dataProviders != null)
                {
                    dataProviders.ConnectionString = conn;
                    return dataProviders;
                }
            }
            return null;
        }
    }
}

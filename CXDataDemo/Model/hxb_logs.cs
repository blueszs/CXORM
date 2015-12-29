using System.Configuration;
using CXData.ADO;
using Model.Model;

namespace Model
{
    public class hxb_logs
    {
        public hxb_logs(string dbConn = "DbLogConnection")
        {
            string providerName = ConfigurationManager.ConnectionStrings[dbConn].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings[dbConn].ConnectionString;
            DbHelper.SetDataProviders(providerName, connectionString);
        }

        public Hk_HotWord Hk_HotWord
        {
            get;
            set;
        }
    }
}

using System.Configuration;
using CXData.ADO;
using Model.Model;

namespace Model
{
    public class hjss_hxb
    {
        public hjss_hxb(string dbConn = "DbConnection")
        {
            string providerName = ConfigurationManager.ConnectionStrings[dbConn].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings[dbConn].ConnectionString;
            DbHelper.SetDataProviders(providerName, connectionString);
        }

        public Hk_Region Hk_Region
        {
            get;
            set;
        }

        public Hk_Region_AutoCheck Hk_Region_AutoCheck
        {
            get;
            set;
        }

        public Hk_Orders Hk_Orders
        {
            get;
            set;
        }

        public Hk_Orders_Sub Hk_Orders_Sub
        {
            get;
            set;
        }

        public Hk_Order_Goods Hk_Order_Goods
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using CXData.Helper;
using Model;
using CXData.ORM;
using Model.Model;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            hjss_hxb data = new hjss_hxb();
            Console.WriteLine("获取实体列表");
            List<Hk_Region> list = data.Hk_Region.Where(x => x.Region_Id > 1);
            Console.WriteLine(list.ToJson());
            Console.WriteLine("2表连表查询实体返回第三种类型实体");
            Expression<Func<Hk_Region, Hk_Region_AutoCheck, bool>> fWhere = (x, y) => x.Region_Id == 2;
            fWhere = fWhere.AndAlsoExp((x, y) => y.AddTime < DateTime.Now);
            HkRegionAutoCheck model = data.Hk_Region.Find(new Hk_Region_AutoCheck(), JoinType.Left, x => x.Region_Id,
                y => y.Region_Id, (x, y) => new HkRegionAutoCheck
                {
                    Region_Id = x.Region_Id,
                    RegionName = x.Name,
                    IsCommAutoCheck = y.IsCommAutoCheck.Value,
                    IsSpecialAutoCheck = y.IsSpecialAutoCheck,
                    AddTime = y.AddTime
                }, fWhere);
            Console.WriteLine(model.ToJson());
            Thread.Sleep(20000);
            Hk_Region rModel = data.Hk_Region.Find(x => x.Region_Id == 2);
            Console.WriteLine(rModel.ToJson());
            Expression<Func<Hk_Orders, OrdersSub, Hk_Order_Goods, bool>> fWhereOrder = (x, y, z) => x.Id == 1000;
            Hk_Order_Goods order = data.Hk_Orders.Find(new OrdersSub(), JoinType.Inner, x => x.Order_No, y => y.Order_No, new Hk_Orders(), new Hk_Order_Goods(), JoinType.Inner, x => x.Order_No, z => z.OrderNo,
               (x, y, z) => z, fWhereOrder, null);
            Hk_Orders order2 = data.Hk_Orders.Find(new OrdersSub(), JoinType.Inner, x => x.Order_No, y => y.Order_No, new OrdersSub(), new Hk_Order_Goods(), JoinType.Inner, y => y.Sub_Order_No, z => z.SubOrderNo,
               (x, y, z) => x, fWhereOrder, null);
            Console.WriteLine("主数据库3表连接");
            Console.WriteLine(order.ToJson());
            Console.WriteLine(order2.ToJson());
            hxb_logs logs = new hxb_logs();
            Hk_HotWord hotWord = logs.Hk_HotWord.Find(x => x.SearchType == 1);
            Console.WriteLine("hxb_logs数据库");
            Console.WriteLine(hotWord.ToJson());
            Console.ReadLine();
        }
    }
}

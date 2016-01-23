using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CXData.Helper;
using CXData.ORM;
using Model;
using Model.Model;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            hjss_hxb data = new hjss_hxb();
            Response.Write("获取实体列表<br/>");
            List<Hk_Region> list = data.Hk_Region.SelectList(x => x.Region_Id > 1);
            Response.Write(list.ToJson() + "<br/>");
            Response.Write("2表连表查询实体返回第三种类型实体<br/>");
            Expression<Func<Hk_Region, Hk_Region_AutoCheck, bool>> fWhere = (x, y) => x.Region_Id.Value.In(new[] { 1, 5 });
            fWhere = fWhere.AndAlsoExp((x, y) => y.AddTime < DateTime.Now);
            HkRegionAutoCheck model = data.Hk_Region.JoinOnFirst(new Hk_Region_AutoCheck(), JoinType.Left, x => x.Region_Id,
                y => y.Region_Id, (x, y) => new HkRegionAutoCheck
                {
                    Region_Id = x.Region_Id,
                    RegionName = x.Name,
                    IsCommAutoCheck = y.IsCommAutoCheck,
                    IsSpecialAutoCheck = y.IsSpecialAutoCheck,
                    AddTime = y.AddTime
                }, fWhere);
            Response.Write(model.ToJson() + "<br/>");
            int num = 0;
            Response.Write("2表连表分组查询实体返回第三种类型实体<br/>");
            List<HkRegionAutoCheck> datlist = data.Hk_Region.JoinGroupByList(new Hk_Region_AutoCheck(), JoinType.Left, x => x.Region_Id,
                y => y.Region_Id, (x, y) => new {x.Name }, x => new HkRegionAutoCheck
                {
                    RegionName = x.Name,
                    IsCommAutoCheck = x.Name.Len()
                }, fWhere, null, 0, 1, ref num);
            Response.Write(datlist.ToJson() + "<br/>");
            Hk_Region rModel = data.Hk_Region.SelectFirst(x => x.Region_Id == 2);
            Response.Write(rModel.ToJson() + "<br/>");
            Expression<Func<Hk_Orders, OrdersSub, Hk_Order_Goods, bool>> fWhereOrder = (x, y, z) => x.Id == 1000;
            Hk_Order_Goods order = data.Hk_Orders.JoinOnFirst(new OrdersSub(), JoinType.Inner, x => x.Order_No, y => y.Order_No, new Hk_Orders(), new Hk_Order_Goods(), JoinType.Inner, x => x.Order_No, z => z.OrderNo,
               (x, y, z) => z, fWhereOrder, null);
            Hk_Orders order2 = data.Hk_Orders.JoinOnFirst(new OrdersSub(), JoinType.Inner, x => x.Order_No, y => y.Order_No, new OrdersSub(), new Hk_Order_Goods(), JoinType.Inner, y => y.Sub_Order_No, z => z.SubOrderNo,
               (x, y, z) => x, fWhereOrder, null);
            Response.Write("主数据库3表连接<br/>");
            Response.Write(order.ToJson() + "<br/>");
            Response.Write(order2.ToJson() + "<br/>");

            hxb_logs logs = new hxb_logs();
            Hk_HotWord hotWord = logs.Hk_HotWord.SelectFirst(x => x.SearchType == 1);
            Response.Write("hxb_logs数据库<br/>");
            Response.Write(hotWord.ToJson() + "<br/>");
            List<Hk_Region> lsitregion = new List<Hk_Region>();
            List<Hk_Region_AutoCheck> lsitregionAuto = new List<Hk_Region_AutoCheck>();
            //datlist.GroupBy(x=>new { x.Region_Id}).Sum(x=>x.Key.).Select(x=> { x.Key,x.Key.SumValue()})
            return View();
        }

    }
}

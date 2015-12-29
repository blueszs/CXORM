using System;
using CXData.ORM;

namespace Model.Model
{
    /// <summary>
 	/// 订单商品表
 	/// </summary>
	public class Hk_Order_Goods
    {
        #region Public Properties
        /// <summary>
        /// 自增ID
        /// </summary>
        [Table(Identity = true)]
        public int? Id
        {
            get;
            set;
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int? Order_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long? Goods_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 商品标题
        /// </summary>
        public string Goods_Title
        {
            get;
            set;
        }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal? Goods_Price
        {
            get;
            set;
        }

        /// <summary>
        /// 实际价格
        /// </summary>
        public decimal? Real_Price
        {
            get;
            set;
        }

        /// <summary>
        /// 订购数量
        /// </summary>
        public int? Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// point
        /// </summary>
        public int? Point
        {
            get;
            set;
        }

        /// <summary>
        /// 商品属性
        /// </summary>
        public string Attrvalue
        {
            get;
            set;
        }

        /// <summary>
        /// 商品积分
        /// </summary>
        public decimal? Integral
        {
            get;
            set;
        }

        /// <summary>
        /// 分享ID
        /// </summary>
        public int? Share_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 返利积分
        /// </summary>
        public decimal? Profit
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int? Warehouse_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Warehouse_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库地址
        /// </summary>
        public string Warehouse_Address
        {
            get;
            set;
        }

        /// <summary>
        /// 子订单ID
        /// </summary>
        public int? Sub_Order_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string Goods_No
        {
            get;
            set;
        }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Definition
        {
            get;
            set;
        }

        /// <summary>
        /// 商品颜色ID
        /// </summary>
        public int? Color_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 商品尺码ID
        /// </summary>
        public int? Unit_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// subOrderNo
        /// </summary>
        public string SubOrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// sku_id
        /// </summary>
        public int? Sku_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 快照图片
        /// </summary>
        public string Img_Url
        {
            get;
            set;
        }

        /// <summary>
        /// jgid
        /// </summary>
        public int? Jgid
        {
            get;
            set;
        }

        /// <summary>
        /// unitprofit
        /// </summary>
        public decimal? Unitprofit
        {
            get;
            set;
        }

        /// <summary>
        /// sku_img_bak
        /// </summary>
        public string Sku_Img_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// goodstype
        /// </summary>
        public int? Goodstype
        {
            get;
            set;
        }

        /// <summary>
        /// pgoods_id
        /// </summary>
        public long? Pgoods_Id
        {
            get;
            set;
        }

        /// <summary>
        /// Activetype
        /// </summary>
        public int? Activetype
        {
            get;
            set;
        }

        /// <summary>
        /// activeid
        /// </summary>
        public int? Activeid
        {
            get;
            set;
        }

        /// <summary>
        /// warehouse_code
        /// </summary>
        public string Warehouse_Code
        {
            get;
            set;
        }

        /// <summary>
        /// unit_basetitle_bak
        /// </summary>
        public string Unit_Basetitle_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// unit_title_bak
        /// </summary>
        public string Unit_Title_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// color_basetitle_bak
        /// </summary>
        public string Color_Basetitle_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// color_title_bak
        /// </summary>
        public string Color_Title_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// weight_bak
        /// </summary>
        public decimal? Weight_Bak
        {
            get;
            set;
        }

        /// <summary>
        /// giftinfo
        /// </summary>
        public string Giftinfo
        {
            get;
            set;
        }

        /// <summary>
        /// updatetime
        /// </summary>
        public DateTime? Updatetime
        {
            get;
            set;
        }

        /// <summary>
        /// is_backstock
        /// </summary>
        public int? Is_Backstock
        {
            get;
            set;
        }

        /// <summary>
        /// cost_price
        /// </summary>
        public decimal? Cost_Price
        {
            get;
            set;
        }

        /// <summary>
        /// card_purchase_amount
        /// </summary>
        public decimal? Card_Purchase_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// fav_profit
        /// </summary>
        public decimal? Fav_Profit
        {
            get;
            set;
        }

        /// <summary>
        /// bonus_points
        /// </summary>
        public int? Bonus_Points
        {
            get;
            set;
        }

        /// <summary>
        /// express_fee
        /// </summary>
        public decimal? Express_Fee
        {
            get;
            set;
        }


        #endregion Public Properties
    }
}

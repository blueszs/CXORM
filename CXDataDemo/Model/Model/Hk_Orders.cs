using System;
using CXData.ORM;

namespace Model.Model
{
    /// <summary>
 	/// 订单表
 	/// </summary>
	public class Hk_Orders
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
        /// 订单号
        /// </summary>
        public string Order_No
        {
            get;
            set;
        }

        /// <summary>
        /// 交易号担保支付用到
        /// </summary>
        public string Trade_No
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? User_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 支付方式与storepayment关联ID值 
        /// </summary>
        public int? Payment_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 支付手续费
        /// </summary>
        public decimal? Payment_Fee
        {
            get;
            set;
        }

        /// <summary>
        /// 支付状态1未支付2已支付
        /// </summary>
        public int? Payment_Status
        {
            get;
            set;
        }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? Payment_Time
        {
            get;
            set;
        }

        /// <summary>
        /// 快递ID
        /// </summary>
        public int? Express_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string Express_No
        {
            get;
            set;
        }

        /// <summary>
        /// 物流费用
        /// </summary>
        public decimal? Express_Fee
        {
            get;
            set;
        }

        /// <summary>
        /// 发货状态1未发货2已发货
        /// </summary>
        public int? Express_Status
        {
            get;
            set;
        }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? Express_Time
        {
            get;
            set;
        }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Accept_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Post_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 购买人
        /// </summary>
        public string Contact_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 联系电话(支付人)
        /// </summary>
        public string Telphone
        {
            get;
            set;
        }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 所属省市区
        /// </summary>
        public string Area
        {
            get;
            set;
        }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 订单留言
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 应付商品总金额
        /// </summary>
        public decimal? Payable_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 实付商品总金额
        /// </summary>
        public decimal? Real_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal? Order_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 积分,正数赠送|负数消费
        /// </summary>
        public int? Point
        {
            get;
            set;
        }

        /// <summary>
        /// 订单状态1生成订单,2确认订单,3完成订单,4取消关闭订单,5作废订单
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? Add_Time
        {
            get;
            set;
        }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? Confirm_Time
        {
            get;
            set;
        }

        /// <summary>
        /// 订单完成时间
        /// </summary>
        public DateTime? Complete_Time
        {
            get;
            set;
        }

        /// <summary>
        /// 订单所属的机构
        /// </summary>
        public int? Jgid
        {
            get;
            set;
        }

        /// <summary>
        /// 订单关联的店铺
        /// </summary>
        public int? Storeid
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需要发票1为要，其他不要
        /// </summary>
        public int? Needinvoice
        {
            get;
            set;
        }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Invoicetitle
        {
            get;
            set;
        }

        /// <summary>
        /// 订单产生的返利
        /// </summary>
        public decimal? Profit
        {
            get;
            set;
        }

        /// <summary>
        /// 订单产生的积分
        /// </summary>
        public decimal? Integral
        {
            get;
            set;
        }

        /// <summary>
        /// 系统标识
        /// </summary>
        public int? SysFlage
        {
            get;
            set;
        }

        /// <summary>
        /// 用户支付的url地址
        /// </summary>
        public string Payment_Url
        {
            get;
            set;
        }

        /// <summary>
        /// 0&Null没有退单 1申请退单等待验货 2验货成功  3回收货未退款 4退款成功5不予退款
        /// </summary>
        public int? Returnstatus
        {
            get;
            set;
        }

        /// <summary>
        /// has_tax_goods
        /// </summary>
        public int? Has_Tax_Goods
        {
            get;
            set;
        }

        /// <summary>
        /// tax_price
        /// </summary>
        public decimal? Tax_Price
        {
            get;
            set;
        }

        /// <summary>
        /// realname
        /// </summary>
        public string Realname
        {
            get;
            set;
        }

        /// <summary>
        /// card_no
        /// </summary>
        public string Card_No
        {
            get;
            set;
        }

        /// <summary>
        /// front_img
        /// </summary>
        public string Front_Img
        {
            get;
            set;
        }

        /// <summary>
        /// back_img
        /// </summary>
        public string Back_Img
        {
            get;
            set;
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int? OrderType
        {
            get;
            set;
        }

        /// <summary>
        /// RentId
        /// </summary>
        public int? RentId
        {
            get;
            set;
        }

        /// <summary>
        /// IsPushToRadio
        /// </summary>
        public int? IsPushToRadio
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
        /// radio_remark
        /// </summary>
        public string Radio_Remark
        {
            get;
            set;
        }

        /// <summary>
        /// openid
        /// </summary>
        public string Openid
        {
            get;
            set;
        }

        /// <summary>
        /// IsPushVPClub
        /// </summary>
        public int? IsPushVPClub
        {
            get;
            set;
        }

        /// <summary>
        /// expressname
        /// </summary>
        public string Expressname
        {
            get;
            set;
        }

        /// <summary>
        /// appid
        /// </summary>
        public int? Appid
        {
            get;
            set;
        }

        /// <summary>
        /// sub_orderno
        /// </summary>
        public string Sub_Orderno
        {
            get;
            set;
        }

        /// <summary>
        /// province
        /// </summary>
        public string Province
        {
            get;
            set;
        }

        /// <summary>
        /// city
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// deleted
        /// </summary>
        public int? Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// wxopenid
        /// </summary>
        public string Wxopenid
        {
            get;
            set;
        }

        /// <summary>
        /// share_masterid
        /// </summary>
        public string Share_Masterid
        {
            get;
            set;
        }

        /// <summary>
        /// idcard
        /// </summary>
        public string Idcard
        {
            get;
            set;
        }

        /// <summary>
        /// card_mobile
        /// </summary>
        public string Card_Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// order_style
        /// </summary>
        public int? Order_Style
        {
            get;
            set;
        }

        /// <summary>
        /// ip
        /// </summary>
        public string Ip
        {
            get;
            set;
        }

        /// <summary>
        /// order_querycode
        /// </summary>
        public string Order_Querycode
        {
            get;
            set;
        }

        /// <summary>
        /// group_guid
        /// </summary>
        public string Group_Guid
        {
            get;
            set;
        }

        /// <summary>
        /// version
        /// </summary>
        public int? Version
        {
            get;
            set;
        }

        /// <summary>
        /// activetype
        /// </summary>
        public int? Activetype
        {
            get;
            set;
        }

        /// <summary>
        /// payflowid
        /// </summary>
        public string Payflowid
        {
            get;
            set;
        }

        /// <summary>
        /// region_id
        /// </summary>
        public int? Region_Id
        {
            get;
            set;
        }


        #endregion Public Properties
    }
}

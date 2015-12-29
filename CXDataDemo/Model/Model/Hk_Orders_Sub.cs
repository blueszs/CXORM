using System;
using CXData.ORM;

namespace Model.Model
{
    /// <summary>
 	/// hk_orders_sub
 	/// </summary>
	public class Hk_Orders_Sub
    {
        #region Public Properties
        /// <summary>
        /// id
        /// </summary>
        [Table(Identity = true)]
        public int? Id
        {
            get;
            set;
        }

        /// <summary>
        /// sub_order_no
        /// </summary>
        public string Sub_Order_No
        {
            get;
            set;
        }

        /// <summary>
        /// order_id
        /// </summary>
        public int? Order_Id
        {
            get;
            set;
        }

        /// <summary>
        /// order_goods_id
        /// </summary>
        public long? Order_Goods_Id
        {
            get;
            set;
        }

        /// <summary>
        /// priority
        /// </summary>
        public int? Priority
        {
            get;
            set;
        }

        /// <summary>
        /// express_id
        /// </summary>
        public int? Express_Id
        {
            get;
            set;
        }

        /// <summary>
        /// total_amount
        /// </summary>
        public decimal? Total_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// actual_amount
        /// </summary>
        public decimal? Actual_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// express_no
        /// </summary>
        public string Express_No
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

        /// <summary>
        /// express_status
        /// </summary>
        public int? Express_Status
        {
            get;
            set;
        }

        /// <summary>
        /// express_time
        /// </summary>
        public DateTime? Express_Time
        {
            get;
            set;
        }

        /// <summary>
        /// payment_status
        /// </summary>
        public int? Payment_Status
        {
            get;
            set;
        }

        /// <summary>
        /// status
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// returnstatus
        /// </summary>
        public int? Returnstatus
        {
            get;
            set;
        }

        /// <summary>
        /// ParentOrderNo
        /// </summary>
        public string ParentOrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// order_no
        /// </summary>
        public string Order_No
        {
            get;
            set;
        }

        /// <summary>
        /// expresscode
        /// </summary>
        public string Expresscode
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
        /// order_amount
        /// </summary>
        public decimal? Order_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// returnwhy
        /// </summary>
        public string Returnwhy
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
        /// sourceappid
        /// </summary>
        public int? Sourceappid
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
        /// updatetime
        /// </summary>
        public DateTime? Updatetime
        {
            get;
            set;
        }

        /// <summary>
        /// createtime
        /// </summary>
        public DateTime? Createtime
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
        /// payment_Time
        /// </summary>
        public DateTime? Payment_Time
        {
            get;
            set;
        }

        /// <summary>
        /// confirm_Time
        /// </summary>
        public DateTime? Confirm_Time
        {
            get;
            set;
        }

        /// <summary>
        /// complete_time
        /// </summary>
        public DateTime? Complete_Time
        {
            get;
            set;
        }

        /// <summary>
        /// profit
        /// </summary>
        public decimal? Profit
        {
            get;
            set;
        }

        /// <summary>
        /// integral
        /// </summary>
        public int? Integral
        {
            get;
            set;
        }

        /// <summary>
        /// returntime
        /// </summary>
        public DateTime? Returntime
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
        /// receipt_time
        /// </summary>
        public DateTime? Receipt_Time
        {
            get;
            set;
        }

        /// <summary>
        /// isreceipt
        /// </summary>
        public int? Isreceipt
        {
            get;
            set;
        }

        /// <summary>
        /// storeid
        /// </summary>
        public int? Storeid
        {
            get;
            set;
        }

        /// <summary>
        /// pay_tel
        /// </summary>
        public string Pay_Tel
        {
            get;
            set;
        }

        /// <summary>
        /// receiver_tel
        /// </summary>
        public string Receiver_Tel
        {
            get;
            set;
        }

        /// <summary>
        /// buyerid
        /// </summary>
        public string Buyerid
        {
            get;
            set;
        }

        /// <summary>
        /// pay_username
        /// </summary>
        public string Pay_Username
        {
            get;
            set;
        }

        /// <summary>
        /// receiver_username
        /// </summary>
        public string Receiver_Username
        {
            get;
            set;
        }

        /// <summary>
        /// remark
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// ordertype
        /// </summary>
        public int? Ordertype
        {
            get;
            set;
        }

        /// <summary>
        /// refund_amount
        /// </summary>
        public decimal? Refund_Amount
        {
            get;
            set;
        }


        #endregion Public Properties
    }
}

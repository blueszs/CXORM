using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CXData.ORM;

namespace Model.Model
{
    public class Hk_LotteryQuestion
    {
        #region Public Properties
        /// <summary>
        /// Id
        /// </summary>
        [Identity]
        public int? Id
        {
            get;
            set;
        }

        /// <summary>
        /// 答题抽奖期号
        /// </summary>
        public string IssueNo
        {
            get;
            set;
        }

        /// <summary>
        /// AppId
        /// </summary>
        public int? AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 抽奖类型(1：接近答案抽奖，2：完全正确抽奖）
        /// </summary>
        public int? TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// 状态（0：未启用，1：已启用）
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 答题活动背景图片
        /// </summary>
        public string BackgroundImage
        {
            get;
            set;
        }

        /// <summary>
        /// 问题图片
        /// </summary>
        public string QuestionImage
        {
            get;
            set;
        }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string QuestionInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer
        {
            get;
            set;
        }

        /// <summary>
        /// 中奖个数
        /// </summary>
        public int? WinningCount
        {
            get;
            set;
        }

        /// <summary>
        /// 公布答案时间
        /// </summary>
        public DateTime? AnnounceTime
        {
            get;
            set;
        }

        /// <summary>
        /// AddTime
        /// </summary>
        public DateTime? AddTime
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// DeleteFlag
        /// </summary>
        public int? DeleteFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 竞猜商品名称
        /// </summary>
        public string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 竞猜商品图片
        /// </summary>
        public string ProductImage
        {
            get;
            set;
        }

        /// <summary>
        /// 竞猜商品链接
        /// </summary>
        public string ProductUrl
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string ShareTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 竞猜商品链接
        /// </summary>
        public string ShareDesc
        {
            get;
            set;
        }
        #endregion Public Properties
    }
}

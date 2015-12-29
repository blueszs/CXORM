using System;

namespace Model.Model
{
    /// <summary>
 	/// 地市自动审核配置表
 	/// </summary>
	public class HkRegionAutoCheck
    {
        #region Public Properties
        /// <summary>
        /// 地市编号
        /// </summary>
        public int? Region_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 地市名称
        /// </summary>
        public string RegionName
        {
            get;
            set;
        }

        /// <summary>
        /// 普通商品是否需要自动审核（0：人工审核，1：自动审核）
        /// </summary>
        public int? IsCommAutoCheck
        {
            get;
            set;
        }

        /// <summary>
        /// 九九购商品是否需要自动审核（0：人工审核，1：自动审核）
        /// </summary>
        public int? IsSpecialAutoCheck
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime
        {
            get;
            set;
        }
        #endregion Public Properties
    }
}

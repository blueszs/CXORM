using System;
using CXData.ORM;

namespace Model.Model
{
    /// <summary>
 	/// 地区信息表
 	/// </summary>
	public class Hk_Region
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
        /// appid
        /// </summary>
        public int? Appid
        {
            get;
            set;
        }

        /// <summary>
        /// 地区编号,可重复,与appid构成唯一键
        /// </summary>
        public int? Region_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime? Add_Time
        {
            get;
            set;
        }


        #endregion Public Properties
    }
}

using CXData.ORM;

namespace Model.Model
{
    /// <summary>
 	/// Hk_HotWord
 	/// </summary>
    public class Hk_HotWord
    {
        #region Public Properties
        /// <summary>
        /// id
        /// </summary>
        [Identity]
        public int? Id
        {
            get;
            set;
        }

        /// <summary>
        /// hotWord
        /// </summary>
        public string HotWord
        {
            get;
            set;
        }

        /// <summary>
        /// clickCount
        /// </summary>
        public int? ClickCount
        {
            get;
            set;
        }

        /// <summary>
        /// searchType
        /// </summary>
        public int? SearchType
        {
            get;
            set;
        }


        #endregion Public Properties
    }
}

using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// UA_USAGE_DETAIL:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UA_USAGE_DETAIL
    {
        public UA_USAGE_DETAIL()
        { }
        #region Model
        private Guid _id;
        private Guid _usageid;
        private string _key;
        private string _value;
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 主表ID
        /// </summary>
        public Guid UsageID
        {
            set { _usageid = value; }
            get { return _usageid; }
        }
        /// <summary>
        /// 查询条件名
        /// </summary>
        public string QueryKey
        {
            set { _key = value; }
            get { return _key; }
        }
        /// <summary>
        /// 查询条件值
        /// </summary>
        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
        #endregion Model

    }
}


using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_GROUPBY:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_GROUPBY
    {
        public BS_GROUPBY()
        { }
        #region Model
        private Guid _id;
        private Guid _templateid;
        private string _columns;
        private string _location;
        private string _computecolumn;
        private decimal _sequence;
        private string _sql;
        private DateTime _created;
        private DateTime _modified;
        private string _author;
        private string _editor;
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        public Guid TemplateID
        {
            set { _templateid = value; }
            get { return _templateid; }
        }
        /// <summary>
        /// 汇总列
        /// </summary>
        public string Columns
        {
            set { _columns = value; }
            get { return _columns; }
        }
        /// <summary>
        /// 显示位置
        /// </summary>
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        /// <summary>
        /// 计算列
        /// </summary>
        public string ComputeColumn
        {
            set { _computecolumn = value; }
            get { return _computecolumn; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Sequence
        {
            set { _sequence = value; }
            get { return _sequence; }
        }
        /// <summary>
        /// SQL
        /// </summary>
        public string SQL
        {
            set { _sql = value; }
            get { return _sql; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Modified
        {
            set { _modified = value; }
            get { return _modified; }
        }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }
        /// <summary>
        /// 修改者
        /// </summary>
        public string Editor
        {
            set { _editor = value; }
            get { return _editor; }
        }
        #endregion Model

    }
}


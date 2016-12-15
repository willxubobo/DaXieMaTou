using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_TEMPLATE_COLUMNS:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_TEMPLATE_COLUMNS
    {
        public BS_TEMPLATE_COLUMNS()
        { }
        #region Model
        private Guid _id;
        private Guid _templateid;
        private string _columnname;
        private string _columndatatype;
        private bool _visiable;
        private string _displayname;
        private string _mergecolumnname;
        private decimal _sequence;
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
        /// 数据源列名
        /// </summary>
        public string ColumnName
        {
            set { _columnname = value; }
            get { return _columnname; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnDataType
        {
            set { _columndatatype = value; }
            get { return _columndatatype; }
        }
        /// <summary>
        ///  是否显示
        /// </summary>
        public bool Visiable
        {
            set { _visiable = value; }
            get { return _visiable; }
        }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName
        {
            set { _displayname = value; }
            get { return _displayname; }
        }
        /// <summary>
        /// 合并表头名
        /// </summary>
        public string MergeColumnName
        {
            set { _mergecolumnname = value; }
            get { return _mergecolumnname; }
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


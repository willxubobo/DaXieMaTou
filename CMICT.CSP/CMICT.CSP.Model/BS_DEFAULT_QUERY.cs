using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_DEFAULT_QUERY:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_DEFAULT_QUERY
    {
        public BS_DEFAULT_QUERY()
        { }
        #region Model
        private Guid _id;
        private Guid _templateid;
        private string _mainlogic;
        private int _moduleid;
        private string _sublogic;
        private string _columnname;
        private string _desction;
        private string _comparevalue;
        private string _compare;
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
        /// 主逻辑and或or
        /// </summary>
        public string MainLogic
        {
            set { _mainlogic = value; }
            get { return _mainlogic; }
        }
        /// <summary>
        /// 组默认筛选条件编号
        /// </summary>
        public int ModuleID
        {
            set { _moduleid = value; }
            get { return _moduleid; }
        }
        /// <summary>
        /// 组内筛选条件对应and或or关系
        /// </summary>
        public string SubLogic
        {
            set { _sublogic = value; }
            get { return _sublogic; }
        }
        /// <summary>
        /// 默认筛选条件
        /// </summary>
        public string ColumnName
        {
            set { _columnname = value; }
            get { return _columnname; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desction
        {
            set { _desction = value; }
            get { return _desction; }
        }
        /// <summary>
        /// 比较值
        /// </summary>
        public string CompareValue
        {
            set { _comparevalue = value; }
            get { return _comparevalue; }
        }
        /// <summary>
        /// 比较符，LOOKUP_CODE:BS_COMPARE
        /// </summary>
        public string Compare
        {
            set { _compare = value; }
            get { return _compare; }
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


using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_TEMPLATE_MAIN:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_TEMPLATE_MAIN
    {
        public BS_TEMPLATE_MAIN()
        { }
        #region Model
        private Guid _templateid;
        private Guid _sourceid;
        private string _templatename;
        private string _diaplaytype;
        private string _templatedesc;
        private string _bigcategory;
        private string _smallcategory;
        private string _unit;
        private string _reminder;
        private decimal _pagesize;
        private decimal _columnsize;
        private string _templatestatus;
        private string _sql;
        private DateTime _created;
        private DateTime _modified;
        private string _author;
        private string _editor;
        /// <summary>
        /// 模板ID
        /// </summary>
        public Guid TemplateID
        {
            set { _templateid = value; }
            get { return _templateid; }
        }
        /// <summary>
        /// 数据源ID
        /// </summary>
        public Guid SourceID
        {
            set { _sourceid = value; }
            get { return _sourceid; }
        }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            set { _templatename = value; }
            get { return _templatename; }
        }
        /// <summary>
        /// 报表展现方式，LOOKUP_CODE:BS_DISPLAY_TYPE
        /// </summary>
        public string DiaplayType
        {
            set { _diaplaytype = value; }
            get { return _diaplaytype; }
        }
        /// <summary>
        /// 模板描述
        /// </summary>
        public string TemplateDesc
        {
            set { _templatedesc = value; }
            get { return _templatedesc; }
        }
        /// <summary>
        /// 报表大类
        /// </summary>
        public string BigCategory
        {
            set { _bigcategory = value; }
            get { return _bigcategory; }
        }
        /// <summary>
        /// 报表细类
        /// </summary>
        public string SmallCategory
        {
            set { _smallcategory = value; }
            get { return _smallcategory; }
        }
        /// <summary>
        /// 报表使用单位
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Reminder
        {
            set { _reminder = value; }
            get { return _reminder; }
        }
        /// <summary>
        /// 每页条数
        /// </summary>
        public decimal PageSize
        {
            set { _pagesize = value; }
            get { return _pagesize; }
        }
        /// <summary>
        /// 每行字段数
        /// </summary>
        public decimal ColumnSize
        {
            set { _columnsize = value; }
            get { return _columnsize; }
        }
        /// <summary>
        /// 模板状态，LOOKUP_CODE:BS_TEMPLATE_STATUS
        /// </summary>
        public string TemplateStatus
        {
            set { _templatestatus = value; }
            get { return _templatestatus; }
        }
        /// <summary>
        /// 查询语句
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


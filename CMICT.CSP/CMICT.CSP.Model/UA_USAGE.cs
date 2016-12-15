using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// UA_USAGE:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UA_USAGE
    {
        public UA_USAGE()
        { }
        #region Model
        private Guid _id;
        private string _pagename;
        private string _url;
        private Guid _templateid;
        private string _templatename;
        private decimal _loadtime;
        private string _browsertype;
        private DateTime _created;
        private string _author;
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        ///   当前页名
        /// </summary>
        public string PageName
        {
            set { _pagename = value; }
            get { return _pagename; }
        }
        /// <summary>
        /// 当前页Url
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
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
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            set { _templatename = value; }
            get { return _templatename; }
        }
        /// <summary>
        /// 报表展现加载时间
        /// </summary>
        public decimal LoadTime
        {
            set { _loadtime = value; }
            get { return _loadtime; }
        }
        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string BrowserType
        {
            set { _browsertype = value; }
            get { return _browsertype; }
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
        /// 创建者
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }
        #endregion Model

    }
}


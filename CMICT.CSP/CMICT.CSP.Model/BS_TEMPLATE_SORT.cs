using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_TEMPLATE_SORT:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_TEMPLATE_SORT
    {
        public BS_TEMPLATE_SORT()
        { }
        #region Model
        private Guid _id;
        private Guid _templateid;
        private string _sortcolumn;
        private string _type;
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
        /// 列名
        /// </summary>
        public string SortColumn
        {
            set { _sortcolumn = value; }
            get { return _sortcolumn; }
        }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
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


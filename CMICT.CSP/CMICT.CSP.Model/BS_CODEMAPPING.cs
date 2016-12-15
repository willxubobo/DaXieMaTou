using System;
namespace CMICT.CSP.Model
{
    /// <summary>
    /// BS_CODEMAPPING:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BS_CODEMAPPING
    {
        public BS_CODEMAPPING()
        { }
        #region Model
        private Guid _mappingid;
        private string _customerid;
        private string _customername;
        private string _customerdesc;
        private string _semanticdesc;
        private string _businesscode;
        private string _businesscodedesc;
        private string _businesstranslation;
        private string _targetcode;
        private string _targetcodeendesc;
        private string _targetcodecndesc;
        private string _cmictcode;
        private string _cmictcodeendesc;
        private string _cmictcodecndesc;
        private DateTime _startdate;
        private DateTime _expiredate;
        private DateTime _created;
        private DateTime _modified;
        private string _author;
        private string _editor;
        /// <summary>
        /// 映射GUID
        /// </summary>
        public Guid MappingID
        {
            set { _mappingid = value; }
            get { return _mappingid; }
        }
        /// <summary>
        /// 目标客户公司代码
        /// </summary>
        public string CustomerID
        {
            set { _customerid = value; }
            get { return _customerid; }
        }
        /// <summary>
        /// 目标客户公司名称
        /// </summary>
        public string CustomerName
        {
            set { _customername = value; }
            get { return _customername; }
        }
        /// <summary>
        /// 目标客户说明
        /// </summary>
        public string CustomerDesc
        {
            set { _customerdesc = value; }
            get { return _customerdesc; }
        }
        /// <summary>
        /// 语义说明
        /// </summary>
        public string SemanticDesc
        {
            set { _semanticdesc = value; }
            get { return _semanticdesc; }
        }
        /// <summary>
        /// 业务代码(业务类型)
        /// </summary>
        public string BusinessCode
        {
            set { _businesscode = value; }
            get { return _businesscode; }
        }
        /// <summary>
        /// 业务代码说明
        /// </summary>
        public string BusinessCodeDesc
        {
            set { _businesscodedesc = value; }
            get { return _businesscodedesc; }
        }
        /// <summary>
        /// 业务翻译说明
        /// </summary>
        public string BusinessTranslation
        {
            set { _businesstranslation = value; }
            get { return _businesstranslation; }
        }
        /// <summary>
        /// 目标客户代码
        /// </summary>
        public string TargetCode
        {
            set { _targetcode = value; }
            get { return _targetcode; }
        }
        /// <summary>
        /// 目标客户代码英文说明
        /// </summary>
        public string TargetCodeEnDesc
        {
            set { _targetcodeendesc = value; }
            get { return _targetcodeendesc; }
        }
        /// <summary>
        /// 目标客户代码中文说明
        /// </summary>
        public string TargetCodeCnDesc
        {
            set { _targetcodecndesc = value; }
            get { return _targetcodecndesc; }
        }
        /// <summary>
        /// 我方代码
        /// </summary>
        public string CMICTCode
        {
            set { _cmictcode = value; }
            get { return _cmictcode; }
        }
        /// <summary>
        /// 我方代码英文说明
        /// </summary>
        public string CMICTCodeEnDesc
        {
            set { _cmictcodeendesc = value; }
            get { return _cmictcodeendesc; }
        }
        /// <summary>
        /// 我方代码中文说明
        /// </summary>
        public string CMICTCodeCnDesc
        {
            set { _cmictcodecndesc = value; }
            get { return _cmictcodecndesc; }
        }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpireDate
        {
            set { _expiredate = value; }
            get { return _expiredate; }
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


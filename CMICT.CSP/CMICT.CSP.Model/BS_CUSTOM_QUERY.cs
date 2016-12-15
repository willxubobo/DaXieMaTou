/**  版本信息模板在安装目录下，可自行修改。
* BS_CUSTOM_QUERY.cs
*
* 功 能： N/A
* 类 名： BS_CUSTOM_QUERY
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:34   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：盟拓软件　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace CMICT.CSP.Model
{
	/// <summary>
	/// BS_CUSTOM_QUERY:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BS_CUSTOM_QUERY
	{
        public BS_CUSTOM_QUERY()
        { }
        #region Model
        private Guid _id;
        private Guid _templateid;
        private string _columnname;
        private string _displayname;
        private string _controltype;
        private string _compare;
        private decimal _sequence;
        private string _defaultvalue;
        private string _reminder;
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
        /// 筛选列名
        /// </summary>
        public string ColumnName
        {
            set { _columnname = value; }
            get { return _columnname; }
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
        /// 控件类型，LOOKUP_CODE:BS_CONTROL_TYPE
        /// </summary>
        public string ControlType
        {
            set { _controltype = value; }
            get { return _controltype; }
        }
        /// <summary>
        /// 比较符
        /// </summary>
        public string Compare
        {
            set { _compare = value; }
            get { return _compare; }
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
        /// 
        /// </summary>
        public string DefaultValue
        {
            set { _defaultvalue = value; }
            get { return _defaultvalue; }
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


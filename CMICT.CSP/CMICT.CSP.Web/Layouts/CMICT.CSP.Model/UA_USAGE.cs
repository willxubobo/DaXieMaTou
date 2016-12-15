/**  版本信息模板在安装目录下，可自行修改。
* UA_USAGE.cs
*
* 功 能： N/A
* 类 名： UA_USAGE
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:37   N/A    初版
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
	/// UA_USAGE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UA_USAGE
	{
		public UA_USAGE()
		{}
		#region Model
		private Guid _id;
		private Guid _pagename;
		private string _url;
		private Guid _templateid;
		private string _templatename;
		private decimal _loadtime;
		private string _query;
		private string _browsertype;
		private DateTime _created;
		private string _author;
		/// <summary>
		/// 主键ID
		/// </summary>
		public Guid ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		///   当前页名
		/// </summary>
		public Guid PageName
		{
			set{ _pagename=value;}
			get{return _pagename;}
		}
		/// <summary>
		/// 当前页Url
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 模板ID
		/// </summary>
		public Guid TemplateID
		{
			set{ _templateid=value;}
			get{return _templateid;}
		}
		/// <summary>
		/// 模板名称
		/// </summary>
		public string TemplateName
		{
			set{ _templatename=value;}
			get{return _templatename;}
		}
		/// <summary>
		/// 报表展现加载时间
		/// </summary>
		public decimal LoadTime
		{
			set{ _loadtime=value;}
			get{return _loadtime;}
		}
		/// <summary>
		/// 查询条件
		/// </summary>
		public string Query
		{
			set{ _query=value;}
			get{return _query;}
		}
		/// <summary>
		/// 浏览器类型
		/// </summary>
		public string BrowserType
		{
			set{ _browsertype=value;}
			get{return _browsertype;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string Author
		{
			set{ _author=value;}
			get{return _author;}
		}
		#endregion Model

	}
}


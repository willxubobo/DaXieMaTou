/**  版本信息模板在安装目录下，可自行修改。
* BS_TEMPLATE_PAGES.cs
*
* 功 能： N/A
* 类 名： BS_TEMPLATE_PAGES
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:36   N/A    初版
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
	/// BS_TEMPLATE_PAGES:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BS_TEMPLATE_PAGES
	{
		public BS_TEMPLATE_PAGES()
		{}
		#region Model
		private Guid _pageid;
		private Guid _templateid;
		private string _url;
		private string _pagename;
		private DateTime _created;
		private DateTime _modified;
		private string _author;
		private string _editor;
		/// <summary>
		/// 页面ID
		/// </summary>
		public Guid PageID
		{
			set{ _pageid=value;}
			get{return _pageid;}
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
		/// 页面地址
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 页面名称
		/// </summary>
		public string PageName
		{
			set{ _pagename=value;}
			get{return _pagename;}
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
		/// 修改时间
		/// </summary>
		public DateTime Modified
		{
			set{ _modified=value;}
			get{return _modified;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string Author
		{
			set{ _author=value;}
			get{return _author;}
		}
		/// <summary>
		/// 修改者
		/// </summary>
		public string Editor
		{
			set{ _editor=value;}
			get{return _editor;}
		}
		#endregion Model

	}
}


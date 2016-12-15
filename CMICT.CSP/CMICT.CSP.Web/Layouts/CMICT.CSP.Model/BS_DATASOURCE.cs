/**  版本信息模板在安装目录下，可自行修改。
* BS_DATASOURCE.cs
*
* 功 能： N/A
* 类 名： BS_DATASOURCE
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:35   N/A    初版
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
	/// BS_DATASOURCE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BS_DATASOURCE
	{
		public BS_DATASOURCE()
		{}
		#region Model
		private Guid _sourceid;
		private string _sourcename;
		private string _sourcedesc;
		private string _sourceip;
		private string _username;
		private string _password;
		private string _dbname;
		private string _objecttype;
		private string _objectname;
		private string _sourcestatus;
		private DateTime _created;
		private DateTime _modified;
		private string _author;
		private string _editor;
		/// <summary>
		/// 数据源ID
		/// </summary>
		public Guid SourceID
		{
			set{ _sourceid=value;}
			get{return _sourceid;}
		}
		/// <summary>
		/// 数据源名称
		/// </summary>
		public string SourceName
		{
			set{ _sourcename=value;}
			get{return _sourcename;}
		}
		/// <summary>
		/// 数据源描述
		/// </summary>
		public string SourceDesc
		{
			set{ _sourcedesc=value;}
			get{return _sourcedesc;}
		}
		/// <summary>
		/// 数据库服务器地址
		/// </summary>
		public string SourceIP
		{
			set{ _sourceip=value;}
			get{return _sourceip;}
		}
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 密码
		/// </summary>
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 数据库名称
		/// </summary>
		public string DBName
		{
			set{ _dbname=value;}
			get{return _dbname;}
		}
		/// <summary>
		/// 数据源类型，LOOKUP_CODE:BS_OBJECT_TYPE
		/// </summary>
		public string ObjectType
		{
			set{ _objecttype=value;}
			get{return _objecttype;}
		}
		/// <summary>
		/// 数据源类型名称
		/// </summary>
		public string ObjectName
		{
			set{ _objectname=value;}
			get{return _objectname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SourceStatus
		{
			set{ _sourcestatus=value;}
			get{return _sourcestatus;}
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


/**  版本信息模板在安装目录下，可自行修改。
* BS_COMMUNICATION_DETAIL.cs
*
* 功 能： N/A
* 类 名： BS_COMMUNICATION_DETAIL
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
	/// BS_COMMUNICATION_DETAIL:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class BS_COMMUNICATION_DETAIL
	{
		public BS_COMMUNICATION_DETAIL()
		{}
		#region Model
		private Guid _id;
		private Guid _communicationid;
		private string _sourcecolumnname;
		private string _targetcolumnname;
		private DateTime _created;
		private DateTime _modified;
		private string _author;
		private string _editor;
		/// <summary>
		/// 主键ID
		/// </summary>
		public Guid ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 通信关系ID
		/// </summary>
		public Guid CommunicationID
		{
			set{ _communicationid=value;}
			get{return _communicationid;}
		}
		/// <summary>
		/// 源列名
		/// </summary>
		public string SourceColumnName
		{
			set{ _sourcecolumnname=value;}
			get{return _sourcecolumnname;}
		}
		/// <summary>
		/// 目标列名
		/// </summary>
		public string TargetColumnName
		{
			set{ _targetcolumnname=value;}
			get{return _targetcolumnname;}
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


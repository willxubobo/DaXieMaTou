/**  版本信息模板在安装目录下，可自行修改。
* BS_TEMPLATE_PAGES.cs
*
* 功 能： N/A
* 类 名： BS_TEMPLATE_PAGES
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:37   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：盟拓　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace CMICT.CSP.BLL
{
	/// <summary>
	/// 数据访问类:BS_TEMPLATE_PAGES
	/// </summary>
	public partial class BS_TEMPLATE_PAGESBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_TEMPLATE_PAGESBLL()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_TEMPLATE_PAGES model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_TEMPLATE_PAGES(");
			strSql.Append("PageID,TemplateID,Url,PageName,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@PageID,@TemplateID,@Url,@PageName,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@PageID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Url", SqlDbType.NVarChar,500),
					new SqlParameter("@PageName", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = Guid.NewGuid();
			parameters[2].Value = model.Url;
			parameters[3].Value = model.PageName;
			parameters[4].Value = model.Created;
			parameters[5].Value = model.Modified;
			parameters[6].Value = model.Author;
			parameters[7].Value = model.Editor;

			int rows=dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(CMICT.CSP.Model.BS_TEMPLATE_PAGES model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_TEMPLATE_PAGES set ");
			strSql.Append("TemplateID=@TemplateID,");
			strSql.Append("Url=@Url,");
			strSql.Append("PageName=@PageName,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
            strSql.Append(" where PageID=@PageID");
			SqlParameter[] parameters = {
					new SqlParameter("@PageID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Url", SqlDbType.NVarChar,500),
					new SqlParameter("@PageName", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.PageID;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.Url;
			parameters[3].Value = model.PageName;
			parameters[4].Value = model.Created;
			parameters[5].Value = model.Modified;
			parameters[6].Value = model.Author;
			parameters[7].Value = model.Editor;

			int rows=dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool Delete(Guid PageID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_TEMPLATE_PAGES ");
            strSql.Append(" where PageID=@PageID");
			SqlParameter[] parameters = {
                                            new SqlParameter("@PageID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = PageID;
			int rows=dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public CMICT.CSP.Model.BS_TEMPLATE_PAGES GetModel(Guid PageID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 PageID,TemplateID,Url,PageName,Created,Modified,Author,Editor from BS_TEMPLATE_PAGES ");
            strSql.Append(" where PageID=@PageID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@PageID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = PageID;

			CMICT.CSP.Model.BS_TEMPLATE_PAGES model=new CMICT.CSP.Model.BS_TEMPLATE_PAGES();
			DataSet ds=dbhelper.ExecuteDataSet(CommandType.Text, strSql.ToString(), parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public CMICT.CSP.Model.BS_TEMPLATE_PAGES DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_TEMPLATE_PAGES model=new CMICT.CSP.Model.BS_TEMPLATE_PAGES();
			if (row != null)
			{
				if(row["PageID"]!=null && row["PageID"].ToString()!="")
				{
					model.PageID= new Guid(row["PageID"].ToString());
				}
				if(row["TemplateID"]!=null && row["TemplateID"].ToString()!="")
				{
					model.TemplateID= new Guid(row["TemplateID"].ToString());
				}
				if(row["Url"]!=null)
				{
					model.Url=row["Url"].ToString();
				}
				if(row["PageName"]!=null)
				{
					model.PageName=row["PageName"].ToString();
				}
				if(row["Created"]!=null && row["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(row["Created"].ToString());
				}
				if(row["Modified"]!=null && row["Modified"].ToString()!="")
				{
					model.Modified=DateTime.Parse(row["Modified"].ToString());
				}
				if(row["Author"]!=null)
				{
					model.Author=row["Author"].ToString();
				}
				if(row["Editor"]!=null)
				{
					model.Editor=row["Editor"].ToString();
				}
			}
			return model;
		}


		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}


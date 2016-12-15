/**  版本信息模板在安装目录下，可自行修改。
* BS_TEMPLATE_MAIN.cs
*
* 功 能： N/A
* 类 名： BS_TEMPLATE_MAIN
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:36   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：盟拓　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace CMICT.CSP.BLL
{
	/// <summary>
	/// 数据访问类:BS_TEMPLATE_MAIN
	/// </summary>
	public partial class BS_TEMPLATE_MAINBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_TEMPLATE_MAINBLL()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_TEMPLATE_MAIN model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_TEMPLATE_MAIN(");
			strSql.Append("TemplateID,SourceID,TemplateName,DiaplayType,TemplateDesc,Reminder,PageSize,ColumnSize,TemplateStatus,SQL,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@TemplateID,@SourceID,@TemplateName,@DiaplayType,@TemplateDesc,@Reminder,@PageSize,@ColumnSize,@TemplateStatus,@SQL,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateName", SqlDbType.NVarChar,50),
					new SqlParameter("@DiaplayType", SqlDbType.NVarChar,50),
					new SqlParameter("@TemplateDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@Reminder", SqlDbType.NVarChar,50),
					new SqlParameter("@PageSize", SqlDbType.Decimal,9),
					new SqlParameter("@ColumnSize", SqlDbType.Decimal,9),
					new SqlParameter("@TemplateStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@SQL", SqlDbType.NVarChar,4000),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = Guid.NewGuid();
			parameters[2].Value = model.TemplateName;
			parameters[3].Value = model.DiaplayType;
			parameters[4].Value = model.TemplateDesc;
			parameters[5].Value = model.Reminder;
			parameters[6].Value = model.PageSize;
			parameters[7].Value = model.ColumnSize;
			parameters[8].Value = model.TemplateStatus;
			parameters[9].Value = model.SQL;
			parameters[10].Value = model.Created;
			parameters[11].Value = model.Modified;
			parameters[12].Value = model.Author;
			parameters[13].Value = model.Editor;

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
		public bool Update(CMICT.CSP.Model.BS_TEMPLATE_MAIN model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_TEMPLATE_MAIN set ");
			strSql.Append("SourceID=@SourceID,");
			strSql.Append("TemplateName=@TemplateName,");
			strSql.Append("DiaplayType=@DiaplayType,");
			strSql.Append("TemplateDesc=@TemplateDesc,");
			strSql.Append("Reminder=@Reminder,");
			strSql.Append("PageSize=@PageSize,");
			strSql.Append("ColumnSize=@ColumnSize,");
			strSql.Append("TemplateStatus=@TemplateStatus,");
			strSql.Append("SQL=@SQL,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
            strSql.Append(" where TemplateID=@TemplateID");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateName", SqlDbType.NVarChar,50),
					new SqlParameter("@DiaplayType", SqlDbType.NVarChar,50),
					new SqlParameter("@TemplateDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@Reminder", SqlDbType.NVarChar,50),
					new SqlParameter("@PageSize", SqlDbType.Decimal,9),
					new SqlParameter("@ColumnSize", SqlDbType.Decimal,9),
					new SqlParameter("@TemplateStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@SQL", SqlDbType.NVarChar,4000),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.TemplateID;
			parameters[1].Value = model.SourceID;
			parameters[2].Value = model.TemplateName;
			parameters[3].Value = model.DiaplayType;
			parameters[4].Value = model.TemplateDesc;
			parameters[5].Value = model.Reminder;
			parameters[6].Value = model.PageSize;
			parameters[7].Value = model.ColumnSize;
			parameters[8].Value = model.TemplateStatus;
			parameters[9].Value = model.SQL;
			parameters[10].Value = model.Created;
			parameters[11].Value = model.Modified;
			parameters[12].Value = model.Author;
			parameters[13].Value = model.Editor;

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
        public bool Delete(Guid TemplateID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_TEMPLATE_MAIN ");
            strSql.Append(" where TemplateID=@TemplateID");
			SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
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
        public CMICT.CSP.Model.BS_TEMPLATE_MAIN GetModel(Guid TemplateID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 TemplateID,SourceID,TemplateName,DiaplayType,TemplateDesc,Reminder,PageSize,ColumnSize,TemplateStatus,SQL,Created,Modified,Author,Editor from BS_TEMPLATE_MAIN ");
            strSql.Append(" where TemplateID=@TemplateID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;

			CMICT.CSP.Model.BS_TEMPLATE_MAIN model=new CMICT.CSP.Model.BS_TEMPLATE_MAIN();
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
		public CMICT.CSP.Model.BS_TEMPLATE_MAIN DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_TEMPLATE_MAIN model=new CMICT.CSP.Model.BS_TEMPLATE_MAIN();
			if (row != null)
			{
				if(row["TemplateID"]!=null && row["TemplateID"].ToString()!="")
				{
					model.TemplateID= new Guid(row["TemplateID"].ToString());
				}
				if(row["SourceID"]!=null && row["SourceID"].ToString()!="")
				{
					model.SourceID= new Guid(row["SourceID"].ToString());
				}
				if(row["TemplateName"]!=null)
				{
					model.TemplateName=row["TemplateName"].ToString();
				}
				if(row["DiaplayType"]!=null)
				{
					model.DiaplayType=row["DiaplayType"].ToString();
				}
				if(row["TemplateDesc"]!=null)
				{
					model.TemplateDesc=row["TemplateDesc"].ToString();
				}
				if(row["Reminder"]!=null)
				{
					model.Reminder=row["Reminder"].ToString();
				}
				if(row["PageSize"]!=null && row["PageSize"].ToString()!="")
				{
					model.PageSize=decimal.Parse(row["PageSize"].ToString());
				}
				if(row["ColumnSize"]!=null && row["ColumnSize"].ToString()!="")
				{
					model.ColumnSize=decimal.Parse(row["ColumnSize"].ToString());
				}
				if(row["TemplateStatus"]!=null)
				{
					model.TemplateStatus=row["TemplateStatus"].ToString();
				}
				if(row["SQL"]!=null)
				{
					model.SQL=row["SQL"].ToString();
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

	}
}


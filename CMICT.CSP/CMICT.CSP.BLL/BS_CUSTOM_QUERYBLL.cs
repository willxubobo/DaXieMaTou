﻿/**  版本信息模板在安装目录下，可自行修改。
* BS_CUSTOM_QUERY.cs
*
* 功 能： N/A
* 类 名： BS_CUSTOM_QUERY
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:35   N/A    初版
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
	/// 数据访问类:BS_CUSTOM_QUERY
	/// </summary>
	public partial class BS_CUSTOM_QUERYBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_CUSTOM_QUERYBLL()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_CUSTOM_QUERY model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BS_CUSTOM_QUERY(");
            strSql.Append("ID,TemplateID,ColumnName,DisplayName,ControlType,Compare,Sequence,DefaultValue,Reminder,Created,Modified,Author,Editor)");
            strSql.Append(" values (");
            strSql.Append("@ID,@TemplateID,@ColumnName,@DisplayName,@ControlType,@Compare,@Sequence,@DefaultValue,@Reminder,@Created,@Modified,@Author,@Editor)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@ColumnName", SqlDbType.NVarChar,50),
					new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
					new SqlParameter("@ControlType", SqlDbType.NVarChar,50),
					new SqlParameter("@Compare", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@DefaultValue", SqlDbType.NVarChar,200),
					new SqlParameter("@Reminder", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
            parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.ColumnName;
			parameters[3].Value = model.DisplayName;
			parameters[4].Value = model.ControlType;
			parameters[5].Value = model.Compare;
            if (model.Sequence == -1) {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                parameters[6].Value = model.Sequence;
            }
            parameters[7].Value = model.DefaultValue;
			parameters[8].Value = model.Reminder;
			parameters[9].Value = model.Created;
			parameters[10].Value = model.Modified;
			parameters[11].Value = model.Author;
			parameters[12].Value = model.Editor;

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
		public bool Update(CMICT.CSP.Model.BS_CUSTOM_QUERY model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_CUSTOM_QUERY set ");
            strSql.Append("TemplateID=@TemplateID,");
            strSql.Append("ColumnName=@ColumnName,");
            strSql.Append("DisplayName=@DisplayName,");
            strSql.Append("ControlType=@ControlType,");
            strSql.Append("Compare=@Compare,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("DefaultValue=@DefaultValue,");
            strSql.Append("Reminder=@Reminder,");
            strSql.Append("Created=@Created,");
            strSql.Append("Modified=@Modified,");
            strSql.Append("Author=@Author,");
            strSql.Append("Editor=@Editor");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@ColumnName", SqlDbType.NVarChar,50),
					new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
					new SqlParameter("@ControlType", SqlDbType.NVarChar,50),
					new SqlParameter("@Compare", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@DefaultValue", SqlDbType.NVarChar,200),
					new SqlParameter("@Reminder", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TemplateID;
            parameters[2].Value = model.ColumnName;
            parameters[3].Value = model.DisplayName;
            parameters[4].Value = model.ControlType;
            parameters[5].Value = model.Compare;
            parameters[6].Value = model.Sequence;
            parameters[7].Value = model.DefaultValue;
            parameters[8].Value = model.Reminder;
            parameters[9].Value = model.Created;
            parameters[10].Value = model.Modified;
            parameters[11].Value = model.Author;
            parameters[12].Value = model.Editor;

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
		public bool Delete(Guid ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_CUSTOM_QUERY ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = ID;

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
		public CMICT.CSP.Model.BS_CUSTOM_QUERY GetModel(Guid ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,TemplateID,ColumnName,DisplayName,ControlType,Compare,Sequence,DefaultValue,Reminder,Created,Modified,Author,Editor from BS_CUSTOM_QUERY ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = ID;

			CMICT.CSP.Model.BS_CUSTOM_QUERY model=new CMICT.CSP.Model.BS_CUSTOM_QUERY();
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
		public CMICT.CSP.Model.BS_CUSTOM_QUERY DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_CUSTOM_QUERY model=new CMICT.CSP.Model.BS_CUSTOM_QUERY();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID= new Guid(row["ID"].ToString());
				}
				if(row["TemplateID"]!=null && row["TemplateID"].ToString()!="")
				{
					model.TemplateID= new Guid(row["TemplateID"].ToString());
				}
				if(row["ColumnName"]!=null)
				{
					model.ColumnName=row["ColumnName"].ToString();
				}
				if(row["DisplayName"]!=null)
				{
					model.DisplayName=row["DisplayName"].ToString();
				}
				if(row["ControlType"]!=null)
				{
					model.ControlType=row["ControlType"].ToString();
				}
				if(row["Compare"]!=null)
				{
					model.Compare=row["Compare"].ToString();
				}
				if(row["Sequence"]!=null && row["Sequence"].ToString()!="")
				{
					model.Sequence=decimal.Parse(row["Sequence"].ToString());
				}
                if (row["DefaultValue"] != null)
                {
                    model.DefaultValue = row["DefaultValue"].ToString();
                }
				if(row["Reminder"]!=null)
				{
					model.Reminder=row["Reminder"].ToString();
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

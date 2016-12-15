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
*│　版权所有：盟拓　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace CMICT.CSP.BLL
{
	/// <summary>
	/// 数据访问类:BS_DATASOURCE
	/// </summary>
	public partial class BS_DATASOURCEBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_DATASOURCEBLL()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_DATASOURCE model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_DATASOURCE(");
			strSql.Append("SourceID,SourceName,SourceDesc,SourceIP,UserName,Password,DBName,ObjectType,ObjectName,SourceStatus,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@SourceID,@SourceName,@SourceDesc,@SourceIP,@UserName,@Password,@DBName,@ObjectType,@ObjectName,@SourceStatus,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SourceName", SqlDbType.NVarChar,50),
					new SqlParameter("@SourceDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@SourceIP", SqlDbType.NVarChar,50),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@DBName", SqlDbType.NVarChar,50),
					new SqlParameter("@ObjectType", SqlDbType.NVarChar,50),
					new SqlParameter("@ObjectName", SqlDbType.NVarChar,50),
					new SqlParameter("@SourceStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.SourceName;
			parameters[2].Value = model.SourceDesc;
			parameters[3].Value = model.SourceIP;
			parameters[4].Value = model.UserName;
			parameters[5].Value = model.Password;
			parameters[6].Value = model.DBName;
			parameters[7].Value = model.ObjectType;
			parameters[8].Value = model.ObjectName;
			parameters[9].Value = model.SourceStatus;
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
		public bool Update(CMICT.CSP.Model.BS_DATASOURCE model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_DATASOURCE set ");
			strSql.Append("SourceName=@SourceName,");
			strSql.Append("SourceDesc=@SourceDesc,");
			strSql.Append("SourceIP=@SourceIP,");
			strSql.Append("UserName=@UserName,");
			strSql.Append("Password=@Password,");
			strSql.Append("DBName=@DBName,");
			strSql.Append("ObjectType=@ObjectType,");
			strSql.Append("ObjectName=@ObjectName,");
			strSql.Append("SourceStatus=@SourceStatus,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
            strSql.Append(" where SourceID=@SourceID");
			SqlParameter[] parameters = {
					new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SourceName", SqlDbType.NVarChar,50),
					new SqlParameter("@SourceDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@SourceIP", SqlDbType.NVarChar,50),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@DBName", SqlDbType.NVarChar,50),
					new SqlParameter("@ObjectType", SqlDbType.NVarChar,50),
					new SqlParameter("@ObjectName", SqlDbType.NVarChar,50),
					new SqlParameter("@SourceStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.SourceID;
			parameters[1].Value = model.SourceName;
			parameters[2].Value = model.SourceDesc;
			parameters[3].Value = model.SourceIP;
			parameters[4].Value = model.UserName;
			parameters[5].Value = model.Password;
			parameters[6].Value = model.DBName;
			parameters[7].Value = model.ObjectType;
			parameters[8].Value = model.ObjectName;
			parameters[9].Value = model.SourceStatus;
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
        public bool Delete(Guid SourceID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_DATASOURCE ");
            strSql.Append(" where SourceID=@SourceID");
			SqlParameter[] parameters = {
                                            new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = SourceID;
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
        public CMICT.CSP.Model.BS_DATASOURCE GetModel(Guid SourceID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SourceID,SourceName,SourceDesc,SourceIP,UserName,Password,DBName,ObjectType,ObjectName,SourceStatus,Created,Modified,Author,Editor from BS_DATASOURCE ");
            strSql.Append(" where SourceID=@SourceID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = SourceID;

			CMICT.CSP.Model.BS_DATASOURCE model=new CMICT.CSP.Model.BS_DATASOURCE();
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
		public CMICT.CSP.Model.BS_DATASOURCE DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_DATASOURCE model=new CMICT.CSP.Model.BS_DATASOURCE();
			if (row != null)
			{
				if(row["SourceID"]!=null && row["SourceID"].ToString()!="")
				{
					model.SourceID= new Guid(row["SourceID"].ToString());
				}
				if(row["SourceName"]!=null)
				{
					model.SourceName=row["SourceName"].ToString();
				}
				if(row["SourceDesc"]!=null)
				{
					model.SourceDesc=row["SourceDesc"].ToString();
				}
				if(row["SourceIP"]!=null)
				{
					model.SourceIP=row["SourceIP"].ToString();
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["Password"]!=null)
				{
					model.Password=row["Password"].ToString();
				}
				if(row["DBName"]!=null)
				{
					model.DBName=row["DBName"].ToString();
				}
				if(row["ObjectType"]!=null)
				{
					model.ObjectType=row["ObjectType"].ToString();
				}
				if(row["ObjectName"]!=null)
				{
					model.ObjectName=row["ObjectName"].ToString();
				}
				if(row["SourceStatus"]!=null)
				{
					model.SourceStatus=row["SourceStatus"].ToString();
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


/**  版本信息模板在安装目录下，可自行修改。
* BS_COMPUTE.cs
*
* 功 能： N/A
* 类 名： BS_COMPUTE
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:34   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：盟拓　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace CMICT.CSP.BLL
{
	/// <summary>
	/// 数据访问类:BS_COMPUTE
	/// </summary>
	public partial class BS_COMPUTEBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_COMPUTEBLL()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_COMPUTE model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_COMPUTE(");
			strSql.Append("ID,TemplateID,Compute,DisplayName,MergeColumnName,Sequence,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@ID,@TemplateID,@Compute,@DisplayName,@MergeColumnName,@Sequence,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Compute", SqlDbType.NVarChar,50),
					new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
					new SqlParameter("@MergeColumnName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = Guid.NewGuid();
			parameters[2].Value = model.Compute;
			parameters[3].Value = model.DisplayName;
			parameters[4].Value = model.MergeColumnName;
			parameters[5].Value = model.Sequence;
			parameters[6].Value = model.Created;
			parameters[7].Value = model.Modified;
			parameters[8].Value = model.Author;
			parameters[9].Value = model.Editor;

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
		public bool Update(CMICT.CSP.Model.BS_COMPUTE model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_COMPUTE set ");
			strSql.Append("TemplateID=@TemplateID,");
			strSql.Append("Compute=@Compute,");
			strSql.Append("DisplayName=@DisplayName,");
			strSql.Append("MergeColumnName=@MergeColumnName,");
			strSql.Append("Sequence=@Sequence,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
            strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Compute", SqlDbType.NVarChar,50),
					new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
					new SqlParameter("@MergeColumnName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.Compute;
			parameters[3].Value = model.DisplayName;
			parameters[4].Value = model.MergeColumnName;
			parameters[5].Value = model.Sequence;
			parameters[6].Value = model.Created;
			parameters[7].Value = model.Modified;
			parameters[8].Value = model.Author;
			parameters[9].Value = model.Editor;

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
			strSql.Append("delete from BS_COMPUTE ");
            strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value =ID;
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
		public CMICT.CSP.Model.BS_COMPUTE GetModel(Guid ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,TemplateID,Compute,DisplayName,MergeColumnName,Sequence,Created,Modified,Author,Editor from BS_COMPUTE ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = ID;

			CMICT.CSP.Model.BS_COMPUTE model=new CMICT.CSP.Model.BS_COMPUTE();
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
		public CMICT.CSP.Model.BS_COMPUTE DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_COMPUTE model=new CMICT.CSP.Model.BS_COMPUTE();
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
				if(row["Compute"]!=null)
				{
					model.Compute=row["Compute"].ToString();
				}
				if(row["DisplayName"]!=null)
				{
					model.DisplayName=row["DisplayName"].ToString();
				}
				if(row["MergeColumnName"]!=null)
				{
					model.MergeColumnName=row["MergeColumnName"].ToString();
				}
				if(row["Sequence"]!=null && row["Sequence"].ToString()!="")
				{
					model.Sequence=decimal.Parse(row["Sequence"].ToString());
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

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID,TemplateID,Compute,DisplayName,MergeColumnName,Sequence,Created,Modified,Author,Editor ");
			strSql.Append(" FROM BS_COMPUTE ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            return dbhelper.ExecuteDataSet(CommandType.Text, strSql.ToString(), null);
		}

		

		#endregion  BasicMethod
	}
}


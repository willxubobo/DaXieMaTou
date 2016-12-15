using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL
{
   /// <summary>
	/// 数据访问类:BS_GROUPBY
	/// </summary>
	public partial class BS_GROUPBYBLL
	{
        DbHelperSQL dbhelper = new DbHelperSQL();
		public BS_GROUPBYBLL()
		{}
		#region  BasicMethod


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_GROUPBY model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_GROUPBY(");
			strSql.Append("ID,TemplateID,Columns,Location,ComputeColumn,Sequence,SQL,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@ID,@TemplateID,@Columns,@Location,@ComputeColumn,@Sequence,@SQL,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Columns", SqlDbType.NVarChar,500),
					new SqlParameter("@Location", SqlDbType.NVarChar,50),
					new SqlParameter("@ComputeColumn", SqlDbType.NVarChar,500),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@SQL", SqlDbType.NVarChar,-1),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.Columns;
			parameters[3].Value = model.Location;
			parameters[4].Value = model.ComputeColumn;
			parameters[5].Value = model.Sequence;
			parameters[6].Value = model.SQL;
			parameters[7].Value = model.Created;
			parameters[8].Value = model.Modified;
			parameters[9].Value = model.Author;
			parameters[10].Value = model.Editor;

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
		public bool Update(CMICT.CSP.Model.BS_GROUPBY model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_GROUPBY set ");
			strSql.Append("ID=@ID,");
			strSql.Append("TemplateID=@TemplateID,");
			strSql.Append("Columns=@Columns,");
			strSql.Append("Location=@Location,");
			strSql.Append("ComputeColumn=@ComputeColumn,");
			strSql.Append("Sequence=@Sequence,");
			strSql.Append("SQL=@SQL,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Columns", SqlDbType.NVarChar,500),
					new SqlParameter("@Location", SqlDbType.NVarChar,50),
					new SqlParameter("@ComputeColumn", SqlDbType.NVarChar,500),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@SQL", SqlDbType.NVarChar,-1),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.Columns;
			parameters[3].Value = model.Location;
			parameters[4].Value = model.ComputeColumn;
			parameters[5].Value = model.Sequence;
			parameters[6].Value = model.SQL;
			parameters[7].Value = model.Created;
			parameters[8].Value = model.Modified;
			parameters[9].Value = model.Author;
			parameters[10].Value = model.Editor;

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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_GROUPBY ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
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
		public CMICT.CSP.Model.BS_GROUPBY GetModel(Guid ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,TemplateID,Columns,Location,ComputeColumn,Sequence,SQL,Created,Modified,Author,Editor from BS_GROUPBY ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = ID;

			CMICT.CSP.Model.BS_GROUPBY model=new CMICT.CSP.Model.BS_GROUPBY();
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
		public CMICT.CSP.Model.BS_GROUPBY DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_GROUPBY model=new CMICT.CSP.Model.BS_GROUPBY();
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
				if(row["Columns"]!=null)
				{
					model.Columns=row["Columns"].ToString();
				}
				if(row["Location"]!=null)
				{
					model.Location=row["Location"].ToString();
				}
				if(row["ComputeColumn"]!=null)
				{
					model.ComputeColumn=row["ComputeColumn"].ToString();
				}
				if(row["Sequence"]!=null && row["Sequence"].ToString()!="")
				{
					model.Sequence=decimal.Parse(row["Sequence"].ToString());
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
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL
{
    public class BS_CODEMAPPINGBLL
    {
        DbHelperSQL dbhelper = new DbHelperSQL();

        private string ConString = System.Configuration.ConfigurationSettings.AppSettings["CMICTCodeMappingConnectionString"].ToString();


    public BS_CODEMAPPINGBLL()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(CMICT.CSP.Model.BS_CODEMAPPING model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BS_CODEMAPPING(");
			strSql.Append("MappingID,CustomerID,CustomerName,CustomerDesc,SemanticDesc,BusinessCode,BusinessCodeDesc,BusinessTranslation,TargetCode,TargetCodeEnDesc,TargetCodeCnDesc,CMICTCode,CMICTCodeEnDesc,CMICTCodeCnDesc,StartDate,ExpireDate,Created,Modified,Author,Editor)");
			strSql.Append(" values (");
			strSql.Append("@MappingID,@CustomerID,@CustomerName,@CustomerDesc,@SemanticDesc,@BusinessCode,@BusinessCodeDesc,@BusinessTranslation,@TargetCode,@TargetCodeEnDesc,@TargetCodeCnDesc,@CMICTCode,@CMICTCodeEnDesc,@CMICTCodeCnDesc,@StartDate,@ExpireDate,@Created,@Modified,@Author,@Editor)");
			SqlParameter[] parameters = {
					new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@CustomerID", SqlDbType.NVarChar,50),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,50),
					new SqlParameter("@CustomerDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@SemanticDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@BusinessCode", SqlDbType.NVarChar,50),
					new SqlParameter("@BusinessCodeDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessTranslation", SqlDbType.NVarChar,500),
					new SqlParameter("@TargetCode", SqlDbType.NVarChar,50),
					new SqlParameter("@TargetCodeEnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TargetCodeCnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@CMICTCode", SqlDbType.NVarChar,50),
					new SqlParameter("@CMICTCodeEnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@CMICTCodeCnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@ExpireDate", SqlDbType.DateTime),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.CustomerID;
			parameters[2].Value = model.CustomerName;
			parameters[3].Value = model.CustomerDesc;
			parameters[4].Value = model.SemanticDesc;
			parameters[5].Value = model.BusinessCode;
			parameters[6].Value = model.BusinessCodeDesc;
			parameters[7].Value = model.BusinessTranslation;
			parameters[8].Value = model.TargetCode;
			parameters[9].Value = model.TargetCodeEnDesc;
			parameters[10].Value = model.TargetCodeCnDesc;
			parameters[11].Value = model.CMICTCode;
			parameters[12].Value = model.CMICTCodeEnDesc;
			parameters[13].Value = model.CMICTCodeCnDesc;
			parameters[14].Value = model.StartDate;
			parameters[15].Value = model.ExpireDate;
			parameters[16].Value = model.Created;
			parameters[17].Value = model.Modified;
			parameters[18].Value = model.Author;
			parameters[19].Value = model.Editor;

            int rows = dbhelper.ExecuteNonQuery(ConString, CommandType.Text, strSql.ToString(), parameters);
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
		public bool Update(CMICT.CSP.Model.BS_CODEMAPPING model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BS_CODEMAPPING set ");
			strSql.Append("MappingID=@MappingID,");
			strSql.Append("CustomerID=@CustomerID,");
			strSql.Append("CustomerName=@CustomerName,");
			strSql.Append("CustomerDesc=@CustomerDesc,");
			strSql.Append("SemanticDesc=@SemanticDesc,");
			strSql.Append("BusinessCode=@BusinessCode,");
			strSql.Append("BusinessCodeDesc=@BusinessCodeDesc,");
			strSql.Append("BusinessTranslation=@BusinessTranslation,");
			strSql.Append("TargetCode=@TargetCode,");
			strSql.Append("TargetCodeEnDesc=@TargetCodeEnDesc,");
			strSql.Append("TargetCodeCnDesc=@TargetCodeCnDesc,");
			strSql.Append("CMICTCode=@CMICTCode,");
			strSql.Append("CMICTCodeEnDesc=@CMICTCodeEnDesc,");
			strSql.Append("CMICTCodeCnDesc=@CMICTCodeCnDesc,");
			strSql.Append("StartDate=@StartDate,");
			strSql.Append("ExpireDate=@ExpireDate,");
			strSql.Append("Created=@Created,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("Author=@Author,");
			strSql.Append("Editor=@Editor");
			strSql.Append(" where MappingID=@MappingID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@CustomerID", SqlDbType.NVarChar,50),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,50),
					new SqlParameter("@CustomerDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@SemanticDesc", SqlDbType.NVarChar,200),
					new SqlParameter("@BusinessCode", SqlDbType.NVarChar,50),
					new SqlParameter("@BusinessCodeDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessTranslation", SqlDbType.NVarChar,500),
					new SqlParameter("@TargetCode", SqlDbType.NVarChar,50),
					new SqlParameter("@TargetCodeEnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TargetCodeCnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@CMICTCode", SqlDbType.NVarChar,50),
					new SqlParameter("@CMICTCodeEnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@CMICTCodeCnDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@ExpireDate", SqlDbType.DateTime),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.MappingID;
			parameters[1].Value = model.CustomerID;
			parameters[2].Value = model.CustomerName;
			parameters[3].Value = model.CustomerDesc;
			parameters[4].Value = model.SemanticDesc;
			parameters[5].Value = model.BusinessCode;
			parameters[6].Value = model.BusinessCodeDesc;
			parameters[7].Value = model.BusinessTranslation;
			parameters[8].Value = model.TargetCode;
			parameters[9].Value = model.TargetCodeEnDesc;
			parameters[10].Value = model.TargetCodeCnDesc;
			parameters[11].Value = model.CMICTCode;
			parameters[12].Value = model.CMICTCodeEnDesc;
			parameters[13].Value = model.CMICTCodeCnDesc;
			parameters[14].Value = model.StartDate;
			parameters[15].Value = model.ExpireDate;
			parameters[16].Value = model.Created;
			parameters[17].Value = model.Modified;
			parameters[18].Value = model.Author;
			parameters[19].Value = model.Editor;

            int rows = dbhelper.ExecuteNonQuery(ConString, CommandType.Text, strSql.ToString(), parameters);
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
		public bool Delete(Guid MappingID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BS_CODEMAPPING ");
			strSql.Append(" where MappingID=@MappingID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = MappingID;

            int rows = dbhelper.ExecuteNonQuery(ConString, CommandType.Text, strSql.ToString(), parameters);
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
		public CMICT.CSP.Model.BS_CODEMAPPING GetModel(Guid MappingID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 MappingID,CustomerID,CustomerName,CustomerDesc,SemanticDesc,BusinessCode,BusinessCodeDesc,BusinessTranslation,TargetCode,TargetCodeEnDesc,TargetCodeCnDesc,CMICTCode,CMICTCodeEnDesc,CMICTCodeCnDesc,StartDate,ExpireDate,Created,Modified,Author,Editor from BS_CODEMAPPING ");
			strSql.Append(" where MappingID=@MappingID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MappingID", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = MappingID;

			CMICT.CSP.Model.BS_CODEMAPPING model=new CMICT.CSP.Model.BS_CODEMAPPING();
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
		public CMICT.CSP.Model.BS_CODEMAPPING DataRowToModel(DataRow row)
		{
			CMICT.CSP.Model.BS_CODEMAPPING model=new CMICT.CSP.Model.BS_CODEMAPPING();
			if (row != null)
			{
				if(row["MappingID"]!=null && row["MappingID"].ToString()!="")
				{
					model.MappingID= new Guid(row["MappingID"].ToString());
				}
				if(row["CustomerID"]!=null)
				{
					model.CustomerID=row["CustomerID"].ToString();
				}
				if(row["CustomerName"]!=null)
				{
					model.CustomerName=row["CustomerName"].ToString();
				}
				if(row["CustomerDesc"]!=null)
				{
					model.CustomerDesc=row["CustomerDesc"].ToString();
				}
				if(row["SemanticDesc"]!=null)
				{
					model.SemanticDesc=row["SemanticDesc"].ToString();
				}
				if(row["BusinessCode"]!=null)
				{
					model.BusinessCode=row["BusinessCode"].ToString();
				}
				if(row["BusinessCodeDesc"]!=null)
				{
					model.BusinessCodeDesc=row["BusinessCodeDesc"].ToString();
				}
				if(row["BusinessTranslation"]!=null)
				{
					model.BusinessTranslation=row["BusinessTranslation"].ToString();
				}
				if(row["TargetCode"]!=null)
				{
					model.TargetCode=row["TargetCode"].ToString();
				}
				if(row["TargetCodeEnDesc"]!=null)
				{
					model.TargetCodeEnDesc=row["TargetCodeEnDesc"].ToString();
				}
				if(row["TargetCodeCnDesc"]!=null)
				{
					model.TargetCodeCnDesc=row["TargetCodeCnDesc"].ToString();
				}
				if(row["CMICTCode"]!=null)
				{
					model.CMICTCode=row["CMICTCode"].ToString();
				}
				if(row["CMICTCodeEnDesc"]!=null)
				{
					model.CMICTCodeEnDesc=row["CMICTCodeEnDesc"].ToString();
				}
				if(row["CMICTCodeCnDesc"]!=null)
				{
					model.CMICTCodeCnDesc=row["CMICTCodeCnDesc"].ToString();
				}
				if(row["StartDate"]!=null && row["StartDate"].ToString()!="")
				{
					model.StartDate=DateTime.Parse(row["StartDate"].ToString());
				}
				if(row["ExpireDate"]!=null && row["ExpireDate"].ToString()!="")
				{
					model.ExpireDate=DateTime.Parse(row["ExpireDate"].ToString());
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
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace CMICT.CSP.BLL
{
    /// <summary>
    /// 数据访问类:BS_TEMPLATE_SORT
    /// </summary>
    public partial class BS_TEMPLATE_SORTBLL
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public BS_TEMPLATE_SORTBLL()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(CMICT.CSP.Model.BS_TEMPLATE_SORT model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BS_TEMPLATE_SORT(");
            strSql.Append("ID,TemplateID,SortColumn,Type,Sequence,Created,Modified,Author,Editor)");
            strSql.Append(" values (");
            strSql.Append("@ID,@TemplateID,@SortColumn,@Type,@Sequence,@Created,@Modified,@Author,@Editor)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SortColumn", SqlDbType.NVarChar,50),
					new SqlParameter("@Type", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
            parameters[0].Value = Guid.NewGuid();
            parameters[1].Value = model.TemplateID;
            parameters[2].Value = model.SortColumn;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.Created;
            parameters[6].Value = model.Modified;
            parameters[7].Value = model.Author;
            parameters[8].Value = model.Editor;

            int rows = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
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
        public bool Update(CMICT.CSP.Model.BS_TEMPLATE_SORT model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_TEMPLATE_SORT set ");
            strSql.Append("ID=@ID,");
            strSql.Append("TemplateID=@TemplateID,");
            strSql.Append("SortColumn=@SortColumn,");
            strSql.Append("Type=@Type,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("Created=@Created,");
            strSql.Append("Modified=@Modified,");
            strSql.Append("Author=@Author,");
            strSql.Append("Editor=@Editor");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@SortColumn", SqlDbType.NVarChar,50),
					new SqlParameter("@Type", SqlDbType.NVarChar,50),
					new SqlParameter("@Sequence", SqlDbType.Decimal,9),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50),
					new SqlParameter("@Editor", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TemplateID;
            parameters[2].Value = model.SortColumn;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.Created;
            parameters[6].Value = model.Modified;
            parameters[7].Value = model.Author;
            parameters[8].Value = model.Editor;

            int rows = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BS_TEMPLATE_SORT ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = ID;

            int rows = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
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
        public CMICT.CSP.Model.BS_TEMPLATE_SORT GetModel(Guid ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,TemplateID,SortColumn,Type,Sequence,Created,Modified,Author,Editor from BS_TEMPLATE_SORT ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = ID;

            CMICT.CSP.Model.BS_TEMPLATE_SORT model = new CMICT.CSP.Model.BS_TEMPLATE_SORT();
            DataSet ds = dbhelper.ExecuteDataSet(CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
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
        public CMICT.CSP.Model.BS_TEMPLATE_SORT DataRowToModel(DataRow row)
        {
            CMICT.CSP.Model.BS_TEMPLATE_SORT model = new CMICT.CSP.Model.BS_TEMPLATE_SORT();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["TemplateID"] != null && row["TemplateID"].ToString() != "")
                {
                    model.TemplateID = new Guid(row["TemplateID"].ToString());
                }
                if (row["SortColumn"] != null)
                {
                    model.SortColumn = row["SortColumn"].ToString();
                }
                if (row["Type"] != null)
                {
                    model.Type = row["Type"].ToString();
                }
                if (row["Sequence"] != null && row["Sequence"].ToString() != "")
                {
                    model.Sequence = decimal.Parse(row["Sequence"].ToString());
                }
                if (row["Created"] != null && row["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(row["Created"].ToString());
                }
                if (row["Modified"] != null && row["Modified"].ToString() != "")
                {
                    model.Modified = DateTime.Parse(row["Modified"].ToString());
                }
                if (row["Author"] != null)
                {
                    model.Author = row["Author"].ToString();
                }
                if (row["Editor"] != null)
                {
                    model.Editor = row["Editor"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TemplateID,SortColumn,Type,Sequence,Created,Modified,Author,Editor ");
            strSql.Append(" FROM BS_TEMPLATE_SORT ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return dbhelper.ExecuteDataSet(CommandType.Text, strSql.ToString(), null);
        }

        

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}


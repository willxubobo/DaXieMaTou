/**  版本信息模板在安装目录下，可自行修改。
* UA_USAGE.cs
*
* 功 能： N/A
* 类 名： UA_USAGE
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  08/17/2015 10:10:37   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：盟拓软件　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace CMICT.CSP.BLL
{
    /// <summary>
    /// 数据访问类:UA_USAGE
    /// </summary>
    public partial class UA_USAGEBLL
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public UA_USAGEBLL()
        { }
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(CMICT.CSP.Model.UA_USAGE model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UA_USAGE(");
            strSql.Append("ID,PageName,Url,TemplateID,TemplateName,LoadTime,BrowserType,Created,Author)");
            strSql.Append(" values (");
            strSql.Append("@ID,@PageName,@Url,@TemplateID,@TemplateName,@LoadTime,@BrowserType,@Created,@Author)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@PageName", SqlDbType.NVarChar,50),
					new SqlParameter("@Url", SqlDbType.NVarChar,500),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateName", SqlDbType.NVarChar,50),
					new SqlParameter("@LoadTime", SqlDbType.Decimal,9),
					new SqlParameter("@BrowserType", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.PageName;
            parameters[2].Value = model.Url;
            parameters[3].Value = model.TemplateID;
            parameters[4].Value = model.TemplateName;
            parameters[5].Value = model.LoadTime;
            parameters[6].Value = model.BrowserType;
            parameters[7].Value = model.Created;
            parameters[8].Value = model.Author;

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
        public bool Update(CMICT.CSP.Model.UA_USAGE model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UA_USAGE set ");
            strSql.Append("ID=@ID,");
            strSql.Append("PageName=@PageName,");
            strSql.Append("Url=@Url,");
            strSql.Append("TemplateID=@TemplateID,");
            strSql.Append("TemplateName=@TemplateName,");
            strSql.Append("LoadTime=@LoadTime,");
            strSql.Append("BrowserType=@BrowserType,");
            strSql.Append("Created=@Created,");
            strSql.Append("Author=@Author");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@PageName", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Url", SqlDbType.NVarChar,50),
					new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@TemplateName", SqlDbType.NVarChar,50),
					new SqlParameter("@LoadTime", SqlDbType.Decimal,9),
					new SqlParameter("@BrowserType", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Author", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.PageName;
            parameters[2].Value = model.Url;
            parameters[3].Value = model.TemplateID;
            parameters[4].Value = model.TemplateName;
            parameters[5].Value = model.LoadTime;
            parameters[6].Value = model.BrowserType;
            parameters[7].Value = model.Created;
            parameters[8].Value = model.Author;

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
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UA_USAGE ");
            strSql.Append(" where  ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
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
        public CMICT.CSP.Model.UA_USAGE GetModel(Guid ID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,PageName,Url,TemplateID,TemplateName,LoadTime,Query,BrowserType,Created,Author from UA_USAGE ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = ID;

            CMICT.CSP.Model.UA_USAGE model = new CMICT.CSP.Model.UA_USAGE();
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
        public CMICT.CSP.Model.UA_USAGE DataRowToModel(DataRow row)
        {
            CMICT.CSP.Model.UA_USAGE model = new CMICT.CSP.Model.UA_USAGE();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["PageName"] != null && row["PageName"].ToString() != "")
                {
                    model.PageName = row["PageName"].ToString();
                }
                if (row["Url"] != null)
                {
                    model.Url = row["Url"].ToString();
                }
                if (row["TemplateID"] != null && row["TemplateID"].ToString() != "")
                {
                    model.TemplateID = new Guid(row["TemplateID"].ToString());
                }
                if (row["TemplateName"] != null)
                {
                    model.TemplateName = row["TemplateName"].ToString();
                }
                if (row["LoadTime"] != null && row["LoadTime"].ToString() != "")
                {
                    model.LoadTime = decimal.Parse(row["LoadTime"].ToString());
                }
                if (row["BrowserType"] != null)
                {
                    model.BrowserType = row["BrowserType"].ToString();
                }
                if (row["Created"] != null && row["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(row["Created"].ToString());
                }
                if (row["Author"] != null)
                {
                    model.Author = row["Author"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM UA_USAGE ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), null);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from UA_USAGE T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), null);


        #endregion  BasicMethod
            #region  ExtensionMethod

            #endregion  ExtensionMethod
        }
    }
}


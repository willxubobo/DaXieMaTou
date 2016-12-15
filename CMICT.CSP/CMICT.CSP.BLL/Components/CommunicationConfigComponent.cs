using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class CommunicationConfigComponent
    {
        DbHelperSQL dbhelper;
        public CommunicationConfigComponent()
        {
            dbhelper = new DbHelperSQL();
        }
        public CommunicationConfigComponent(string ip, string uname, string pwd, string dbname)
        {
            string constr = "Max Pool Size=2048;server=" + ip + ";uid=" + uname + ";pwd=" + pwd + ";database=" + dbname + "";
            dbhelper = new DbHelperSQL(constr);
        }

        /// <summary>
        /// 根据当前模板ID获取通信配置列表
        /// </summary>
        /// <param name="templateID">当前模板ID</param>
        /// <returns>通信配置列表</returns>
        public DataTable GetCommunicationList(string templateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select cm.*, tt.TemplateName as TargetName,st.TemplateName as SourceName ");
            strSql.Append(" FROM BS_COMMUNICATION_MAIN cm left join BS_TEMPLATE_MAIN tt on tt.TemplateID=cm.TargetTemplateID");
            strSql.Append(" left join BS_TEMPLATE_MAIN st on st.TemplateID=cm.SourceTemplateID ");
            strSql.Append(" where TargetTemplateID=@TargetTemplateID  order by  cm.Name collate chinese_prc_ci_as_ks_ws asc");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TargetTemplateID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = templateID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条通信配置
        /// </summary>
        /// <param name="communicationID">通信配置ID</param>
        /// <returns>执行结果</returns>
        public bool DeleteCommunication(string communicationID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from BS_COMMUNICATION_MAIN where CommunicationID=@CommunicationID;");
            strSql.Append(" delete from BS_COMMUNICATION_DETAIL where CommunicationID=@CommunicationID;");

            SqlParameter[] parameters = {
                                            new SqlParameter("@CommunicationID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = communicationID;

            int count = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            return count > 0 ? true : false;
        }

        /// <summary>
        /// 得出除当前模板外的其他所有模板
        /// </summary>
        /// <param name="templateID">需要排除的模板ID</param>
        /// <returns></returns>
        public DataTable GetAllTemplateByExcludeID(string templateID,string bigcate,string smallcate)
        {
            List<SqlParameter> parameterlist = new List<SqlParameter>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select templateID, TemplateName,case when LEN(TemplateName)<14 then TemplateName else SUBSTRING(TemplateName,0,13)+'...' end as namedesc ");
            strSql.Append(" from BS_TEMPLATE_MAIN  ");
            strSql.Append(" where TemplateID <>  @TemplateID and TemplateStatus not in ('DISABLE','DRAFT')");
            if (!string.IsNullOrEmpty(bigcate))
            {
                 strSql.Append(" and BigCategory = @bigcate");
                SqlParameter sp = new SqlParameter("@bigcate", bigcate);
                parameterlist.Add(sp);
            }
            if (!string.IsNullOrEmpty(smallcate))
            {
                 strSql.Append(" and SmallCategory = @smallcate");
                SqlParameter sp = new SqlParameter("@smallcate", smallcate);
                parameterlist.Add(sp);
            }
            strSql.Append(" order by  TemplateName collate chinese_prc_ci_as_ks_ws asc");
            SqlParameter tidsp = new SqlParameter("@TemplateID", templateID);
            parameterlist.Add(tidsp);
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameterlist.ToArray());
        }

        /// <summary>
        /// 根据ID获取通信配置信息
        /// </summary>
        /// <param name="communicationID"></param>
        /// <returns></returns>
        public DataTable GetCommunicationConfigInfoByID(string communicationID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from BS_COMMUNICATION_MAIN  ");
            strSql.Append(" where CommunicationID=@CommunicationID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@CommunicationID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = communicationID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        ///  根据ID获取模板列信息
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public DataTable GetDataColumnByTemplateID(string templateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from BS_TEMPLATE_COLUMNS  ");
            strSql.Append(" where TemplateID=@TemplateID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = templateID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }
        public string GetTargetColumnNameByColNameAndComid(string ColName,string CommID)
        {
            string sql = "select [TargetColumnName] from [BS_COMMUNICATION_DETAIL] where [CommunicationID]='" + CommID + "' and [SourceColumnName]=N'"+ColName+"'";
            object result = dbhelper.ExecuteScalar(CommandType.Text, sql, null);
            if (result != null && result.ToString().Trim() != "")
            {
                return result.ToString();
            }
            else
            {
                return "";
            }
        }
        //根据templateid得到数据源信息
        public string GetSourceInfoByTemID(string temid)
        {
            string result = string.Empty;
            string temsql = "select ObjectType,ObjectName from BS_DATASOURCE where SourceID=(select SourceID from BS_TEMPLATE_MAIN where TemplateID='"+temid+"')";
            DataTable temdt = dbhelper.ExecuteTable(CommandType.Text, temsql, null);
            if (temdt != null && temdt.Rows.Count > 0)
            {
                result = temdt.Rows[0]["ObjectType"].ToString() + "|" + temdt.Rows[0]["ObjectName"].ToString();
            }
            return result;
        }
        //绑定源数据源信息
        public DataTable GetSourceDataColumnBytemID(string temid, string ObjectType, string ObjectName)
        {
            if (!string.IsNullOrEmpty(ObjectType)&&!string.IsNullOrEmpty(ObjectName))
            {
                DataTable dt = null;
                string sql = string.Empty;
                if (ObjectType == "TABLE" || ObjectType == "VIEW")
                {
                    sql = "SELECT column_name as ColumnName,data_type as ColumnDataType ,'' as TargetColumnName FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME=N'" + ObjectName + "' ";
                }
                if (ObjectType == "PROC")
                {
                    List<SqlParameter> parameterlist = new List<SqlParameter>();
                    SqlParameter[] parameterss = null;
                    sql = "select colid,name from syscolumns where id in (select id from sysobjects where xtype='p' and name=N'" + ObjectName + "') order by colid ";
                    DataTable dvlist = dbhelper.ExecuteTable(CommandType.Text, sql, null);//首先获取存储过程中所有参数
                    if (dvlist != null && dvlist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dvlist.Rows)
                        {
                            SqlParameter sp = new SqlParameter(dr["name"].ToString(), "");
                            parameterlist.Add(sp);
                        }
                    }
                    if (parameterlist.Count > 0)
                    {
                        parameterss = parameterlist.ToArray();
                    }
                    DataTable dtt = dbhelper.ExecuteTable(CommandType.StoredProcedure, ObjectName, parameterss);
                    DataTable NewT = new DataTable();
                    NewT.Columns.Add("ColumnName", Type.GetType("System.String"));
                    NewT.Columns.Add("ColumnDataType", Type.GetType("System.String"));
                    NewT.Columns.Add("TargetColumnName", Type.GetType("System.String"));
                    foreach (DataColumn dc in dtt.Columns)
                    {
                        string dtype = dc.DataType.ToString().ToLower().Replace("system.", "").Replace("guid", "uniqueidentifier");
                        DataRow dr = NewT.NewRow();
                        dr["ColumnName"] = dc.ColumnName;
                        dr["ColumnDataType"] = dtype;
                        dr["TargetColumnName"] = "";
                        NewT.Rows.Add(dr);
                    }
                    dt = NewT;
                }
                else
                {
                    dt = dbhelper.ExecuteTable(CommandType.Text, sql, null);
                }
                return dt;
            }
            else
            {
                return null;
            }
        }
        public DataTable GetSourceDataColumnByTemplateID(string templateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [ColumnName], ColumnDataType, '' as TargetColumnName ");
            strSql.Append(" from BS_TEMPLATE_COLUMNS  ");
            strSql.Append(" where TemplateID=@TemplateID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = templateID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }

        ///// <summary>
        ///// 根据ID获取通信配置关联详细信息
        ///// </summary>
        ///// <param name="communicationID"></param>
        ///// <returns></returns>
        //public DataTable GetCommunicationDetailByID(string communicationID)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select tc.*,cd.SourceColumnName,cd.TargetColumnName from BS_TEMPLATE_COLUMNS tc ");
        //    strSql.Append("left join BS_COMMUNICATION_MAIN cm on cm.SourceTemplateID = tc.TemplateID ");
        //    strSql.Append("left join BS_COMMUNICATION_DETAIL cd on tc.ColumnName=cd.SourceColumnName and cd.CommunicationID=cm.CommunicationID ");
        //    strSql.Append("where cm.CommunicationID=@CommunicationID ");
        //    SqlParameter[] parameters = {
        //        new SqlParameter("@CommunicationID", SqlDbType.NVarChar,50)
        //    };
        //    parameters[0].Value = communicationID;
        //    return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);

        //}

        public DataTable GetCommunicationFields(string commID)
        {
            string sql = "select TargetColumnName,SourceColumnName from BS_COMMUNICATION_DETAIL where CommunicationID=@CommunicationID";
            SqlParameter[] parameters = {
                new SqlParameter("@CommunicationID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = commID;
            return dbhelper.ExecuteTable(CommandType.Text, sql, parameters);
        }

        public bool CommunicationIsExist(string communicationName, string TargetTemplateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) ");
            strSql.Append(" from BS_COMMUNICATION_MAIN  ");
            strSql.Append(" where Name=@Name and TargetTemplateID=@TargetTemplateID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@Name", SqlDbType.NVarChar,50),
                                             new SqlParameter("@TargetTemplateID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = communicationName;
            parameters[1].Value = TargetTemplateID;
            object obj = dbhelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            if (obj != null)
            {
                if (obj.ToString() == "0")
                    return false;
                else
                    return true;
            }
            return false;
        }

        public bool UpdateTemplateStatus(string templateID)
        {
            bool result = true;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BS_TEMPLATE_MAIN set TemplateStatus='FREE'");
                strSql.Append(" where TemplateID=@TemplateID and TemplateStatus!='ENABLE' ");
                SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.NVarChar,50)
			};
                parameters[0].Value = templateID;
                int obj = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            }
            catch (Exception ee)
            {
                result = false;
                BaseComponent.Error(ee.Message);
            }
            return result;
        }

        public DataTable GetCommunicationByTemplateID(string templateID)
        {
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CommunicationID,Name,SourceTemplateID FROM BS_COMMUNICATION_MAIN ");
            strSql.Append(" WHERE TargetTemplateID=@TargetTemplateID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TargetTemplateID", SqlDbType.UniqueIdentifier,50)
			};
            parameters[0].Value = new Guid(templateID);
            //int obj = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据源模板编号获取目标模板信息
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public DataTable GetCommunicationBySourceTemplateID(string templateID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT distinct TargetTemplateID FROM BS_COMMUNICATION_MAIN ");
            strSql.Append(" WHERE SourceTemplateID=@SourceTemplateID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@SourceTemplateID", SqlDbType.UniqueIdentifier,50)
			};
            parameters[0].Value = new Guid(templateID);
            //int obj = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一个模板下所有通信配置
        /// </summary>
        public bool DeleteCommunicationByTemplateID(string TemplateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from BS_COMMUNICATION_DETAIL where CommunicationID in (select CommunicationID from BS_COMMUNICATION_MAIN where (TargetTemplateID=@TargetTemplateID or SourceTemplateID=@TargetTemplateID));");
            strSql.Append("delete from BS_COMMUNICATION_MAIN where (TargetTemplateID=@TargetTemplateID or SourceTemplateID=@TargetTemplateID);");

            SqlParameter[] parameters = {
                                            new SqlParameter("@TargetTemplateID", SqlDbType.UniqueIdentifier,50)
			};
            parameters[0].Value = new Guid(TemplateID);

            int count = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            return count > 0 ? true : false;
        }
    }
}

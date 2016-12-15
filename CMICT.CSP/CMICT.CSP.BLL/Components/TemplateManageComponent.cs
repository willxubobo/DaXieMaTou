using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class TemplateManageComponent
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public TemplateManageComponent() { }
        /// <summary>
        /// 模板信息分页列表
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Author"></param>
        /// <param name="TemplateStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public DataTable GetTemplateList(string TemplateName, string bigcate, string smallcate, string Author, string TemplateStatus,string orderby, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            List<SqlParameter> parameterlist = new List<SqlParameter>();
            SqlParameter[] parameterss = null;
            string P_Sql = " where 1=1 ";

            if (TemplateName != "")
            {
                P_Sql += " and TemplateName like @TemplateName";
                SqlParameter sp = new SqlParameter("@TemplateName", "%" + TemplateName + "%");
                parameterlist.Add(sp);
            }
            if (!string.IsNullOrEmpty(bigcate))
            {
                P_Sql += " and BigCategory = @bigcate";
                SqlParameter sp = new SqlParameter("@bigcate", bigcate);
                parameterlist.Add(sp);
            }
            if (!string.IsNullOrEmpty(smallcate))
            {
                P_Sql += " and SmallCategory = @smallcate";
                SqlParameter sp = new SqlParameter("@smallcate", smallcate);
                parameterlist.Add(sp);
            }
            //if (StartDate != "")
            //{
            //    P_Sql += " and Created>='" + @StartDate + "'";
            //}
            //if (EndDate != "")
            //{
            //    P_Sql += " and Created<'" + @EndDate + " 23:59'";
            //}
            if (Author != "")
            {
                P_Sql += " and Author like @Author";
                SqlParameter sp = new SqlParameter("@Author", "%" + Author + "%");
                parameterlist.Add(sp);
            }
            if (TemplateStatus != "")
            {
                P_Sql += " and TemplateStatus = @TemplateStatus";
                SqlParameter sp = new SqlParameter("@TemplateStatus", TemplateStatus);
                parameterlist.Add(sp);
            }
            string AllFields = "*";
            string Condition = " BS_TEMPLATE_MAIN " + P_Sql;
            string IndexField = "TemplateID";
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "BigCategory asc,SmallCategory asc,templatename asc,created desc";
            }
            string OrderFields = "order by "+orderby;
            if (parameterlist != null && parameterlist.Count > 0)
            {
                parameterss = parameterlist.ToArray();
            }
            return dbhelper.ExecutePage(AllFields, Condition, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, parameterss);

        }

        /// <summary>
        /// 根据templateid获得page列表
        /// </summary>
        public DataTable GetListByTemplateID(string TemplateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PageID,TemplateID,Url,PageName,Created,Modified,Author,Editor ");
            strSql.Append(" FROM BS_TEMPLATE_PAGES where TemplateID=@TemplateID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = TemplateID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据templateid获得page列表
        /// </summary>
        public DataTable GetPageList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PageID,TemplateID,Url,PageName,Created,Modified,Author,Editor ");
            strSql.Append(" FROM BS_TEMPLATE_PAGES");
           
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), null);
        }
        //删除模板信息
        public bool DelTemplateInfo(string TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BS_TEMPLATE_COLUMNS");
            strSql.Append(" where TemplateID='" + @TemplateID + "';delete from BS_TEMPLATE_SORT where TemplateID='" + @TemplateID + "';delete from BS_GROUPBY  where TemplateID='" + @TemplateID + "';delete from BS_COMPUTE  where TemplateID='" + @TemplateID + "';delete from BS_CUSTOM_QUERY  where TemplateID='" + @TemplateID + "';delete from BS_DEFAULT_QUERY where TemplateID='" + @TemplateID + "';delete from BS_TEMPLATE_PAGES where TemplateID='" + @TemplateID + "';delete from BS_TEMPLATE_MAIN where TemplateID='" + @TemplateID + "';");
            int rows = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), null);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //禁用模板信息
        public bool EnableTemplateInfo(string TemplateID, string tstatus = "DISABLE")
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_TEMPLATE_MAIN set TemplateStatus='"+tstatus+"'");
            strSql.Append(" where TemplateID='" + @TemplateID + "'");
            int rows = dbhelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), null);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //获取id复制用
        public DataTable GetBS_CUSTOM_QUERYIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_CUSTOM_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_DEFAULT_QUERYIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_DEFAULT_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_GROUPBYIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_GROUPBY ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_TEMPLATE_COLUMNSIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_TEMPLATE_COLUMNS ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_TEMPLATE_PAGESIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PageID from BS_TEMPLATE_PAGES ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_TEMPLATE_SORTIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_TEMPLATE_SORT ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_COMMUNICATION_MAINIDByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CommunicationID from BS_COMMUNICATION_MAIN ");
            strSql.Append(" where TargetTemplateID=@TargetTemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TargetTemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        public DataTable GetBS_COMMUNICATION_DETAILByID(Guid CommunicationID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from BS_COMMUNICATION_DETAIL ");
            strSql.Append(" where CommunicationID=@CommunicationID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@CommunicationID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = CommunicationID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
    }
}

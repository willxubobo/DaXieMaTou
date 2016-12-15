using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class DataSourceConfigComponent
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public DataSourceConfigComponent() { }

        /// <summary>
        /// 选择数据源后更新原模板状态
        /// </summary>
        public bool UpdateSourceStatusBySourceID(string SourceID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_DATASOURCE set SourceStatus='FREE' where  SourceID='" + @SourceID + "'");

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

        /// <summary>
        /// 选择数据源后更新模板与数据源状态
        /// </summary>
        public bool OperConfigDataSource(string TemplateID,string SourceID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update BS_TEMPLATE_MAIN set SourceID='" + @SourceID + "' where TemplateID='" + @TemplateID + "';update BS_DATASOURCE set SourceStatus='ENABLE' where  SourceID='" + @SourceID + "';");
            
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
        public  DataTable GetDataSourceList(string bigcate,string smallcate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  *,case when LEN(SourceName)<16 then SourceName else SUBSTRING(SourceName,0,15)+'...' end as namedesc from BS_DATASOURCE ");
            strSql.Append(" where (SourceStatus='FREE' or SourceStatus='ENABLE') and BigCategory='" + @bigcate + "' and SmallCategory='" + @smallcate + "'");

            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), null);
        }

        /// <summary>
        /// 根据sourceid获得template列表
        /// </summary>
        public DataTable GetTemplateListBySourceID(string SourceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TemplateID,TemplateName ");
            strSql.Append(" FROM BS_TEMPLATE_MAIN where SourceID=@SourceID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@SourceID", SqlDbType.NVarChar,90)
			};
            parameters[0].Value = SourceID;
            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 数据源信息分页列表
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
        public DataTable GetDataSourceList(string SourceName, string SourceIP, string DBName, string ObjectType,string ObjectName, string SourceStatus,string bigcate,string smallcate, string orderby,int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            List<SqlParameter> parameterlist = new List<SqlParameter>();
            SqlParameter[] parameterss = null;
            string P_Sql = " where 1=1 ";

            if (SourceName != "")
            {
                P_Sql += " and SourceName like @SourceName";
                SqlParameter sp = new SqlParameter("@SourceName", "%"+SourceName+"%");
                parameterlist.Add(sp);
            }

            if (SourceIP != "")
            {
                P_Sql += " and SourceIP like @SourceIP";
                SqlParameter sp = new SqlParameter("@SourceIP", "%" + SourceIP + "%");
                parameterlist.Add(sp);
            }
            if (DBName != "")
            {
                P_Sql += " and DBName like @DBName";
                SqlParameter sp = new SqlParameter("@DBName", "%" + DBName + "%");
                parameterlist.Add(sp);
            }
            if (ObjectType != "")
            {
                P_Sql += " and ObjectType = @ObjectType";
                SqlParameter sp = new SqlParameter("@ObjectType", ObjectType);
                parameterlist.Add(sp);
            }
            if (ObjectName != "")
            {
                P_Sql += " and ObjectName like @ObjectName";
                SqlParameter sp = new SqlParameter("@ObjectName", "%" + ObjectName + "%");
                parameterlist.Add(sp);
            }
            if (SourceStatus != "")
            {
                P_Sql += " and SourceStatus = @SourceStatus";
                SqlParameter sp = new SqlParameter("@SourceStatus", SourceStatus);
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
            string AllFields = "*";
            string Condition = " BS_DATASOURCE " + P_Sql;
            string IndexField = "SourceID";
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "BigCategory asc,SmallCategory asc,ObjectType asc,Created desc";
            }
            string OrderFields = "order by "+orderby;
            if (parameterlist != null && parameterlist.Count > 0)
            {
                parameterss = parameterlist.ToArray();
            }
            return dbhelper.ExecutePage(AllFields, Condition, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, parameterss);

        }
    }
}

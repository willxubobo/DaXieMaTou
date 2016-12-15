using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class DataSourceConfigComponent
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public DataSourceConfigComponent() { }


        public  DataTable GetDataSourceList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  SourceID,SourceName,SourceDesc,SourceIP,UserName,Password,DBName,ObjectType,ObjectName,SourceStatus,Created,Modified,Author,Editor from BS_DATASOURCE ");
            strSql.Append(" where SourceStatus='1'");

            return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), null);
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
        public DataTable GetDataSourceList(string SourceName, string SourceIP, string DBName, string ObjectType,string ObjectName, string SourceStatus, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            string P_Sql = " where 1=1 ";

            if (SourceName != "")
            {
                P_Sql += " and SourceName like '%" + @SourceName + "%'";
            }

            if (SourceIP != "")
            {
                P_Sql += " and SourceIP like '%" + @SourceIP + "%'";
            }
            if (DBName != "")
            {
                P_Sql += " and DBName like '%" + @DBName + "%'";
            }
            if (ObjectType != "")
            {
                P_Sql += " and ObjectType = '" + @ObjectType + "'";
            }
            if (ObjectName != "")
            {
                P_Sql += " and ObjectName like '%" + @ObjectName + "%'";
            }
            
            string AllFields = "*";
            string Condition = " BS_DATASOURCE " + P_Sql;
            string IndexField = "SourceID";
            string OrderFields = "order by Created desc";

            return dbhelper.ExecutePage(AllFields, Condition, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, null);

        }
    }
}

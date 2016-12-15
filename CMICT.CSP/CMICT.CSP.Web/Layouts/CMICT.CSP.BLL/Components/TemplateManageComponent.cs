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
        public DataTable GetTemplateList(string TemplateName, string StartDate, string EndDate, string Author, string TemplateStatus, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            string P_Sql = " where 1=1 ";

            if (TemplateName != "")
            {
                P_Sql += " and TemplateName like '%" + @TemplateName + "%'";
            }

            if (StartDate != "")
            {
                P_Sql += " and Created>='" + @StartDate + "'";
            }
            if (EndDate != "")
            {
                P_Sql += " and Created<'" + @EndDate + " 23:59'";
            }
            if (Author != "")
            {
                P_Sql += " and Author = '" + @Author + "'";
            }
            if (TemplateStatus != "")
            {
                P_Sql += " and TemplateStatus = '" + @TemplateStatus + "'";
            }
            string AllFields = "*";
            string Condition = " BS_TEMPLATE_MAIN " + P_Sql;
            string IndexField = "TemplateID";
            string OrderFields = "order by Created desc";

            return dbhelper.ExecutePage(AllFields, Condition, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, null);

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

       
    }
}

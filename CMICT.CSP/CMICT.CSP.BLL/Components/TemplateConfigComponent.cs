﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class TemplateConfigComponent
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public TemplateConfigComponent() { }

        /// <summary>
        /// 检查模板名称是否维一
        /// </summary>
        public bool GetTemplateNameIsExists(string TemplateName, string TemplateID)
        {
            bool result = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM BS_TEMPLATE_MAIN where TemplateName = @TemplateName ");
            if (!string.IsNullOrEmpty(TemplateID))
            {
                strSql.Append(" and TemplateID!='" + @TemplateID + "'");
            }
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = TemplateName;
            object rows = dbhelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            if (rows != null && rows.ToString()!="")
            {
                if (Convert.ToInt32(rows) > 0)
                {
                    result = true;
                }
            }
            return result;
            
        }

        public DataTable GetEnableTemplate(string big, string small)
        {
            string strSql = @"select TemplateID,TemplateName from BS_TEMPLATE_MAIN where TemplateStatus!='DRAFT' and TemplateStatus!='DISABLE' and BigCategory='"+big+"' and SmallCategory='"+small+"'";
            return dbhelper.ExecuteTable(CommandType.Text, strSql);
        }
    }
}

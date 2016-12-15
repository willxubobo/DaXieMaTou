using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class QueryConfigComponent
    {
        DbHelperSQL dbhelper;
        public QueryConfigComponent()
        {
            dbhelper = new DbHelperSQL();
        }
        //根据templatid获取对应的默认筛选表模块信息
        public DataTable GetDefaultQueryListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select mainlogic,moduleid,sublogic,count(moduleid) as subcount from BS_DEFAULT_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID group by mainlogic,moduleid,sublogic order by moduleid");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        //根据templatid获取对应的默认筛选表详细信息
        public DataTable GetDefaultQueryListInfoByTemplateID(Guid TemplateID,int ModuleID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BS_DEFAULT_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID and ModuleID=@ModuleID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
                                            new SqlParameter("@ModuleID", SqlDbType.Int)
			};
            parameters[0].Value = TemplateID;
            parameters[1].Value = ModuleID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        //根据templatid获取对应的排序配置表信息
        public DataTable GetUserQueryListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BS_CUSTOM_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        /// <summary>
        /// 用作生成TemplateModel
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public DataTable GetUserQueryListByTemplateIDForModel(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *,case  when sequence is null then 999999999999999999 else sequence end as NewSequence from BS_CUSTOM_QUERY ");
            strSql.Append(" where TemplateID=@TemplateID order by NewSequence,DisplayName");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }

        //修改前先删除相应信息
        public bool DeleteTemInfoByTemplateID(string TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BS_CUSTOM_QUERY");
            strSql.Append(" where TemplateID='" + @TemplateID + "';delete from BS_DEFAULT_QUERY where TemplateID='" + @TemplateID + "';");
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
        //根据templatid获取对应的用户筛选列信息
        public DataTable GetUserQueryColListByTemplateID(Guid TemplateID,int type=0)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ColumnName as name,DisplayName from BS_TEMPLATE_COLUMNS ");
            strSql.Append(" where TemplateID=@TemplateID");
            if (type == 1)//过滤字段类型
            {
                strSql.Append(" and ColumnDataType in ('int','decimal','smallint','bigint','float','numeric')");
            }
            strSql.Append(" order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }

        //根据templatid获取对应的用户筛选列信息包括存储过程中参数
        public DataTable GetUserQueryColListByTemplateID(Guid TemplateID, string objecttype, string objectname, int type = 0)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            if (objecttype == "PROC")
            {
                strSql.Append("select name from syscolumns where id in (select id from sysobjects where xtype='p' and name='"+objectname+"') union ");
            }
            strSql.Append("select ColumnName as name from BS_TEMPLATE_COLUMNS ");
            strSql.Append(" where TemplateID=@TemplateID");
            if (type == 1)//过滤字段类型
            {
                strSql.Append(" and ColumnDataType in ('int','decimal','smallint','bigint','float','numeric')");
            }
            //strSql.Append(" order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
    }
}

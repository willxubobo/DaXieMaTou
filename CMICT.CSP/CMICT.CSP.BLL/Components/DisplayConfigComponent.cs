using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class DisplayConfigComponent
    {
        DbHelperSQL dbhelper;
        public DisplayConfigComponent()
        {
            dbhelper = new DbHelperSQL();
        }
        public DisplayConfigComponent(string ip, string uname, string pwd, string dbname)
        {
            string constr = "Max Pool Size=2048;server=" + ip + ";uid=" + uname + ";pwd=" + pwd + ";database="+dbname+"";
            dbhelper = new DbHelperSQL(constr);
        }
        //绑定列配置
        public DataTable GetColumnListByType(string ObjectType,string ObjectName)
        {
            DataTable dt = null;
            string sql = string.Empty;
            string checkcanexecutesql = string.Empty;
            if (ObjectType == "TABLE" || ObjectType == "VIEW")
            {
                checkcanexecutesql = "select top 1 * from " + ObjectName;
                sql = "SELECT column_name as name,ordinal_position,data_type,data_type+';'+column_name as cv FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME=N'" + ObjectName + "' ";
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
                        SqlParameter sp = new SqlParameter(dr["name"].ToString(),"");
                        parameterlist.Add(sp);
                    }
                }
                if (parameterlist.Count > 0)
                {
                    parameterss = parameterlist.ToArray();
                }
                DataTable dtt = dbhelper.ExecuteTable(CommandType.StoredProcedure, ObjectName, parameterss);
                DataTable NewT = new DataTable();
                NewT.Columns.Add("name", Type.GetType("System.String"));
                NewT.Columns.Add("data_type", Type.GetType("System.String"));
                NewT.Columns.Add("cv", Type.GetType("System.String"));
                foreach (DataColumn dc in dtt.Columns)
                {
                    string dtype = dc.DataType.ToString().ToLower().Replace("system.","");
                    DataRow dr = NewT.NewRow();
                    dr["name"] = dc.ColumnName;
                    dr["data_type"] = dtype;
                    dr["cv"] = dtype + ";" + dc.ColumnName;
                    NewT.Rows.Add(dr);
                }
                dt = NewT;
            }
            else
            {
                dt = dbhelper.ExecuteTable(CommandType.Text, sql, null);
            }
            if (!string.IsNullOrEmpty(checkcanexecutesql))//测试是否能执行
            {
                dbhelper.ExecuteTable(CommandType.Text, checkcanexecutesql, null);
            }
            return dt;
        }

        //绑定列配置配置筛选条件用，存储过程列中累加变量参数
        public DataTable GetColumnListByType(string ObjectType, string ObjectName,string type)
        {
            DataTable dt = null;
            string sql = string.Empty;
            if (ObjectType == "TABLE" || ObjectType == "VIEW")
            {
                sql = "SELECT column_name as name,ordinal_position,data_type,data_type+';'+column_name as cv FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME=N'" + ObjectName + "' ";
            }
            if (ObjectType == "PROC")
            {
                DataTable NewT = new DataTable();
                NewT.Columns.Add("name", Type.GetType("System.String"));
                NewT.Columns.Add("data_type", Type.GetType("System.String"));
                NewT.Columns.Add("cv", Type.GetType("System.String"));

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
                        DataRow drn = NewT.NewRow();
                        drn["name"] = dr["name"].ToString();
                        drn["data_type"] = "";
                        drn["cv"] = ";" + dr["name"].ToString();
                        NewT.Rows.Add(drn);
                    }
                }
                if (parameterlist != null && parameterlist.Count > 0)
                {
                    parameterss = parameterlist.ToArray();
                }
                DataTable dtt = dbhelper.ExecuteTable(CommandType.StoredProcedure, ObjectName, parameterss);
                foreach (DataColumn dc in dtt.Columns)
                {
                    string dtype = dc.DataType.ToString().ToLower().Replace("system.", "");
                    DataRow dr = NewT.NewRow();
                    dr["name"] = dc.ColumnName;
                    dr["data_type"] = dtype;
                    dr["cv"] = dtype + ";" + dc.ColumnName;
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

        //田字型更新每行字段数
        public bool UpdatePageSizeByTemplateID(Guid TemplateID, decimal ColumnSize)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_TEMPLATE_MAIN set ColumnSize=@ColumnSize ");
            strSql.Append(" where TemplateID=@TemplateID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
                                            new SqlParameter("@ColumnSize", SqlDbType.Decimal,9)
			};
            parameters[0].Value = TemplateID;
            parameters[1].Value = ColumnSize;
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

        //田字型更新每行字段数
        public bool UpdateDisplayTypeByTemplateID(Guid TemplateID, string DisplayType)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_TEMPLATE_MAIN set DiaplayType=@DiaplayType ");
            strSql.Append(" where TemplateID=@TemplateID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16),
                                            new SqlParameter("@DiaplayType", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = TemplateID;
            parameters[1].Value = DisplayType;
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
        //修改前先删除相应信息
        public bool DeleteTemInfoByTemplateID(string TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BS_TEMPLATE_COLUMNS");
            strSql.Append(" where TemplateID='" + @TemplateID + "';delete from BS_TEMPLATE_SORT where TemplateID='" + @TemplateID + "';delete from BS_GROUPBY  where TemplateID='" + @TemplateID + "';delete from BS_COMPUTE  where TemplateID='" + @TemplateID + "';");
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
        //根据模板编号，修改模板状态
        public bool UpdateStatusByTemplateID(string TemplateID, string status)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BS_TEMPLATE_MAIN set TemplateStatus='"+@status+"'  where TemplateID='" + @TemplateID + "'");
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
        //取计算列table,只取数字型字段
        public DataTable GetColumnListByTypeOnlyNum(string ObjectType, string ObjectName)
        {
            DataTable dt = null;
            string sql = string.Empty;
            if (ObjectType == "TABLE" || ObjectType == "VIEW")
            {
                sql = "SELECT column_name as name,ordinal_position,data_type FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='" + ObjectName + "' and data_type IN ('int','decimal','smallint','bigint','float','numeric') ";
            }
            if (ObjectType == "PROC")
            {
                List<SqlParameter> parameterlist = new List<SqlParameter>();
                SqlParameter[] parameterss = null;
                sql = "select colid,name from syscolumns where id in (select id from sysobjects where xtype='p' and name='" + ObjectName + "') order by colid ";
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
                NewT.Columns.Add("name", Type.GetType("System.String"));
                foreach (DataColumn dc in dtt.Columns)
                {
                    if (dc.DataType == typeof(Int32) || dc.DataType == typeof(Int64) || dc.DataType == typeof(decimal) || dc.DataType == typeof(float) || dc.DataType == typeof(int) || dc.DataType == typeof(double))
                    {
                        DataRow dr = NewT.NewRow();
                        dr["name"] = dc.ColumnName;
                        NewT.Rows.Add(dr);
                    }
                }
                dt = NewT;
            }
            else
            {
                dt = dbhelper.ExecuteTable(CommandType.Text, sql, null);
            }
            return dt;
        }

        //根据templatid获取对应的列配置表
        public DataTable GetColumnListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.*,b.DiaplayType,b.ColumnSize from BS_TEMPLATE_COLUMNS a left join BS_TEMPLATE_MAIN b on a.TemplateID=b.TemplateID ");
            strSql.Append(" where a.TemplateID=@TemplateID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        //根据templatid获取对应的排序配置表信息
        public DataTable GetSortListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BS_TEMPLATE_SORT ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
        //根据templatid获取对应的计算列表信息
        public DataTable GetCalListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BS_COMPUTE ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }

        //根据templatid获取对应的行汇总表信息
        public DataTable GetGroupByListByTemplateID(Guid TemplateID)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BS_GROUPBY ");
            strSql.Append(" where TemplateID=@TemplateID order by Created");
            SqlParameter[] parameters = {
                                            new SqlParameter("@TemplateID", SqlDbType.UniqueIdentifier,16)
			};
            parameters[0].Value = TemplateID;
            DataTable dt = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
            return dt;
        }
    }
}

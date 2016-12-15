using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class ConnectionConfigComponent
    {
        DbHelperSQL dbhelper;
        public ConnectionConfigComponent() {
            dbhelper = new DbHelperSQL();
        }

        public ConnectionConfigComponent(string ip,string uname,string pwd)
        {
            string constr = "Max Pool Size=2048;server=" + ip + ";uid=" + uname + ";pwd=" + pwd + ";database=master";
            dbhelper = new DbHelperSQL(constr);
        }
        public ConnectionConfigComponent(string ip, string uname, string pwd,string dbname)
        {
            string constr = "Max Pool Size=2048;server=" + ip + ";uid=" + uname + ";pwd=" + pwd + ";database="+dbname+"";
            dbhelper = new DbHelperSQL(constr);
        }
        //检查连接语句是否有效
        public bool CheckConnect()
        {
            return dbhelper.CheckConnection();
        }
        //获取数据源ip
        public DataTable GetDataSourceIpList()
        {
            string sql = "select distinct sourceip from [BS_DATASOURCE]";
            return dbhelper.ExecuteTable(CommandType.Text, sql, null);
        }

        //获取数据源下所有数据库
        public DataTable GetAllDBOnDataSource(string DbName="")
        {
            string sql = "Select name,case when LEN(name)<23 then name else SUBSTRING(name,0,22)+'...' end as namedesc from master..sysdatabases where name not in('master','model','msdb','tempdb','northwind','pubs') ";
            if (DbName != "")
            {
                sql += " and name=N'"+DbName+"'";
            }
            sql += "  order by name";
            return dbhelper.ExecuteTable(CommandType.Text, sql, null);
        }


        //根据类型返回相应数据源
        public DataTable GetDataListByDataType(string DataType,string objectname="")
        {
            DataTable dt = null;
            string sql = "";
            if (DataType == "TABLE")
            {
                sql = "select name,case when LEN(name)<23 then name else SUBSTRING(name,0,22)+'...' end as namedesc from sysobjects where   type = 'U'";
            }
            if (DataType == "VIEW")
            {
                sql = "select name,case when LEN(name)<23 then name else SUBSTRING(name,0,22)+'...' end as namedesc from sysobjects where   type = 'V'";
            }
            if (DataType == "PROC")
            {
                sql = "select name,case when LEN(name)<23 then name else SUBSTRING(name,0,22)+'...' end as namedesc from sysobjects where   type = 'P'";
            }
            if (sql != "")
            {
                if (objectname != "")
                {
                    sql += " and name=N'"+objectname+"'";
                }
                sql += " order by name";
                dt = dbhelper.ExecuteTable(CommandType.Text, sql, null);
            }
            return dt;
        }


        //测试是否能连接并执行
        public string CheckCanExecute(string ObjectType, string ObjectName)
        {
            string result = string.Empty;
            try
            {
                DataTable dt = null;
                string sql = string.Empty;
                if (ObjectType == "TABLE" || ObjectType == "VIEW")
                {
                    sql = "select top 1 * from " + ObjectName;
                    //sql = "SELECT column_name as name,ordinal_position,data_type,data_type+';'+column_name as cv FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='" + ObjectName + "' ";
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
                            SqlParameter sp = new SqlParameter(dr["name"].ToString(), DBNull.Value);
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
            }catch(Exception ex){
                BaseComponent.Error("ConnectionConfigComponent-CheckCanExecute出错："+ex.Message);
                result = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 检查数据源名称是否维一
        /// </summary>
        public bool GetSourceNameIsExists(string SourceName,string SourceID)
        {
            bool result = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM BS_DATASOURCE where SourceName = @SourceName ");
            if (!string.IsNullOrEmpty(SourceID))
            {
                strSql.Append(" and SourceID!='" + @SourceID + "'");
            }
            SqlParameter[] parameters = {
                                            new SqlParameter("@SourceName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = SourceName;
            object rows = dbhelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters);
            if (rows != null && rows.ToString() != "")
            {
                if (Convert.ToInt32(rows) > 0)
                {
                    result = true;
                }
            }
            return result;

        }

        public DataTable GetTemplateIDBySourceID(Guid SourceID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TemplateID FROM BS_TEMPLATE_MAIN where SourceID = @SourceID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@SourceID", SqlDbType.UniqueIdentifier)
			};
            parameters[0].Value = SourceID;
           return dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);
        }
    }
}

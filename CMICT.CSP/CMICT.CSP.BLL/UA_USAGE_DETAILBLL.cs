﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL
{
    public partial class UA_USAGE_DETAILBLL
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        public UA_USAGE_DETAILBLL()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(CMICT.CSP.Model.UA_USAGE_DETAIL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UA_USAGE_DETAIL(");
            strSql.Append("ID,UsageID,QueryKey,Value)");
            strSql.Append(" values (");
            strSql.Append("@ID,@UsageID,@Key,@Value)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UsageID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Key", SqlDbType.NVarChar,200),
					new SqlParameter("@Value", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsageID;
            parameters[2].Value = model.QueryKey;
            parameters[3].Value = model.Value;

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
        public bool Update(CMICT.CSP.Model.UA_USAGE_DETAIL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UA_USAGE_DETAIL set ");
            strSql.Append("ID=@ID,");
            strSql.Append("UsageID=@UsageID,");
            strSql.Append("Key=@Key,");
            strSql.Append("Value=@Value");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@UsageID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@Key", SqlDbType.NVarChar,200),
					new SqlParameter("@Value", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsageID;
            parameters[2].Value = model.QueryKey;
            parameters[3].Value = model.Value;

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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UA_USAGE_DETAIL ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
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
        public CMICT.CSP.Model.UA_USAGE_DETAIL GetModel(Guid ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsageID,Key,Value from UA_USAGE_DETAIL ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = ID;

            CMICT.CSP.Model.UA_USAGE_DETAIL model = new CMICT.CSP.Model.UA_USAGE_DETAIL();
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
        public CMICT.CSP.Model.UA_USAGE_DETAIL DataRowToModel(DataRow row)
        {
            CMICT.CSP.Model.UA_USAGE_DETAIL model = new CMICT.CSP.Model.UA_USAGE_DETAIL();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = new Guid(row["ID"].ToString());
                }
                if (row["UsageID"] != null && row["UsageID"].ToString() != "")
                {
                    model.UsageID = new Guid(row["UsageID"].ToString());
                }
                if (row["Key"] != null)
                {
                    model.QueryKey = row["Key"].ToString();
                }
                if (row["Value"] != null)
                {
                    model.Value = row["Value"].ToString();
                }
            }
            return model;
        }
    }
}

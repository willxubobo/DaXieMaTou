using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml.Schema;
using CMICT.CSP.WebDataSubscribe.Commons;
using CMICT.CSP.WebDataSubscribe.Entities;
using Newtonsoft.Json;

namespace CMICT.CSP.WebDataSubscribe
{
    public class SubscribeData
    {
        public string Operater(string applydata)
        {

            ResultData resultData = new ResultData();
            SearchResultsData searchResultsData = new SearchResultsData();

            if (string.IsNullOrEmpty(applydata))
            {
                resultData.MsgDesc = "applydata is not empty";
                resultData.MsgId = "001";
            }
            else
            {
                var model = JsonConvert.DeserializeObject<ApplyDataPar>(applydata);
                if (model == null)
                {
                    resultData.MsgDesc = "applydata is error";
                    resultData.MsgId = "002";
                }
                else
                {
                    if (model.Username != Common.ApplyDataUserName || model.Password != Common.ApplyDataUserPwd)
                    {
                        resultData.MsgDesc = "username is error or password is error";
                        resultData.MsgId = "003";
                    }
                    else
                    {
                        string type = model.Type;
                        if (type == "C")
                        {
                            resultData = Add(model);
                        }
                        else if (type == "U")
                        {
                            resultData = Update(model);
                        }
                        else if (type == "D")
                        {
                            resultData = Delete(model);
                        }
                    }
                }

                //search
                var searchModel = JsonConvert.DeserializeObject<SearchEntityData>(applydata);
                if (searchModel == null)
                {
                    searchResultsData.MsgDesc = "applydata is error";
                    searchResultsData.MsgID = "002";
                }
                else
                {
                    if (searchModel.Username != Common.ApplyDataUserName || searchModel.Password != Common.ApplyDataUserPwd)
                    {
                        searchResultsData.MsgDesc = "username is error or password is error";
                        searchResultsData.MsgID = "003";
                    }
                    else
                    {
                        string type = searchModel.Type;
                        if (type == "R")
                        {
                            searchResultsData = Search(searchModel);
                            return JsonConvert.SerializeObject(searchResultsData);
                        }
                    }
                }

            }
            return JsonConvert.SerializeObject(resultData);
        }

        private ResultData Add(ApplyDataPar model)
        {

            SqlHelper sqlHelper = new SqlHelper();
            int ii = 0;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = null;

            ResultData resultData = new ResultData();

            DataTable data = model.Data;
            string entityName = model.Entity;
            string key = Common.GetKeyByEntityName(entityName);

            if (data != null && data.Rows.Count > 0)
            {
                string sql = "select count(1) from {0} where {1}='{2}'";
                sql = string.Format(sql, entityName, key, data.Rows[0][key]);
                object oo = sqlHelper.ExecuteScalar(sql);

                StringBuilder strSqlColumns = new StringBuilder();
                StringBuilder strSqlColumnsVlues = new StringBuilder();
                StringBuilder strText = new StringBuilder();
                //not isexist

                #region

                if (Convert.ToInt32(oo) == 0)
                {
                    string sqlIn = "insert into {0}({1}) values({2});";
                    string sqlColmuns = "";
                    string sqlValues = "";
                    foreach (DataColumn dataColumn in data.Columns)
                    {
                        if (dataColumn.ColumnName != "INSERT_TIME" && dataColumn.ColumnName != "UPDATE_TIME" &&
                            !string.IsNullOrEmpty(Convert.ToString(data.Rows[0][dataColumn.ColumnName])))
                        {
                            strSqlColumns.Append("," + dataColumn.ColumnName);
                            //strSqlColumnsVlues.Append(",'" + Convert.ToString(dr[dataColumn.ColumnName]).Replace("'","''") + "'");
                            strSqlColumnsVlues.Append(",@" + dataColumn.ColumnName + "");
                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = "@" + dataColumn.ColumnName;
                            sqlParameter.Value = Convert.ToString(data.Rows[0][dataColumn.ColumnName]);
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    if (strSqlColumns.ToString().Length > 0)
                    {
                        sqlColmuns = strSqlColumns.ToString().Substring(1);
                        sqlValues = strSqlColumnsVlues.ToString().Substring(1);
                    }
                    sqlIn = string.Format(sqlIn, entityName, sqlColmuns, sqlValues);
                    strText.Append(sqlIn);
                }
                else
                {
                    string sqlUp = "update {0} set {1} where {2}='{3}';";
                    foreach (DataColumn dataColumn in data.Columns)
                    {
                        if (dataColumn.ColumnName != "INSERT_TIME" && dataColumn.ColumnName != "UPDATE_TIME" &&
                            !string.IsNullOrEmpty(Convert.ToString(data.Rows[0][dataColumn.ColumnName])))
                        {
                            //strSqlColumns.Append("," + dataColumn.ColumnName + "='" + Convert.ToString(dr[dataColumn.ColumnName]).Replace("'", "''") + "'");
                            strSqlColumns.Append("," + dataColumn.ColumnName + "=@" + dataColumn.ColumnName + "");

                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = "@" + dataColumn.ColumnName;
                            sqlParameter.Value = Convert.ToString(data.Rows[0][dataColumn.ColumnName]);
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    string sqlColmuns = "";
                    if (strSqlColumns.ToString().Length > 0)
                    {
                        sqlColmuns = strSqlColumns.ToString().Substring(1);
                    }
                    sqlUp = string.Format(sqlUp, entityName, sqlColmuns, key, data.Rows[0][key]);
                    strText.Append(sqlUp);
                }

                #endregion

                try
                {
                    if (!string.IsNullOrEmpty(strText.ToString()))
                    {
                        int ren = sqlHelper.ExecuteNonQuery(strText.ToString(), sqlCommand);
                        strSqlColumns.Clear();
                        strSqlColumnsVlues.Clear();
                        resultData.MsgDesc = "Success";
                        resultData.MsgId = "000";
                    }
                }
                catch (Exception ex)
                {
                    resultData.MsgDesc = "Fail message:" + ex.Message;
                    resultData.MsgId = "004";
                }
            }
            return resultData;
        }
        private ResultData Delete(ApplyDataPar model)
        {
            SqlHelper sqlHelper = new SqlHelper();
            int ii = 0;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = null;

            ResultData resultData = new ResultData();

            DataTable data = model.Data;
            string entityName = model.Entity;
            string key = Common.GetKeyByEntityName(entityName);

            if (data != null && data.Rows.Count > 0)
            {
                string sql = "select count(1) from {0} where {1}='{2}'";
                sql = string.Format(sql, entityName, key, data.Rows[0][key]);
                object oo = sqlHelper.ExecuteScalar(sql);

                StringBuilder strSqlColumns = new StringBuilder();
                StringBuilder strSqlColumnsVlues = new StringBuilder();
                StringBuilder strText = new StringBuilder();
                //not isexist

                #region

                if (Convert.ToInt32(oo) == 0)
                {
                    resultData.MsgDesc = "这条数据不存在远程数据库中，请检查！";
                    resultData.MsgId = "004";
                }
                else
                {
                    string sqlDel = "delete from {0} where {1}=@" + key + "";
                    sqlDel = string.Format(sqlDel, entityName, key);

                    strText.Append(sqlDel);
                    sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@" + key;
                    sqlParameter.Value = Convert.ToString(data.Rows[0][key]);
                    sqlCommand.Parameters.Add(sqlParameter);
                }

                #endregion

                try
                {
                    if (!string.IsNullOrEmpty(strText.ToString()))
                    {
                        int ren = sqlHelper.ExecuteNonQuery(strText.ToString(), sqlCommand);
                        strSqlColumns.Clear();
                        strSqlColumnsVlues.Clear();
                        resultData.MsgDesc = "Success";
                        resultData.MsgId = "000";
                    }

                }
                catch (Exception ex)
                {
                    resultData.MsgDesc = "Fail message:" + ex.Message;
                    resultData.MsgId = "004";
                }
            }
            return resultData;
        }
        private ResultData Update(ApplyDataPar model)
        {
            SqlHelper sqlHelper = new SqlHelper();
            int ii = 0;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = null;

            ResultData resultData = new ResultData();

            DataTable data = model.Data;
            string entityName = model.Entity;
            string key = Common.GetKeyByEntityName(entityName);

            if (data != null && data.Rows.Count > 0)
            {
                string sql = "select count(1) from {0} where {1}='{2}'";
                sql = string.Format(sql, entityName, key, data.Rows[0][key]);
                object oo = sqlHelper.ExecuteScalar(sql);

                StringBuilder strSqlColumns = new StringBuilder();
                StringBuilder strSqlColumnsVlues = new StringBuilder();
                StringBuilder strText = new StringBuilder();
                //not isexist

                #region

                if (Convert.ToInt32(oo) == 0)
                {
                    resultData.MsgDesc = "这条数据不存在远程数据库中，请检查！";
                    resultData.MsgId = "004";
                }
                else
                {
                    string sqlUp = "update {0} set {1} where {2}='{3}';";
                    foreach (DataColumn dataColumn in data.Columns)
                    {
                        if (dataColumn.ColumnName != "INSERT_TIME" && dataColumn.ColumnName != "UPDATE_TIME" &&
                            !string.IsNullOrEmpty(Convert.ToString(data.Rows[0][dataColumn.ColumnName])))
                        {
                            //strSqlColumns.Append("," + dataColumn.ColumnName + "='" + Convert.ToString(dr[dataColumn.ColumnName]).Replace("'", "''") + "'");
                            strSqlColumns.Append("," + dataColumn.ColumnName + "=@" + dataColumn.ColumnName + "");

                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = "@" + dataColumn.ColumnName;
                            sqlParameter.Value = Convert.ToString(data.Rows[0][dataColumn.ColumnName]);
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    string sqlColmuns = "";
                    if (strSqlColumns.ToString().Length > 0)
                    {
                        sqlColmuns = strSqlColumns.ToString().Substring(1);
                    }
                    sqlUp = string.Format(sqlUp, entityName, sqlColmuns, key, data.Rows[0][key]);
                    strText.Append(sqlUp);
                }

                #endregion

                try
                {
                    if (!string.IsNullOrEmpty(strText.ToString()))
                    {
                        int ren = sqlHelper.ExecuteNonQuery(strText.ToString(), sqlCommand);
                        strSqlColumns.Clear();
                        strSqlColumnsVlues.Clear();
                        resultData.MsgDesc = "Success";
                        resultData.MsgId = "000";
                    }
                }
                catch (Exception ex)
                {
                    resultData.MsgDesc = "Fail message:" + ex.Message;
                    resultData.MsgId = "004";
                }
            }
            return resultData;
        }
        private SearchResultsData Search(SearchEntityData model)
        {
            SearchResultsData searchResultsData = new SearchResultsData();
            string entityName = model.Entity;
            string key = Common.GetKeyByEntityName(entityName);
            int total = 0;
            try
            {
                DataTable dt = Common.GetRecordFromPage(entityName, key, model.Page, model.PageSize, out total);
                searchResultsData.MsgDesc = "success";
                searchResultsData.MsgID = "000";

                SearchResultsData.Result2 result2 = new SearchResultsData.Result2();

                result2.Page = Convert.ToString(model.Page);
                result2.Pagesize =  Convert.ToString(model.PageSize);
                result2.Total = Convert.ToString( total);

                int allpage = 0;
                if (model.PageSize != 0)
                {
                    allpage = (total / model.PageSize);
                    allpage = ((total % model.PageSize) != 0 ? allpage + 1 : allpage);
                    allpage = (allpage == 0 ? 1 : allpage);
                }

                result2.TotalPage =  Convert.ToString(allpage);
                result2.DataList = dt;
                if ((model.PageSize * model.Page + dt.Rows.Count) < total)
                {
                    result2.IsMore = "Y";
                }
                else
                {
                    result2.IsMore = "N";
                }
                searchResultsData.Result = result2;

            }
            catch (Exception ex)
            {
                searchResultsData.MsgDesc = "Fail message:" + ex.Message;
                searchResultsData.MsgDesc = "004";
            }

            return searchResultsData;
        }
    }
}
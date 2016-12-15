using CMICT.CSP.Model;
using CMICT.CSP.Model.BusinessSearchModels;
using Microsoft.SharePoint.Utilities;
using NET.Framework.Common.CacheHelper;
using NET.Framework.Common.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class BusinessSearchComponent
    {
        static string cacheHeader = "CSP_BS_";




        public TemplateModel GetTemplateByGuid(string guid, bool IsRelease = true)
        {
            MemcachedCache mc = new MemcachedCache();
            if (mc.Get<TemplateModel>(cacheHeader + guid.ToLower()) == null)
                AddTemplateByGuid(guid.ToLower());
            TemplateModel model = mc.Get<TemplateModel>(cacheHeader + guid.ToLower());
            if (model == null || (!model.IsReleased && IsRelease))
                return null;
            return model;
        }

        private void AddTemplateByGuid(string guid)
        {
            Guid templateID = new Guid(guid);
            BS_TEMPLATE_MAINBLL templateBll = new BS_TEMPLATE_MAINBLL();
            BS_TEMPLATE_MAIN template = templateBll.GetModel(templateID);
            if (template == null)
                return; //模板已删除
            if (template.TemplateStatus == "DISABLE")//|| template.TemplateStatus == "DRAFT")
                return;//模板为禁用状态

            TemplateModel templateModel = new TemplateModel();
            templateModel.TemplateName = template.TemplateName;
            templateModel.TemplateDesc = template.TemplateDesc;
            templateModel.IsReleased = (template.TemplateStatus == "FREE" || template.TemplateStatus == "ENABLE");
            //GridBuilder

            GridBuilder gb = new GridBuilder();
            gb.DisplayType = template.DiaplayType;
            gb.ColumnSize = template.ColumnSize;
            templateModel.GridBuilder = gb;

            //SQLBuilder
            templateModel.SQLBuilder = new SQLBuilder();
            BS_DATASOURCEBLL sourceBll = new BS_DATASOURCEBLL();
            BS_DATASOURCE source = sourceBll.GetModel(template.SourceID);
            templateModel.SQLBuilder.IsProcudure = source.ObjectType == "PROC";
            if (templateModel.SQLBuilder.IsProcudure)
                templateModel.SQLBuilder.ProcCalColumns = new Dictionary<string, string>();
            templateModel.SQLBuilder.ConnectionStrings = BuildConnection(source.SourceIP, source.UserName, source.Password, source.DBName);
            if (templateModel.SQLBuilder.IsProcudure)
                templateModel.SQLBuilder.TableName = source.ObjectName;
            else
                templateModel.SQLBuilder.TableName = "[" + source.ObjectName + "]";

            //Columns
            DisplayConfigComponent displayCom = new DisplayConfigComponent();

            DataTable columnsData = displayCom.GetColumnListByTemplateID(templateID);
            DataTable computeColumns = displayCom.GetCalListByTemplateID(templateID);

            DataTable queryData = new DataTable();
            queryData.Columns.Add("IsCal");
            queryData.Columns.Add("ColumnName");
            queryData.Columns.Add("DisplayName");
            queryData.Columns.Add("Sequence", typeof(double));
            queryData.Columns.Add("MergeColumnName");
            queryData.Columns.Add("DecimalCount");
            
            if (columnsData != null && columnsData.Rows.Count > 0)
            {
                foreach (DataRow dr in columnsData.Rows)
                {
                    if (!bool.Parse(Convert.ToString(dr["Visiable"])))
                        continue;
                    DataRow newDr = queryData.NewRow();
                    newDr["IsCal"] = "0";
                    newDr["ColumnName"] = dr["ColumnName"];
                    newDr["DisplayName"] = dr["DisplayName"];
                    newDr["Sequence"] = string.IsNullOrEmpty(Convert.ToString(dr["Sequence"])) ? double.MaxValue : dr["Sequence"];
                    newDr["MergeColumnName"] = dr["MergeColumnName"];
                    newDr["DecimalCount"] = "-1";
                    queryData.Rows.Add(newDr);
                }
            }
            if (computeColumns != null && computeColumns.Rows.Count > 0)
            {
                foreach (DataRow dr in computeColumns.Rows)
                {
                    DataRow newDr = queryData.NewRow();
                    newDr["IsCal"] = "1";
                    newDr["ColumnName"] = dr["ComputeColumn"];
                    newDr["DisplayName"] = dr["DisplayName"];
                    newDr["Sequence"] = string.IsNullOrEmpty(Convert.ToString(dr["Sequence"])) ? double.MaxValue : dr["Sequence"];
                    newDr["MergeColumnName"] = dr["MergeColumnName"];
                    newDr["DecimalCount"] = dr["DecimalCount"];
                    queryData.Rows.Add(newDr);
                }
            }
            DataView dv = queryData.DefaultView;
            dv.Sort = "Sequence Asc,DisplayName Asc";
            DataTable sortedColumns = dv.ToTable();

            string columns = "";//列名
            string displayNames = "";//显示名
            string mergeNames = "";//合并表头
            string queryFields = "";//用于拼sql语句

            int calSeq = 1;
            foreach (DataRow dr in sortedColumns.Rows)
            {
                if (Convert.ToString(dr["IsCal"]) == "0")
                {

                    displayNames += Convert.ToString(dr["DisplayName"]) + ",";
                    mergeNames += Convert.ToString(dr["MergeColumnName"]) + ",";
                    queryFields += Convert.ToString(dr["ColumnName"]) + ",";
                    if (!templateModel.SQLBuilder.IsProcudure)
                        columns += "[" + Convert.ToString(dr["ColumnName"]) + "],";
                }
                else
                {
                    string decimalcount = Convert.ToString(dr["DecimalCount"]);
                    if (string.IsNullOrEmpty(Convert.ToString(dr["Sequence"])))
                        continue;
                    queryFields += "bs_cal_col" + calSeq + ",";

                    mergeNames += Convert.ToString(dr["MergeColumnName"]) + ",";
                    displayNames += Convert.ToString(dr["DisplayName"]) + ",";
                    string calstr = Convert.ToString(dr["ColumnName"]);
                    string proccalstr = string.Empty;
                    if (!string.IsNullOrEmpty(decimalcount)&&decimalcount != "-1")
                    {
                        //proccalstr = "(Convert((" + calstr + ") * 1000 + 0.000000001,'System.Int32') / 1000)+'d3'";
                        proccalstr = "(Convert(" + calstr + ",'System.String'))+'|"+decimalcount+"'";
                        calstr = "Convert(decimal(20,"+decimalcount+")," + calstr+")";
                    }
                    if (templateModel.SQLBuilder.IsProcudure)
                    {
                        templateModel.SQLBuilder.ProcCalColumns.Add("bs_cal_col" + calSeq, proccalstr);
                        //templateModel.SQLBuilder.ProcCalColumns.Add("bs_cal_col" + calSeq, Convert.ToString(dr["ColumnName"])); Convert(" + calstr + ",decimal(20," + decimalcount + "))
                    }
                    else
                    {
                        //columns += "(" + Convert.ToString(dr["ColumnName"]) + ") as bs_cal_col" + calSeq + ",";
                        columns += "(" + calstr + ") as bs_cal_col" + calSeq + ",";
                    }
                    calSeq++;
                }
            }
            //排序：Sequence为空时，排在最后
            foreach (DataRow dr in sortedColumns.Rows)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["Sequence"])))
                    continue;
                queryFields += "bs_cal_col" + calSeq + ",";
                string decimalcount = Convert.ToString(dr["DecimalCount"]);
                mergeNames += Convert.ToString(dr["MergeColumnName"]) + ",";
                displayNames += Convert.ToString(dr["DisplayName"]) + ",";
                string calstr = Convert.ToString(dr["ColumnName"]);
                string proccalstr = string.Empty;
                if (!string.IsNullOrEmpty(decimalcount) && decimalcount != "-1")
                {
                    //proccalstr = "(Convert((" + calstr + ") * 1000 + 0.000000001,'System.Int32') / 1000)+'d3'";
                    proccalstr = "(Convert(" + calstr + ",'System.String'))+'|" + decimalcount + "'";
                    calstr = "Convert(decimal(20," + decimalcount + ")," + calstr + ")";
                }
                if (templateModel.SQLBuilder.IsProcudure)
                {
                    templateModel.SQLBuilder.ProcCalColumns.Add("bs_cal_col" + calSeq, proccalstr);
                }
                else
                {
                    columns += "(" + calstr + ") as bs_cal_col" + calSeq + ",";
                }
                calSeq++;
            }

            queryFields = queryFields.TrimEnd(',');
            columns = columns.TrimEnd(',');
            if (!string.IsNullOrEmpty(mergeNames))
                mergeNames = mergeNames.Substring(0, mergeNames.Length - 1);
            displayNames = displayNames.TrimEnd(',');


            templateModel.SQLBuilder.ColumnNames = queryFields;
            templateModel.SQLBuilder.DisplayNames = displayNames;
            templateModel.SQLBuilder.MergeColumnNames = mergeNames;
            templateModel.SQLBuilder.SelectSQL = columns;


            //隐藏列
            DataTable commColumns = GetCommunicationColumns(guid);
            if (commColumns != null && commColumns.Rows.Count > 0)
            {
                string hiddenColumns = string.Empty;
                string hiddenSelectColumns = string.Empty;
                foreach (DataRow dr in commColumns.Rows)
                {
                    string hiddenColumn = Convert.ToString(dr["SourceColumnName"]);
                    if (!queryFields.Split(',').Contains(hiddenColumn))
                    {
                        hiddenSelectColumns += "[" + Convert.ToString(dr["SourceColumnName"]) + "],";
                        hiddenColumns += Convert.ToString(dr["SourceColumnName"]) + ",";
                    }
                }

                if (!string.IsNullOrEmpty(hiddenColumns.TrimEnd(',')))
                {
                    templateModel.SQLBuilder.SelectSQL += "," + hiddenSelectColumns.TrimEnd(',');
                    templateModel.SQLBuilder.HiddenNames = hiddenColumns.TrimEnd(',');

                }
            }



            //DefaultQuery
            QueryConfigComponent queryCom = new QueryConfigComponent();
            StringBuilder defaultQuery = new StringBuilder();
            defaultQuery.Append(" where 1=1 ");
            StringBuilder subQuery = new StringBuilder();
            string mainLogic = string.Empty;
            int i = 1;
            while (true)
            {
                DataTable queryDt = queryCom.GetDefaultQueryListInfoByTemplateID(templateID, i);
                if (queryDt == null || queryDt.Rows.Count == 0)
                    break;
                if (Convert.ToString(queryDt.Rows[0]["SubLogic"]) == "proc") //处理存储过程参数
                {
                    Dictionary<string, string> list = new Dictionary<string, string>();
                    foreach (DataRow dr in queryDt.Rows)
                    {
                        list.Add(Convert.ToString(dr["ColumnName"]), Convert.ToString(dr["CompareValue"]));

                    }
                    templateModel.SQLBuilder.Parameters = list;
                    i++;
                    continue;
                }
                mainLogic = Convert.ToString(queryDt.Rows[0]["MainLogic"]);
                string subLogic = Convert.ToString(queryDt.Rows[0]["SubLogic"]);
                subQuery.Append(" (");
                foreach (DataRow dr in queryDt.Rows)
                {
                    if (templateModel.SQLBuilder.IsProcudure)
                        subQuery.Append(Convert.ToString(dr["ColumnName"]) + " " + string.Format(FormatCompare(Convert.ToString(dr["Compare"]), templateModel.SQLBuilder.IsProcudure), Convert.ToString(dr["CompareValue"])));
                    else
                        subQuery.Append("[" + Convert.ToString(dr["ColumnName"]) + "] " + string.Format(FormatCompare(Convert.ToString(dr["Compare"]), templateModel.SQLBuilder.IsProcudure), Convert.ToString(dr["CompareValue"])));
                    subQuery.Append(" " + subLogic + " ");
                }
                subQuery.Remove(subQuery.Length - subLogic.Length - 1, subLogic.Length);
                subQuery.Append(") ");
                subQuery.Append(mainLogic);
                i++;
            }
            if (subQuery.Length != 0)
            {
                subQuery.Remove(subQuery.Length - mainLogic.Length, mainLogic.Length);
                defaultQuery.Append(" and (");
                defaultQuery.Append(subQuery.ToString());
                defaultQuery.Append(")");
            }

            templateModel.SQLBuilder.DefauleQuery = defaultQuery.ToString();


            //QueryControls
            QueryConfigComponent queryBll = new QueryConfigComponent();
            DataTable query = queryBll.GetUserQueryListByTemplateIDForModel(templateID);
            if (query != null && query.Rows.Count > 0)
            {
                templateModel.QueryControls = new List<QueryControls>();

                foreach (DataRow dr in query.Rows)
                {
                    QueryControls controls = new QueryControls();
                    controls.Reminder = Convert.ToString(dr["Reminder"]);
                    controls.DefautValue = Convert.ToString(dr["DefaultValue"]);
                    if (templateModel.SQLBuilder.IsProcudure)
                        controls.ColumnName = Convert.ToString(dr["ColumnName"]);
                    else
                        controls.ColumnName = "[" + Convert.ToString(dr["ColumnName"]) + "]";

                    controls.DisplayName = Convert.ToString(dr["DisplayName"]);
                    controls.ControlType = Convert.ToString(dr["ControlType"]);
                    controls.Compare = controls.ColumnName + FormatCompare(Convert.ToString(dr["Compare"]), templateModel.SQLBuilder.IsProcudure);
                    if (Convert.ToString(dr["Compare"]) == "CONTAIN" || Convert.ToString(dr["Compare"]) == "NOTCONTAIN")
                        controls.IsLike = true;
                    else
                        controls.IsLike = false;
                    if (controls.ControlType == "ENUM" || controls.ControlType == "MULTENUM"||controls.ControlType == "MATCH")
                        controls.SourceSql = "select distinct(" + controls.ColumnName + ") as EnumValue from " + templateModel.SQLBuilder.TableName + " " + templateModel.SQLBuilder.DefauleQuery;

                    templateModel.QueryControls.Add(controls);
                }
            }


            //DefaultOrderby
            DataTable orderby = displayCom.GetSortListByTemplateID(templateID);
            if (orderby != null && orderby.Rows.Count > 0)
            {
                string orderbySQL = "";
                foreach (DataRow dr in orderby.Rows)
                {
                    if (templateModel.SQLBuilder.IsProcudure)
                        orderbySQL += Convert.ToString(dr["SortColumn"]) + " " + Convert.ToString(dr["Type"]) + ",";
                    else
                        orderbySQL += "[" + Convert.ToString(dr["SortColumn"]) + "] " + Convert.ToString(dr["Type"]) + ",";
                }
                orderbySQL = orderbySQL.TrimEnd(',');

                templateModel.SQLBuilder.Orderby = orderbySQL;
            }

            //Groupby

            DataTable groupData = displayCom.GetGroupByListByTemplateID(templateID);

            if (groupData != null && groupData.Rows.Count > 0)
            {
                templateModel.GroupBy = new List<GroupBy>();
                foreach (DataRow dr in groupData.Rows)
                {
                    GroupBy group = new GroupBy();
                    group.Columns = Convert.ToString(dr["Columns"]);
                    group.IsAtLast = Convert.ToString(dr["Location"]) == "LAST";
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string groupSql in Convert.ToString(dr["ComputeColumn"]).Split(';'))
                    {
                        string[] gs = groupSql.Split(',');
                        //string key = groupSql.Substring(groupSql.IndexOf('(')).TrimEnd(')').TrimStart('(');
                        string key = gs[0].Substring(gs[0].IndexOf('(')).TrimEnd(')').TrimStart('(');
                        dic.Add(key, groupSql);
                    }
                    group.GroupByColumns = dic;
                    templateModel.GroupBy.Add(group);
                }
            }

            //Communication
            CommunicationConfigComponent communicationCom = new CommunicationConfigComponent();
            DataTable commData = communicationCom.GetCommunicationByTemplateID(guid);
            if (commData != null && commData.Rows.Count > 0)
            {
                templateModel.Communication = new List<Communication>();

                foreach (DataRow dr in commData.Rows)
                {
                    Communication comModel = new Communication();
                    comModel.SourceTemplateID = Convert.ToString(dr["SourceTemplateID"]);
                    comModel.CoumunicationID = Convert.ToString(dr["CommunicationID"]);
                    comModel.Fields = new Dictionary<string, string>();
                    DataTable commDetailData = communicationCom.GetCommunicationFields(Convert.ToString(dr["CommunicationID"]));
                    if (commDetailData != null && commDetailData.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in commDetailData.Rows)
                        {
                            if (dr2["SourceColumnName"] == null || string.IsNullOrEmpty(dr2["SourceColumnName"].ToString()))
                                continue;
                            comModel.Fields.Add(Convert.ToString(dr2["SourceColumnName"]), Convert.ToString(dr2["TargetColumnName"]));
                        }
                    }
                    templateModel.Communication.Add(comModel);
                }

            }


            templateModel.SQLBuilder.PageSize = Convert.ToString(template.PageSize);

            if (templateModel.SQLBuilder.IsProcudure != true && templateModel.GroupBy == null)
                templateModel.IsTruePaged = true;
            else
                templateModel.IsTruePaged = false;


            MemcachedCache mc = new MemcachedCache();
            mc.Put<TemplateModel>(cacheHeader + guid.ToLower(), templateModel);
        }

        private DataTable GetCommunicationColumns(string guid)
        {
            string sql = "select distinct(SourceColumnName) from BS_COMMUNICATION_DETAIL where CommunicationID in (select CommunicationID from BS_COMMUNICATION_MAIN where SourceTemplateID='" + guid + "')";
            DbHelperSQL dbhelper = new DbHelperSQL();
            return dbhelper.ExecuteTable(CommandType.Text, sql, null);
        }

        public void RefreshTemplateByGuid(string guid)
        {
            MemcachedCache mc = new MemcachedCache();
            if (mc.Get<TemplateModel>(cacheHeader + guid.ToLower()) != null)
                mc.Remove(cacheHeader + guid.ToLower());
            AddTemplateByGuid(guid.ToLower());
        }
        public void RemoveTemplateByGuid(string guid)
        {
            MemcachedCache mc = new MemcachedCache();
            if (mc.Get<TemplateModel>(cacheHeader + guid.ToLower()) != null)
                mc.Remove(cacheHeader + guid.ToLower());
        }
        //完成时根据是否配置页面更新模板状态
        public void RefreshTemplateStatusByGuid(string TemplateID)
        {
            TemplateManageComponent tcc = new TemplateManageComponent();
            DataTable dt = tcc.GetListByTemplateID(TemplateID);
            if (dt != null && dt.Rows.Count > 0)
            {
                tcc.EnableTemplateInfo(TemplateID, "ENABLE");
            }
        }

        public void RefreshAllTemplate()
        {

        }

        public string FormatCompare(string value, bool isProc)
        {
            if (isProc)
            {
                switch (value)
                {
                    case "EQUAL":
                        return " = '{0}' ";
                    case "NOTEQUAL":
                        return " != '{0}' ";
                    case "BIGGER":
                        return " > '{0}' ";
                    case "BIGGEREQUAL":
                        return " >= '{0}' ";
                    case "LESS":
                        return " < '{0}' ";
                    case "LESSEQUAL":
                        return " <= '{0}' ";
                    case "CONTAIN":
                        return " like '%{0}%' ";
                    case "NOTCONTAIN":
                        return " not like '%{0}%' ";

                    default:
                        return "";

                }
            }
            else
            {
                switch (value)
                {
                    case "EQUAL":
                        return " = N'{0}' ";
                    case "NOTEQUAL":
                        return " != N'{0}' ";
                    case "BIGGER":
                        return " > N'{0}' ";
                    case "BIGGEREQUAL":
                        return " >= N'{0}' ";
                    case "LESS":
                        return " < N'{0}' ";
                    case "LESSEQUAL":
                        return " <= N'{0}' ";
                    case "CONTAIN":
                        return " like N'%{0}%' ";
                    case "NOTCONTAIN":
                        return " not like N'%{0}%' ";

                    default:
                        return "";

                }
            }
        }

        public string BuildConnection(string ip, string uname, string pwd, string dbname)
        {
            string constr = "Max Pool Size=2048;server=" + ip + ";uid=" + uname + ";pwd=" + pwd + ";database=" + dbname + "";
            return constr;
        }

        /// <summary>
        /// 返回总页数
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public string PageCount(TemplateModel template, string pPageSize, string pWhere)
        {
            DbHelperSQL dbhelper = new DbHelperSQL(template.SQLBuilder.ConnectionStrings);
            string pageSize = pPageSize;
            string where = template.SQLBuilder.DefauleQuery.Replace("[本人]", BaseComponent.GetCurrentUserLoginName()) + pWhere;
            string result = "0";


            string sql = "select count(*) from " + template.SQLBuilder.TableName + " where " + where;

            object rows = dbhelper.ExecuteScalar(CommandType.Text, sql);
            BaseComponent.Info("SQL:" + sql);
            if (rows != null && rows.ToString() != "")
            {
                result = Convert.ToString(Convert.ToInt32(rows));
            }

            return result;
        }

        /// <summary>
        /// 真分页情况下，返回分页数据
        /// </summary>
        /// <param name="template"></param>
        /// <param name="pPageNo"></param>
        /// <param name="pPageSize"></param>
        /// <param name="pWhere"></param>
        /// <param name="paras"></param>
        /// <param name="pOrderby"></param>
        /// <returns></returns>
        public DataTable GetDataIsTruePaged(TemplateModel template, int pPageNo, int pPageSize, string pWhere, string pOrderby, bool paged = true)
        {
            DbHelperSQL dbhelper = new DbHelperSQL(template.SQLBuilder.ConnectionStrings);
            DataTable dt;
            string sql = string.Empty;
            if (template.IsTruePaged == true) //真分页，表示直接从数据库取数据
            {
                StringBuilder sb = new StringBuilder();
                //分页语句 例：select * from (select row_number() over(order by @@servername) as bs_row_num,* from BS_TEMPLATE_MAIN where TemplateName like N'%%')t where bs_row_num>4 and bs_row_num <8
                sb.Append(@"select * from (select row_number() over(order by ");
                if (!string.IsNullOrWhiteSpace(pOrderby))
                {
                    sb.Append(pOrderby);
                }
                else if (!string.IsNullOrWhiteSpace(template.SQLBuilder.Orderby))
                {
                    sb.Append(template.SQLBuilder.Orderby);
                }
                else
                {
                    sb.Append("@@servername");
                }
                sb.Append(") as bs_row_num, ");
                sb.Append(template.SQLBuilder.SelectSQL);
                sb.Append(" from ");
                sb.Append(template.SQLBuilder.TableName);
                sb.Append(template.SQLBuilder.DefauleQuery.Replace("[本人]", BaseComponent.GetCurrentUserLoginName()));
                if (!string.IsNullOrEmpty(pWhere))
                {
                    sb.Append(" and ");
                    sb.Append(pWhere);
                }

                sb.Append(")t");
                if (paged)
                {
                    string startPage = ((pPageNo - 1) * pPageSize).ToString();
                    string endPage = (pPageNo * pPageSize + 1).ToString();

                    sb.Append(" where bs_row_num>");
                    sb.Append(startPage);
                    sb.Append(" and bs_row_num<");
                    sb.Append(endPage);
                }

                BaseComponent.Info("GetDataIsTruePaged SQL:" + sb.ToString());

                dt = dbhelper.ExecuteTable(CommandType.Text, sb.ToString());

                return dt;
            }
            return null;
        }

        public int GetTotalCount(TemplateModel template, string pWhere)
        {
            DbHelperSQL dbhelper = new DbHelperSQL(template.SQLBuilder.ConnectionStrings);
            StringBuilder sb = new StringBuilder();
            sb.Append("select count(*) from ");
            sb.Append(template.SQLBuilder.TableName);
            sb.Append(template.SQLBuilder.DefauleQuery.Replace("[本人]", BaseComponent.GetCurrentUserLoginName()));
            if (!string.IsNullOrWhiteSpace(pWhere))
            {
                sb.Append(" and ");
                sb.Append(pWhere);
            }

            BaseComponent.Info("GetTotalCount SQL:" + sb.ToString());

            return (int)dbhelper.ExecuteScalar(CommandType.Text, sb.ToString());
        }


        /// <summary>
        /// 假分页情况下，返回所有数据
        /// </summary>
        /// <param name="template"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public DataTable GetDataIsNotTruePaged(TemplateModel template, string pWhere, SqlParameter[] paras,string orderBy="")
        {
            DbHelperSQL dbhelper = new DbHelperSQL(template.SQLBuilder.ConnectionStrings);
            DataTable dt;
            if (template.SQLBuilder.IsProcudure == true)
            {
                //SqlParameter[] newParas = new SqlParameter[template.SQLBuilder.Parameters.Length + paras.Length];
                //template.SQLBuilder.Parameters.CopyTo(newParas, 0);
                //paras.CopyTo(newParas, template.SQLBuilder.Parameters.Length);

                List<SqlParameter> listParas = new List<SqlParameter>();
                if (paras != null && paras.Count() > 0)
                {
                    listParas.AddRange(paras.ToList());
                }
                if (template.SQLBuilder.Parameters != null && template.SQLBuilder.Parameters.Count() > 0)
                {
                    foreach (var dic in template.SQLBuilder.Parameters)
                    {
                        listParas.Add(new SqlParameter(dic.Key, dic.Value.Replace("[本人]", BaseComponent.GetCurrentUserLoginName())));
                    }
                }

                DataTable returnDt = dbhelper.ExecuteTable(CommandType.StoredProcedure, template.SQLBuilder.TableName, listParas.ToArray());

                string filter = string.Empty;
                if (template.SQLBuilder.DefauleQuery != "where 1=1")
                {
                    filter += template.SQLBuilder.DefauleQuery.Substring(7).Replace("[本人]", BaseComponent.GetCurrentUserLoginName());
                }
                if (!string.IsNullOrWhiteSpace(pWhere))
                {
                    filter += " and " + pWhere;
                }
                BaseComponent.Info("存储过程本人查询条件:" + filter);
                var rows = returnDt.Select(filter);
                dt = returnDt.Clone();
                for (int i = 0; i < rows.Length; i++)
                {
                    dt.ImportRow(rows[i]);
                }

                //列排序，计算列
                int columnIndex = 0;
                foreach (string proColumnName in template.SQLBuilder.ColumnNames.Split(','))
                {
                    if (dt.Columns.Contains(proColumnName))
                    {
                        dt.Columns[proColumnName].SetOrdinal(columnIndex);
                    }
                    else
                    {
                        dt.Columns.Add(proColumnName);
                        dt.Columns[proColumnName].Expression = template.SQLBuilder.ProcCalColumns[proColumnName];
                        dt.Columns[proColumnName].SetOrdinal(columnIndex);
                    }
                    columnIndex++;
                }

                //while (dt.Columns.Count > columnIndex) //不用的列全干掉，性能上说不定有奇效:)
                //{
                //    dt.Columns.RemoveAt(columnIndex);
                //}

                //排序
                //string sortview = string.Empty;
                string sortview = template.SQLBuilder.Orderby;
                if (!string.IsNullOrEmpty(orderBy))
                {
                    sortview = orderBy;
                    //string sortcolname = orderBy.Split(' ')[0].ToLower().Trim();
                    //if (dorderby.Contains(sortcolname + " "))
                    //{
                    //    string[] dorderlist = dorderby.Split(',');
                    //    foreach (string ds in dorderlist)
                    //    {
                    //        if (ds.Contains(sortcolname + " "))
                    //        {
                    //            sortview += orderBy + ",";
                    //        }
                    //        else
                    //        {
                    //            sortview += ds + ",";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    sortview = dorderby + "," + orderBy;
                    //}
                }
                //else
                //{
                //    sortview = dorderby;
                //}
                if (!string.IsNullOrEmpty(sortview))
                {
                    dt.DefaultView.Sort = sortview.TrimEnd(','); //TODO:行汇总排序
                }
                dt = dt.DefaultView.ToTable();

            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                sb.Append(template.SQLBuilder.SelectSQL);
                sb.Append(" from ");
                sb.Append(template.SQLBuilder.TableName);
                sb.Append(template.SQLBuilder.DefauleQuery.Replace("[本人]", BaseComponent.GetCurrentUserLoginName()));
                if (!string.IsNullOrWhiteSpace(pWhere))
                {
                    sb.Append(" and ");
                    sb.Append(pWhere);
                }
                if (!string.IsNullOrWhiteSpace(template.SQLBuilder.Orderby))//TODO:行汇总排序
                {
                    sb.Append(" order by ");
                    //排序
                    //string sortview = string.Empty;
                    string sortview = template.SQLBuilder.Orderby;
                    if (!string.IsNullOrEmpty(orderBy))
                    {
                        sortview = orderBy;
                        //string sortcolname = orderBy.Split(' ')[0].ToLower().Trim();
                        //if (dorderby.Contains(sortcolname + " "))
                        //{
                        //    string[] dorderlist = dorderby.Split(',');
                        //    foreach (string ds in dorderlist)
                        //    {
                        //        if (ds.Contains(sortcolname + " "))
                        //        {
                        //            sortview += orderBy + ",";
                        //        }
                        //        else
                        //        {
                        //            sortview += ds + ",";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    sortview = dorderby + "," + orderBy;
                        //}
                    }
                    //else
                    //{
                    //    sortview = dorderby;
                    //}
                    sb.Append(sortview.TrimEnd(',')); 
                }
                dt = dbhelper.ExecuteTable(CommandType.Text, sb.ToString());
                BaseComponent.Info("SQL:" + sb.ToString());
            }

            DataTable dtResult = dt.Copy();


            dtResult.Columns.Add("bs_row_num");
            dtResult.Columns.Add("bs_is_groupby");

            //分组统计
            if (template.GroupBy != null && template.GroupBy.Count > 0)
            {
                

                //表中汇总
                var middleGroup = template.GroupBy.Where(p => p.IsAtLast == false);
                foreach (GroupBy gb in middleGroup) //每次循环后 得到分组后的数据
                {
                    DataTable distinct = dtResult.DefaultView.ToTable(true, gb.Columns.Split(','));
                    DataTable tempResult = dtResult.Clone();

                    foreach (DataRow dr in distinct.Rows)
                    {
                        string select = string.Empty;
                        foreach (string colName in gb.Columns.Split(','))
                        {
                            select += colName + "='" + dr[colName] + "' and ";
                        }
                        if (!string.IsNullOrWhiteSpace(select))
                            select = select.Substring(0, select.Length - 5);

                        DataRow[] rows = dtResult.Select(select);  //取出一组数据

                        //temp用来存储筛选出来的数据
                        DataTable temp = dtResult.Clone();
                        foreach (DataRow row in rows)
                        {
                            temp.Rows.Add(row.ItemArray);
                            tempResult.Rows.Add(row.ItemArray);
                        }

                        DataRow groupDr = tempResult.NewRow();
                        foreach (string colName in gb.Columns.Split(','))
                        {
                            groupDr[colName] = dr[colName];
                        }
                        foreach (var groupName in gb.GroupByColumns)
                        {
                            string[] gn = groupName.Value.Split(',');
                            string res = Convert.ToString(temp.Compute(gn[0], "bs_is_groupby is null"));
                            if (gn[1] == "-1")//自动小数
                            {
                                groupDr[groupName.Key] = temp.Compute(gn[0], "bs_is_groupby is null");
                            }
                            else
                            {
                                double comresult = 0.00;
                                if (!string.IsNullOrEmpty(res))
                                {
                                    double.TryParse(res, out comresult);
                                }
                                if (comresult != 0.0)
                                {
                                    groupDr[groupName.Key] = comresult.ToString("f" + gn[1]);
                                }
                                else
                                {
                                    groupDr[groupName.Key] = DBNull.Value;
                                }
                            }
                        }
                        groupDr["bs_is_groupby"] = "1";
                        groupDr["bs_row_num"] = "小计（合计）";
                        tempResult.Rows.Add(groupDr);
                    }

                    dtResult = tempResult.Copy();

                }

                //表尾汇总
                var lastGroup = template.GroupBy.Where(p => p.IsAtLast == true);
                foreach (GroupBy gb in lastGroup)
                {
                    //DataTable newdt = new DataTable();
                    //newdt = dtResult.Clone(); 
                    //DataRow[] realRows = dtResult.Select("bs_is_groupby is null"); 
                    //foreach (DataRow row in realRows)  
                    //{
                    //    newdt.Rows.Add(row.ItemArray);
                    //}
                    if (string.IsNullOrEmpty(gb.Columns))
                    {
                        DataRow groupDr = dtResult.NewRow();

                        foreach (var groupName in gb.GroupByColumns)
                        {
                            string[] gn = groupName.Value.Split(',');
                            string res = Convert.ToString(dtResult.Compute(gn[0], "bs_is_groupby is null"));
                            if (gn[1] == "-1")//自动小数
                            {
                                groupDr[groupName.Key] = dtResult.Compute(gn[0], "bs_is_groupby is null");
                            }
                            else
                            {
                                double comresult = 0.00;
                                if (!string.IsNullOrEmpty(res))
                                {
                                    double.TryParse(res, out comresult);
                                }
                                if (comresult != 0.0)
                                {
                                    groupDr[groupName.Key] = comresult.ToString("f" + gn[1]);
                                }
                                else
                                {
                                    groupDr[groupName.Key] = DBNull.Value;
                                }
                            }
                        }
                        groupDr["bs_is_groupby"] = "1";
                        groupDr["bs_row_num"] = "总计";
                        dtResult.Rows.Add(groupDr);
                    }
                    else
                    {
                        DataTable distinct = dt.DefaultView.ToTable(true, gb.Columns.Split(','));

                        foreach (DataRow dr in distinct.Rows)
                        {
                            string select = string.Empty;
                            foreach (string colName in gb.Columns.Split(','))
                            {
                                select += colName + "='" + dr[colName] + "' and ";
                            }
                            if (!string.IsNullOrWhiteSpace(select))
                                select = select.Substring(0, select.Length - 5);

                            DataRow[] rows = dtResult.Select(select);  //取出一组数据

                            //temp用来存储筛选出来的数据
                            DataTable temp = dtResult.Clone();
                            foreach (DataRow row in rows)
                            {
                                temp.Rows.Add(row.ItemArray);
                            }

                            DataRow groupDr = dtResult.NewRow();
                            foreach (string colName in gb.Columns.Split(','))
                            {
                                groupDr[colName] = dr[colName];
                            }
                            foreach (var groupName in gb.GroupByColumns)
                            {
                                string[] gn = groupName.Value.Split(',');
                                string res = Convert.ToString(temp.Compute(gn[0], "bs_is_groupby is null"));
                                if (gn[1] == "-1")//自动小数
                                {
                                    groupDr[groupName.Key] = temp.Compute(gn[0], "bs_is_groupby is null");
                                }
                                else
                                {
                                    double comresult = 0.00;
                                    if (!string.IsNullOrEmpty(res))
                                    {
                                        double.TryParse(res, out comresult);
                                    }
                                    if (comresult != 0.0)
                                    {
                                        groupDr[groupName.Key] = comresult.ToString("f" + gn[1]);
                                    }
                                    else
                                    {
                                        groupDr[groupName.Key] = DBNull.Value;
                                    }
                                }
                            }
                            groupDr["bs_is_groupby"] = "1";
                            groupDr["bs_row_num"] = "小计（合计）";
                            dtResult.Rows.Add(groupDr);
                        }
                    }
                }
            }

            int index = 1;
            foreach (DataRow dr in dtResult.Rows)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["bs_is_groupby"])))
                    continue;
                dr["bs_row_num"] = index.ToString();
                index++;
            }

            return dtResult;
        }

        public DataTable GetDistinctEnumValue(DataTable sourceDt, string columnName)
        {
            DataTable distinct = sourceDt.DefaultView.ToTable(true, columnName);
            if (distinct != null && distinct.Rows.Count > 0)
                distinct.Columns[0].ColumnName = "EnumValue";
            return distinct;
        }

        public DataTable GetDataTablePaged(DataTable sourceDt, int PageIndex, int PageSize, string pOrderby)
        {
            DataView dv = new DataView();
            dv.Table = sourceDt;
            if (!string.IsNullOrEmpty(pOrderby))
            {
                dv.Sort = pOrderby;
            }
            DataTable dt = dv.ToTable();


            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Clone();
            //newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow dr = dt.Rows[i];
                newdt.Rows.Add(dr.ItemArray);
            }

            return newdt;
        }
        public DataTable GetTableBySql(string connectionString, string sql)
        {
            DbHelperSQL dbhelper = new DbHelperSQL(connectionString);
            return dbhelper.ExecuteTable(CommandType.Text, sql, null);
        }

        public Dictionary<string, string> GetCommunicationFields(TemplateModel targetTemplate, string communicationID, string sourceTemplate)
        {
            if (targetTemplate.Communication == null)
                return null;
            var comm = targetTemplate.Communication.Where(p => p.CoumunicationID == communicationID && p.SourceTemplateID == sourceTemplate).FirstOrDefault();
            if (comm == null)
                return null;
            return comm.Fields;
        }

        public string ExportFile(TemplateModel model, DataTable dt,string PageTitle)
        {
            if (dt == null || dt.Rows.Count == 0)
                return "";
            string path = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\CMICT.CSP.Web\BusinessSearch\");

            // 判定该路径是否存在
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string newFileName = Guid.NewGuid().ToString();
            string newFilePath = path + newFileName + ".xlsx";

            var columns = ("bs_row_num," + model.SQLBuilder.ColumnNames).Split(',').ToList();
            var columnsDisplay = ("序号,"+model.SQLBuilder.DisplayNames).Split(',').ToList();
            var mergeColumns = (","+model.SQLBuilder.MergeColumnNames).Split(',').ToList();

            for (int i = 0; i < columns.Count; i++)
            {
                //dt.Columns[columns[i]].ColumnName = columnsDisplay[i];
                dt.Columns[columns[i]].SetOrdinal(i);
            }
            int j = dt.Columns.Count;
            while (j > columns.Count)
            {
                dt.Columns.RemoveAt(columns.Count);
                j = dt.Columns.Count;
            }
            EPPlus.CreateFile(newFilePath, dt, columnsDisplay, mergeColumns, PageTitle, model.TemplateName);

            return newFilePath;
        }
    }

}

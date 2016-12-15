using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.SharePoint;
using NET.Framework.Common.Extensions;
using SP.Framework.DAL;
using CamlexNET;
using System.Linq.Expressions;
using System.Data.SqlClient;
using NET.Framework.Common.ConstantClass;

namespace CMICT.CSP.BLL.Components
{
    public class RightsManagementComponent
    {
        DataTable dtResult = null;

        /// <summary>
        /// 获取根目录下所有子网站的页面
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public DataTable GetAllPages(string siteUrl, out int RecordCount, string pageName, string useCompany, string creater, int currentPage, int pageSize)
        {

            #region 拼接query
            string caml = @"<Where><And>
                    <Neq><FieldRef Name='LinkFilename' /><Value Type='Text'>default.aspx</Value></Neq>";

            if (!string.IsNullOrWhiteSpace(pageName))
            {
                caml = caml + "<And>";
                caml = caml + "<Contains><FieldRef Name='Title'/><Value Type='Text'>" + pageName + "</Value></Contains>";
            }

            if (!string.IsNullOrWhiteSpace(creater))
            {
                caml = caml + "<And>";
                caml = caml + "<Contains><FieldRef Name='Author'/><Value Type='User'>" + creater + "</Value></Contains>";

            }

            caml = caml + "<Neq><FieldRef Name='LinkFilename' /><Value Type='Text'>PageNotFoundError.aspx</Value></Neq></And>";

            if (!string.IsNullOrWhiteSpace(creater))
            {
                caml = caml + "</And>";
            }

            if (!string.IsNullOrWhiteSpace(pageName))
            {
                caml = caml + "</And>";
            }

            caml = caml + "</Where><OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";

            #endregion

            //获取所有子网站的页面
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.RootWeb)
                {
                    //获取网站集的页面
                    GetAllWebPages(web, site.ID, caml);
                }
            }

            //获取页面相关联的模板信息

            GetTemplateInfo();

            if (!string.IsNullOrWhiteSpace(useCompany))
            {

                DataTable dtFinal = dtResult.Clone();

                if (dtResult != null && dtResult.Rows.Count > 0)
                {

                    DataRow[] rows = dtResult.Select("UseUnit like '%" + useCompany + "%'", "Created desc");

                    if (rows != null && rows.Length > 0)
                    {
                        DataTable dtSearchResult = rows.CopyToDataTable();

                        dtFinal = new DataTable();
                        dtFinal = GetDataTable(dtSearchResult, currentPage, pageSize);
                        RecordCount = dtSearchResult.Rows.Count;
                    }
                    else
                    {
                        RecordCount = 0;
                    }

                }
                else
                {
                    RecordCount = 0;
                }

                return dtFinal;
            }
            else
            {
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataTable dtFinal = GetDataTable(dtResult, currentPage, pageSize);

                    DataView view = dtFinal.DefaultView;
                    view.Sort = "Created desc";

                    DataTable dtSortResult = view.ToTable();

                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        RecordCount = dtResult.Rows.Count;
                    }
                    else
                    {
                        RecordCount = 0;
                    }
                    return dtSortResult;
                }
                else
                {
                    RecordCount = 0;
                    DataTable dtFinal = dtResult.Clone();
                    return dtFinal;
                }



            }

        }

        /// <summary>
        /// 获取模板名称及使用单位
        /// </summary>
        private void GetTemplateInfo()
        {
            DbHelperSQL dbhelper = new DbHelperSQL();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select m.TemplateName,m.Unit from bs_template_pages p left join BS_TEMPLATE_MAIN m ");
            strSql.Append(" on p.TemplateID=m.TemplateID  ");
            strSql.Append(" where p.Url=@Url ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@Url", SqlDbType.NVarChar,50)
			};

            foreach (DataRow row in dtResult.Rows)
            {
                parameters[0].Value = Convert.ToString(row["RelativeUrl"]);
                DataTable dtTemp = dbhelper.ExecuteTable(CommandType.Text, strSql.ToString(), parameters);

                if (dtTemp.Rows.Count > 0)
                {
                    row["TemplateName"] = Convert.ToString(dtTemp.Rows[0]["TemplateName"]);
                    row["UseUnit"] = Convert.ToString(dtTemp.Rows[0]["Unit"]);
                }


            }
        }

        /// <summary>
        /// 获取所有子网站的页面
        /// </summary>
        /// <param name="webCollection">子网站</param>
        /// <param name="siteID"></param>
        /// <param name="query"></param>
        private void GetSubWebPages(SPWebCollection webCollection, Guid siteID, string query)
        {
            foreach (SPWeb subWeb in webCollection)
            {
                DataTable dtItems = new DataTable();
                SPListItemCollection items = SPHelper.GetAllSPListItems(siteID, subWeb.ServerRelativeUrl, ConstantClass.PAGESFORM, query, "", 0);

                if (items != null && items.Count > 0)
                {
                    dtItems = GetDataTable(items, subWeb.Url);
                }

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    MergeTable(dtItems, dtResult);
                }
                else
                {
                    dtResult = dtItems.Copy();
                }

                SPWebCollection subWebCollection = subWeb.GetSubwebsForCurrentUser();

                if (subWebCollection.Count > 0)
                {
                    GetSubWebPages(subWebCollection, siteID, query);
                }

                subWeb.Dispose();

            }
        }

        private void GetAllWebPages(SPWeb rootWeb, Guid siteID, string query)
        {
            //DataTable dtItems = new DataTable();
            //SPListItemCollection items = SPHelper.GetAllSPListItems(siteID, rootWeb.ServerRelativeUrl, ConstantClass.PAGESFORM, query, "", 0);


            //if (items != null && items.Count > 0)
            //{
            //    dtItems = GetDataTable(items, rootWeb.Url);
            //}

            //if (dtResult != null && dtResult.Rows.Count > 0)
            //{
            //    MergeTable(dtItems, dtResult);
            //}
            //else
            //{
            //    dtResult = dtItems.Copy();
            //}

            SPWebCollection subWebCollection = rootWeb.GetSubwebsForCurrentUser();

            if (subWebCollection.Count > 0)
            {
                GetSubWebPages(subWebCollection, siteID, query);
            }
        }

        /// <summary>
        /// 合并DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target">合并后的Table</param>
        private void MergeTable(DataTable source, DataTable target)
        {
            foreach (DataRow row in source.Rows)
            {
                target.ImportRow(row);
            }

        }

        /// <summary>
        /// 获取制定字段的Table
        /// </summary>
        /// <param name="items"></param>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public DataTable GetDataTable(SPListItemCollection items, string webUrl)
        {
            DataTable dtAllInfo = new DataTable();
            dtAllInfo.Columns.Add("Href");
            dtAllInfo.Columns.Add("RelativeUrl");
            dtAllInfo.Columns.Add("Title");
            dtAllInfo.Columns.Add("SubTitle");
            dtAllInfo.Columns.Add("Author");
            dtAllInfo.Columns.Add("Created");
            dtAllInfo.Columns.Add("PageStatus");
            dtAllInfo.Columns.Add("TemplateName");
            dtAllInfo.Columns.Add("UseUnit");
            dtAllInfo.Columns.Add("FileGuid");
            dtAllInfo.Columns.Add("WebUrl");
            dtAllInfo.Columns.Add("IsDisplay");

            string folderNames = System.Configuration.ConfigurationSettings.AppSettings["FilterFolderName"].ToString();
            string[] folderNameList = folderNames.Split(';');
            bool isFilterFolder = false;

            foreach (SPListItem item in items)
            {
                isFilterFolder = false;

                foreach (string folderName in folderNameList)
                {
                    if (item.File.ParentFolder.Name.ToLower() == folderName.ToLower())
                    {
                        isFilterFolder = true;
                    }
                }

                if (isFilterFolder)
                {
                    continue;
                }

                DataRow newRow = dtAllInfo.NewRow();

                newRow["Href"] = webUrl.TrimEnd('/') + "/" + Convert.ToString(item.File.Url);
                newRow["FileGuid"] = Convert.ToString(item.File.UniqueId);
                newRow["WebUrl"] = item.Web.ServerRelativeUrl;
                newRow["RelativeUrl"] = Convert.ToString(item.File.ServerRelativeUrl);
                string title = Convert.ToString(item["Title"]);
                newRow["Title"] = title;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    int len = title.Length;
                    newRow["SubTitle"] = Convert.ToString(item["Title"]).Substring(0, ConstantClass.PAGETITLESUBLEN > len ? len : ConstantClass.PAGETITLESUBLEN);
                }
                else
                {
                    newRow["SubTitle"] = "";
                }

                string author = Convert.ToString(item["Author"]);
                newRow["Author"] = author.Split("#")[1];
                string checkoutUser = Convert.ToString(item["CheckoutUser"]);
                newRow["PageStatus"] = string.IsNullOrWhiteSpace(checkoutUser) ? ConstantClass.CHECKIN : ConstantClass.CHECKEDOUT;
                newRow["Created"] = DateTime.Parse(Convert.ToString(item["Created"])).ToString("yyyy-MM-dd");
                newRow["IsDisplay"] = string.IsNullOrWhiteSpace(checkoutUser) ? "none" : "";

                dtAllInfo.Rows.Add(newRow);
            }
            return dtAllInfo;
        }

        public DataTable GetDataTable(DataTable items, int currpage, int pagesize)
        {
            DataTable dt = new DataTable();
            dt = items.Clone();

            int count = pagesize * currpage;
            if (count > items.Rows.Count)
                count = items.Rows.Count;
            for (int i = pagesize * (currpage - 1); i < count; i++)
            {
                DataRow row = dt.NewRow();
                row["Href"] = Convert.ToString(items.Rows[i]["Href"]);
                row["RelativeUrl"] = Convert.ToString(items.Rows[i]["RelativeUrl"]);
                row["Title"] = Convert.ToString(items.Rows[i]["Title"]);
                row["Author"] = Convert.ToString(items.Rows[i]["Author"]);
                row["Created"] = Convert.ToString(items.Rows[i]["Created"]);
                row["PageStatus"] = Convert.ToString(items.Rows[i]["PageStatus"]);
                row["TemplateName"] = Convert.ToString(items.Rows[i]["TemplateName"]);
                row["UseUnit"] = Convert.ToString(items.Rows[i]["UseUnit"]);
                row["FileGuid"] = Convert.ToString(items.Rows[i]["FileGuid"]);
                row["WebUrl"] = Convert.ToString(items.Rows[i]["WebUrl"]);
                row["SubTitle"] = Convert.ToString(items.Rows[i]["SubTitle"]);
                row["IsDisplay"] = Convert.ToString(items.Rows[i]["IsDisplay"]);

                dt.Rows.Add(row);
            }
            return dt;
        }

    }
}

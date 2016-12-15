using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using CMICT.CSP.Async.Commons;
using CMICT.CSP.Async.Module;
using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Newtonsoft.Json;
using NET.Framework.Common.ExcelHelper;
using Wuqi.Webdiyer;

namespace CMICT.CSP.Async.WebParts.GlobalsSynchronize.GlobalData
{
    [ToolboxItemAttribute(false)]
    public partial class GlobalData : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public GlobalData()
        {
        }

        private string SyncPageSize
        {
            get
            {
                if (ConfigurationManager.AppSettings["SyncPageSize"] == null)
                {
                    return "200";
                }
                else
                {
                    return ConfigurationManager.AppSettings["SyncPageSize"];
                }
            }
        }

        private string ConnString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnection"];
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "_spOriginalFormAction = document.forms[0].action;\n_spSuppressFormOnSubmitWrapper = true;";

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", script, true);
            if (!Page.IsPostBack)
            {
                InitData();
            }
        }
        protected void lbtnQuery_OnClick(object sender, EventArgs e)
        {
            AspNetPager1.Visible = true;
            this.AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            BindEntitySource();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');layer.closeAll();layer.closeLoad();", true);
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindEntitySource();
        }
        protected void lbtnSync_OnClick(object sender, EventArgs e)
        {


            //try
            //{
            //    GetAllData(ddlEntityList.SelectedItem.Text, ddlEntityList.SelectedValue.Split('|')[0]);
            //}
            //catch (Exception ex)
            //{
            //    BaseComponent.Error(ex.Message);
            //    if (ex.InnerException != null)
            //    {
            //        BaseComponent.Error(ex.InnerException.Message);
            //    }
            //}

            SPLongOperation.Begin(
    delegate(SPLongOperation longOperation)
    {
        // Do something that takes a long time to complete.
        Thread.Sleep(5000);

        foreach (ListItem item in this.ddlEntityList.Items)
        {

            if (!string.IsNullOrEmpty(item.Value) && item.Value != "0")
            {
                GetAllData(item.Text, item.Value.Split('|')[0]);
            }
            continue;

        }

        // Inform the server that the work is done
        // and that the page used to indicate progress
        // is no longer needed.
        longOperation.End("default.aspx");
    }
);


            //SPLongOperation longOperation = new SPLongOperation(this.Page);

            //// Provide the text displayd in bold
            //longOperation.LeadingHTML = "Long running operation is being performed";

            //// Provide the normal formatted text
            //longOperation.TrailingHTML = "Please wait while your request is being performed. This can take a couple of seconds.";

            //// Let's start the code that takes a while from here
            //longOperation.Begin();
            //try
            //{
            //    // The code that might take a while
            //    Thread.Sleep(5000);

            //    foreach (ListItem item in this.ddlEntityList.Items)
            //    {

            //        if (!string.IsNullOrEmpty(item.Value) && item.Value != "0")
            //        {
            //            GetAllData(item.Text, item.Value.Split('|')[0]);
            //        }
            //        continue;

            //    }
            //}
            //catch (ThreadAbortException)
            //{
            //    // Don't do anything, this error can occur because the SPLongOperation.End
            //    // performs a Response.Redirect internally and doesnt take into account that other code
            //    // might still be executed
            //}
            //catch (Exception ex)
            //{
            //    // When an exception occurs, the page is redirected to the error page.
            //    // Redirection to another (custom) page is also possible
            //    SPUtility.TransferToErrorPage(ex.ToString());
            //}
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sdel123", "layer.msg('同步完成',2);", true);
        }

        protected void lbtnExport_OnClick(object sender, EventArgs e)
        {
            string entityName = this.hdEntityName.Value;
            if (!string.IsNullOrEmpty(entityName))
            {
                string[] ss = this.hdEntityName.Value.Split('|');
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        string sql = "select * from " + ss[0] + " order by " + ss[1];
                        SqlHelper sqlHelper = new SqlHelper(ConnString);
                        DataTable dt = sqlHelper.ExecuteDataTable(sql);
                        string fileName = ss[0] + DateTime.Now.ToString("yyyyMMddHHss") + ".xlsx";
                        string configPath = "C:\\";
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExportFilePath"]))
                        {
                            configPath = ConfigurationManager.AppSettings["ExportFilePath"];
                        }
                        // 判定该路径是否存在
                        if (!Directory.Exists(configPath))
                            Directory.CreateDirectory(configPath);
                        string filePath = configPath + fileName;
                        EPPlus.CreateFile(filePath, dt, new List<string>(), new List<string>(), "", ss[0]);

                        System.IO.FileStream files = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                            FileShare.Read);
                        long filesize = files.Length;
                        byte[] byteFile = null;
                        if (files.Length == 0)
                        {
                            byteFile = new byte[1];
                        }
                        else
                        {
                            byteFile = new byte[files.Length];
                        }
                        files.Read(byteFile, 0, (int)byteFile.Length);
                        files.Close();

                        if (File.Exists(filePath))
                        {
                            //删除生成文件
                            File.Delete(filePath);
                        }

                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "loadfiletip",
                            "layer.closeAll();", true);

                        Page.Response.ContentType = "application/octet-stream";
                        Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                        Page.Response.AddHeader("Content-Length", filesize.ToString());
                        Page.Response.BinaryWrite(byteFile);
                        Page.Response.Flush();
                        Page.Response.Close();
                        //Page.Response.End();
                    }
                    catch (Exception ex)
                    {
                        BaseComponent.Error(ex.Message);
                        if (ex.InnerException != null)
                        {
                            BaseComponent.Error(ex.InnerException.Message);
                        }
                        throw;
                    }
                });

            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "loadfiletip",
                            "layer.closeAll();", true);
        }
        protected void GvDataSource_OnPreRender(object sender, EventArgs e)
        {
            if (GvDataSource.Rows.Count > 0)
            {
                // 使用<TH>替换<TD>
                //GvDataSource.UseAccessibleHeader = true;

                //This will add the <thead> and <tbody> elements
                //HeaderRow将被<thead>包裹，数据行将被<tbody>包裹
                // GvDataSource.HeaderRow.TableSection = TableRowSection.TableHeader;

                // FooterRow将被<tfoot>包裹
                //gvOfficeSuppliesDetailList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        private void InitData()
        {
            //init dropwownlist for entity name list
            //check list isexist
            #region  check list isexist and get entity list
            DataTable dt = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWeb webSite = null;
                if (SPContext.Current != null)
                {
                    webSite = SPContext.Current.Site.OpenWeb();
                    webSite.AllowUnsafeUpdates = true;
                    SPList list = null;
                    try
                    {
                        list = webSite.Lists["EntityList"];
                    }
                    catch (Exception ex)
                    {
                    }

                    if (list == null)
                    {
                        webSite.Lists.Add("EntityList", " entyty list ", SPListTemplateType.GenericList);
                        list = webSite.Lists["EntityList"];
                        list.Fields.Add("Description", SPFieldType.Note, false);
                        list.Fields.Add("Model", SPFieldType.Text, false);
                        list.Fields.Add("PrimaryKey", SPFieldType.Text, false);
                    }
                    list.Update();
                    webSite.Update();
                    webSite.AllowUnsafeUpdates = false;
                    dt = list.Items.GetDataTable();
                }

            });
            #endregion


            ddlEntityList.DataSource = dt;
            ddlEntityList.DataTextField = "Title";
            ddlEntityList.DataValueField = "PrimaryKey";
            ddlEntityList.DataBind();
            ddlEntityList.Items.Insert(0, new ListItem("请选择", "0"));

            this.AspNetPager1.RecordCount = 0;
            AspNetPager1.Visible = false;
        }
        private void BindEntitySource()
        {
            //string isEnableNet = ConfigurationManager.AppSettings["isEnableNet"];
            ////java
            //if (string.IsNullOrEmpty(isEnableNet))
            //{

            //    WebJavaReference.EDIESBService javaeEdiesbService = new WebJavaReference.EDIESBService();

            //    GlobalRequestEntity globalRequestEntity = new GlobalRequestEntity();
            //    globalRequestEntity.Type = "R";
            //    globalRequestEntity.Action = "TRANSFOR";
            //    globalRequestEntity.Model = "MDM_NBG";
            //    globalRequestEntity.Entity = this.ddlEntityList.SelectedItem.Text;
            //    globalRequestEntity.Pagesize = this.AspNetPager1.PageSize.ToString();
            //    globalRequestEntity.Page = this.AspNetPager1.CurrentPageIndex.ToString();
            //    globalRequestEntity.Username = Common.ApplyDataUserName;
            //    globalRequestEntity.Password = Common.ApplyDataUserPwd;
            //    string applyData = "[" + JsonConvert.SerializeObject(globalRequestEntity) + "]";

            //    string returnJson = javaeEdiesbService.callEDIESBPub("BLCTZS", "MDM", "mdm_publish", applyData, Common.EdiUserName, Common.EdiPwd);

            //    var model = JsonConvert.DeserializeObject<GlobalResultEntity>(returnJson);

            //    if (model != null)
            //    {
            //        GlobalResultEntity.Result2 result2 = model.Result;
            //        if (result2 != null)
            //        {
            //            this.AspNetPager1.RecordCount = result2.Total;

            //            GvDataSource.DataSource = result2.DataList;
            //            GvDataSource.DataBind();
            //        }
            //    }
            //}
            int total = 0;
            DataTable dt = Common.GetRecordFromPage(this.ddlEntityList.SelectedItem.Text,
                this.ddlEntityList.SelectedValue.Split('|')[0], this.AspNetPager1.CurrentPageIndex + 1, this.AspNetPager1.PageSize,
                out total);

            this.AspNetPager1.RecordCount = total;

            GvDataSource.DataSource = dt;
            GvDataSource.DataBind();
            ReCodePager();
            hdEntityName.Value = ddlEntityList.SelectedItem.Text + "|" + ddlEntityList.SelectedValue;
        }
        protected void ReCodePager()
        {
            if (AspNetPager1.RecordCount <= AspNetPager1.PageSize)
            {
                AspNetPager1.ShowFirstLast = false;
                AspNetPager1.ShowPageIndex = false;
                AspNetPager1.ShowPrevNext = false;
                AspNetPager1.ShowPageIndexBox = ShowPageIndexBox.Never;
            }
            else
            {
                AspNetPager1.ShowFirstLast = true;
                AspNetPager1.ShowPageIndex = true;
                AspNetPager1.ShowPrevNext = true;
                AspNetPager1.ShowPageIndexBox = ShowPageIndexBox.Always;
                if (AspNetPager1.CurrentPageIndex == 1) //第一页时不显示首页与上一页
                {
                    AspNetPager1.FirstPageText = "";
                    AspNetPager1.PrevPageText = "";
                }
                else
                {
                    AspNetPager1.FirstPageText = "首页";
                    AspNetPager1.PrevPageText = "上一页";
                }
                if (AspNetPager1.PageCount == AspNetPager1.CurrentPageIndex)
                {
                    AspNetPager1.LastPageText = "";
                    AspNetPager1.NextPageText = "";
                }
                else
                {
                    AspNetPager1.LastPageText = "尾页";
                    AspNetPager1.NextPageText = "下一页";
                }
            }
        }
        private void GetAllData(string entityName, string key, int page = 1)
        {
            string isEnableNet = ConfigurationManager.AppSettings["isEnableNet"];
            //java
            if (string.IsNullOrEmpty(isEnableNet))
            {

                WebJavaReference.EDIESBService javaeEdiesbService = new WebJavaReference.EDIESBService();

                GlobalRequestEntity globalRequestEntity = new GlobalRequestEntity();
                globalRequestEntity.Type = "R";
                globalRequestEntity.Action = "TRANSFOR";
                globalRequestEntity.Model = "MDM_NBG";
                globalRequestEntity.Entity = entityName;
                globalRequestEntity.Pagesize = SyncPageSize;
                globalRequestEntity.Page = page.ToString();
                globalRequestEntity.Username = Common.ApplyDataUserName;
                globalRequestEntity.Password = Common.ApplyDataUserPwd;
                string applyData = "[" + JsonConvert.SerializeObject(globalRequestEntity) + "]";

                string returnJson = javaeEdiesbService.callEDIESBPub("BLCTZS", "MDM", "mdm_publish", applyData, Common.EdiUserName, Common.EdiPwd);

                var model = JsonConvert.DeserializeObject<GlobalResultEntity>(returnJson);

                if (model != null)
                {
                    GlobalResultEntity.Result2 result2 = model.Result;
                    if (result2 != null)
                    {
                        DataTable dt = result2.DataList;
                        if (dt != null)
                        {
                            SyncDataToSqlDb(entityName, key, dt);

                            if (result2.IsMore == "Y" && result2.Total != 0)
                            {
                                page++;
                                GetAllData(entityName, key, page);
                            }
                        }

                    }
                }
            }
        }
        private void SyncDataToSqlDb(string entity, string key, DataTable dt)
        {

            SqlHelper sqlHelper = new SqlHelper(ConnString);
            int ii = 0;
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter sqlParameter = null;
            foreach (DataRow dr in dt.Rows)
            {
                //check isexist
                string sql = "select count(1) from {0} where {1}='{2}'";
                sql = string.Format(sql, entity, key, dr[key]);
                object oo = sqlHelper.ExecuteScalar(sql);

                StringBuilder strSqlColumns = new StringBuilder();
                StringBuilder strSqlColumnsVlues = new StringBuilder();
                StringBuilder strText = new StringBuilder();

                if (Convert.ToInt32(oo) == 0)
                {
                    string sqlIn = "insert into {0}({1}) values({2});";
                    string sqlColmuns = "";
                    string sqlValues = "";
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        if (dataColumn.ColumnName != "INSERT_TIME" && dataColumn.ColumnName != "UPDATE_TIME" && !string.IsNullOrEmpty(Convert.ToString(dr[dataColumn.ColumnName])))
                        {
                            strSqlColumns.Append("," + dataColumn.ColumnName);
                            // strSqlColumnsVlues.Append(",'" + Convert.ToString(dr[dataColumn.ColumnName]).Replace("'","''") + "'");
                            strSqlColumnsVlues.Append(",@" + dataColumn.ColumnName + "");
                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = "@" + dataColumn.ColumnName;
                            sqlParameter.Value = Convert.ToString(dr[dataColumn.ColumnName]);
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    if (strSqlColumns.ToString().Length > 0)
                    {
                        sqlColmuns = strSqlColumns.ToString().Substring(1);
                        sqlValues = strSqlColumnsVlues.ToString().Substring(1);
                    }
                    sqlIn = string.Format(sqlIn, entity, sqlColmuns, sqlValues);
                    strText.Append(sqlIn);
                }
                else
                {
                    string sqlUp = "update {0} set {1} where {2}='{3}';";
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        if (dataColumn.ColumnName != "INSERT_TIME" && dataColumn.ColumnName != "UPDATE_TIME" && !string.IsNullOrEmpty(Convert.ToString(dr[dataColumn.ColumnName])))
                        {
                            //strSqlColumns.Append("," + dataColumn.ColumnName + "='" + Convert.ToString(dr[dataColumn.ColumnName]).Replace("'", "''") + "'");
                            strSqlColumns.Append("," + dataColumn.ColumnName + "=@" + dataColumn.ColumnName + "");

                            sqlParameter = new SqlParameter();
                            sqlParameter.ParameterName = "@" + dataColumn.ColumnName;
                            sqlParameter.Value = Convert.ToString(dr[dataColumn.ColumnName]);
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    string sqlColmuns = "";
                    if (strSqlColumns.ToString().Length > 0)
                    {
                        sqlColmuns = strSqlColumns.ToString().Substring(1);
                    }
                    sqlUp = string.Format(sqlUp, entity, sqlColmuns, key, dr[key]);
                    strText.Append(sqlUp);
                }
                try
                {
                    int ren = sqlHelper.ExecuteNonQuery(strText.ToString(), sqlCommand);
                    strSqlColumns.Clear();
                    strSqlColumnsVlues.Clear();
                    strText.Clear();
                }
                catch (Exception ex)
                {
                    BaseComponent.Error(ex.Message);
                    BaseComponent.Info(strText.ToString());
                    if (ex.InnerException != null)
                    {
                        BaseComponent.Error(ex.InnerException.Message);
                    }
                    if (ex.Message.Contains("正在中止线程") || ex.Message.Contains("Apploication_ErrorID"))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "loadfiletips",
                            "layer.closeAll();", true);
                    }
                    strText.Clear();
                    sqlCommand.Parameters.Clear();
                    continue;
                }

            }
        }

    }
}

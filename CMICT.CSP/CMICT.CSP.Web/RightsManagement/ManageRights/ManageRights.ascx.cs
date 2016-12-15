using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.RightsManagement.ManageRights
{
    [ToolboxItemAttribute(false)]
    public partial class ManageRights : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ManageRights()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //hidpagesize.Value = "10";
                hidTopName.Value = ConfigurationSettings.AppSettings["OrganizationTopName"].ToString();

                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
                BindDataSourceList();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.load('查询中,请稍后...');", true);
            BindDataSourceList();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.closeAll();", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }

        protected void BindDataSourceList()
        {
            try
            {
                AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
                int recordCount;
                string pageName = txtPageName.Text.Trim();
                string useCompany = txtUseCompany.Text.Trim();
                string creater = txtCreater.Text.Trim();
                int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
                int PageSize = AspNetPager1.PageSize;

                RightsManagementComponent rightBll = new RightsManagementComponent();
                DataTable dt = rightBll.GetAllPages(SPContext.Current.Site.Url, out recordCount, pageName, useCompany, creater, AspNetPager1.CurrentPageIndex, PageSize);

                ViewState["AuthorationDataSource"] = dt;

                if (recordCount == 0)
                {
                    lblnodata.Visible = true;
                }
                else
                {
                    lblnodata.Visible = false;
                }

                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.RecordCount = recordCount;
                DataSourceList.DataSource = dt;
                DataSourceList.DataBind();

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setAllsel", "BindSelectAllEvent();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsel", "BindEachCboEvent();", true);
            }
            catch (Exception ex)
            {
                BaseComponent.Error("授权首页数据绑定： " + ex.Message + "--------" + ex.StackTrace);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataSourceList();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.closeAll();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            SyncADInfoComponent sync = new SyncADInfoComponent();
            sync.SyncADInfo(SPContext.Current.Site.Url);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.closeAll();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setAllsel", "BindSelectAllEvent();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsel", "BindEachCboEvent();", true);
        }

        protected void DataSourceList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                LinkButton lkbtnSort = (LinkButton)e.Item.FindControl(e.CommandName.Trim());
                if (ViewState[e.CommandName.Trim()] == null)
                {
                    ViewState[e.CommandName.Trim()] = "ASC";
                    lkbtnSort.Text = lkbtnSort.Text + "▲";
                }
                else
                {
                    if (ViewState[e.CommandName.Trim()].ToString().Trim() == "ASC")
                    {
                        ViewState[e.CommandName.Trim()] = "DESC";
                        if (lkbtnSort.Text.IndexOf("▲") != -1)
                            lkbtnSort.Text = lkbtnSort.Text.Replace("▲", "▼");
                        else
                            lkbtnSort.Text = lkbtnSort.Text + "▼";
                    }
                    else
                    {
                        ViewState[e.CommandName.Trim()] = "ASC";
                        if (lkbtnSort.Text.IndexOf("▼") != -1)
                            lkbtnSort.Text = lkbtnSort.Text.Trim().Replace("▼", "▲");
                        else
                            lkbtnSort.Text = lkbtnSort.Text + "▲";
                    }
                }
                ViewState["text"] = lkbtnSort.Text;
                ViewState["id"] = e.CommandName.Trim();
                string orderby = e.CommandName.ToString().Trim() + " " + ViewState[e.CommandName.Trim()].ToString().Trim();

                if (ViewState["AuthorationDataSource"] == null)
                {
                    BindDataSourceList();
                }
                else
                {
                    DataTable dtSource = (DataTable)ViewState["AuthorationDataSource"];

                    if (dtSource != null || dtSource.Rows.Count > 0)
                    {
                        DataView view = dtSource.DefaultView;
                        view.Sort = orderby;
                        DataTable dtResult = view.ToTable();

                        AspNetPager1.CurrentPageIndex = 1;
                        AspNetPager1.RecordCount = dtResult.Rows.Count;
                        DataSourceList.DataSource = dtResult;
                        DataSourceList.DataBind();
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.closeAll();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setAllsel", "BindSelectAllEvent();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsel", "BindEachCboEvent();", true);
            }
        }

        protected void DataSourceList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (ViewState["id"] != null)
                {
                    LinkButton lkbtnSort = (LinkButton)e.Item.FindControl(ViewState["id"].ToString().Trim());
                    lkbtnSort.Text = ViewState["text"].ToString();
                }
            }
        }

    }
}

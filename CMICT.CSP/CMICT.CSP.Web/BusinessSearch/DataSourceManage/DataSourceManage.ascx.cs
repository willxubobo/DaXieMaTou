using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Wuqi.Webdiyer;

namespace CMICT.CSP.Web.BusinessSearch.DataSourceManage
{
    [ToolboxItemAttribute(false)]
    public partial class DataSourceManage : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DataSourceManage()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        //BaseComponent bc = new BaseComponent();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindBigCategory();
                BindObjectType();
                BindStatus();
                AspNetPager1.CurrentPageIndex = 1;
                //AspNetPager1.NumericButtonCount = int.MaxValue;
                BindDataSourceList();
            }
        }
        //绑定报表大类
        protected void BindBigCategory()
        {
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("数据源");
            ddlCATEGORY.DataSource = dt;
            ddlCATEGORY.DataTextField = "LOOKUP_NAME";
            ddlCATEGORY.DataValueField = "LOOKUP_CODE";
            ddlCATEGORY.DataBind();
            ddlCATEGORY.Items.Insert(0, new ListItem("全部", "all"));
            ddlsmallcategory.Items.Insert(0, new ListItem("全部", "all"));
        }

        //绑定报表细类
        protected void BindSmallCategory(string bigcode)
        {
            DataTable dt = BaseComponent.GetUserLookupValuesByType(bigcode);
            ddlsmallcategory.DataSource = dt;
            ddlsmallcategory.DataTextField = "LOOKUP_VALUE_NAME";
            ddlsmallcategory.DataValueField = "LOOKUP_VALUE";
            ddlsmallcategory.DataBind();
            ddlsmallcategory.Items.Insert(0, new ListItem("全部", "all"));
        }
        protected void ddlCATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scategory = ddlCATEGORY.SelectedValue;
            if (!string.IsNullOrEmpty(scategory))
            {
                BindSmallCategory(scategory);
            }
        }
        //绑定类型
        protected void BindObjectType()
        {
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_OBJECT_TYPE");
            ddlDataType.DataSource = dt;
            ddlDataType.DataTextField = "LOOKUP_VALUE_NAME";
            ddlDataType.DataValueField = "LOOKUP_VALUE";
            ddlDataType.DataBind();
            ddlDataType.Items.Insert(0, new ListItem("全部", "all"));
            ddlDataType.SelectedValue = "TABLE";
        }
        //绑定状态
        protected void BindStatus()
        {
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_SOURCE_STATUS");
            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "LOOKUP_VALUE_NAME";
            ddlStatus.DataValueField = "LOOKUP_VALUE";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("全部", "all"));
            ddlStatus.SelectedValue = "ENABLE";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hidisdefault.Value = "search";
            AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            BindDataSourceList();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }
        //删除
        protected void lbtndel_Click(object sender, EventArgs e)
        {
            string SourceID = hidsourceid.Value.Trim();

            if (!string.IsNullOrEmpty(SourceID))
            {
                BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
                if (bll.Delete(Guid.Parse(SourceID)))
                {
                    hidsourceid.Value = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "dsdelsuc", "layer.alert('数据源删除成功！',9);", true);
                    BindDataSourceList();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "dsdelerror", "layer.alert('数据源删除失败！',8);", true);
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindDataSourceList();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }

        protected void BindDataSourceList()
        {
            ViewState["id"] = null;
            int i, j;
            string SourceName = txtSourceName.Text.Trim();
            ViewState["txtSourceName"] = SourceName;
            string SourceIP = txtSourceIP.Text.Trim();
            ViewState["txtSourceIP"] = SourceIP;
            string DBName = txtDBName.Text.Trim();
            ViewState["txtDBName"] = DBName;
            string ObjectType = "";
            if (!string.IsNullOrWhiteSpace(hidisdefault.Value))
            {
                ObjectType = (ddlDataType.SelectedValue == "all" ? "" : ddlDataType.SelectedValue);
            }
            ViewState["ddlDataType"] = ObjectType;
            string ObjectName = txtObjectName.Text;
            ViewState["txtObjectName"] = ObjectName;
            string SourceStatus = "";
            if (!string.IsNullOrWhiteSpace(hidisdefault.Value))
            {
                SourceStatus = (ddlStatus.SelectedValue == "all" ? "" : ddlStatus.SelectedValue);
            }
            ViewState["ddlStatus"] = SourceStatus;
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            string bigcate = (ddlCATEGORY.SelectedValue == "all" ? "" : ddlCATEGORY.SelectedValue);
            ViewState["ddlCATEGORY"] = bigcate;
            string smallcate = (ddlsmallcategory.SelectedValue == "all" ? "" : ddlsmallcategory.SelectedValue);
            ViewState["ddlsmallcategory"] = smallcate;
            DataSourceConfigComponent tmbll = new DataSourceConfigComponent();
            DataTable dt = tmbll.GetDataSourceList(SourceName, SourceIP, DBName, ObjectType, ObjectName, SourceStatus,bigcate,smallcate,"", AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            AspNetPager1.RecordCount = i;
            DataSourceList.DataSource = dt;
            DataSourceList.DataBind();
            if (dt == null || dt.Rows.Count == 0)
            {
                lblnodata.Visible = true;
            }
            else
            {
                lblnodata.Visible = false;
            }
            ReCodePager();
            //ViewState["TemplateList"] = dt;
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

        protected void BindDataSourceListByOrderBy(string orderby)
        {
            int i, j;
            string SourceName = (ViewState["txtSourceName"]==null?txtSourceName.Text.Trim():ViewState["txtSourceName"].ToString());
            string SourceIP = (ViewState["txtSourceIP"]==null?txtSourceIP.Text.Trim():ViewState["txtSourceIP"].ToString());
            string DBName = (ViewState["txtDBName"]==null?txtDBName.Text.Trim():ViewState["txtDBName"].ToString());
            string ObjectType = (ViewState["ddlDataType"]==null?ddlDataType.SelectedValue:ViewState["ddlDataType"].ToString());
            string ObjectName = (ViewState["txtObjectName"]==null?txtObjectName.Text.Trim():ViewState["txtObjectName"].ToString());
            string SourceStatus = (ViewState["ddlStatus"]==null?ddlStatus.SelectedValue:ViewState["ddlStatus"].ToString());
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            string bigcate = (ViewState["ddlCATEGORY"]==null?ddlCATEGORY.SelectedValue:ViewState["ddlCATEGORY"].ToString());
            string smallcate = (ViewState["ddlsmallcategory"]==null?ddlsmallcategory.SelectedValue:ViewState["ddlsmallcategory"].ToString());
            DataSourceConfigComponent tmbll = new DataSourceConfigComponent();
            DataTable dt = tmbll.GetDataSourceList(SourceName, SourceIP, DBName, ObjectType, ObjectName, SourceStatus, bigcate, smallcate, orderby, AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            AspNetPager1.RecordCount = i;
            DataSourceList.DataSource = dt;
            DataSourceList.DataBind();
            //ViewState["TemplateList"] = dt;
            ReCodePager();
        }
        /// <summary>
        /// 获取数据源对应的模板信息
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public string GetTemplateInfoBySourceID(string SourceID)
        {
            string pinfo = string.Empty;
            if (!string.IsNullOrEmpty(SourceID))
            {
                DataSourceConfigComponent pbll = new DataSourceConfigComponent();
                DataTable dt = pbll.GetTemplateListBySourceID(SourceID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pinfo += dr["TemplateName"].ToString() + ",";
                    }
                }
            }
            return pinfo.TrimEnd(',');
        }
        protected void TemplateList_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                string orderby= e.CommandName.ToString().Trim() + " " + ViewState[e.CommandName.Trim()].ToString().Trim();
                BindDataSourceListByOrderBy(orderby);
                
            }
        }

        protected void TemplateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

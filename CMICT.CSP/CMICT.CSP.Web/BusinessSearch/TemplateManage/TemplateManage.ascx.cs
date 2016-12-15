using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Wuqi.Webdiyer;

namespace CMICT.CSP.Web.BusinessSearch.TemplateManage
{
    [ToolboxItemAttribute(false)]
    public partial class TemplateManage : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TemplateManage()
        {
        }
        TemplateManageComponent tmbll = new TemplateManageComponent();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindBigCategory();
                BindStatus();
                AspNetPager1.CurrentPageIndex = 1;
                //AspNetPager1.NumericButtonCount = int.MaxValue;
                BindTemplateList();
            }
        }

        //绑定报表大类
        protected void BindBigCategory()
        {
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("模板");
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
        //删除
        protected void lbtndel_Click(object sender, EventArgs e)
        {
            try
            {
                string TemplateID = hidtemplateid.Value.Trim();
                string SourceID = hidsourceid.Value.Trim();
                if (!string.IsNullOrEmpty(TemplateID))
                {
                    //删除通信配置
                    CommunicationConfigComponent cccbll = new CommunicationConfigComponent();
                    cccbll.DeleteCommunicationByTemplateID(TemplateID);
                    if (tmbll.DelTemplateInfo(TemplateID))
                    {
                        hidtemplateid.Value = "";
                        if (!string.IsNullOrEmpty(SourceID))
                        {
                            //删除后查询原数据源是否还有被其它模板引用，没有则更新状态为空闲
                            ConnectionConfigComponent ccc = new ConnectionConfigComponent();
                            DataTable dstable = ccc.GetTemplateIDBySourceID(Guid.Parse(SourceID));
                            if (dstable == null || dstable.Rows.Count == 0)
                            {
                                DataSourceConfigComponent dbll = new DataSourceConfigComponent();
                                dbll.UpdateSourceStatusBySourceID(hidsourceid.Value.Trim());
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sdel", "layer.alert('模板删除成功！',9);", true);
                        BindTemplateList();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sdel", "layer.alert('模板删除失败！',8);", true);
                    }
                }
            }
            catch (Exception ee)
            {
                BaseComponent.Error(ee.Message);
            }
        }
        //复制
        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                string TemplateID = hidtemplateid.Value.Trim();
                string CopyNewName = hidCopyName.Value.Trim();
                if (!string.IsNullOrEmpty(TemplateID) && !string.IsNullOrEmpty(CopyNewName))
                {
                    Guid temid = Guid.Parse(TemplateID);
                    //复制templatemain表
                    BS_TEMPLATE_MAINBLL tmain = new BS_TEMPLATE_MAINBLL();
                    BS_TEMPLATE_MAIN tmodel = tmain.GetModel(temid);
                    tmodel.TemplateName = CopyNewName;
                    tmodel.Created = DateTime.Now;
                    tmodel.Modified = DateTime.Now;
                    tmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                    tmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                    tmodel.TemplateStatus = "FREE";
                    string newtemplateid = tmain.Add(tmodel);
                    if (!string.IsNullOrEmpty(newtemplateid))
                    {
                        DisplayConfigComponent dcc = new DisplayConfigComponent();
                        #region 复制表
                        Guid newtemplateguid = Guid.Parse(newtemplateid);
                        //复制bscompare表
                        DataTable dt = dcc.GetCalListByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_COMPUTEBLL cmain = new BS_COMPUTEBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_COMPUTE cmodel = cmain.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (cmodel != null)
                                {
                                    cmodel.TemplateID = newtemplateguid;
                                    cmodel.Created = DateTime.Now;
                                    cmodel.Modified = DateTime.Now;
                                    cmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    cmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    cmain.Add(cmodel);
                                }
                            }
                        }
                        //复制BS_CUSTOM_QUERY表
                        dt = tmbll.GetBS_CUSTOM_QUERYIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_CUSTOM_QUERYBLL cbll = new BS_CUSTOM_QUERYBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_CUSTOM_QUERY cqmodel = cbll.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (cqmodel != null)
                                {
                                    cqmodel.TemplateID = newtemplateguid;
                                    cqmodel.Created = DateTime.Now;
                                    cqmodel.Modified = DateTime.Now;
                                    cqmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    cqmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    cbll.Add(cqmodel);
                                }
                            }
                        }
                        //复制BS_CUSTOM_QUERY表
                        dt = tmbll.GetBS_DEFAULT_QUERYIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_DEFAULT_QUERYBLL dbll = new BS_DEFAULT_QUERYBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_DEFAULT_QUERY dmodel = dbll.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (dmodel != null)
                                {
                                    dmodel.TemplateID = newtemplateguid;
                                    dmodel.Created = DateTime.Now;
                                    dmodel.Modified = DateTime.Now;
                                    dmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    dmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    dbll.Add(dmodel);
                                }
                            }
                        }
                        //复制BS_GROUPBY表
                        dt = tmbll.GetBS_GROUPBYIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_GROUPBYBLL gbll = new BS_GROUPBYBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_GROUPBY gmodel = gbll.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (gmodel != null)
                                {
                                    gmodel.TemplateID = newtemplateguid;
                                    gmodel.Created = DateTime.Now;
                                    gmodel.Modified = DateTime.Now;
                                    gmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    gmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    gbll.Add(gmodel);
                                }
                            }
                        }
                        //复制BS_TEMPLATE_COLUMNS表
                        dt = tmbll.GetBS_TEMPLATE_COLUMNSIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_TEMPLATE_COLUMNSBLL tbll = new BS_TEMPLATE_COLUMNSBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_TEMPLATE_COLUMNS tcmodel = tbll.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (tcmodel != null)
                                {
                                    tcmodel.TemplateID = newtemplateguid;
                                    tcmodel.Created = DateTime.Now;
                                    tcmodel.Modified = DateTime.Now;
                                    tcmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    tcmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    tbll.Add(tcmodel);
                                }
                            }
                        }

                        //复制BS_TEMPLATE_SORT表
                        dt = tmbll.GetBS_TEMPLATE_SORTIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_TEMPLATE_SORTBLL sbll = new BS_TEMPLATE_SORTBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BS_TEMPLATE_SORT smodel = sbll.GetModel(Guid.Parse(dr["id"].ToString()));
                                if (smodel != null)
                                {
                                    smodel.TemplateID = newtemplateguid;
                                    smodel.Created = DateTime.Now;
                                    smodel.Modified = DateTime.Now;
                                    smodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    smodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    sbll.Add(smodel);
                                }
                            }
                        }
                        //复制BS_COMMUNICATION_MAIN表
                        dt = tmbll.GetBS_COMMUNICATION_MAINIDByTemplateID(temid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BS_COMMUNICATION_MAINBLL sbll = new BS_COMMUNICATION_MAINBLL();
                            BS_COMMUNICATION_DETAILBLL detailbll = new BS_COMMUNICATION_DETAILBLL();
                            foreach (DataRow dr in dt.Rows)
                            {
                                Guid commid = Guid.NewGuid();
                                BS_COMMUNICATION_MAIN smodel = sbll.GetModel(Guid.Parse(dr["CommunicationID"].ToString()));
                                if (smodel != null)
                                {
                                    smodel.CommunicationID = commid;
                                    smodel.TargetTemplateID = newtemplateguid;
                                    smodel.Created = DateTime.Now;
                                    smodel.Modified = DateTime.Now;
                                    smodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                    smodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                    sbll.Add(smodel);
                                }
                                DataTable dtdetail = tmbll.GetBS_COMMUNICATION_DETAILByID(Guid.Parse(dr["CommunicationID"].ToString()));
                                if (dtdetail != null && dtdetail.Rows.Count > 0)
                                {
                                    foreach (DataRow drd in dtdetail.Rows)
                                    {
                                        BS_COMMUNICATION_DETAIL sdmodel = detailbll.GetModel(Guid.Parse(drd["id"].ToString()));
                                        if (smodel != null)
                                        {
                                            sdmodel.ID = Guid.NewGuid();
                                            sdmodel.CommunicationID = commid;
                                            sdmodel.Created = DateTime.Now;
                                            sdmodel.Modified = DateTime.Now;
                                            sdmodel.Author = BaseWebPart.GetCurrentUserLoginId();
                                            sdmodel.Editor = BaseWebPart.GetCurrentUserLoginId();
                                            detailbll.Add(sdmodel);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sucadd", "layer.alert('模板复制成功！',9);", true);
                        BindTemplateList();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "erroradd", "layer.alert('模板复制失败！',8);", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "errora", "layer.alert('未获取到模板源信息，无法复制！',8);", true);
                }
            }
            catch (Exception ee)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "erroradd", "layer.alert('模板复制失败！',8);", true);
                BaseComponent.Error(ee.Message);
            }
        }
        //禁用
        protected void btnenable_Click(object sender, EventArgs e)
        {
            string TemplateID = hidtemplateid.Value.Trim();
            if (!string.IsNullOrEmpty(TemplateID))
            {
                if (tmbll.EnableTemplateInfo(TemplateID))
                {
                    //刷新模板缓存
                    BusinessSearchComponent bsbll = new BusinessSearchComponent();
                    bsbll.RefreshTemplateByGuid(TemplateID);
                    hidtemplateid.Value = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "senable", "layer.alert('模板禁用成功！',9);", true);
                    BindTemplateList();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "senable", "layer.alert('模板禁用失败！',8);", true);
                }
            }
        }
        protected void BindStatus()
        {
            //BaseComponent bc = new BaseComponent();
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_TEMPLATE_STATUS");
            ddlStatus.DataSource = dt;
            ddlStatus.DataTextField = "LOOKUP_VALUE_NAME";
            ddlStatus.DataValueField = "LOOKUP_VALUE";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("全部", "all"));
            //ddlStatus.SelectedValue = "ENABLE";
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hidisdefault.Value = "search";
            AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            BindTemplateList();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindTemplateList();
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

        protected void BindTemplateList()
        {
            ViewState["id"] = null;
            int i, j;
            string TemplateName = txtTemplateName.Text.Trim();
            ViewState["txtTemplateName"] = TemplateName;
            string StartDate = txtStartDate.Text.Trim();
            string EndDate = txtEndDate.Text.Trim();
            string Author = txtAuthor.Text.Trim();
            ViewState["txtAuthor"] = Author;
            string TemplateStatus = "";
            if (!string.IsNullOrWhiteSpace(hidisdefault.Value))
            {
                TemplateStatus = (ddlStatus.SelectedValue == "all" ? "" : ddlStatus.SelectedValue);
            }
            ViewState["ddlStatus"] = TemplateStatus;
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            string bigcate = (ddlCATEGORY.SelectedValue == "all" ? "" : ddlCATEGORY.SelectedValue);
            ViewState["ddlCATEGORY"] = bigcate;
            string smallcate = (ddlsmallcategory.SelectedValue == "all" ? "" : ddlsmallcategory.SelectedValue);
            ViewState["ddlsmallcategory"] = smallcate;
            DataTable dt = tmbll.GetTemplateList(TemplateName, bigcate, smallcate, Author, TemplateStatus, "", AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("bs_row_num", Type.GetType("System.String"));
                dt.Columns.Add("bigcate", Type.GetType("System.String"));
                dt.Columns.Add("smallcate", Type.GetType("System.String"));
                dt.Columns.Add("statusname", Type.GetType("System.String"));
                dt.Columns.Add("pagelist", Type.GetType("System.String"));
                int pindex = (AspNetPager1.CurrentPageIndex - 1) * PageSize;
                DataTable bigdata = null;
                DataTable statusdata = null;
                DataTable pagelist = null;
                DataTable smalldata = null;
                if (ViewState["bigcate"] != null)
                {
                    bigdata = ViewState["bigcate"] as DataTable;
                    smalldata = ViewState["smalldata"] as DataTable;
                    statusdata = ViewState["statusdata"] as DataTable;
                    pagelist = ViewState["pagelist"] as DataTable;
                }
                else
                {
                    bigdata = BaseComponent.GetBigCateData("/Lists/USER_LOOKUP_TYPES");
                     smalldata = BaseComponent.GetBigCateData("/Lists/USER_LOOKUP_VALUES");
                    statusdata = BaseComponent.GetBigCateData("/Lists/SYS_LOOKUP_VALUES");
                    TemplateManageComponent pbll = new TemplateManageComponent();
                    pagelist = pbll.GetPageList();
                    ViewState["bigcate"] = bigdata;
                    ViewState["smalldata"] = smalldata;
                    ViewState["statusdata"] = statusdata;
                    ViewState["pagelist"] = pagelist;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    pindex++;
                    dr["bs_row_num"] = pindex.ToString();
                    dr["bigcate"] = BaseComponent.GetBigCateName(bigdata, dr["BigCategory"].ToString());
                    dr["smallcate"] = BaseComponent.GetSmallCateName(smalldata, "模板-"+dr["BigCategory"].ToString(), dr["SmallCategory"].ToString());
                    dr["statusname"] = BaseComponent.GetStatusName(statusdata, "BS_TEMPLATE_STATUS", Convert.ToString(dr["TemplateStatus"]));
                    DataRow[] drs = pagelist.Select("Templateid='" + dr["Templateid"].ToString() + "'");
                    string pinfo = string.Empty;
                    if (drs != null)
                    {
                        foreach (DataRow dd in drs)
                        {
                            pinfo += "<a href='" + dd["Url"].ToString() + "' target='_blank' class='operateLink'>" + dd["PageName"].ToString() + "</a><br/>";
                        }
                    }
                    dr["pagelist"] = pinfo;
                }
            }
            AspNetPager1.RecordCount = i;
            TemplateList.DataSource = dt;
            TemplateList.DataBind();
            if (dt == null || dt.Rows.Count == 0)
            {
                lblnodata.Visible = true;
            }
            else
            {
                lblnodata.Visible = false;
            }
            //ViewState["TemplateList"] = dt;
            ReCodePager();
        }

        //排序获取datatable
        protected void GetTemplateListByOrderBy(string orderby)
        {
            int i, j;
            string TemplateName = (ViewState["txtTemplateName"] == null ? txtTemplateName.Text.Trim() : ViewState["txtTemplateName"].ToString());
            //string StartDate = (ViewState["TemplateName"] == null ? txtTemplateName.Text.Trim() : ViewState["TemplateName"].ToString()); txtStartDate.Text.Trim();
            //string EndDate = (ViewState["TemplateName"] == null ? txtTemplateName.Text.Trim() : ViewState["TemplateName"].ToString()); txtEndDate.Text.Trim();
            string Author = (ViewState["txtAuthor"] == null ? txtAuthor.Text.Trim() : ViewState["txtAuthor"].ToString());
            string TemplateStatus = (ViewState["ddlStatus"] == null ? ddlStatus.SelectedValue : ViewState["ddlStatus"].ToString());
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            string bigcate = (ViewState["ddlCATEGORY"] == null ? ddlCATEGORY.SelectedValue : ViewState["ddlCATEGORY"].ToString());
            string smallcate = (ViewState["ddlsmallcategory"] == null ? ddlsmallcategory.SelectedValue : ViewState["ddlsmallcategory"].ToString());
            DataTable dt = tmbll.GetTemplateList(TemplateName, bigcate, smallcate, Author, TemplateStatus, orderby, AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("bs_row_num", Type.GetType("System.String"));
                int pindex = (AspNetPager1.CurrentPageIndex - 1) * PageSize;
                foreach (DataRow dr in dt.Rows)
                {
                    pindex++;
                    dr["bs_row_num"] = pindex.ToString();
                }
            }
            AspNetPager1.RecordCount = i;
            TemplateList.DataSource = dt;
            TemplateList.DataBind();
            ReCodePager();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public string GetPageInfoByTemplateID(string TemplateID)
        {
            string pinfo = string.Empty;
            if (!string.IsNullOrEmpty(TemplateID))
            {
                TemplateManageComponent pbll = new TemplateManageComponent();
                DataTable dt = pbll.GetListByTemplateID(TemplateID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pinfo += "<a href='" + dr["Url"].ToString() + "' target='_blank' class='operateLink'>" + dr["PageName"].ToString() + "</a><br/>";
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
                string orderby = e.CommandName.ToString().Trim() + " " + ViewState[e.CommandName.Trim()].ToString().Trim();
                GetTemplateListByOrderBy(orderby);

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
        //启用
        protected void btnuse_Click(object sender, EventArgs e)
        {
            try
            {
                string TemplateID = hidtemplateid.Value.Trim();
                if (!string.IsNullOrEmpty(TemplateID))
                {
                    QueryConfigComponent qcc = new QueryConfigComponent();
                    DataTable qdt = qcc.GetUserQueryColListByTemplateID(Guid.Parse(TemplateID));
                    if (qdt != null && qdt.Rows.Count > 0)
                    {
                        string tstatus = "FREE";
                        TemplateManageComponent tcc = new TemplateManageComponent();
                        DataTable dt = tcc.GetListByTemplateID(TemplateID);
                        if (dt != null && dt.Rows.Count > 0)//判断有无页面
                        {
                            tstatus = "ENABLE";
                        }
                        if (tmbll.EnableTemplateInfo(TemplateID, tstatus))
                        {
                            //刷新模板缓存
                            BusinessSearchComponent bsbll = new BusinessSearchComponent();
                            bsbll.RefreshTemplateByGuid(TemplateID);
                            hidtemplateid.Value = "";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suseenable", "layer.alert('模板启用成功！',9);", true);
                            BindTemplateList();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suseenable", "layer.alert('模板启用失败！',8);", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suseenablerror", "layer.alert('模板未配置完成，无法启用！',8);", true);
                    }
                }
            }
            catch (Exception ee)
            {
                BaseComponent.Error(ee.Message);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suseenablcatch", "layer.alert('模板启用出错！',8);", true);
            }
        }
    }
}

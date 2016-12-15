using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.TemplateInfoConfig
{
    [ToolboxItemAttribute(false)]
    public partial class TemplateInfoConfig : BaseWebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TemplateInfoConfig()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
       // BaseComponent bc = new BaseComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPageSize();
                BindBigCategory();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                {
                    hidsourceid.Value = Page.Request.QueryString["sourceID"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["templateID"]))
                {
                    hidid.Value = Page.Request.QueryString["templateID"];
                    //trpage.Visible = true;
                    hidOperType.Value = "edit";
                    Guid TemplateID = Guid.Parse(Page.Request.QueryString["templateID"]);
                    ShowInfo(TemplateID);
                }
                else
                {
                    hidOperType.Value = "add";
                }
            }
        }
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
        //绑定报表大类
        protected void BindBigCategory()
        {
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("模板");
            ddlCATEGORY.DataSource = dt;
            ddlCATEGORY.DataTextField = "LOOKUP_NAME";
            ddlCATEGORY.DataValueField = "LOOKUP_CODE";
            ddlCATEGORY.DataBind();
            ddlCATEGORY.Items.Insert(0, new ListItem("请选择", ""));
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
        }

        //绑定报表细类
        protected void BindSmallCategory(string bigcode)
        {
            DataTable dt = BaseComponent.GetUserLookupValuesByType(bigcode);
            ddlsmallcategory.DataSource = dt;
            ddlsmallcategory.DataTextField = "LOOKUP_VALUE_NAME";
            ddlsmallcategory.DataValueField = "LOOKUP_VALUE";
            ddlsmallcategory.DataBind();
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
        }
        //绑定pagesize
        protected void BindPageSize()
        {
            //BaseComponent bc = new BaseComponent();
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_PAGE_SIZE");
            ddlPageSize.DataSource = dt;
            ddlPageSize.DataTextField = "LOOKUP_VALUE_NAME";
            ddlPageSize.DataValueField = "LOOKUP_VALUE";
            ddlPageSize.DataBind();
            ddlPageSize.Items.Insert(0, new ListItem("请选择", ""));
            ddlPageSize.SelectedValue = "20";
            txtPageSize.Value = "20";
        }
        private void ShowInfo(Guid TemplateID)
        {
            CMICT.CSP.BLL.BS_TEMPLATE_MAINBLL bll = new CMICT.CSP.BLL.BS_TEMPLATE_MAINBLL();
            CMICT.CSP.Model.BS_TEMPLATE_MAIN model = bll.GetModel(TemplateID);
            if (model != null)
            {
                this.txtTemplateName.Value = model.TemplateName;
                this.txtTemplateDesc.Value = model.TemplateDesc;
                this.txtPageSize.Value = model.PageSize.ToString();
                ddlCATEGORY.SelectedValue = model.BigCategory;
                txtCATEGORY.Text = model.BigCategory;
                BindSmallCategory(ddlCATEGORY.SelectedValue);
                ddlsmallcategory.SelectedValue = model.SmallCategory;
                txtsmallcategory.Text = model.SmallCategory;
                ddlPageSize.SelectedValue = model.PageSize.ToString();
                //txtPageSize.Value = model.PageSize.ToString();
                txtUnit.Value = model.Unit;
                if (model.TemplateStatus == "DISABLE")
                {
                    chkDisabled.Checked = true;
                }
                hidDisabled.Value = model.TemplateStatus;
                lblpageinfo.Text = GetPageInfoByTemplateID(TemplateID.ToString());
            }
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            bool result = false;
            Guid TemplateID = Guid.NewGuid();
            try
            {
                BS_TEMPLATE_MAINBLL mbll = new BS_TEMPLATE_MAINBLL();
                CMICT.CSP.Model.BS_TEMPLATE_MAIN model = new CMICT.CSP.Model.BS_TEMPLATE_MAIN();
                string TemplateName = txtTemplateName.Value.Trim();
                string TemplateDesc = txtTemplateDesc.Value.Trim();
                string PageSize = txtPageSize.Value.Trim();
                string TemplateStatus = "DRAFT";
                model.TemplateName = TemplateName;
                model.DiaplayType = "";
                model.TemplateDesc = TemplateDesc;
                model.BigCategory = txtCATEGORY.Text.Trim();
                model.SmallCategory = txtsmallcategory.Text.Trim();
                model.Unit = txtUnit.Value.Trim();
                model.Reminder = "";
                model.PageSize = Convert.ToDecimal(PageSize);
                model.ColumnSize = 0;
                model.TemplateStatus = TemplateStatus;
                model.SQL = "";
                model.Created = DateTime.Now;
                model.Modified = DateTime.Now;
                model.Author = GetCurrentUserLoginId();
                model.Editor = GetCurrentUserLoginId();
                string ReturnTID = string.Empty;
                if (hidOperType.Value == "add")
                {
                    ReturnTID = mbll.Add(model);
                    if (string.IsNullOrEmpty(ReturnTID))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "malert", "alert('模板基本信息配置失败！');", true);
                    }
                    else
                    {
                        result = true;
                        TemplateID = Guid.Parse(ReturnTID);
                        //Page.Response.Redirect("DataSourceConfig.aspx?templateID=" + ReturnTID + "&sourceID=" + hidsourceid.Value.Trim(), true);
                    }
                }
                else
                {
                    TemplateID = Guid.Parse(Page.Request.QueryString["templateID"]);
                    CMICT.CSP.Model.BS_TEMPLATE_MAIN modele = mbll.GetModel(TemplateID);
                    model.TemplateID = TemplateID;
                    model.Created = modele.Created;
                    model.SQL = modele.SQL;
                    model.Reminder = modele.Reminder;
                    model.DiaplayType = modele.DiaplayType;
                    model.ColumnSize = modele.ColumnSize;
                    model.Editor = GetCurrentUserLoginId();
                    model.Author = modele.Author;
                    if (!string.IsNullOrEmpty(hidsourceid.Value))
                    {
                        model.SourceID = Guid.Parse(hidsourceid.Value.Trim());
                    }
                    //if (chkDisabled.Checked)
                    //{
                    //    model.TemplateStatus = "DISABLE";
                    //}
                    //else
                    //{
                    model.TemplateStatus = "DRAFT";
                    //}
                    if (!mbll.Update(model))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "malert", "alert('模板基本信息修改失败！');", true);
                    }
                    else
                    {
                        BusinessSearchComponent bsbll = new BusinessSearchComponent();
                        bsbll.RemoveTemplateByGuid(TemplateID.ToString());
                        result = true;
                    }
                }
            }
            catch (Exception ee) { BaseComponent.Error(ee.Message); }
            if (result)
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    edit = "&type=edit";
                }
                string bigname = ddlCATEGORY.SelectedItem.Text;
                string smallname = ddlsmallcategory.SelectedItem.Text;
                Page.Response.Redirect("DataSourceConfig.aspx?bname=" + Page.Server.UrlEncode(bigname) + "&sname=" + Page.Server.UrlEncode(smallname) + "&templateID=" + TemplateID.ToString() + "&sourceID=" + hidsourceid.Value.Trim() + edit, true);

            }
        }

        protected void ddlCATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtsmallcategory.Text = "";
            string scategory = ddlCATEGORY.SelectedValue;
            if (!string.IsNullOrEmpty(scategory))
            {
                BindSmallCategory(scategory);
            }
            else
            {
                ddlsmallcategory.Items.Clear();
                ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
            }
        }

        
    }
}

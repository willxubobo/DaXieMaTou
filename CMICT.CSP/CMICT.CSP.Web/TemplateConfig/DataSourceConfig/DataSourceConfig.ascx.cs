using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.DataSourceConfig
{
    [ToolboxItemAttribute(false)]
    public partial class DataSourceConfig : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DataSourceConfig()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        DataSourceConfigComponent dbll = new DataSourceConfigComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string bname = Page.Server.UrlDecode(Page.Request.QueryString["bname"]);
                    string sname = Page.Server.UrlDecode(Page.Request.QueryString["sname"]);
                    //BindBigCategory();
                    if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                    {
                        hidsourceid.Value = Page.Request.QueryString["sourceID"];
                        ShowInfo(Page.Request.QueryString["sourceID"]);
                    }
                    else
                    {
                        string TemplateID = Page.Request.QueryString["templateID"];
                        if (!string.IsNullOrEmpty(TemplateID))
                        {
                            BS_TEMPLATE_MAINBLL mbll = new BS_TEMPLATE_MAINBLL();
                            CMICT.CSP.Model.BS_TEMPLATE_MAIN model = mbll.GetModel(Guid.Parse(TemplateID));
                            if (model != null)
                            {
                                BindBigCategory(bname,sname);
                                //ddlCATEGORY.SelectedValue = model.BigCategory;
                               
                               // ddlsmallcategory.SelectedValue = model.SmallCategory;
                                BindDataSource(txtCATEGORY.Text, txtsmallcategory.Text);
                               // txtCATEGORY.Text = model.BigCategory;
                                //txtsmallcategory.Text = model.SmallCategory;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("DataSourceConfig-PageLoad:" + ex.Message);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "dsorceconfigerror", "layer.alert('页面加载出错，请联系管理员！',8);", true);
            }
        }

        protected void ShowInfo(string sourceID)
        {
            BindBigCategory();
            if (!string.IsNullOrEmpty(sourceID))
            {
                BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
                CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(sourceID));
                if (model != null)
                {
                    hidOperType.Value = "edit";

                    ddlCATEGORY.SelectedValue = model.BigCategory;
                    BindSmallCategory(model.BigCategory);
                    ddlsmallcategory.SelectedValue = model.SmallCategory;
                    BindDataSource(model.BigCategory, model.SmallCategory);
                    ddlSourceName.SelectedValue = model.SourceID.ToString();
                    txtCATEGORY.Text = model.BigCategory;
                    txtsmallcategory.Text = model.SmallCategory;
                    txtSourceName.Text = model.SourceName;
                }
            }
        }

        protected void BindDataSource(string bigcate, string smallcate)
        {
            string tipcontent = string.Empty;
            DataTable dt = dbll.GetDataSourceList(bigcate, smallcate);
            ddlSourceName.DataSource = dt;
            ddlSourceName.DataTextField = "namedesc";
            ddlSourceName.DataValueField = "SourceID";
            ddlSourceName.DataBind();
            ddlSourceName.Items.Insert(0, new ListItem("请选择", ""));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    tipcontent += dr["SourceID"].ToString() + "," + dr["SourceName"].ToString() + "!";
                }
                hidtipcontent.Value = tipcontent.TrimEnd('!');
            }
        }
        //绑定报表大类
        protected void BindBigCategory(string stext,string btext)
        {
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("数据源");
            ddlCATEGORY.DataSource = dt;
            ddlCATEGORY.DataTextField = "LOOKUP_NAME";
            ddlCATEGORY.DataValueField = "LOOKUP_CODE";
            ddlCATEGORY.DataBind();
            ddlCATEGORY.Items.Insert(0, new ListItem("请选择", ""));
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
            ddlSourceName.Items.Insert(0, new ListItem("请选择", ""));
            if (!string.IsNullOrEmpty(stext))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["LOOKUP_NAME"].ToString().Trim() == stext)
                    {
                        ddlCATEGORY.SelectedValue = dr["LOOKUP_CODE"].ToString();
                        txtCATEGORY.Text = dr["LOOKUP_CODE"].ToString();
                        break;
                    }
                }
            }
            BindSmallCategory(txtCATEGORY.Text, btext);
        }
        protected void BindBigCategory()
        {
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("数据源");
            ddlCATEGORY.DataSource = dt;
            ddlCATEGORY.DataTextField = "LOOKUP_NAME";
            ddlCATEGORY.DataValueField = "LOOKUP_CODE";
            ddlCATEGORY.DataBind();
            ddlCATEGORY.Items.Insert(0, new ListItem("请选择", ""));
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
            ddlSourceName.Items.Insert(0, new ListItem("请选择", ""));
           
        }

        //绑定报表细类
        protected void BindSmallCategory(string bigcode, string stext)
        {
            DataTable dt = BaseComponent.GetUserLookupValuesByType(bigcode);
            ddlsmallcategory.DataSource = dt;
            ddlsmallcategory.DataTextField = "LOOKUP_VALUE_NAME";
            ddlsmallcategory.DataValueField = "LOOKUP_VALUE";
            ddlsmallcategory.DataBind();
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
            if (!string.IsNullOrEmpty(stext))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["LOOKUP_VALUE_NAME"].ToString().Trim() == stext)
                    {
                        ddlsmallcategory.SelectedValue = dr["LOOKUP_VALUE"].ToString();
                        txtsmallcategory.Text = dr["LOOKUP_VALUE"].ToString();
                        break;
                    }
                }
            }
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
        protected void ddlCATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scategory = ddlCATEGORY.SelectedValue;
            if (!string.IsNullOrEmpty(scategory))
            {
                txtsmallcategory.Text = "";
                BindSmallCategory(scategory);
            }
        }

        protected void ddlsmallcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string smallcate = ddlsmallcategory.SelectedValue;
            if (!string.IsNullOrEmpty(smallcate))
            {
                BindDataSource(ddlCATEGORY.SelectedValue, smallcate);
            }
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string edit = string.Empty;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
            {
                edit = "&type=edit";
            }
            try
            {
                string TemplateID = Page.Request.QueryString["templateID"];
                string SourceID = ddlSourceName.SelectedValue;
                if (hidsourceid.Value.Trim() != SourceID.Trim())//修改数据源后删除显示与查询配置信息
                {
                    if (!string.IsNullOrEmpty(TemplateID))
                    {
                        if (dbll.OperConfigDataSource(TemplateID, SourceID))
                        {
                            if (!string.IsNullOrEmpty(hidsourceid.Value.Trim()))
                            {
                                //更新后查询原数据源是否还有被其它模板引用，没有则更新状态为草稿
                                ConnectionConfigComponent ccc = new ConnectionConfigComponent();
                                DataTable dstable = ccc.GetTemplateIDBySourceID(Guid.Parse(hidsourceid.Value.Trim()));
                                if (dstable == null || dstable.Rows.Count == 0)
                                {
                                    dbll.UpdateSourceStatusBySourceID(hidsourceid.Value.Trim());
                                }
                            }
                            DisplayConfigComponent dcbll = new DisplayConfigComponent();
                            dcbll.DeleteTemInfoByTemplateID(TemplateID);
                            dcbll.UpdateStatusByTemplateID(TemplateID, "DRAFT");

                            QueryConfigComponent qcbll = new QueryConfigComponent();
                            qcbll.DeleteTemInfoByTemplateID(TemplateID);
                            CommunicationConfigComponent cccbll = new CommunicationConfigComponent();
                            //取出所有与此模板有通信关联的源模板信息
                            DataTable SourceTemList = cccbll.GetCommunicationBySourceTemplateID(TemplateID);
                            //删除通信配置
                            cccbll.DeleteCommunicationByTemplateID(TemplateID);
                            //刷新模板缓存
                            BusinessSearchComponent bsbll = new BusinessSearchComponent();
                            //bsbll.RefreshTemplateByGuid(TemplateID);
                            bsbll.RemoveTemplateByGuid(TemplateID);
                            if (SourceTemList != null && SourceTemList.Rows.Count > 0)
                            {
                                foreach (DataRow dr in SourceTemList.Rows)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["TargetTemplateID"])))
                                    {
                                        bsbll.RemoveTemplateByGuid(Convert.ToString(dr["TargetTemplateID"]));
                                    }
                                }
                            }

                            Page.Response.Redirect("DisplayConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID +edit+ "");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sourceerror", "layer.alert('数据源配置失败！',8);", true);
                        }
                    }
                }
                else
                {
                    Page.Response.Redirect("DisplayConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID +edit+ "");
                }
            }
            catch(Exception ex)
            {
                BaseComponent.Error("DataSourceConfig-lbtnSave_Click:" + ex.Message);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sourceerrorex", "layer.alert('数据源配置失败！',8);", true);
            }
        }

        protected void lbtnPrev_Click(object sender, EventArgs e)
        {
            string TemplateID = Page.Request.QueryString["templateID"];
            if (!string.IsNullOrEmpty(TemplateID))
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    edit = "&type=edit";
                }
                Page.Response.Redirect("TemplateInfoConfig.aspx?templateID=" + TemplateID + "&sourceID="+hidsourceid.Value+edit+"");
            }
        }
    }
}

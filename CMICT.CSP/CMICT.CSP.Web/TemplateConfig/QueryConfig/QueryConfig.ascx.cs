using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.QueryConfig
{
    [ToolboxItemAttribute(false)]
    public partial class QueryConfig : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public QueryConfig()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        DisplayConfigComponent dc = new DisplayConfigComponent();
        QueryConfigComponent bll = new QueryConfigComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                {
                    hidsourceid.Value = Page.Request.QueryString["sourceID"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["templateID"]))
                {
                    hidTemplateID.Value = Page.Request.QueryString["templateID"];
                }
                
                if (!string.IsNullOrEmpty(hidsourceid.Value) && !string.IsNullOrEmpty(hidTemplateID.Value))
                {
                    string sourceid = hidsourceid.Value.Trim();
                    BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
                    CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(sourceid));
                    if (model != null)
                    {
                        dc = new DisplayConfigComponent(model.SourceIP, model.UserName, model.Password, model.DBName);
                    }
                    BindColumnList();
                    BindControltype();
                    BindComparetype();
                    BindInfoIfEdit(hidTemplateID.Value);
                }
            }
        }

        protected void BindInfoIfEdit(string TemplateID)
        {
            if (!string.IsNullOrEmpty(TemplateID))
            {
                DataTable datadefault = bll.GetDefaultQueryListByTemplateID(Guid.Parse(TemplateID));
                if (datadefault != null && datadefault.Rows.Count > 0)
                {
                    hidopertype.Value = "edit";
                    hidquerynum.Value = datadefault.Rows.Count.ToString();
                    rptdefaultmain.DataSource = datadefault;
                    rptdefaultmain.DataBind();
                    string mainlogic = datadefault.Rows[0]["mainlogic"].ToString().Trim();
                    if (mainlogic == "and")
                    {
                        radand.Checked = true;
                    }
                    if (mainlogic == "or")
                    {
                        rador.Checked = true;
                    }
                }
                //绑定用户筛选条件
                DataTable datacolumn = bll.GetUserQueryListByTemplateID(Guid.Parse(TemplateID));
                if (datacolumn != null && datacolumn.Rows.Count > 0)
                {
                    ViewState["datacolumn"] = datacolumn;
                    hidopertype.Value = "edit";
                    UserQueryList.DataSource = datacolumn;
                    UserQueryList.DataBind();
                    hidSortNum.Value = datacolumn.Rows.Count.ToString();
                }
            }
        }
        protected void UserQueryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (hidopertype.Value == "edit")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (ViewState["ColumnList"] != null)//编辑绑定值
                    {
                        //绑定筛选列
                        Label lblcolumnname = (Label)e.Item.FindControl("lblcolumnname");
                        DropDownList ddluqcol = (DropDownList)e.Item.FindControl("ddluqcol");
                        ddluqcol.DataSource = ViewState["ColumnList"] as DataTable;
                        ddluqcol.DataTextField = "name";
                        ddluqcol.DataValueField = "name";
                        ddluqcol.DataBind();
                        ddluqcol.SelectedValue = lblcolumnname.Text.Trim();
                    }
                    if (ViewState["CONTROL_TYPE"] != null)//绑定类型
                    {
                        //绑定类型
                        Label lblctype = (Label)e.Item.FindControl("lblctype");
                        DropDownList ddlcontroltype = (DropDownList)e.Item.FindControl("ddlcontroltype");
                        ddlcontroltype.DataSource = ViewState["CONTROL_TYPE"] as DataTable;
                        ddlcontroltype.DataTextField = "LOOKUP_VALUE_NAME";
                        ddlcontroltype.DataValueField = "LOOKUP_VALUE";
                        ddlcontroltype.DataBind();
                        ddlcontroltype.SelectedValue = lblctype.Text.Trim();
                        TextBox defv = (TextBox)e.Item.FindControl("txtdefault");
                        string ctype = lblctype.Text.Trim();
                        if (ctype == "DATE")
                        {
                            defv.CssClass = "setIpt laydate-icon dtleft";
                            defv.Attributes.Add("onclick", "laydate({ istime: true, format: 'YYYY-MM-DD' })");
                        }
                        if (ctype == "YDATE")
                        {
                            defv.CssClass = "setIpt laydate-icon";
                            defv.Attributes.Add("onclick", "laydate({ format: 'YYYY' })");
                        }
                        if (ctype == "YMDATE")
                        {
                            defv.CssClass = "setIpt laydate-icon";
                            defv.Attributes.Add("onclick", "laydate({ format: 'YYYY-MM' })");
                        }
                    }
                    if (ViewState["BS_COMPARE"] != null)//绑定比较符
                    {
                        Label lblcompare = (Label)e.Item.FindControl("lblcompare");
                        DropDownList ddlcomparetype = (DropDownList)e.Item.FindControl("ddlcomparetype");
                        ddlcomparetype.DataSource = ViewState["BS_COMPARE"] as DataTable;
                        ddlcomparetype.DataTextField = "LOOKUP_VALUE_NAME";
                        ddlcomparetype.DataValueField = "LOOKUP_VALUE";
                        ddlcomparetype.DataBind();
                        ddlcomparetype.SelectedValue = lblcompare.Text.Trim();
                    }
                }
            }
        }

        protected void rptdefaultmain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (hidopertype.Value == "edit")
            {
                try
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        Label lblmoduleid = (Label)e.Item.FindControl("lblmoduleid");
                        string TemplateID = hidTemplateID.Value;
                        DataTable dtd = bll.GetDefaultQueryListInfoByTemplateID(Guid.Parse(TemplateID), Convert.ToInt32(lblmoduleid.Text));
                        if (dtd != null && dtd.Rows.Count > 0)
                        {
                            Repeater rptdefaultmaininfo = (Repeater)e.Item.FindControl("rptdefaultmaininfo");
                            rptdefaultmaininfo.ItemDataBound += rptdefaultmaininfo_ItemDataBound;
                            rptdefaultmaininfo.DataSource = dtd;
                            rptdefaultmaininfo.DataBind();

                        }
                    }
                }
                catch
                {

                }
            }
        }

        protected void rptdefaultmaininfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (hidopertype.Value == "edit")
            {
                try
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        if (ViewState["DfColumnList"] != null)//编辑绑定值
                        {
                            DataTable clist = ViewState["DfColumnList"] as DataTable;
                            //绑定筛选列
                            Label lblcolumnname = (Label)e.Item.FindControl("lblcolumnname");
                            DropDownList ddlColumnName = (DropDownList)e.Item.FindControl("ddlColumnName");
                            ddlColumnName.DataSource = clist;
                            ddlColumnName.DataTextField = "name";
                            ddlColumnName.DataValueField = "cv";
                            ddlColumnName.DataBind();
                            DataRow[] dr = clist.Select("name='" + lblcolumnname.Text.Trim() + "'");
                            if (dr != null && dr.Length > 0)
                            {
                                DataRow drinfo = dr[0];
                                ddlColumnName.SelectedValue = drinfo["cv"].ToString();
                            }
                            //ddlColumnName.Items.FindByText(lblcolumnname.Text.Trim()).Selected=true;
                        }
                        if (ViewState["BS_COMPARE"] != null)//绑定比较符
                        {
                            Label lblcompare = (Label)e.Item.FindControl("lblCompare");
                            DropDownList ddlCompare = (DropDownList)e.Item.FindControl("ddlCompare");
                            ddlCompare.DataSource = ViewState["BS_COMPARE"] as DataTable;
                            ddlCompare.DataTextField = "LOOKUP_VALUE_NAME";
                            ddlCompare.DataValueField = "LOOKUP_VALUE";
                            ddlCompare.DataBind();
                            ddlCompare.SelectedValue = lblcompare.Text.Trim();
                        }
                    }
                }
                catch
                {

                }
            }
        }
        //根据sourceid获取数据源信息
        protected void BindColumnList()
        {
            string sourceid = hidsourceid.Value.Trim();
            BS_DATASOURCEBLL blld = new BS_DATASOURCEBLL();
            CMICT.CSP.Model.BS_DATASOURCE model = blld.GetModel(Guid.Parse(sourceid));
            string templateid = hidTemplateID.Value.Trim();
            
            if (model != null)
            {
                if (!string.IsNullOrEmpty(templateid))//绑定已经配置的列名与显示名信息
                {
                    DataTable dtt = bll.GetUserQueryColListByTemplateID(Guid.Parse(templateid));
                    if (dtt != null && dtt.Rows.Count > 0)
                    {
                        string configcolinfo = string.Empty;
                        foreach (DataRow drr in dtt.Rows)
                        {
                            configcolinfo += Convert.ToString(drr["name"]) + "|" + Convert.ToString(drr["DisplayName"]) + ";";
                        }
                        hidconfigcolinfo.Value = configcolinfo;
                    }
                }
                DataTable dt = dc.GetColumnListByType(model.ObjectType, model.ObjectName, "");
                
                //绑定默认筛选条件中列名下拉
                ddlColumnName.DataSource = dt;
                ddlColumnName.DataTextField = "name";
                ddlColumnName.DataValueField = "data_type";
                ddlColumnName.DataBind();
                ddlColumnName.Items.Insert(0, new ListItem("请选择", ""));
                ViewState["DfColumnList"] = dt;
                //绑定排序中列名下拉
                ddluqcol.DataSource = dt;
                ddluqcol.DataTextField = "name";
                ddluqcol.DataValueField = "name";
                ddluqcol.DataBind();
                ddluqcol.Items.Insert(0, new ListItem("请选择", ""));
                ViewState["ColumnList"] = dt;
                if (model.ObjectType == "PROC")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string paralist = string.Empty;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string cname = Convert.ToString(dr["name"]).Trim();
                            if (cname.IndexOf('@') != -1)
                            {
                                paralist += cname + ",";
                            }
                        }
                        hidprocparalist.Value = paralist.TrimEnd(',');
                    }
                }
            }
        }
        //绑定控件类型下拉
        protected void BindControltype()
        {
            //绑定计算方式
            DataTable dtt = BaseComponent.GetLookupValuesByType("BS_CONTROL_TYPE");
            ddlcontroltype.DataSource = dtt;
            ddlcontroltype.DataTextField = "LOOKUP_VALUE_NAME";
            ddlcontroltype.DataValueField = "LOOKUP_VALUE";
            ddlcontroltype.DataBind();
            ddlcontroltype.Items.Insert(0, new ListItem("请选择", ""));
            ViewState["CONTROL_TYPE"] = dtt;
        }
        //绑定比较符下拉
        protected void BindComparetype()
        {
            //绑定计算方式
            DataTable dtt = BaseComponent.GetLookupValuesByType("BS_COMPARE");
            ddlcomparetype.DataSource = dtt;
            ddlcomparetype.DataTextField = "LOOKUP_VALUE_NAME";
            ddlcomparetype.DataValueField = "LOOKUP_VALUE";
            ddlcomparetype.DataBind();
            ddlcomparetype.Items.Insert(0, new ListItem("请选择", ""));
            ViewState["BS_COMPARE"] = dtt;
            ddlCompare.DataSource = dtt;
            ddlCompare.DataTextField = "LOOKUP_VALUE_NAME";
            ddlCompare.DataValueField = "LOOKUP_VALUE";
            ddlCompare.DataBind();
            ddlCompare.Items.Insert(0, new ListItem("请选择", ""));
        }

        protected void lbtnPrev_Click(object sender, EventArgs e)
        {
            string TemplateID = hidTemplateID.Value;
            string SourceID = hidsourceid.Value;
            if (!string.IsNullOrEmpty(TemplateID) && !string.IsNullOrEmpty(SourceID))
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    edit = "&type=edit";
                }
                Page.Response.Redirect("DisplayConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID+edit);
            }
        }

        //保存
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hidTemplateID.Value))
                {
                    bool adduserquery = false;
                    bool adddefaultquery = false;
                    //bool iddel = true;
                    if (hidopertype.Value == "edit")//如果是更新，先删除之前的配置信息
                    {
                        bll.DeleteTemInfoByTemplateID(hidTemplateID.Value);
                    }
                    string DefaultQueryContent = hidDefaultContent.Value.TrimEnd(';');//用户默认筛选条件内容
                    if (!string.IsNullOrEmpty(DefaultQueryContent))
                    {
                        string MainLogic = "";
                        if (radand.Checked)
                        {
                            MainLogic = "and";
                        }
                        if (rador.Checked)
                        {
                            MainLogic = "or";
                        }
                        BS_DEFAULT_QUERYBLL qcqbll = new BS_DEFAULT_QUERYBLL();
                        string[] uqlist = DefaultQueryContent.Split(';');
                        double addm = 10;
                        foreach (string uqinfo in uqlist)
                        {
                            addm++;
                            string[] uqinfolist = uqinfo.Split('|');
                            BS_DEFAULT_QUERY model = new BS_DEFAULT_QUERY();
                            model.TemplateID = Guid.Parse(hidTemplateID.Value);
                            model.ColumnName = uqinfolist[0];
                            model.MainLogic = MainLogic;
                            model.Compare = uqinfolist[1];
                            model.CompareValue = uqinfolist[2];
                            model.Desction = uqinfolist[3];
                            model.ModuleID = Convert.ToInt32(uqinfolist[4]);
                            model.SubLogic = uqinfolist[5];

                            model.Created = DateTime.Now.AddMilliseconds(addm);
                            model.Modified = DateTime.Now;
                            model.Author = BaseWebPart.GetCurrentUserLoginId();
                            model.Editor = BaseWebPart.GetCurrentUserLoginId();
                            adddefaultquery = qcqbll.Add(model);
                        }
                    }
                    string UserQueryContent = hidUserQueryContent.Value.TrimEnd('$');//用户筛选配置内容
                    if (!string.IsNullOrEmpty(UserQueryContent))
                    {
                        BS_CUSTOM_QUERYBLL qcqbll = new BS_CUSTOM_QUERYBLL();
                        string[] uqlist = UserQueryContent.Split('$');
                        double addm = 10;
                        foreach (string uqinfo in uqlist)
                        {
                            addm++;
                            string[] uqinfolist = uqinfo.Split('|');
                            BS_CUSTOM_QUERY model = new BS_CUSTOM_QUERY();
                            model.TemplateID = Guid.Parse(hidTemplateID.Value);
                            model.ColumnName = uqinfolist[0];
                            model.DisplayName = uqinfolist[1];
                            model.ControlType = uqinfolist[2];
                            model.Compare = uqinfolist[3];
                            model.Sequence = -1;
                            if (!string.IsNullOrEmpty(uqinfolist[4]))
                            {
                                model.Sequence = decimal.Parse(uqinfolist[4]);
                            }
                            model.DefaultValue = uqinfolist[6];
                            model.Reminder = uqinfolist[5];
                            model.Created = DateTime.Now.AddMilliseconds(addm);
                            model.Modified = DateTime.Now;
                            model.Author = BaseWebPart.GetCurrentUserLoginId();
                            model.Editor = BaseWebPart.GetCurrentUserLoginId();
                            adduserquery = qcqbll.Add(model);
                        }
                    }
                    string edit = string.Empty;
                    if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                    {
                        edit = "&type=edit";
                    }
                    BusinessSearchComponent bsbll = new BusinessSearchComponent();
                    bsbll.RemoveTemplateByGuid(hidTemplateID.Value);
                    string sourceid = hidsourceid.Value.Trim();
                    if (adduserquery || adddefaultquery)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "subsuc", "layer.alert('用户筛选条件配置成功！');", true);
                        Page.Response.Redirect("CommunicationConfig.aspx?templateID=" + hidTemplateID.Value + "&sourceID=" + sourceid+edit);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "subsucerror", "layer.alert('用户筛选条件配置失败！');", true);
                    }
                    if (string.IsNullOrEmpty(DefaultQueryContent) && string.IsNullOrEmpty(UserQueryContent))
                    {
                        Page.Response.Redirect("CommunicationConfig.aspx?templateID=" + hidTemplateID.Value + "&sourceID=" + sourceid+edit);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suberror", "layer.alert('未获取到模板编号！');", true);
                }
            }
            catch (Exception ee)
            {
                BaseComponent.Error(ee.Message);
            }
        }
    }
}

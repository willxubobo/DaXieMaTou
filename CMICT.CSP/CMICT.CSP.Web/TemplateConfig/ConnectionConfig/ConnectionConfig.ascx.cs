using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.ConnectionConfig
{
    [ToolboxItemAttribute(false)]
    public partial class ConnectionConfig : BaseWebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ConnectionConfig()
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
                MatchSourceIP();
                BindBigCategory();
                BindObjectType();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                {
                    hidid.Value = Page.Request.QueryString["sourceID"];
                    hidOperType.Value = "edit";
                    Guid SourceID = Guid.Parse(Page.Request.QueryString["sourceID"]);
                    ShowInfo(SourceID);
                }
                else
                {
                    hidOperType.Value = "add";
                }
            }
        }
        //数据源ip智能匹配
        protected void MatchSourceIP()
        {
            StringBuilder matchvar = new StringBuilder();
            ConnectionConfigComponent ccc = new ConnectionConfigComponent();
            DataTable dt = ccc.GetDataSourceIpList();
            if (dt != null && dt.Rows.Count > 0)
            {
                matchvar.Append("strlist_ip=[");
                string listc = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    listc += "'" + dr["sourceip"].ToString() + "',";
                }
                listc = listc.TrimEnd(',');
                matchvar.Append(listc.ToString() + "];");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "matchip", matchvar.ToString(), true);
            }
        }
        //绑定报表大类
        protected void BindBigCategory()
        {
            ddlCATEGORY.Items.Clear();
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("数据源");
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
            ddlsmallcategory.Items.Clear();
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "executeconnectsur();", true);
            }
        }
        //绑定类型
        protected void BindObjectType()
        {
            ddlObjectType.Items.Clear();
            //BaseComponent bc = new BaseComponent();
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_OBJECT_TYPE");
            ddlObjectType.DataSource = dt;
            ddlObjectType.DataTextField = "LOOKUP_VALUE_NAME";
            ddlObjectType.DataValueField = "LOOKUP_VALUE";
            ddlObjectType.DataBind();
            ddlObjectType.Items.Insert(0, new ListItem("请选择", ""));
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlObjectType.SelectedValue = "TABLE";
                txtObjectType.Text = "TABLE";
            }
        }
        protected string GetTemplateInfoBySourceID(string SourceID)
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
        private void ShowInfo(Guid SourceID)
        {
            CMICT.CSP.BLL.BS_DATASOURCEBLL bll = new CMICT.CSP.BLL.BS_DATASOURCEBLL();
            CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(SourceID);
            if (model != null)
            {
                this.txtSourceName.Value = model.SourceName;
                this.txtSourceIP.Value = model.SourceIP;
                this.txtUserName.Value = model.UserName;
                this.txtPwd.Text = model.Password;
                this.txtSourceDesc.Value = model.SourceDesc;
                this.txtPwd.Attributes.Add("value", model.Password);
                string SourceIP = txtSourceIP.Value.Trim();
                string UserName = txtUserName.Value.Trim();
                string Pwd = txtPwd.Text.Trim();
                string DBName = model.DBName;
                string ObjectType = model.ObjectType;
                this.hidSourceStatus.Value = model.SourceStatus;
                ddlObjectType.SelectedValue = model.ObjectType;
                if (model.SourceStatus.Trim() == "ENABLE")
                {
                    string temnames = GetTemplateInfoBySourceID(SourceID.ToString());
                    hidtemnames.Value = temnames;
                }
                try
                {
                    string tipcontent = string.Empty;
                    TestConn(out tipcontent,1);
                    ddlDBName.SelectedValue = model.DBName;
                    BindObjectName();
                    ddlObjectName.SelectedValue = model.ObjectName;
                    ddlCATEGORY.SelectedValue = model.BigCategory;
                    txtCATEGORY.Text = model.BigCategory;
                    BindSmallCategory(model.BigCategory);
                    ddlsmallcategory.SelectedValue = model.SmallCategory;
                    txtsmallcategory.Text = model.SmallCategory;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "executeconnectsur();", true);

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('数据源连接不上！无法修改！');", true);

                }
                this.txtDBName.Text = model.DBName;
                this.txtObjectType.Text = model.ObjectType;
                this.txtObjectName.Text = model.ObjectName;
                hidauthor.Value = model.Author;
                hidcreated.Value = model.Created.ToString();
            }
        }
        //测试是否连接通过
        protected bool TestConn(out string alertstr,int ctype=0)
        {
            alertstr = "";
            string SourceIP = txtSourceIP.Value.Trim();
            string UserName = txtUserName.Value.Trim();
            string Pwd = txtPwd.Text.Trim();
            string DbName = txtDBName.Text.Trim();
            bool result = false;
            ConnectionConfigComponent ccc = new ConnectionConfigComponent(SourceIP, UserName, Pwd);
            if (ccc.CheckConnect())//如果连接成功
            {
                DataTable dbtable = null;
                if (ctype == 1)
                {
                    dbtable=ccc.GetAllDBOnDataSource();
                    //绑定数据源下所有数据库
                    ddlDBName.DataSource = dbtable;
                    ddlDBName.DataTextField = "namedesc";
                    ddlDBName.DataValueField = "name";
                    ddlDBName.DataBind();
                    ddlDBName.Items.Insert(0, new ListItem("请选择", ""));
                }
                else//检查数据库与库中内容是否存在
                {
                    dbtable = ccc.GetAllDBOnDataSource(DbName);
                    if (dbtable != null && dbtable.Rows.Count > 0)
                    {
                        string ObjectType = ddlObjectType.SelectedValue;
                        string objname = ddlObjectName.SelectedValue;
                        ConnectionConfigComponent ccctestobj = new ConnectionConfigComponent(SourceIP, UserName, Pwd, DbName);
                        DataTable dt = ccctestobj.GetDataListByDataType(ObjectType, objname);
                        if (dt != null && dt.Rows.Count > 0)//首先检查对象名是否存在
                        {
                            string checkresult = ccctestobj.CheckCanExecute(ObjectType, objname).Trim();
                            if (string.IsNullOrEmpty(checkresult))
                            {
                                result = true;
                            }
                            else
                            {
                                alertstr = ":"+checkresult.Replace("'","").Replace("\"","");
                            }
                        }
                    }
                }
                
            }
            
            return result;
        }
        protected bool BindDbName(int contype = 0)
        {
            string SourceIP = txtSourceIP.Value.Trim();
            string UserName = txtUserName.Value.Trim();
            string Pwd = txtPwd.Text.Trim();
            bool result = false;
            ConnectionConfigComponent ccc = new ConnectionConfigComponent(SourceIP, UserName, Pwd);
            if (ccc.CheckConnect())//如果连接成功
            {
                //绑定数据源下所有数据库
                DataTable dbtable = ccc.GetAllDBOnDataSource();
                ddlDBName.DataSource = dbtable;
                ddlDBName.DataTextField = "namedesc";
                ddlDBName.DataValueField = "name";
                ddlDBName.DataBind();
                ddlDBName.Items.Insert(0, new ListItem("请选择", ""));
                txtDBName.Text = "";
                txtSourceName.Value = "";
                //ddlObjectType.SelectedValue = "";
                //txtObjectType.Text = "";
                ddlObjectName.Items.Clear();
                ddlObjectName.Items.Insert(0, new ListItem("请选择", ""));
                txtObjectName.Text = "";
                ddlCATEGORY.SelectedValue = "";
                txtCATEGORY.Text = "";
                ddlsmallcategory.Items.Clear();
                ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
                txtsmallcategory.Text = "";
                txtSourceDesc.Value = "";
                result = true;
            }
            return result;
        }
        protected void lbtnConnect_Click(object sender, EventArgs e)
        {

            if (BindDbName())//如果连接成功
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "executeconnectsur();layer.closeAll();", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connerror", "layer.alert('连接数据库失败，请检查填写的信息是否正确！',8);executeconnecterror();", true);
            }

        }


        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string tipcontent = string.Empty;
                if (!TestConn(out tipcontent))//如果连接失败
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveconntest", "layer.alert('连接测试失败"+tipcontent+"!',8);executeconnectsur();", true);
                    return;
                }
                string SourceName = this.txtSourceName.Value.Trim();
                string SourceDesc = this.txtSourceDesc.Value.Trim();
                string SourceIP = this.txtSourceIP.Value.Trim();
                string UserName = this.txtUserName.Value.Trim();
                string Password = this.txtPwd.Text.Trim();
                string DBName = this.txtDBName.Text.Trim();
                string ObjectType = this.txtObjectType.Text.Trim();
                string ObjectName = this.txtObjectName.Text.Trim();
                string SourceStatus = "FREE";
                DateTime Created = DateTime.Now;
                DateTime Modified = DateTime.Now;
                string Author = GetCurrentUserLoginId();
                string Editor = GetCurrentUserLoginId();
                CMICT.CSP.BLL.BS_DATASOURCEBLL bll = new CMICT.CSP.BLL.BS_DATASOURCEBLL();
                string rsourceid = string.Empty;
                if (hidOperType.Value == "edit")
                {
                    rsourceid = Page.Request.QueryString["sourceID"];
                }
                if (bll.GetExists(SourceName,rsourceid))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sourcenameexist", "layer.alert('已经存在此数据源名称："+SourceName+"!请重新填写！',8);executeconnectsur();", true);
                    return;
                }

                CMICT.CSP.Model.BS_DATASOURCE model = new CMICT.CSP.Model.BS_DATASOURCE();
                model.SourceName = SourceName;
                model.SourceDesc = SourceDesc;
                model.SourceIP = SourceIP;
                model.UserName = UserName;
                model.Password = Password;
                model.DBName = DBName;
                model.ObjectType = ObjectType;
                model.ObjectName = ObjectName;
                model.SourceStatus = SourceStatus;
                model.Created = Created;
                model.Modified = Modified;
                model.Author = Author;
                model.Editor = Editor;
                model.BigCategory = txtCATEGORY.Text;
                model.SmallCategory = txtsmallcategory.Text;
                model.SourceCNName = "";

                if (hidOperType.Value == "add")
                {
                    if (bll.Add(model))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuca", "layer.alert('数据源新增成功！',9);", true);
                        Page.Response.Redirect("DataSourceManage.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connerrora", "layer.alert('数据源新增失败！',8);", true);
                    }
                }
                else//修改
                {
                    Guid SourceID = Guid.Parse(Page.Request.QueryString["sourceID"]);
                    CMICT.CSP.Model.BS_DATASOURCE oldmodel = bll.GetModel(SourceID);
                    model.SourceID = SourceID;
                    model.Author = hidauthor.Value;
                    model.Created = DateTime.Parse(hidcreated.Value);
                    model.SourceStatus = hidSourceStatus.Value.Trim();
                    if (bll.Update(model))
                    {
                        //更新数据源后清空相应模板的配置信息
                        if (model.DBName != oldmodel.DBName || model.SourceIP != oldmodel.SourceIP || model.UserName != oldmodel.UserName || model.Password != oldmodel.Password || model.ObjectType != oldmodel.ObjectType || model.ObjectName != oldmodel.ObjectName)
                        {
                            ConnectionConfigComponent ccc = new ConnectionConfigComponent();
                            DataTable dstable = ccc.GetTemplateIDBySourceID(SourceID);
                            if (dstable != null && dstable.Rows.Count > 0)
                            {
                                DisplayConfigComponent dcbll = new DisplayConfigComponent();
                                QueryConfigComponent qcbll = new QueryConfigComponent();
                                BusinessSearchComponent bsbll = new BusinessSearchComponent();
                                //删除通信配置
                                CommunicationConfigComponent cccbll = new CommunicationConfigComponent();
                                foreach (DataRow dr in dstable.Rows)
                                {
                                    dcbll.UpdateStatusByTemplateID(dr["TemplateID"].ToString(), "DRAFT");
                                    dcbll.DeleteTemInfoByTemplateID(dr["TemplateID"].ToString());
                                    qcbll.DeleteTemInfoByTemplateID(dr["TemplateID"].ToString());
                                    //取出所有与此模板有通信关联的源模板信息
                                    DataTable SourceTemList = cccbll.GetCommunicationBySourceTemplateID(dr["TemplateID"].ToString());
                                    cccbll.DeleteCommunicationByTemplateID(dr["TemplateID"].ToString());
                                    bsbll.RefreshTemplateByGuid(dr["TemplateID"].ToString());
                                    if (SourceTemList != null && SourceTemList.Rows.Count > 0)
                                    {
                                        foreach (DataRow drr in SourceTemList.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(Convert.ToString(drr["TargetTemplateID"])))
                                            {
                                                bsbll.RefreshTemplateByGuid(Convert.ToString(drr["TargetTemplateID"]));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsucad", "layer.alert('数据源修改成功！');", true);
                        Page.Response.Redirect("DataSourceManage.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connerrorae", "layer.alert('数据源修改失败！');", true);
                    }
                }
            }
            catch (Exception ee)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveerror", "layer.alert('" + ee.Message + "');", true);
            }
        }

        protected bool BindObjectName()
        {
            try
            {
                bool result = false;
                string SourceIP = txtSourceIP.Value.Trim();
                string UserName = txtUserName.Value.Trim();
                string Pwd = txtPwd.Text.Trim();
                string DBName = ddlDBName.SelectedValue;
                string ObjectType = ddlObjectType.SelectedValue;
                if (!string.IsNullOrEmpty(ObjectType) && !string.IsNullOrEmpty(DBName))
                {
                    ddlObjectName.Items.Clear();
                    ConnectionConfigComponent ccc = new ConnectionConfigComponent(SourceIP, UserName, Pwd, DBName);
                    DataTable dt = ccc.GetDataListByDataType(ObjectType);
                    ddlObjectName.DataSource = dt;
                    ddlObjectName.DataTextField = "namedesc";
                    ddlObjectName.DataValueField = "name";
                    ddlObjectName.DataBind();
                    ddlObjectName.Items.Insert(0, new ListItem("请选择", ""));
                    result = true;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindObjectName();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsucadsd", "executeconnectsur();", true);
        }

        protected void ddlDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlObjectType.SelectedValue))
            {
                BindObjectName();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsucadsdbn", "executeconnectsur();", true);
            }
        }

        protected void lbtnTestCon_Click(object sender, EventArgs e)
        {
            string tipcontent = string.Empty;
            if (!TestConn(out tipcontent))//如果连接失败
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "dsesded", "layer.alert('数据源连接失败"+tipcontent+"！',8);executeconnectsur();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveconntestconerror", "layer.alert('数据源连接成功！',9);executeconnectsur();", true);
            }
        }
    }
}

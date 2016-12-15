using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System.Data;
using System.Web.UI;

namespace CMICT.CSP.Web.TemplateConfig.CommunicationConfig
{
    [ToolboxItemAttribute(false)]
    public partial class CommunicationConfig : BaseWebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CommunicationConfig()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        CommunicationConfigComponent dc = new CommunicationConfigComponent();
        BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    hidisedit.Value = "edit";
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                {
                    try
                    {
                        hidsourceid.Value = Page.Request.QueryString["sourceID"];
                        CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(hidsourceid.Value));
                        if (model != null)
                        {
                            BindBigCategory();
                            CommunicationConfigComponent dcserver = new CommunicationConfigComponent(model.SourceIP, model.UserName, model.Password, model.DBName);
                            if (!string.IsNullOrEmpty(Page.Request.QueryString["templateID"]))
                            {
                                hidTemplateID.Value = Page.Request.QueryString["templateID"];
                                BindCommunicationData();
                                
                                    DataTable dtTargetColumns = dcserver.GetSourceDataColumnBytemID(hidTemplateID.Value, model.ObjectType, model.ObjectName);
                                    //DataTable dtTargetColumns = dc.GetDataColumnByTemplateID(hidTemplateID.Value);
                                    ViewState["TargetColumns"] = dtTargetColumns;
                                
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        BaseComponent.Error("CommunicationConfig-页面加载出错："+ex.Message);
                    }
                }
            }
        }

        private void BindCommunicationData()
        {
            try
            {
                string templateID = hidTemplateID.Value;
                DataTable dt = dc.GetCommunicationList(templateID);

                CommunicationList.DataSource = dt;
                CommunicationList.DataBind();
            } 
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        private void BindSourceTemplate(string bigid,string smallid)
        {
            try
            {
                string tipcontent = string.Empty;
                string templateID = hidTemplateID.Value;
                DataTable dt = dc.GetAllTemplateByExcludeID(templateID, bigid,smallid);

                ddlSourceTemplate.DataSource = dt;
                ddlSourceTemplate.DataTextField = "namedesc";
                ddlSourceTemplate.DataValueField = "TemplateID";
                ddlSourceTemplate.DataBind();
                ddlSourceTemplate.Items.Insert(0, new ListItem("请选择", ""));
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tipcontent += dr["TemplateID"].ToString() + "," + dr["TemplateName"].ToString() + "!";
                    }
                    hidtipcontent.Value = tipcontent.TrimEnd('!');
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string communicationID = hidCommunicationID.Value;
                if (dc.DeleteCommunication(communicationID))
                {
                    BindCommunicationData();
                    hidCommunicationID.Value = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('删除成功！',9);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert2", "javascript:showMessage('删除失败！',8);", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        protected void ddlSourceTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sourceTemplateID = ddlSourceTemplate.SelectedValue;
                if (!string.IsNullOrEmpty(sourceTemplateID))
                {
                    CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(sourceTemplateID);
                    if (model != null)
                    {
                        CommunicationConfigComponent dcserver = new CommunicationConfigComponent(model.SourceIP, model.UserName, model.Password, model.DBName);
                        DataTable dtCommu = dcserver.GetSourceDataColumnBytemID(sourceTemplateID, model.ObjectType, model.ObjectName);
                        CommunicationDetailList.DataSource = dtCommu;
                        CommunicationDetailList.DataBind();
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showDetail", "showDetail();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }
        //上一步
        protected void lbtnPrev_Click(object sender, EventArgs e)
        {
            string SourceID = hidsourceid.Value;
            string TemplateID = hidTemplateID.Value;
            if (!string.IsNullOrEmpty(TemplateID))
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    edit = "&type=edit";
                }
                Page.Response.Redirect("QueryConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID+edit);
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                trbig.Attributes.Add("style", "display:none;");
                trsmall.Attributes.Add("style", "display:none;");
                BindSourceTemplate("","");

                string communicationID = hidCommunicationID.Value;
                if (communicationID.Length <= 0) return;

                DataTable dtCommu = dc.GetCommunicationConfigInfoByID(communicationID);

                string sourceTemplateID = "";
                if (dtCommu != null && dtCommu.Rows.Count > 0)
                {
                    sourceTemplateID = Convert.ToString(dtCommu.Rows[0]["SourceTemplateID"]);
                    txtName.Value = Convert.ToString(dtCommu.Rows[0]["Name"]);
                }

                if (!string.IsNullOrEmpty(sourceTemplateID))
                {
                    ddlSourceTemplate.Items.FindByValue(sourceTemplateID).Selected = true;
                    txtSourceTemplate.Text = sourceTemplateID;
                    CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(sourceTemplateID);
                    if (model != null)
                    {
                        CommunicationConfigComponent dcserver = new CommunicationConfigComponent(model.SourceIP, model.UserName, model.Password, model.DBName);
                        DataTable dtSourceColumns = dcserver.GetSourceDataColumnBytemID(sourceTemplateID, model.ObjectType,model.ObjectName);
                        if (dtSourceColumns != null && dtSourceColumns.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSourceColumns.Rows)
                            {
                                dr["TargetColumnName"] = dc.GetTargetColumnNameByColNameAndComid(dr["ColumnName"].ToString(), communicationID);
                            }
                        }
                        //DataTable dtSourceColumns = dc.GetCommunicationDetailByID(communicationID);
                        CommunicationDetailList.DataSource = dtSourceColumns;
                        CommunicationDetailList.DataBind();

                        if (dtSourceColumns != null)
                        {
                            txtSelected.Text = dtSourceColumns.Rows.Count.ToString();
                        }
                    }
                }

                ddlSourceTemplate.Enabled = false;

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showDetail", "showDetail();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        protected void CommunicationDetailList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlTargetColumn = (DropDownList)e.Item.FindControl("ddlTargetColumn");
                    Literal lblTargetColumnName = (Literal)e.Item.FindControl("lblTargetColumnName");
                    Literal lblColumnDataType = (Literal)e.Item.FindControl("lblColumnDataType");
                    if (ddlTargetColumn != null && lblTargetColumnName != null)
                    {
                        string targetColumnName = lblTargetColumnName.Text;
                        int index = 1;
                        int selectIndex = 0;
                        string selectedv = string.Empty;
                        if (ViewState["TargetColumns"] != null)
                        {
                            DataTable dtTargetColumns = (DataTable)ViewState["TargetColumns"];
                            ddlTargetColumn.Items.Clear();
                            ddlTargetColumn.Items.Add(new ListItem("空", ""));
                            foreach (DataRow row in dtTargetColumns.Rows)
                            {
                                string columnName = Convert.ToString(row["ColumnName"]);

                                if (targetColumnName.Length > 0)
                                {
                                    if (columnName.ToLower() == targetColumnName.ToLower())
                                    {
                                        selectIndex = index;
                                        selectedv = columnName;
                                    }
                                }

                                string dataType = Convert.ToString(row["ColumnDataType"]);
                                string dataTypeDisplayName = BaseComponent.GetLookupNameBuValue("BS_DATATYPE", dataType);

                                if (dataTypeDisplayName.Length <= 0) dataTypeDisplayName = dataType;
                                columnName += " - " + dataTypeDisplayName;
                                if (!selectedv.Contains("-"))
                                {
                                    selectedv += " - " + dataTypeDisplayName;
                                }
                               // ddlTargetColumn.Items.Add(new ListItem(columnName, dataTypeDisplayName));
                                ddlTargetColumn.Items.Add(new ListItem(columnName, columnName));
                                ++index;
                            }
                            ddlTargetColumn.SelectedValue = selectedv;
                            //if (ddlTargetColumn.Items.Count > selectIndex)
                            //{
                            //    ddlTargetColumn.Items[selectIndex].Selected = true;
                            //}
                        }
                    }

                    Literal lblSourceDataType = (Literal)e.Item.FindControl("lblSourceDataType");

                    if (lblColumnDataType != null && lblSourceDataType != null && lblColumnDataType.Text.Length > 0)
                    {
                        string sourceDataType = BaseComponent.GetLookupNameBuValue("BS_DATATYPE", lblColumnDataType.Text);
                        if (sourceDataType.Length <= 0) sourceDataType = lblColumnDataType.Text;
                        ddlTargetColumn.Attributes.Add("onchange", "matchColumnDataType(this,'" + sourceDataType + "');");
                        lblSourceDataType.Text = sourceDataType;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hidTemplateID.Value))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertnullExist", "javascript:showMessage('未获取到目标模板信息！',8);showDetail();", true);
                    return;
                }
                string communicationDetails = hidCommunicationDetails.Value;
                if (hidSaveModel.Value == "add")
                {
                    if (dc.CommunicationIsExist(txtName.Value, hidTemplateID.Value.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertExist", "javascript:showMessage('该业务名称已存在，请重新输入！',8);showDetail();", true);
                        return;
                    }
                    Guid sourceTemplateID = new Guid(ddlSourceTemplate.SelectedValue);
                    Guid templateID = new Guid(hidTemplateID.Value);
                    BS_COMMUNICATION_MAIN model = new BS_COMMUNICATION_MAIN();
                    Guid communicationID = Guid.NewGuid();
                    model.CommunicationID = communicationID;
                    string currentUser = GetCurrentUserLoginId();
                    model.Author = currentUser;
                    model.Created = DateTime.Now;
                    model.Editor = currentUser;
                    model.Modified = DateTime.Now;
                    model.Name = txtName.Value;
                    model.SourceTemplateID = sourceTemplateID;
                    model.TargetTemplateID = templateID;

                    BS_COMMUNICATION_MAINBLL bll = new BS_COMMUNICATION_MAINBLL();
                    if (bll.Add(model))
                    {
                        string[] communication = communicationDetails.Split('|');
                        foreach (string details in communication)
                        {
                            string[] strDetail = details.Split(';');
                            BS_COMMUNICATION_DETAIL detailModel = new BS_COMMUNICATION_DETAIL();
                            detailModel.Author = currentUser;
                            detailModel.CommunicationID = communicationID;
                            detailModel.Created = DateTime.Now;
                            detailModel.Editor = currentUser;
                            detailModel.ID = Guid.NewGuid();
                            detailModel.Modified = DateTime.Now;
                            detailModel.SourceColumnName = strDetail[0];
                            if (strDetail.Length > 1)
                            {
                                detailModel.TargetColumnName = strDetail[1];
                            }
                            BS_COMMUNICATION_DETAILBLL detailBLL = new BS_COMMUNICATION_DETAILBLL();
                            detailBLL.Add(detailModel);
                        }
                        BindCommunicationData();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('新增成功！',9);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('新增失败！',8);", true);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(hidCommunicationID.Value))
                    {
                        string currentUser = GetCurrentUserLoginId();
                        Guid communicationID = new Guid(hidCommunicationID.Value);
                        BS_COMMUNICATION_MAINBLL bll = new BS_COMMUNICATION_MAINBLL();
                        BS_COMMUNICATION_MAIN model = bll.GetModel(communicationID);
                        model.Name = txtName.Value;
                        model.Modified = DateTime.Now;
                        model.Editor = currentUser;

                        if (bll.Update(model))
                        {
                            string[] communication = communicationDetails.Split('|');
                            BS_COMMUNICATION_DETAILBLL detailBLL = new BS_COMMUNICATION_DETAILBLL();
                            detailBLL.DeleteAll(communicationID);
                            foreach (string details in communication)
                            {
                                string[] strDetail = details.Split(';');
                                BS_COMMUNICATION_DETAIL detailModel = new BS_COMMUNICATION_DETAIL();
                                detailModel.Author = currentUser;
                                detailModel.CommunicationID = communicationID;
                                detailModel.Created = DateTime.Now;
                                detailModel.Editor = currentUser;
                                detailModel.ID = Guid.NewGuid();
                                detailModel.Modified = DateTime.Now;
                                detailModel.SourceColumnName = strDetail[0];
                                if (strDetail.Length > 1)
                                {
                                    detailModel.TargetColumnName = strDetail[1];
                                }
                                detailBLL.Add(detailModel);
                            }
                            BindCommunicationData();
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('修改成功！',9);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('修改失败！',8);", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                trbig.Attributes.Add("style", "display:;");
                trsmall.Attributes.Add("style", "display:;");
                //BindSourceTemplate();
                txtName.Value = "";
                txtSelected.Text = "";
                DataTable dt = new DataTable();
                CommunicationDetailList.DataSource = dt;
                CommunicationDetailList.DataBind();
                ddlSourceTemplate.Enabled = true;
                ddlCATEGORY.SelectedIndex = 0;
                ddlsmallcategory.Items.Clear();
                ddlSourceTemplate.Items.Clear();
                ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
                ddlSourceTemplate.Items.Insert(0, new ListItem("请选择", ""));

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showDetail", "showDetail();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
            }
        }

        //刷新所有源模板信息
        protected void RefreshSourceTemplate(string templateID,BusinessSearchComponent bsbll)
        {
            bsbll.RefreshTemplateByGuid(templateID);
            DataTable dtsource = dc.GetCommunicationByTemplateID(templateID);
            if (dtsource != null && dtsource.Rows.Count > 0)
            {
                foreach (DataRow dr in dtsource.Rows)
                {
                    bsbll.RefreshTemplateByGuid(dr["SourceTemplateID"].ToString());
                }
            }
        }
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                string templateID = hidTemplateID.Value;
                if (!string.IsNullOrEmpty(templateID))
                {
                    DataTable dtTargetColumns = dc.GetDataColumnByTemplateID(templateID);
                    if (dtTargetColumns != null && dtTargetColumns.Rows.Count > 0)
                    {
                        if (dc.UpdateTemplateStatus(templateID))
                        {
                            if (!string.IsNullOrEmpty(templateID))
                            {
                                BusinessSearchComponent bsbll = new BusinessSearchComponent();
                                bsbll.RefreshTemplateStatusByGuid(templateID);
                                //bsbll.RefreshTemplateByGuid(templateID);
                                RefreshSourceTemplate(templateID, bsbll);

                            }
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('模板完成成功！',9);", true);
                            Page.Response.Redirect("TemplateManagement.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert1", "javascript:showMessage('模板完成失败！',8);", true);
                        }
                    }
                    else
                    {
                        //删除通信配置
                        dc.DeleteCommunicationByTemplateID(templateID);
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alerterror1", "javascript:showMessage('模板在您完成之前，已被其他人修改，请您重新配置,或联系管理员！',8);", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alerterror1", "javascript:showMessage('未获取到目标模板信息！',8);", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertException", "javascript:showMessage('操作发生错误：" + ex.Message.Replace("'", "").Replace("\"", "") + "！',8);", true);
                BaseComponent.Error(ex.Message);
            }
        }

        protected void lbtnPreView_Click(object sender, EventArgs e)
        {
            string templateid = hidTemplateID.Value;
            if (!string.IsNullOrEmpty(templateid))
            {
                try
                {
                    BusinessSearchComponent bsbll = new BusinessSearchComponent();
                    RefreshSourceTemplate(templateid, bsbll);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "previewred", "GotoPreView();", true);
                }catch(Exception ee){
                    BaseComponent.Error(ee.Message);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "previewerrords", "javascript:showMessage('模板生成失败，请检查配置！',8);", true);
                }
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "previewerror", "javascript:showMessage('未获取到模板信息,无法预览！',8);", true);
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
            ddlCATEGORY.Items.Insert(0, new ListItem("请选择", ""));
            ddlsmallcategory.Items.Insert(0, new ListItem("请选择", ""));
            ddlSourceTemplate.Items.Insert(0, new ListItem("请选择", ""));
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
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showDetailbig", "showDetail();", true);
        }

        protected void ddlsmallcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string smallcate = ddlsmallcategory.SelectedValue;
            if (!string.IsNullOrEmpty(smallcate))
            {
                BindSourceTemplate(ddlCATEGORY.SelectedValue, smallcate);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showDetailsmall", "showDetail();", true);
        }
    }
}

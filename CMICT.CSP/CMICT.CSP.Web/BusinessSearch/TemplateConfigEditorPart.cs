using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.BusinessSearch
{
    public class TemplateConfigEditorPart : EditorPart
    {
        protected DropDownList ddlTemplateID;
        protected DropDownList ddlCommunicationID;
        protected CheckBox cbIsAutoSearch;

        protected DropDownList ddlMainCategory;
        protected DropDownList ddlSubCategory;

        protected Label lbTableID;

        protected TextBox tbJSLink;

        protected TextBox tbTitle;
        protected override void CreateChildControls()
        {
            //ms-TPBorder width:100%

            TemplateConfigComponent Tempcom = new TemplateConfigComponent();


            ddlMainCategory = new DropDownList();
            ddlMainCategory.Attributes.Add("style", "width:100%");
            ddlMainCategory.AutoPostBack = true;
            ddlMainCategory.SelectedIndexChanged += ddlMainCategory_SelectedIndexChanged;
            DataTable dt = BaseComponent.GetUserLookupTypesByCode("模板");
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlMainCategory.DataSource = dt;
                ddlMainCategory.DataTextField = "LOOKUP_NAME";
                ddlMainCategory.DataValueField = "LOOKUP_CODE";
                ddlMainCategory.DataBind();
            }
            
            ddlSubCategory = new DropDownList();
            ddlSubCategory.Attributes.Add("style", "width:100%");
            ddlSubCategory.AutoPostBack = true;
            ddlSubCategory.SelectedIndexChanged += ddlSubCategory_SelectedIndexChanged;



            ddlTemplateID = new DropDownList();
            ddlTemplateID.AutoPostBack = true;
            ddlTemplateID.Attributes.Add("style", "width:100%");
            //DataTable template = Tempcom.GetEnableTemplate();
            //if (template != null && template.Rows.Count > 0)
            //{
            //    ddlTemplateID.DataSource = template;
            //    ddlTemplateID.DataTextField = "TemplateName";
            //    ddlTemplateID.DataValueField = "TemplateID";
            //    ddlTemplateID.DataBind();
            //}

            ddlTemplateID.SelectedIndexChanged += ddlTemplateID_SelectedIndexChanged;


            BindSmallCategory(ddlMainCategory.SelectedValue);
            BindTemplate();


            ddlCommunicationID = new DropDownList();
            ddlCommunicationID.Attributes.Add("style", "width:100%");
            BusinessSearch.BusinessSearch webpart = this.WebPartToEdit as BusinessSearch.BusinessSearch;
            BindCommunication(webpart.TemplateID);

            tbJSLink = new TextBox();
            tbJSLink.TextMode = TextBoxMode.MultiLine;
            tbJSLink.Attributes.Add("style", "width:100%");
            tbJSLink.Height = 200;

            tbTitle = new TextBox();
            tbTitle.Attributes.Add("style", "width:100%");

            lbTableID = new Label();
            lbTableID.Attributes.Add("style", "width:100%");
            lbTableID.Text = this.WebPartToEdit.ID + "_normaltab";

            cbIsAutoSearch = new CheckBox();
            this.Controls.Add(new LiteralControl("<b>标题</b><br/>"));
            this.Controls.Add(tbTitle);
            this.Controls.Add(new LiteralControl("<br/><b>报表ID</b>"));
            this.Controls.Add(lbTableID);
            this.Controls.Add(new LiteralControl("<br/><br/><b>报表大类</b>"));
            this.Controls.Add(ddlMainCategory);
            this.Controls.Add(new LiteralControl("<br/><br/><b>报表细类</b>"));
            this.Controls.Add(ddlSubCategory);
            this.Controls.Add(new LiteralControl("<br/><br/><b>模板名称</b>"));
            this.Controls.Add(ddlTemplateID);
            this.Controls.Add(new LiteralControl("<br/><br/><b>通信业务名称</b><br/>"));
            this.Controls.Add(ddlCommunicationID);
            this.Controls.Add(new LiteralControl("<br/><br/><b>是否自动加载数据</b><br/>"));
            this.Controls.Add(cbIsAutoSearch);
            this.Controls.Add(new LiteralControl("<br/><br/><b>javascript</b><br/>"));
            this.Controls.Add(tbJSLink);
            this.Controls.Add(new LiteralControl("<br/><br/><br/>"));
            //Parent.Controls[2].Visible = false;

            base.CreateChildControls();
            this.ChildControlsCreated = true;
        }

        void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemplateConfigComponent Tempcom = new TemplateConfigComponent();
            string tipcontent = string.Empty;
            DataTable dt = Tempcom.GetEnableTemplate(ddlMainCategory.SelectedValue, ddlSubCategory.SelectedValue);
            ddlTemplateID.DataSource = dt;
            ddlTemplateID.DataTextField = "TemplateName";
            ddlTemplateID.DataValueField = "TemplateID";
            ddlTemplateID.DataBind();
        }

        void ddlMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scategory = ddlMainCategory.SelectedValue;
            if (!string.IsNullOrEmpty(scategory))
            {
                BindSmallCategory(scategory);

                 BindTemplate();
            }
        }

        void BindTemplate(string templateid="")
        {
            TemplateConfigComponent Tempcom = new TemplateConfigComponent();
            string tipcontent = string.Empty;
            DataTable dt = Tempcom.GetEnableTemplate(ddlMainCategory.SelectedValue, ddlSubCategory.SelectedValue);
            //if()
            ddlTemplateID.DataSource = dt;
            ddlTemplateID.DataTextField = "TemplateName";
            ddlTemplateID.DataValueField = "TemplateID";
            ddlTemplateID.DataBind();
            if (templateid != "")
            {
                DataRow[] edt = dt.Select("TemplateID='" + templateid + "'");
                if (edt != null && edt.Length > 0)
                {
                    ddlTemplateID.SelectedValue = templateid;
                }
            }
        }

        private void BindSmallCategory(string scategory)
        {
            DataTable dt = BaseComponent.GetUserLookupValuesByType(scategory);
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlSubCategory.DataSource = dt;
                ddlSubCategory.DataTextField = "LOOKUP_VALUE_NAME";
                ddlSubCategory.DataValueField = "LOOKUP_VALUE";
                ddlSubCategory.DataBind();
            }
            
        }

        private void ddlTemplateID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCommunication(ddlTemplateID.SelectedValue);

        }

        private void BindCommunication(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ddlCommunicationID.DataSource = null;
                ddlCommunicationID.Items.Clear();
                ddlCommunicationID.DataBind();
                return;
            }
            CommunicationConfigComponent commuCom = new CommunicationConfigComponent();
            DataTable communication = commuCom.GetCommunicationByTemplateID(id);
            if (communication == null || communication.Rows.Count == 0)
            {
                ddlCommunicationID.DataSource = null;
                ddlCommunicationID.Items.Clear();
                ddlCommunicationID.DataBind();

                return;
            }
            ddlCommunicationID.DataSource = communication;
            ddlCommunicationID.DataTextField = "Name";
            ddlCommunicationID.DataValueField = "CommunicationID";
            ddlCommunicationID.DataBind();
        }
        public override bool ApplyChanges()
        {

            this.EnsureChildControls();

            BusinessSearch.BusinessSearch webpart = this.WebPartToEdit as BusinessSearch.BusinessSearch;

            if (webpart == null) return false;


            webpart.TemplateID = ddlTemplateID.SelectedValue;

            webpart.CommunicationID = ddlCommunicationID.SelectedValue;

            webpart.IsAutoSearch = cbIsAutoSearch.Checked ? "Y" : "N";

            webpart.JsScript = tbJSLink.Text;

            webpart.BusSearchTitle = tbTitle.Text;
            return true;


        }
        public override void SyncChanges()
        {

            EnsureChildControls();

            BusinessSearch.BusinessSearch webpart = this.WebPartToEdit as BusinessSearch.BusinessSearch;

            if (webpart == null) return;

            


            string templateID = webpart.TemplateID;

            string communicationID = webpart.CommunicationID;

            string isAuto=webpart.IsAutoSearch;

            string jsLink = webpart.JsScript;

            //ddlTemplateID.SelectedValue = templateID;
            ddlCommunicationID.SelectedValue = communicationID;
            cbIsAutoSearch.Checked = isAuto == "Y";
            tbJSLink.Text = jsLink;
            tbTitle.Text = webpart.BusSearchTitle;
            if (!string.IsNullOrEmpty(templateID))
            {
                
                BS_TEMPLATE_MAINBLL temp = new BS_TEMPLATE_MAINBLL();
                BS_TEMPLATE_MAIN model = temp.GetModel(Guid.Parse(templateID));
                if (model != null)
                {
                    ddlMainCategory.SelectedValue = model.BigCategory;

                    BindSmallCategory(model.BigCategory);
                    ddlSubCategory.SelectedValue = model.SmallCategory;


                    BindTemplate(templateID);
                }
            }
        }
    }
}

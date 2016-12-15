using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.TemplateInfoConfig
{
    [ToolboxItemAttribute(false)]
    public partial class TemplateInfoConfig : WebPart
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["templateID"]))
                {
                    trpage.Visible = true;
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

        private void ShowInfo(Guid TemplateID)
        {
            CMICT.CSP.BLL.BS_TEMPLATE_MAINBLL bll = new CMICT.CSP.BLL.BS_TEMPLATE_MAINBLL();
            CMICT.CSP.Model.BS_TEMPLATE_MAIN model = bll.GetModel(TemplateID);
            if (model != null)
            {
                this.txtTemplateName.Value = model.TemplateName;
                this.txtTemplateDesc.Value = model.TemplateDesc;
                this.txtPageSize.Value = model.PageSize.ToString();
                if (model.TemplateStatus == "禁用")
                {
                    chkDisabled.Checked = true;
                }
                hidDisabled.Value = model.TemplateStatus;
            }
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            BS_TEMPLATE_MAINBLL mbll = new BS_TEMPLATE_MAINBLL();
            CMICT.CSP.Model.BS_TEMPLATE_MAIN model = new CMICT.CSP.Model.BS_TEMPLATE_MAIN();
            string TemplateName = txtTemplateName.Value.Trim();
            string TemplateDesc = txtTemplateDesc.Value.Trim();
            string PageSize = txtPageSize.Value.Trim();
            string TemplateStatus = (chkDisabled.Checked == true ? "禁用" : "草稿");
            model.TemplateName = TemplateName;
            model.DiaplayType = "";
            model.TemplateDesc = TemplateDesc;
            model.Reminder = "";
            model.PageSize = Convert.ToDecimal(PageSize);
            model.ColumnSize = 0;
            model.TemplateStatus = "草稿";
            model.SQL = "";
            model.Created = DateTime.Now;
            model.Modified = DateTime.Now;
            model.Author = "";
            model.Editor = "";
            if (hidOperType.Value == "add")
            {
                if (!mbll.Add(model))
                {
                    Page.RegisterStartupScript("malert", "<script>alert('模板基本信息配置失败！');</script>");
                }
            }
            else
            {
                Guid TemplateID = Guid.Parse(Page.Request.QueryString["templateID"]);
                CMICT.CSP.Model.BS_TEMPLATE_MAIN modele = mbll.GetModel(TemplateID);
                model.TemplateID = TemplateID;
                model.Created = modele.Created;
                model.SQL = modele.SQL;
                model.Reminder = modele.Reminder;
                model.DiaplayType = modele.DiaplayType;
                model.ColumnSize = modele.ColumnSize;
                model.Editor = "";
                if (chkDisabled.Checked)
                {
                    model.TemplateStatus = "禁用";
                }
                else
                {
                    model.TemplateStatus = hidDisabled.Value;
                }
                if (!mbll.Update(model))
                {
                    Page.RegisterStartupScript("malert", "<script>alert('模板基本信息修改失败！');</script>");
                }
            }
        }

        
    }
}

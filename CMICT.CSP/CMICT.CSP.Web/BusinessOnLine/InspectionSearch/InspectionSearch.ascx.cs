using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model.InspectionModels;
using CMICT.CSP.Web.BusinessOnlineServices;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Web.UI;

namespace CMICT.CSP.Web.BusinessOnLine.InspectionSearch
{
    [ToolboxItemAttribute(false)]
    public partial class InspectionSearch : WebPart
    {
        BusinessOnLineComponent com = new BusinessOnLineComponent();

        private CUSTOMSINSPECTIONS customsInspections { get; set; }

        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InspectionSearch()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["blno"]))
            {
                BLNo.Value = Page.Request.QueryString["blno"];
                btn_Search_Click(null,null);
            }
               
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string containerNo = boxNo.Value;
            string blNo = BLNo.Value;
            if (string.IsNullOrWhiteSpace(containerNo) && string.IsNullOrWhiteSpace(blNo))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('箱号和提单号条件至少须输入一个！');", true);
                return;
            }
            QGCostcoSoapClient client = new QGCostcoSoapClient();
            string errorMsg = "";
            string errorCode = "";
            string result = "";

            
            string reserved = isReserved.Checked ? "Y" : "N";

            result = client.reserve(containerNo, blNo, reserved, "", out errorMsg, out errorCode);
            if (errorCode == "0")
            {
                customsInspections = com.GetCustomsInspections(result);
                DataSourceList.DataSource = customsInspections.CONTAINER.Take(20);
                DataSourceList.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();", true);
            }
            else
            {
                DataSourceList.DataSource = null;
                DataSourceList.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('" + errorMsg + "');", true);
            }


        }
    }
}

using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
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
            if (!Page.IsPostBack)
            {
                BindDataSource();
            }
        }

        protected void BindDataSource()
        {
            DataTable dt = dbll.GetDataSourceList();
            ddlDataSource.DataSource = dt;
            ddlDataSource.DataTextField = "SourceName";
            ddlDataSource.DataValueField = "SourceID";
            ddlDataSource.DataBind();
            ddlDataSource.Items.Insert(0, "空");
        }
    }
}

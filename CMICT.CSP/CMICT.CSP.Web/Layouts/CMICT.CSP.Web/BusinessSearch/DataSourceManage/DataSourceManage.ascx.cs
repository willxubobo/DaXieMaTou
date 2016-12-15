using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.BusinessSearch.DataSourceManage
{
    [ToolboxItemAttribute(false)]
    public partial class DataSourceManage : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DataSourceManage()
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
                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.NumericButtonCount = int.MaxValue;
                BindDataSourceList();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindDataSourceList();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindDataSourceList();
        }

        protected void BindDataSourceList()
        {
            int i, j;
            string SourceName = txtSourceName.Text.Trim();
            string SourceIP = txtSourceIP.Text.Trim();
            string DBName = txtDBName.Text.Trim();
            string ObjectType = ddlDataType.SelectedValue;
            string ObjectName = txtObjectName.Text;
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            DataSourceConfigComponent tmbll = new DataSourceConfigComponent();
            DataTable dt = tmbll.GetDataSourceList(SourceName, SourceIP, DBName, ObjectType, ObjectName,"", AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            AspNetPager1.RecordCount = i;
            DataSourceList.DataSource = dt;
            DataSourceList.DataBind();
        }
    }
}

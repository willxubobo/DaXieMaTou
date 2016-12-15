using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using NET.Framework.Common.CacheHelper;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TestWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class TestWebPart : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TestWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BaseComponent.Error("TEST00001");
        }
    }
}

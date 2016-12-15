using SP.Framework.Common.Commons;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Branding.WebParts.ImageRender
{
    [ToolboxItemAttribute(false)]
    public partial class ImageRender : WebPart
    {
        public string ImgType { get; set; }
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ImageRender()
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
            string src = "";
            string masterPage = Page.MasterPageFile;
            int i = masterPage.LastIndexOf("/");
            string masterPageName = masterPage.Substring(i+1).Split('.')[0];

            if (ImgType == "HOMEPAGE_IMG")
            {
                src = SYSLookup.GetLookupNameByValue("HOMEPAGE_IMG", masterPageName);
            }
            else if (ImgType == "CONTENTPAGE_IMG")
            {
                src = SYSLookup.GetLookupNameByValue("CONTENTPAGE_IMG", masterPageName);
            }
            imgRender.Src = src;
        }
    }
}

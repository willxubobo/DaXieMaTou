using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Branding.WebParts.BreadCrumbNav
{
    [ToolboxItemAttribute(false)]
    public partial class BreadCrumbNav : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public BreadCrumbNav()
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
            string iurl = BaseComponent.RedirectToSubSite(SPContext.Current.Web.CurrentUser);
            SPSecurity.RunWithElevatedPrivileges(delegate() {
                StringBuilder sb = new StringBuilder();
                sb.Append("<li><a class=\"breadLink\" runat=\"server\" id=\"index\" href=\"" + iurl + "\">无忧首页</a></li>");
                //sb.Append(GetMenuString());
                sb.Append("<li><a class=\"breadLink last\" href=\"" + Page.Request.Url + "\">" + Convert.ToString(SPContext.Current.Item["Title"]) + "</a></li>");
                //sb.Insert(0, "<li><a class=\"breadLink\" href=\"" + SPContext.Current.Web.Url + "\">" + SPContext.Current.Web.Title + "</a></li>");
                //SPWeb web;
                //using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                //{
                //    web = site.OpenWeb(SPContext.Current.Web.ID);
                //    while (!web.IsRootWeb)
                //    {
                //        sb.Insert(0, "<li><a class=\"breadLink\" href=\"" + web.ParentWeb.Url + "\">" + web.ParentWeb.Title + "</a></li>");
                //        web = web.ParentWeb;
                //    }
                //}
                //if (web != null)
                //    web.Dispose();
                breadui.InnerHtml = sb.ToString();
            });
        }

        private SiteMapNode GetSiteMapRootNodeOfCurrentWeb()
        {
            SiteMapProvider smp = PortalSiteMapProvider.GlobalNavSiteMapProvider;
            SiteMapNode rootMap = smp.RootNode;
            return rootMap;
        }
        
    }
}

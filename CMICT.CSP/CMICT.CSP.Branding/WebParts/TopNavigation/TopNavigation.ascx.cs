using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web;
using Microsoft.SharePoint;
using CMICT.CSP.BLL.Components;
using System.Configuration;

namespace CMICT.CSP.Branding.WebParts.TopNavigation
{
    [ToolboxItemAttribute(false)]
    public partial class TopNavigation : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TopNavigation()
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

            index.HRef = BaseComponent.RedirectToSubSite(SPContext.Current.Web.CurrentUser);

            Literal1.Text = GetMenuString();
            if (SPContext.Current.Web.CurrentUser == null)
            {
                linkUser.InnerText = "用户登录";
                linkUser.HRef = "/_layouts/CMICTRedirectPage.aspx";
                linkUser.Attributes.Add("class", "topLink loginLink userLoginName");
            }
            else
            {
                linkUser.InnerText = "欢迎您！" + SPContext.Current.Web.CurrentUser.Name;
                linkUser.Attributes.Add("class", "topLink loginLink userLoginName loginLinkImg ClickLink");
            }

        }
        private SiteMapNode GetSiteMapRootNodeOfCurrentWeb()
        {
            SiteMapProvider smp = PortalSiteMapProvider.GlobalNavSiteMapProvider;
            SiteMapNode rootMap = smp.RootNode;
            return rootMap;
        }

        /// <summary>
        /// get menu html
        /// </summary>
        /// <returns></returns>
        public string GetMenuString()
        {
            StringBuilder sbMenu = new StringBuilder();
            SiteMapNode smn = GetSiteMapRootNodeOfCurrentWeb();
            sbMenu.Append("");
            int i = 0;
            string NodeType = ConfigurationManager.AppSettings["SiteMapNodeType"].ToString();//菜单类型
            foreach (SiteMapNode node in smn.ChildNodes)
            {

                string strUrl = node.Url.ToString();
                string strTitle = node.Title.ToString();
                string strDescription = node.Description.ToString().Trim();
                if (!string.IsNullOrEmpty(strDescription))
                {
                    if (strDescription == NodeType)
                    {
                        continue;
                    }
                }
                sbMenu.Append("<li><a class='topLink' ");
                sbMenu.Append("href=\"" + strUrl + "\">");
                //if (!string.IsNullOrEmpty(strDescription))
                //{
                //    sbMenu.Append(strTitle + "</a>");
                //}
                //else
                //{
                    sbMenu.Append(strTitle + "</a>");

                //}
                sbMenu.Append("</li>");
                i++;
            }
            return sbMenu.ToString();
        }
    }
}

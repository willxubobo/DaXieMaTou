using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMICT.CSP.Branding.WebParts.SubSiteNavigation
{
    [ToolboxItemAttribute(false)]
    public partial class SubSiteNavigation : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SubSiteNavigation()
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

            Literal1.Text = GetMenuString();

        }

        private SiteMapNode GetSiteMapRootNodeOfCurrentWeb(string url)
        {

            SiteMapNode result;

            try
            {
                url = SPContext.Current.Site.Url + url;
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        //获取站点结构
                        PortalSiteMapProvider CombinedNavSiteMapProvider = PortalSiteMapProvider.CurrentNavSiteMapProviderNoEncode;
                        result = CombinedNavSiteMapProvider.FindSiteMapNode(web.ServerRelativeUrl);

                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }


        private SiteMapNode GetTopNodes()
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
            string url = BaseComponent.RedirectToSubSite(SPContext.Current.Web.CurrentUser);

            StringBuilder sbMenu = new StringBuilder();
            SiteMapNode smn = GetSiteMapRootNodeOfCurrentWeb(url);

            SiteMapNode top = GetTopNodes();
            sbMenu.Append("");

            List<NavigationNodeModel> models = new List<NavigationNodeModel>();
            foreach (SiteMapNode node in top.ChildNodes)
            {
                if (node.ChildNodes.Count == 0)
                    continue;
                NavigationNodeModel model = new NavigationNodeModel();
                model.Title = node.Title;
                model.Url = node.Url;
                model.ChildNodes = new List<NavigationNodeModel>();
                foreach (SiteMapNode subNode in node.ChildNodes)
                {
                    NavigationNodeModel subModel = new NavigationNodeModel();
                    subModel.Title = subNode.Title;
                    subModel.Url = subNode.Url;
                    model.ChildNodes.Add(subModel);
                }
                models.Add(model);
            }
            if (smn != null)
            {
                foreach (SiteMapNode node in smn.ChildNodes)
                {
                    var SelectModel = models.Where(p => p.Title == node.Title).FirstOrDefault();
                    if (SelectModel == null)
                    {
                        NavigationNodeModel model = new NavigationNodeModel();
                        model.Title = node.Title;
                        model.Url = node.Url;
                        model.ChildNodes = new List<NavigationNodeModel>();
                        foreach (SiteMapNode subNode in node.ChildNodes)
                        {
                            NavigationNodeModel subModel = new NavigationNodeModel();
                            subModel.Title = subNode.Title;
                            subModel.Url = subNode.Url;
                            model.ChildNodes.Add(subModel);
                        }
                        models.Add(model);
                    }
                    else
                    {
                        foreach (SiteMapNode subNode in node.ChildNodes)
                        {
                            NavigationNodeModel subModel = new NavigationNodeModel();
                            subModel.Title = subNode.Title;
                            subModel.Url = subNode.Url;
                            SelectModel.ChildNodes.Add(subModel);
                        }
                    }
                }
            }
            foreach (var node in models)
            {
                string strUrl = node.Url.ToString();
                string strTitle = node.Title.ToString();
                //string strDescription = node.Description.ToString();
                if (string.IsNullOrEmpty(strUrl))
                {
                    strUrl = "#";
                }
                sbMenu.Append("<li><a class='mainLink'");
                sbMenu.Append("href=\"" + strUrl + "\">");
                //if (!string.IsNullOrEmpty(strDescription))
                //{
                sbMenu.Append(strTitle + "</a>");
                //}
                //else
                //{
                //    sbMenu.Append( strTitle + "</a>");

                //}
                if (node.ChildNodes.Count > 0)
                {
                    sbMenu.Append("<ul class='subNav'>");
                    foreach (var nodechild in node.ChildNodes)
                    {
                        string strChildUrl = nodechild.Url.ToString();
                        string strChildTitle = nodechild.Title.ToString();
                        // string strChildDescription = nodechild.Description.ToString();
                        sbMenu.Append("<li><a class='subLink'");
                        if (strChildUrl.Contains("http"))
                        {
                            sbMenu.Append(" target=\"_blank\" ");
                        }
                        sbMenu.Append("href=\"" + strChildUrl + "\">");
                        //if (!string.IsNullOrEmpty(strChildDescription))
                        //{
                        //    sbMenu.Append("<span class=\"iconMain\" style=\"background: url(" + strChildDescription +
                        //          ") no-repeat center;\"></span>" + strChildTitle + "</a>");
                        //}
                        //else
                        //{
                        sbMenu.Append(strChildTitle + "</a>");
                        //}
                    }
                    sbMenu.Append("</ul>");
                }
                sbMenu.Append("</li>");
            }
            return sbMenu.ToString();
        }
    }
}

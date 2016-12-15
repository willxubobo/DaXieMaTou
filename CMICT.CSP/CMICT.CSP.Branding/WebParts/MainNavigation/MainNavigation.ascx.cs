using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint.Navigation;
using System.Linq;
using System.Collections.Generic;
using CMICT.CSP.Model;

namespace CMICT.CSP.Branding.WebParts.MainNavigation
{
    [ToolboxItemAttribute(false)]
    public partial class MainNavigation : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MainNavigation()
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
            int count = 0;
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
                string strTitle = node.Title.ToString();
                sbMenu.Append(count % 2 == 0 ? "<li class='odd'>" : "<li  class='even'>");

                sbMenu.Append("<p class='contentTitle'>" + strTitle + "</p>");
                if (node.ChildNodes.Count > 0)
                {
                    sbMenu.Append("<div class='txtScroll-left'><div class='bd'>"
                                    + "<ul class='infoList'>");
                    int i = 0;
                    foreach (var nodechild in node.ChildNodes)
                    {
                        if (i % 5 == 0)
                        {
                            sbMenu.Append("<li>");
                        }

                        string strChildUrl = nodechild.Url.ToString();
                        sbMenu.Append("<a class='contentLink' href='" + strChildUrl + "'>");
                        sbMenu.Append(nodechild.Title);
                        sbMenu.Append("</a>");

                        if (i % 5 == 4)
                            sbMenu.Append("</li>");
                        i++;
                    }
                    if (i % 5 != 0)
                        sbMenu.Append("</li>");
                    sbMenu.Append("</ul></div><div class='hd'");
                    if (node.ChildNodes.Count <= 5)
                        sbMenu.Append(" style='display:none' ");
                    sbMenu.Append("><a class='next' href='javascript:;'></a>"
                    + @"<ul class='num'><li>1</li><li>2</li><li>3</li>");
                    sbMenu.Append("</ul><a class='prev' href='javascript:;'></a><span class='pageState'></span></div></div>");
                }
                sbMenu.Append("</li>");
                count++;
            }
            return sbMenu.ToString();
        }
    }
}

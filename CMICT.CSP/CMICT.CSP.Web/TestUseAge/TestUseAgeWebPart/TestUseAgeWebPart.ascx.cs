using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CMICT.CSP.Model;
using CMICT.CSP.BLL.Components;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace CMICT.CSP.Web.TestUseAge.TestUseAgeWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class TestUseAgeWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TestUseAgeWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = BaseComponent.RedirectToSubSite(SPContext.Current.Web.CurrentUser);
        }

    }
}

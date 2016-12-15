using Microsoft.SharePoint;
using System;
using System.Web.UI.WebControls;

namespace SP.Framework.Common.Controls
{
    public class SiteAdminUIPanel : Panel
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (SPContext.Current.Web.CurrentUser == null || !(SPContext.Current.Web.CurrentUser.IsSiteAdmin))
            { Visible = false; }
            else
            { Visible = true; }
        }
    }
}

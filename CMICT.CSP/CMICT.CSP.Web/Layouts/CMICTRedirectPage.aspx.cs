using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using CMICT.CSP.BLL.Components;

namespace CMICT.CSP.Web.Layouts.CMICT.CSP.Web
{
    public partial class CMICTRedirectPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(BaseComponent.RedirectToSubSite(SPContext.Current.Web.CurrentUser));
        }
    }
}

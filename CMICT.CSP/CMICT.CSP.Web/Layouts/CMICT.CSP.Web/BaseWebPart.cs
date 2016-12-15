using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMICT.CSP.Web
{
    public class BaseWebPart : WebPart
    {
        /// <summary>
        /// Getts the current user.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserLoginId()
        {
            SPUser currentSpUser = SPContext.Current.Web.CurrentUser;
            string currentSpUserLoginId = currentSpUser.LoginName;
            if (currentSpUserLoginId.ToLower() == "sharepoint\\system")
            {
                currentSpUserLoginId = HttpContext.Current.User.Identity.Name;
            }
            if (currentSpUserLoginId.Contains("|"))
            {
                currentSpUserLoginId = currentSpUserLoginId.Split('|')[1];
            }

            return currentSpUserLoginId;
        }
    }
}

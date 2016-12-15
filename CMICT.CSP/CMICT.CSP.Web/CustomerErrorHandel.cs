using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMICT.CSP.Web
{
    public class CustomerErrorHandel : IHttpModule
    {
        private readonly static object _mutex = new object();
        private static bool _initialized = false;

        [DllImport("advapi32.dll")]
        public static extern uint EventActivityIdControl(uint controlCode, ref  Guid activityId);
        public const uint EVENT_ACTIVITY_CTRL_GET_ID = 1;
        public static Guid GetCurrentCorrelationToken()
        {
            Guid g = Guid.Empty;
            EventActivityIdControl(EVENT_ACTIVITY_CTRL_GET_ID, ref  g);
            return g;
        }

        public void Init(HttpApplication application)
        {
            if (!_initialized)
                lock (_mutex)
                    if (!_initialized)
                    {
                        Application_Start();
                    }

            application.Error += new EventHandler(Application_Error);
            return;
        }

        private void Application_Start()
        {
            _initialized = true;
        }

        public void Application_Error(object sender, EventArgs e)
        {
            try
            {


                Guid errorGuid = GetCurrentCorrelationToken();

                BaseComponent.Error("Apploication_ErrorID： " + errorGuid);

                HttpContext ctx = HttpContext.Current;
                if (SPContext.Current != null)
                {
                    string path = System.Configuration.ConfigurationSettings.AppSettings["SiteRootUrl"].ToString();

                    Exception ex = ctx.Server.GetLastError();
                    if (ex == null)
                    {
                        int count = ctx.AllErrors.Length;
                        if (count > 0)
                        {
                            ex = ctx.AllErrors[count - 1];
                        }
                    }
                    string error_mes = "";
                    if (ex != null)
                    {
                        error_mes = ex.Message;
                        if (ex.InnerException != null)
                        {
                            Exception iex = ex.InnerException;
                            error_mes = iex.Message;
                        }
                    }

                    //一定要记得清除错误
                    ctx.Server.ClearError();
                    ctx.ClearError();

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string pageUrl = "";
                        using (SPSite spSite = new SPSite(path))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                PublishingWeb publishingWeb = null;
                                if (spWeb != null && PublishingWeb.IsPublishingWeb(spWeb))
                                {
                                    publishingWeb = PublishingWeb.GetPublishingWeb(spWeb);
                                    SPList spList = publishingWeb.PagesList;

                                    SPFile spFile = spWeb.GetFile("" + spList.RootFolder.ServerRelativeUrl + "/errorpage/errorpage.aspx");
                                    if (spFile != null)
                                    {
                                        pageUrl = spFile.ServerRelativeUrl;
                                    }
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(pageUrl))
                        {
                            ctx.Response.Redirect(pageUrl + "?errorID=" + errorGuid.ToString());
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void Dispose()
        {
            return;
        }
    }
}

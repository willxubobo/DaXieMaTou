using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using CMICT.CSP.BLL.Components;
using System.Net;
using Microsoft.SharePoint;
using NET.Framework.Common.LogHelper;
using System.Configuration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

namespace CMICT.CSP.BLL.TimerJobModels
{
    public class WarmUpTimerJobCS : SPJobDefinition
    {
        //private const string AZSynonymList = "AZSynonymList";

        //private const string SynonyColumn = "Synonym";


        public WarmUpTimerJobCS()
            : base()
        {
        }

        public WarmUpTimerJobCS(string _timername, SPWebApplication _wp)
            : base(_timername, _wp, null, SPJobLockType.Job)
        {
            this.Title = "CMICT WarmUp Timer Job";
        }

        public void log(string msg)
        {
            SPDiagnosticsService.Local.WriteTrace(0u, new SPDiagnosticsCategory("CMICT", TraceSeverity.Medium, EventSeverity.Information), TraceSeverity.Medium, msg, new object[0]);

        }
        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);
            try
            {

                log("begin warmup:" + DateTime.Now.ToLongTimeString());
                //string uname=ConfigurationManager.AppSettings["WarmUpUName"].ToString();
                //string pwd = ConfigurationManager.AppSettings["WarmUpPwd"].ToString();
                //string wdomain = ConfigurationManager.AppSettings["WarmUpDomain"].ToString();
                //    SPSecurity.RunWithElevatedPrivileges(delegate()
                //{
                SPFarm farm = SPFarm.Local;
                foreach (var server in farm.Servers)
                {
                    if (server.Role == SPServerRole.Invalid)
                        continue;
                    //layouts page
                    log(server.Address + "-_layouts/15/CMICTRedirectPage.aspx" + " :begin warmup:" + DateTime.Now.ToLongTimeString());
                    getwebpage("http://" + server.Address + "/_layouts/15/CMICTRedirectPage.aspx");
                    log(server.Address + "-_layouts/15/CMICTRedirectPage.aspx" + " :end warmup:" + DateTime.Now.ToLongTimeString());
                    //server.Address;
                    foreach (SPService service in farm.Services)
                    {
                        if (service is SPWebService)
                        {
                            SPWebService webService = (SPWebService)service;
                            foreach (SPWebApplication webApp in webService.WebApplications)
                            {
                                foreach (SPSite spsite in webApp.Sites)
                                {
                                    foreach (SPWeb web in spsite.AllWebs)
                                    {
                                        try
                                        {
                                            if (web.RootFolder.WelcomePage.ToLower().Contains(".aspx"))
                                            {
                                                log(server.Address + "-" + web.Title + " :begin warmup:" + DateTime.Now.ToLongTimeString());
                                                // NetworkCredential nec = new NetworkCredential(uname, pwd, wdomain);
                                                string relativeurl = ((web.ServerRelativeUrl == "" || web.ServerRelativeUrl == "/") ? "/" : web.ServerRelativeUrl + "/");
                                                getwebpage("http://" + server.Address + relativeurl + web.RootFolder.WelcomePage);

                                                log(server.Address + "-" + web.Title + " :end warmup:" + DateTime.Now.ToLongTimeString());
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log(server.Address + "-" + web.Title + "warmup error:" + ex.ToString());
                                            break;
                                        }
                                        finally
                                        {
                                            web.Dispose();

                                        }
                                    }
                                    spsite.Dispose();
                                }
                            }
                        }
                    }

                }
                log("end warmup:" + DateTime.Now.ToLongTimeString());
                //});
            }
            catch (Exception ex)
            {
                log("warmup error:" + ex.ToString());
            }
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public void getwebpage(string url, NetworkCredential cred = null)
        {
            using (WebClient wc = new WebClient())
            {
                if (cred == null)
                {
                    cred = new NetworkCredential();
                }
                //wc.Credentials = cred;
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                wc.DownloadString(url);
            }
        }
    }
}

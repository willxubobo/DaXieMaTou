using NET.Framework.Common.LogHelper;
using SP.Framework.Common.Commons;
using System.Data;
using Microsoft.SharePoint;
using CamlexNET;
using SP.Framework.DAL;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CMICT.CSP.BLL.Components
{
    public class BaseComponent
    {

        public static Log4NetLogger logger = (Log4NetLogger)LoggerManager.GetLogger("CMICT");

        private static string webName = ConfigurationSettings.AppSettings["CMICTSPWebUrl"].ToString(); 

        public static void Error(string message)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate() {
                logger.Error(message);
            });
            
        }

        public static void Info(string message)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                logger.Info(message);
            });
            
        }

        public static DataTable GetLookupValuesByType(string lookupType)
        {
            #region mock data
            //DataTable dt=new DataTable();
            //dt.Columns.Add("LOOKUP_VALUE");
            //dt.Columns.Add("LOOKUP_VALUE_NAME");
            //if (lookupType == "BS_TEMPLATE_STATUS")
            //{
            //    DataRow dr1 = dt.NewRow();
            //    dr1["LOOKUP_VALUE"]="ENABLE";
            //    dr1["LOOKUP_VALUE_NAME"]="启用";
            //    dt.Rows.Add(dr1);

            //    DataRow dr2 = dt.NewRow();
            //    dr2["LOOKUP_VALUE"] = "DISABLE";
            //    dr2["LOOKUP_VALUE_NAME"] = "禁用";
            //    dt.Rows.Add(dr2);
            //}
            //return dt;
            #endregion
            return SYSLookup.GetLookupValuesByType(lookupType);

        }

        public static string GetLookupNameBuValue(string type, string value)
        {
            return SYSLookup.GetLookupNameByValue(type, value);
        }

        public static DataTable GetLookupTypesByCode(string code)
        {
            return SYSLookup.GetLookupTypesByCode(code);
        }

        public static string GetTypeNameByCode(string type)
        {
            return SYSLookup.GetTypeNameByCode(type);
        }

        public static DataTable GetUserLookupTypesByCode(string p)
        {
            return SYSLookup.GetUserLookupTypesByCode(p);
        }

        public static DataTable GetUserLookupValuesByType(string bigcode)
        {
            return SYSLookup.GetUserLookupValuesByType(bigcode);
        }

        public static string GetUserLookupNameBuValue(string type, string value)
        {
            return SYSLookup.GetUserLookupNameByValue(type, value);
        }


        public static string GetUserTypeNameByCode(string type)
        {
            return SYSLookup.GetUserTypeNameByCode(type);
        }

        public static string RedirectToSubSite(SPUser user)
        {
            DateTime startd = DateTime.Now;
            Info("start redirecttosubsite:" + startd.ToLongTimeString());
            string url = string.Empty;
            if (user == null)
            {
                url = "/";
            }
            else
            {
                string listName = "/Lists/NAVIGATION_MANAGEMENT";

                string query = @"<Where>
                          <Eq>
                            <FieldRef Name='LoginUser' LookupId='True' />
                            <Value Type='User'>{0}</Value>
                          </Eq>
                        </Where>";
                query = string.Format(query, user.ID);
                Info("start read site:" + DateTime.Now.ToLongTimeString());
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        Info("start read site id:" +site.ID.ToString()+"  time:"+ DateTime.Now.ToLongTimeString());
                        using (SPWeb web = site.OpenWeb(webName))
                        {
                            Info("start read web name:" + web.Name + "  time:" + DateTime.Now.ToLongTimeString());
                            string ListUrl = webName + listName;
                            SPQuery spQuery=new SPQuery();
                            spQuery.Query=query;
                            Info("start get web splist url:" + ListUrl + "  time:" + DateTime.Now.ToLongTimeString());
                            SPList list=  web.GetList(ListUrl);
                            Info("end get web splist url:" + ListUrl + "  time:" + DateTime.Now.ToLongTimeString());
                            if (list.ItemCount > 0)
                            {
                                Info("start get web splist table time:" + DateTime.Now.ToLongTimeString());
                                DataTable dt = list.GetItems(spQuery).GetDataTable();
                                Info("end get web splist table time:" + DateTime.Now.ToLongTimeString());
                                //SPHelper.GetListData(web, listName, query);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    Info("start read table row time:" + DateTime.Now.ToLongTimeString());
                                    url = Convert.ToString(dt.Rows[0]["RedirectUrl"]).Split(',')[0];
                                    if (url.StartsWith(site.Url))
                                        url = url.Substring(site.Url.Length);
                                    Info("end read table row time:" + DateTime.Now.ToLongTimeString());
                                }
                                else
                                {
                                    Info("start get GetLookupNameBuValue time:" + DateTime.Now.ToLongTimeString());
                                    url = BaseComponent.GetLookupNameBuValue("REDIRECT_URL", "REDIRECT_URL");
                                    Info("end get GetLookupNameBuValue time:" + DateTime.Now.ToLongTimeString());
                                }
                            }
                            else
                            {
                                Info("start not get list data get GetLookupNameBuValue time:" + DateTime.Now.ToLongTimeString());
                                url = BaseComponent.GetLookupNameBuValue("REDIRECT_URL", "REDIRECT_URL");
                                Info("end not get list data get GetLookupNameBuValue time:" + DateTime.Now.ToLongTimeString());
                            }
                            Info("end read web name:" + web.Name + "  time:" + DateTime.Now.ToLongTimeString());
                        }
                        Info("end read site id:" + site.ID.ToString() + "  time:" + DateTime.Now.ToLongTimeString());
                    }
                });
                Info("end read site:" + DateTime.Now.ToLongTimeString());
            }
            DateTime endd = DateTime.Now;
            TimeSpan tsStart = new TimeSpan(startd.Ticks);
            TimeSpan tsEnd = new TimeSpan(endd.Ticks);
            TimeSpan ts = tsEnd.Subtract(tsStart).Duration();
            Info("end redirecttosubsite:" + endd.ToLongTimeString()+" 总共花费时间："+ts.Seconds+"秒");
            if (string.IsNullOrEmpty(url))
                url = "/";
            return url;
        }

        public static string GetDisplayName(string loginName)
        {
            string name = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPContext.Current.Site.RootWeb.AllowUnsafeUpdates = true;
                    SPUser user = SPContext.Current.Site.RootWeb.EnsureUser(loginName);
                    if (user != null)
                    {
                        name = user.Name;
                    }
                    SPContext.Current.Site.RootWeb.AllowUnsafeUpdates = false;
                });
                
            }
            catch (Exception ex)
            {
                Error(ex.ToString());
                name = loginName;
            }
            return name;
        }

        /// <summary>
        /// 获取新增报表类别weburl
        /// </summary>
        /// <returns></returns>
        public static string GetReportTypeUrl()
        {
            return "/cmict/Pages/SysManagement/CategoryConfig.aspx";
        }

        /// <summary>
        /// Getts the current user.2015-11-23 will.xu
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserLoginName()
        {
            SPUser currentSpUser = SPContext.Current.Web.CurrentUser;
            if (currentSpUser == null)
                return "";
            string currentSpUserLoginId = currentSpUser.LoginName;
            if (currentSpUserLoginId.ToLower() == "sharepoint\\system")
            {
                currentSpUserLoginId = HttpContext.Current.User.Identity.Name;
            }
            if (currentSpUserLoginId.Contains("|"))
            {
                currentSpUserLoginId = currentSpUserLoginId.Split('|')[1];
            }
            if (currentSpUserLoginId.Contains("\\"))
            {
                currentSpUserLoginId = currentSpUserLoginId.Split('\\')[1];
            }
            return currentSpUserLoginId;
        }

        public static DataTable GetBigCateData(string listurl)
        {
            return SYSLookup.GetLookupTable(listurl);
        }

        public static string GetBigCateName(DataTable UserLookupTypeTable, string type)
        {
            int typeValue = 0;
            if (int.TryParse(type, out typeValue) && UserLookupTypeTable!=null)
            {
                var rows = UserLookupTypeTable.AsEnumerable().Where(p => p.Field<int>("ID") == typeValue).ToList();
                if (rows != null && rows.Count() > 0)
                    return rows[0]["LinkTitle"].ToString();
            }
            return "";
        }

        public static string GetStatusName(DataTable LookupTable,string type,string value)
        {
            if (LookupTable != null)
            {
                var rows = LookupTable.AsEnumerable().Where(p => p.Field<string>("LOOKUP_CODE_LINE") == type && p.Field<string>("LOOKUP_VALUE") == value).ToList();
                if (rows != null && rows.Count() > 0)
                    return rows[0]["LOOKUP_VALUE_NAME"].ToString();
            }
            return "";
        }

        public static string GetSmallCateName(DataTable LookupTable, string type, string value)
        {
            int typeValue = 0;
            if (LookupTable != null && int.TryParse(value, out typeValue))
            {
                var rows = LookupTable.AsEnumerable().Where(p => p.Field<int>("ID") == typeValue).ToList();
                if (rows != null && rows.Count() > 0)
                    return rows[0]["LinkTitle"].ToString();
            }
            return "";
        }
    }
}

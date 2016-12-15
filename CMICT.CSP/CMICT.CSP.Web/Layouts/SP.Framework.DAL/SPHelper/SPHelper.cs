using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SP.Framework.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SP.Framework.DAL
{
    public class SPHelper : SPFrameworkBase
    {
        public static SPList GetList(SPWeb web, Guid listId)
        {
            return web.Lists.GetList(listId, true);
            
        }

        public static SPList GetList(SPWeb web, string listName)
        {
            return web.GetList(GetListPath(web, listName));
        }

        private static string GetListPath(SPWeb web, string listName)
        {
            string result = listName;
            if (!listName.ToLower().StartsWith(web.ServerRelativeUrl.ToLower()))
            {
                result = string.Format("{0}/{1}", (web.ServerRelativeUrl == "/") ? "" : web.ServerRelativeUrl, listName);
            }
            return result;
        }

        public static DataTable GetListData(SPWeb web, string listUrl, SPQuery query)
        {
            return GetListItems(web, listUrl, query).GetDataTable();
        }

        public static SPListItem GetListItem(SPWeb web, string listUrl, int Id)
        {
            return GetList(web, listUrl).GetItemById(Id);
        }

        public static SPListItemCollection GetListItems(SPWeb web, string listUrl, SPQuery query)
        {
            if (query != null)
            {
                return GetList(web, listUrl).GetItems(query);
            }
            return GetList(web, listUrl).GetItems(new string[0]);
        }

        

        public static DataTable GetListViewData(SPWeb web, string listUrl, string viewName)
        {
            SPList list = GetList(web, listUrl);
            SPView view = GetView(list, viewName);
            SPQuery sPQuery = new SPQuery();
            sPQuery.ViewFields=view.ViewFields.SchemaXml;
            sPQuery.Query=view.Query;
            return list.GetItems(sPQuery).GetDataTable();
        }

        public static SPView GetView(SPList list, string title)
        {
            SPView result = list.DefaultView;
            if (!string.IsNullOrEmpty(title))
            {
                IEnumerable<SPView> source =
                    from SPView p in list.Views
                    where p.Title == title
                    select p;
                if (source.Count<SPView>() >= 1)
                {
                    result = list.Views[title];
                }
            }
            return result;
        }

        public static int GetInt(SPListItem item, string fieldName)
        {
            int result = 0;
            if (item[fieldName] != null)
            {
                result = int.Parse(item[fieldName].ToString());
            }
            return result;
        }
        public static double GetDouble(SPListItem item, string fieldName)
        {
            double result = 0.0;
            if (item[fieldName] != null)
            {
                result = double.Parse(item[fieldName].ToString());
            }
            return result;
        }
        public static bool GetBool(SPListItem item, string fieldName)
        {
            bool result = false;
            if (item[fieldName] != null)
            {
                result = bool.Parse(item[fieldName].ToString());
            }
            return result;
        }
        public static string GetString(SPListItem item, string fieldName)
        {
            string result = string.Empty;
            if (item[fieldName] != null)
            {
                result = item[fieldName].ToString();
            }
            return result;
        }
        public static DateTime GetDateTime(SPListItem item, string fieldName)
        {
            DateTime result = DateTime.MinValue;
            if (item[fieldName] != null)
            {
                result = DateTime.Parse(item[fieldName].ToString());
            }
            return result;
        }

        public static SPFieldUserValue GetSPFieldUserValue(SPListItem item, string fieldName)
        {
            SPField field = item.Fields.GetField(fieldName);
            return (SPFieldUserValue)field.GetFieldValue(string.Concat(item[fieldName]));
        }

        public static List<string> GetLoginsOfGroupMembers(string group, SPWeb curreweb, bool recursive)
        {
            List<string> list = new List<string>();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite sPSite = new SPSite(curreweb.Site.ID))
                {
                    using (SPWeb sPWeb = sPSite.OpenWeb(curreweb.ID))
                    {
                        try
                        {
                            SPPrincipalInfo[] principalsInGroup;
                            try
                            {
                                bool flag;
                                principalsInGroup = SPUtility.GetPrincipalsInGroup(sPWeb, group, 2147483646, out flag);
                            }
                            catch (NullReferenceException)
                            {
                                throw new Exception("Error querying for group members. Ensure the connectivity between the application pool account and the domain that is being queried is configured correctly.");
                            }
                            SPPrincipalInfo[] array = principalsInGroup;
                            for (int i = 0; i < array.Length; i++)
                            {
                                SPPrincipalInfo sPPrincipalInfo = array[i];
                                if (recursive)
                                {
                                    if (sPPrincipalInfo.PrincipalType == SPPrincipalType.User)
                                    {
                                        list.Add(sPPrincipalInfo.LoginName.ToLower());
                                    }
                                    else
                                    {
                                        list.AddRange(GetLoginsOfGroupMembers(sPPrincipalInfo.LoginName, sPWeb, true));
                                    }
                                }
                                else
                                {
                                    list.Add(sPPrincipalInfo.LoginName.ToLower());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            });
            return list;
        }

    }
}

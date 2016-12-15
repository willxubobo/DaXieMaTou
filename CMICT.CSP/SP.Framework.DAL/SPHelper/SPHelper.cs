using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SP.Framework.DAL
{
    public class SPHelper
    {
        public static SPList GetList(SPWeb web, Guid listId)
        {
            return web.Lists.GetList(listId, true);

        }

        public static SPList GetList(SPWeb web, string listName)
        {
            return web.Lists.TryGetList(listName);
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

        public static DataTable GetListData(SPWeb web, string listUrl, string query)
        {
            SPQuery spq = new SPQuery();
            spq.Query = query;
            return GetListItems(web, listUrl, spq).GetDataTable();
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
            sPQuery.ViewFields = view.ViewFields.SchemaXml;
            sPQuery.Query = view.Query;
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

        #region 提升权限获取网站集数据
        public static SPListItemCollection GetSPListItems(Guid siteId, string webUrl, string listname,
           string querystr, string viewFieldsstr, uint rowLimit)
        {
            SPListItemCollection itemCollection = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists.TryGetList(listname);
                            if (list == null) return;
                            SPQuery query = new SPQuery();
                            query.Query = querystr;
                            query.ViewFields = viewFieldsstr;
                            if (rowLimit != 0)
                                query.RowLimit = rowLimit;
                            itemCollection = list.GetItems(query);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itemCollection;
        }

        public static SPListItemCollection GetAllSPListItems(Guid siteId, string webUrl, string listname,
          string querystr, string viewFieldsstr, uint rowLimit)
        {
            SPListItemCollection itemCollection = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPList list = web.Lists.TryGetList(listname);
                            if (list == null) return;
                            SPQuery query = new SPQuery();
                            query.Query = querystr;
                            query.ViewFields = viewFieldsstr;
                            query.ViewAttributes = "Scope=\"Recursive\"";
                            if (rowLimit != 0)
                                query.RowLimit = rowLimit;
                            itemCollection = list.GetItems(query);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itemCollection;
        }


        public static DataTable GetSPListItemsBySiteSelf(Guid siteId, string webUrl, string listTemplate,
            string querystr, string viewFieldsstr, uint rowLimit)
        {
            DataTable dataTable = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            SPSiteDataQuery query = new SPSiteDataQuery();
                            query.Lists = "<Lists ServerTemplate='" + listTemplate + "' />";
                            query.Webs = "<Webs Scope='Recursive' />";
                            query.Query = querystr;
                            query.ViewFields = viewFieldsstr;
                            if (rowLimit != 0)
                                query.RowLimit = rowLimit;
                            dataTable = web.GetSiteData(query);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }
        #endregion

        /// <summary>
        /// 给Item授权
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="web"></param>
        /// <param name="ListItem"></param>
        /// <param name="DelegateName"></param>
        public static void DelegateForListItem(SPPrincipal principal, SPWeb web, SPListItem ListItem, string DelegateName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    using (SPSite site = new SPSite(web.Site.ID))
                    {
                        using (SPWeb _web = site.OpenWeb(web.ServerRelativeUrl))
                        {
                            SPListItem _listitem = _web.Lists[ListItem.ParentList.ID].GetItemById(ListItem.ID);
                            _web.AllowUnsafeUpdates = true;
                            SPRoleAssignment sra = new SPRoleAssignment(principal);
                            SPRoleDefinition srd = _web.RoleDefinitions[DelegateName];
                            sra.RoleDefinitionBindings.Add(srd);
                            _listitem.BreakRoleInheritance(true, false);
                            _listitem.RoleAssignments.Add(sra);
                            _listitem.Update();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }

        public static int GetDelegateCount(SPPrincipal principal, SPWeb web, SPListItem ListItem)
        {
            int permissionCount = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    if (principal != null)
                    {
                        using (SPSite site = new SPSite(web.Site.ID))
                        {
                            using (SPWeb _web = site.OpenWeb(web.ServerRelativeUrl))
                            {
                                SPListItem _listitem = _web.Lists[ListItem.ParentList.ID].GetItemById(ListItem.ID);
                                _web.AllowUnsafeUpdates = true;
                                SPRoleAssignment role = _listitem.RoleAssignments.GetAssignmentByPrincipal(principal);

                                permissionCount = role.RoleDefinitionBindings.Count;


                            }
                        }

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                }
            });

            return permissionCount;

        }

        /// <summary>
        /// 根据组名获取SPGroup对象
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static SPGroup GetSPGroupByName(string siteUrl, string groupName, SPUser groupUser)
        {
            SPGroup group = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            web.AllowUnsafeUpdates = true;

                            if (!IsExistGroup(web, groupName))
                            {
                                web.SiteGroups.Add(groupName, groupUser, null, groupName);//新建组

                                if (IsExistGroup(web, groupName))
                                {
                                    group = web.SiteGroups.GetByName(groupName);
                                }
                            }
                            else
                            {
                                group = web.SiteGroups.GetByName(groupName);
                            }

                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });
                return group;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsExistGroup(SPWeb web, string groupname)
        {
            try
            {
                foreach (SPGroup grouplist in web.SiteGroups)//判断组是否存在
                {
                    if (grouplist.ToString().ToLower() == groupname.ToLower())
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除指定角色的权限
        /// </summary>
        /// <param name="principal">角色</param>
        /// <param name="web"></param>
        /// <param name="ListItem">item对象</param>
        /// <param name="DelegateName">权限名称</param>
        public static void DeleteDelegateFromListItem(SPPrincipal principal, SPWeb web, SPListItem ListItem, string DelegateName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    if (principal != null)
                    {
                        using (SPSite site = new SPSite(web.Site.ID))
                        {
                            using (SPWeb _web = site.OpenWeb(web.ServerRelativeUrl))
                            {
                                SPListItem _listitem = _web.Lists[ListItem.ParentList.ID].GetItemById(ListItem.ID);
                                _web.AllowUnsafeUpdates = true;

                                //断开原来列表项所继承的权限,使其可以设置独立权限
                                if (!_listitem.HasUniqueRoleAssignments)
                                {
                                    _listitem.BreakRoleInheritance(true);
                                }

                                SPRoleAssignment role = _listitem.RoleAssignments.GetAssignmentByPrincipal(principal);
                                if (!string.IsNullOrWhiteSpace(DelegateName))
                                {
                                    SPRoleDefinition def = _web.RoleDefinitions[DelegateName];
                                    if (role != null)
                                    {
                                        if (role.RoleDefinitionBindings.Contains(def))
                                        {
                                            role.RoleDefinitionBindings.Remove(def);
                                            role.Update();
                                            _listitem.SystemUpdate(false);
                                        }
                                    }
                                }
                                else
                                {
                                    if (role != null)
                                    {
                                        role.RoleDefinitionBindings.RemoveAll();
                                        role.Update();
                                        _listitem.SystemUpdate(false);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        //throw new Exception("没有可以删除权限的员工或组织!");
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

    }
}

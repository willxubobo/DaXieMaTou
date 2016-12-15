using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using CMICT.CSP.Model;
using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using NET.Framework.Common.AD;
using System.Data;
using NET.Framework.Common.ConstantClass;
using SP.Framework.DAL;
using CamlexNET;
using CMICT.CSP.BLL;
using NET.Framework.Common.ExcelHelper;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Web.Caching;
using System.Web;


namespace CMICT.CSP.WCF
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CMICTService" in both code and config file together.
    public class CMICTService : ICMICTService
    {
        public string DoWork()
        {
            return "Hello Word!";
        }

        /// <summary>
        /// 保存使用率统计信息
        /// </summary>
        /// <param name="usageJson"></param>
        /// <param name="detailListJson"></param>
        /// <returns></returns>
        public string SaveUsAge(string usageJson, string detailListJson)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(usageJson))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    UA_USAGE ua_usageEntity = serializer.Deserialize<UA_USAGE>(usageJson);

                    List<UA_USAGE_DETAIL> listUsageDetail = serializer.Deserialize<List<UA_USAGE_DETAIL>>(detailListJson);

                    UsageAnalysisComponent usageComponent = new UsageAnalysisComponent();
                    usageComponent.WriteUsage(ua_usageEntity, listUsageDetail);
                }

                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("使用率统计： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }

        }

        /// <summary>
        /// 获取配置的AD的第一层OU
        /// </summary>
        /// <returns></returns>
        public string GetADFirstLevelUnit()
        {
            List<OrgEntity> listOrg = new List<OrgEntity>();

            string firstLevelUnit = string.Empty;

            try
            {
                string adPaths = System.Configuration.ConfigurationSettings.AppSettings["CMICTLDAPPath"].ToString();
                List<AdModel> listAD = new List<AdModel>();

                if (!string.IsNullOrWhiteSpace(adPaths))
                {
                    string[] arrPaths = adPaths.Split(';');

                    if (arrPaths.Length > 0)
                    {
                        listAD = ADHelper.GetFirstLevelOUByPath(arrPaths);
                    }

                }

                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                firstLevelUnit = oSerializer.Serialize(listAD);

            }
            catch (Exception ex)
            {
                BaseComponent.Error("获取配置的AD的第一层OU： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }
            return firstLevelUnit;
        }
        /// <summary>
        /// 根据Ldap路径获取该路径下的子OU
        /// </summary>
        /// <param name="parentOULdapPath"></param>
        /// <returns></returns>
        public string GetADSubUnit(string parentOULdapPath)
        {
            string subUnit = string.Empty;

            try
            {
                List<AdModel> listAD = new List<AdModel>();

                if (!string.IsNullOrWhiteSpace(parentOULdapPath))
                {
                    listAD = ADHelper.GetSubUnitsByPath(parentOULdapPath);

                }

                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                subUnit = oSerializer.Serialize(listAD);

            }
            catch (Exception ex)
            {
                BaseComponent.Error("根据Ldap路径获取该路径下的子OU： " + ex.Message + "--------" + ex.StackTrace);
            }
            return subUnit;

        }
        /// <summary>
        /// 根据Ldap路径获取该路径下的Users
        /// </summary>
        /// <param name="parentOULdapPath"></param>
        /// <returns></returns>
        public string GetADSubUsers(string parentOULdapPath)
        {
            string subUnit = string.Empty;

            try
            {
                List<AdModel> listAD = new List<AdModel>();

                if (!string.IsNullOrWhiteSpace(parentOULdapPath))
                {
                    listAD = ADHelper.GetOUUsersByPath(parentOULdapPath);
                }

                JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                subUnit = oSerializer.Serialize(listAD);

            }
            catch (Exception ex)
            {
                BaseComponent.Error("根据Ldap路径获取该路径下的Users： " + ex.Message + "--------" + ex.StackTrace);
            }
            return subUnit;
        }

        /// <summary>
        /// 获取当前页面的权限
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public string GetPageRight(string siteUrl, string webUrl, string fileGuid)
        {
            SPListItem item = null;
            string member = string.Empty;


            Dictionary<string, string> dicPageRight = new Dictionary<string, string>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPFile file = web.GetFile(new Guid(fileGuid));

                            if (file != null)
                            {
                                item = web.GetListItem(file.ServerRelativeUrl);
                            }

                            //获取原有权限
                            SPRoleAssignmentCollection roleCol = item.RoleAssignments;
                            foreach (SPRoleAssignment role in roleCol)
                            {
                                string permission = string.Empty;
                                string isGroup = string.Empty;
                                Type priType = role.Member.GetType();

                                if (priType.Name == ConstantClass.SPGROUP)
                                {
                                    isGroup = "g";
                                }
                                else
                                {
                                    isGroup = "p";
                                }
                                string userLoginName = role.Member.LoginName;
                                if (userLoginName.Contains(@"\"))
                                {
                                    userLoginName = userLoginName.Substring(userLoginName.LastIndexOf(@"\") + 1);
                                }

                                member = role.Member.Name + "#" + userLoginName + "#" + isGroup;

                                //获取拥有的权限信息
                                SPRoleDefinitionBindingCollection roleBindCol = role.RoleDefinitionBindings;
                                foreach (SPRoleDefinition roleDef in roleBindCol)
                                {

                                    string roleName = roleDef.Name;
                                    switch (roleName.ToLower())
                                    {

                                        case ConstantClass.FULLCONTROL:
                                        case ConstantClass.FULLCONTROLCN:
                                            if (!string.IsNullOrWhiteSpace(permission) && !permission.Contains(ConstantClass.FULLCONTROL))
                                            {
                                                permission = permission + "," + ConstantClass.MANAGE;
                                            }
                                            else
                                            {
                                                permission = ConstantClass.MANAGE;
                                            }

                                            break;
                                        case ConstantClass.LIMITEDACCESS:
                                        case ConstantClass.LIMITEDACCESSCN:
                                            break;
                                        default:
                                            if (!string.IsNullOrWhiteSpace(permission) && !permission.Contains(ConstantClass.VIEW))
                                            {
                                                permission = permission + "," + ConstantClass.VIEW;
                                            }
                                            else
                                            {
                                                permission = ConstantClass.VIEW;
                                            }

                                            //permission = string.IsNullOrWhiteSpace(permission) ? ConstantClass.VIEW : permission;
                                            break;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(permission))
                                {
                                    dicPageRight.Add(member, permission);
                                }

                            }
                            web.AllowUnsafeUpdates = false;
                        }

                    }
                });

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string result = serializer.Serialize(dicPageRight);

                return "success@" + result;
            }
            catch (Exception ex)
            {
                BaseComponent.Error("获取页面权限信息： " + ex.Message + "--------" + ex.StackTrace);
                return "faield@";
            }


        }

        /// <summary>
        /// 更新页面的权限信息
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="webUrl"></param>
        /// <param name="fileGuid"></param>
        /// <param name="pageRights"></param>
        /// <returns></returns>
        public string UpdatePagePersonRight(string siteUrl, string webUrl, string fileGuid, string pageRights, string deleteUserInfo)
        {
            SPPrincipal spPri = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPFile file = web.GetFile(new Guid(fileGuid));

                            //断开原来列表项所继承的权限,使其可以设置独立权限
                            if (!file.Item.HasUniqueRoleAssignments)
                            {
                                file.Item.BreakRoleInheritance(true);
                            }

                            //删除制定人员的权限
                            if (!string.IsNullOrWhiteSpace(deleteUserInfo))
                            {
                                string[] userInfos = deleteUserInfo.Split(';');

                                foreach (string userName in userInfos)
                                {
                                    if (!string.IsNullOrWhiteSpace(userName))
                                    {
                                        string loginName = userName.Split('#')[0];
                                        string isGroup = userName.Split('#')[1];

                                        if (isGroup.ToLower() == "p")
                                        {
                                            //获取将要设置权限的用户
                                            SPUser authorizationSPUser = web.EnsureUser(loginName);

                                            spPri = (SPPrincipal)authorizationSPUser;
                                        }
                                        else
                                        {
                                            //获取将要设置权限的用户组
                                            SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, loginName, web.CurrentUser);

                                            if (spGroup != null)
                                            {
                                                spPri = (SPPrincipal)spGroup;
                                            }
                                        }

                                        SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");
                                    }

                                }

                            }

                            #region 授权
                            if (!string.IsNullOrWhiteSpace(pageRights))
                            {
                                string[] rightArray = pageRights.Split(';');
                                foreach (string item in rightArray)
                                {
                                    string userLoginName = item.Split('#')[0];
                                    string isGroup = item.Split('#')[1];
                                    string userRight = item.Split('#')[2];

                                    if (isGroup.ToLower() == "p")
                                    {
                                        //获取将要设置权限的用户
                                        SPUser authorizationSPUser = web.EnsureUser(userLoginName);

                                        spPri = (SPPrincipal)authorizationSPUser;
                                    }
                                    else
                                    {
                                        //获取将要设置权限的用户组
                                        SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, userLoginName, web.CurrentUser);

                                        if (spGroup != null)
                                        {
                                            spPri = (SPPrincipal)spGroup;
                                        }
                                    }

                                    //删除该人员的权限
                                    SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");

                                    if (!string.IsNullOrWhiteSpace(userRight))
                                    {
                                        if (userRight.Contains(ConstantClass.MANAGE))
                                        {
                                            SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                            SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROLCN);
                                        }
                                        if (userRight.Contains(ConstantClass.VIEW))
                                        {
                                            if (!userRight.Contains(ConstantClass.MANAGE))
                                            {
                                                //若之前有完全控制权限，则移除原有权限，添加只读权限
                                                bool hasManagePermission = CheckListItemPermission(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                                if (hasManagePermission)
                                                {
                                                    SPRoleAssignment role = file.Item.RoleAssignments.GetAssignmentByPrincipal(spPri);
                                                    if (role != null)
                                                    {
                                                        role.RoleDefinitionBindings.RemoveAll();
                                                        role.Update();
                                                    }
                                                }
                                            }

                                            SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READ);
                                            SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READCN);
                                        }
                                    }
                                    //else
                                    //{
                                    //    //删除该人员的权限
                                    //    SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");
                                    //}
                                }
                            }
                            #endregion

                            web.AllowUnsafeUpdates = false;
                        }


                    }
                });

                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("更新页面的权限信息： " + ex.Message + "--------" + ex.StackTrace);
                return "faield@";
            }


        }

        /// <summary>
        /// 签入文件
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="webUrl"></param>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        public string CheckInFile(string siteUrl, string webUrl, string fileGuid)
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPFile file = web.GetFile(new Guid(fileGuid));

                            file.CheckIn("");

                            web.AllowUnsafeUpdates = false;
                        }

                    }
                });
                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("页面签入： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }


        }

        /// <summary>
        /// 检查当前对象对该item的权限
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="web"></param>
        /// <param name="ListItem"></param>
        /// <param name="DelegateName"></param>
        /// <returns></returns>
        public bool CheckListItemPermission(SPPrincipal principal, SPWeb web, SPListItem ListItem, string DelegateName)
        {
            bool result = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    using (SPSite site = new SPSite(web.Site.ID))
                    {
                        using (SPWeb _web = site.OpenWeb(web.ServerRelativeUrl))
                        {
                            SPListItem _listitem = _web.Lists[ListItem.ParentList.ID].GetItemById(ListItem.ID);
                            if (_listitem.RoleAssignments.Count > 0)
                            {
                                for (int i = 0; i < _listitem.RoleAssignments.Count; i++)
                                {
                                    SPRoleAssignment roleAssignment = _listitem.RoleAssignments[i];

                                    if (!string.IsNullOrWhiteSpace(DelegateName))
                                    {
                                        if (roleAssignment.Member.ID == principal.ID && roleAssignment.RoleDefinitionBindings.Xml.ToLower().Contains(DelegateName))
                                        {
                                            result = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (roleAssignment.Member.ID == principal.ID)
                                        {
                                            result = true;
                                            break;
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });

            return result;
        }

        /// <summary>
        /// 查询该角色有权限浏览的页面
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string SerachPersonRightsPages(string siteUrl, string name)
        {
            //var caml = Camlex.Query().Where(x => ((string)x["Title"]) != "主页").ToString();

            string caml = @"<Where><And>
                        <Neq><FieldRef Name='LinkFilename' /><Value Type='Text'>default.aspx</Value></Neq>
                        <And><Contains><FieldRef Name='FSObjType' /><Value Type='FSObjType'>0</Value></Contains>
                        <And><Neq><FieldRef Name='LinkFilename' /><Value Type='Text'>找不到页面</Value></Neq>
                        <Neq><FieldRef Name='LinkFilename' /><Value Type='Text'>PageNotFoundError.aspx</Value>
                        </Neq></And></And></And></Where>";

            SPPrincipal spPri = null;
            List<PagePersonalPermission> listPermission = new List<PagePersonalPermission>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            if (name.Contains("@"))
                            {
                                name = name.Replace("@", @"\");

                                if (web.SiteUsers[name] != null)
                                {
                                    SPUser authorizationSPUser = web.EnsureUser(name);
                                    spPri = (SPPrincipal)authorizationSPUser;
                                }
                            }
                            else
                            {
                                SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, name, web.CurrentUser);

                                if (spGroup != null)
                                {
                                    spPri = (SPPrincipal)spGroup;
                                }
                            }

                            SPGroup allUserGroup = SPHelper.GetSPGroupByName(siteUrl, ConstantClass.ALLUSERGROUP, web.CurrentUser);

                            //获取所有的页面
                            SPSiteDataQuery siteDataQuery = new SPSiteDataQuery();
                            siteDataQuery.Query = caml;
                            siteDataQuery.Webs = "<Webs Scope='Recursive' />";
                            siteDataQuery.Lists = "<Lists ServerTemplate='850'/>";
                            siteDataQuery.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"FSObjType\" />";

                            DataTable dtAllPages = web.GetSiteData(siteDataQuery);

                            if (dtAllPages != null)
                            {
                                //过滤根目录下的页面以及文件夹
                                DataRow[] subPages = dtAllPages.Select("WebId <>'" + web.ID.ToString() + "' and FSObjType like '%0%'");

                                foreach (DataRow row in subPages)
                                {
                                    string listID = Convert.ToString(row[0]);
                                    string webID = Convert.ToString(row[1]);
                                    string itemID = Convert.ToString(row[2]);

                                    GetUserPages(webID, site.ID, listID, itemID, listPermission, spPri, allUserGroup);

                                }
                            }


                            //查找根站点有权限的页面
                            //GetRootWebPages(web, site.ID, caml, listPermission, spPri, allUserGroup);

                            //SPWebCollection webCollection = web.GetSubwebsForCurrentUser();

                            ////查找子站点有权限的页面
                            //if (webCollection.Count > 0)
                            //{
                            //    GetSubWebPages(webCollection, site.ID, caml, listPermission, spPri, allUserGroup);
                            //}
                        }
                    }
                });

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string result = serializer.Serialize(listPermission);

                return "success@" + result;
            }
            catch (Exception ex)
            {
                BaseComponent.Error("查询该角色有权限浏览的页面： " + ex.Message + "--------" + ex.StackTrace);
                return "faield@";
            }


        }

        private void GetUserPages(string webId, Guid siteID, string listID, string itemID, List<PagePersonalPermission> listResult, SPPrincipal spPri, SPGroup allUserGroup)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteID))
                {
                    using (SPWeb subWeb = site.OpenWeb(new Guid(webId)))
                    {
                        SPList list = subWeb.Lists[new Guid(listID)];

                        if (list != null)
                        {
                            SPListItem item = list.GetItemById(Convert.ToInt32(itemID));

                            if (item != null)
                            {
                                int permissionCount = SPHelper.GetDelegateCount(spPri, subWeb, item);

                                //如果有权限，则判断是否有管理权限
                                if (permissionCount >= 1)
                                {
                                    PagePersonalPermission personalPermission = GetUserPermissionForPage(spPri, subWeb, item, permissionCount);

                                    if (personalPermission != null)
                                    {
                                        listResult.Add(personalPermission);
                                    }
                                    else
                                    {
                                        //若没有权限，检查该人员是否在具有权限的group中
                                        List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, subWeb);
                                        listGroup.Add(allUserGroup);
                                        PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, subWeb, item);

                                        if (lastPermission != null)
                                        {
                                            listResult.Add(lastPermission);
                                        }
                                    }
                                }
                                else
                                {
                                    //若没有权限，检查该人员是否在具有权限的group中
                                    List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, subWeb);
                                    listGroup.Add(allUserGroup);
                                    PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, subWeb, item);

                                    if (lastPermission != null)
                                    {
                                        listResult.Add(lastPermission);
                                    }

                                }

                            }
                        }
                    }
                }
            });

        }



        /// <summary>
        /// 删除该页面指定用户的权限
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="webUrl"></param>
        /// <param name="fileGuid"></param>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public string DeletePagePersonalRight(string siteUrl, string webUrl, string fileGuid, string userLoginName)
        {
            SPPrincipal spPri = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            if (userLoginName.Contains("@"))
                            {
                                userLoginName = userLoginName.Replace("@", @"\");

                                if (web.SiteUsers[userLoginName] != null)
                                {
                                    SPUser authorizationSPUser = web.EnsureUser(userLoginName);
                                    spPri = (SPPrincipal)authorizationSPUser;
                                }
                            }
                            else
                            {
                                if (userLoginName.Contains("#"))
                                {
                                    string loginName = userLoginName.Split('#')[0];
                                    string isGroup = userLoginName.Split('#')[1];

                                    if (isGroup.ToLower() == "p")
                                    {
                                        //获取将要设置权限的用户
                                        SPUser authorizationSPUser = web.EnsureUser(loginName);

                                        spPri = (SPPrincipal)authorizationSPUser;
                                    }
                                    else
                                    {
                                        SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, loginName, web.CurrentUser);

                                        if (spGroup != null)
                                        {
                                            spPri = (SPPrincipal)spGroup;
                                        }

                                    }
                                }
                                else
                                {
                                    SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, userLoginName, web.CurrentUser);

                                    if (spGroup != null)
                                    {
                                        spPri = (SPPrincipal)spGroup;
                                    }
                                }

                            }

                            SPFile file = web.GetFile(new Guid(fileGuid));

                            SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");

                            web.AllowUnsafeUpdates = false;

                        }
                    }
                });

                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("删除该页面指定用户的权限： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }


        }

        /// <summary>
        /// 保存该页面指定用户的权限
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="webUrl"></param>
        /// <param name="fileGuid"></param>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public string SavePagePersonalRight(string siteUrl, string webUrl, string fileGuid, string userLoginName, string userRight)
        {
            SPPrincipal spPri = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            if (userLoginName.Contains("@"))
                            {
                                userLoginName = userLoginName.Replace("@", @"\");

                                if (web.SiteUsers[userLoginName] != null)
                                {
                                    SPUser authorizationSPUser = web.EnsureUser(userLoginName);
                                    spPri = (SPPrincipal)authorizationSPUser;
                                }
                            }
                            else
                            {
                                SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, userLoginName, web.CurrentUser);

                                if (spGroup != null)
                                {
                                    spPri = (SPPrincipal)spGroup;
                                }
                            }

                            SPFile file = web.GetFile(new Guid(fileGuid));

                            //删除用户权限
                            SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");

                            if (!string.IsNullOrWhiteSpace(userRight))
                            {
                                if (userRight.Contains(ConstantClass.MANAGE))
                                {
                                    SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                    SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROLCN);
                                }
                                if (userRight.Contains(ConstantClass.VIEW))
                                {
                                    if (!userRight.Contains(ConstantClass.MANAGE))
                                    {
                                        //若之前有完全控制权限，则移除原有权限，添加只读权限
                                        bool hasManagePermission = CheckListItemPermission(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                        if (hasManagePermission)
                                        {
                                            SPRoleAssignment role = file.Item.RoleAssignments.GetAssignmentByPrincipal(spPri);
                                            if (role != null)
                                            {
                                                role.RoleDefinitionBindings.RemoveAll();
                                                role.Update();
                                            }
                                        }
                                    }

                                    SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READ);
                                    SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READCN);
                                }
                            }
                            //else
                            //{
                            //    //删除用户权限
                            //    SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");
                            //}

                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("保存该页面指定用户的权限： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }
        }

        /// <summary>
        /// 页面授权
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="pageInfos"></param>
        /// <param name="userRight"></param>
        /// <returns></returns>
        public string SetPermission(string siteUrl, string pageInfos, string userRight)
        {

            try
            {
                if (!string.IsNullOrWhiteSpace(pageInfos))
                {
                    string[] arrayPages = pageInfos.Split('@');

                    foreach (string pageInfo in arrayPages)
                    {
                        if (!string.IsNullOrWhiteSpace(pageInfo))
                        {
                            string pageGuid = pageInfo.Split(';')[0];
                            string webUrl = pageInfo.Split(';')[1];

                            SetPagePermission(siteUrl, webUrl, pageGuid, userRight);
                        }

                    }


                }

                return "success";
            }
            catch (Exception ex)
            {
                BaseComponent.Error("页面授权： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }
        }

        /// <summary>
        /// 删除代码映射的记录
        /// </summary>
        /// <param name="dataGuid">数据的主键</param>
        /// <returns></returns>
        public string DeleteCodeMapping(string dataGuid)
        {
            try
            {
                CodeMappingComponent codeComponent = new CodeMappingComponent();

                bool result = codeComponent.DeleteCodeMapping(new Guid(dataGuid));

                if (result)
                {
                    return "success";
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("删除代码映射的记录： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }



        }

        /// <summary>
        /// 批量删除代码映射记录
        /// </summary>
        /// <param name="dataGuids"></param>
        /// <returns></returns>
        public string BatchDeleteCodeMapping(string dataGuids)
        {
            string errData = string.Empty;

            try
            {
                if (!string.IsNullOrWhiteSpace(dataGuids))
                {
                    string[] arrayGuid = dataGuids.Split(';');

                    foreach (string guid in arrayGuid)
                    {
                        if (!string.IsNullOrWhiteSpace(guid))
                        {
                            string result = DeleteCodeMapping(guid);

                            if (result != "success")
                            {
                                errData = errData + "," + guid;
                            }
                        }
                    }
                }

                if (errData.Length > 0)
                {
                    errData = errData.Substring(1);
                }

                return "success@" + errData;
            }
            catch (Exception ex)
            {

                BaseComponent.Error("批量删除代码映射记录： " + ex.Message + "--------" + ex.StackTrace);
                return "faield@";
            }

        }

        /// <summary>
        /// 保存代码映射关系
        /// </summary>
        /// <param name="codeMapData">代码映射关系数据</param>
        /// <param name="isUpdate">是否更新</param>
        /// <returns></returns>
        public string SaveAndUpdateCodeMapping(string codeMapData, bool isUpdate)
        {
            bool result = false;
            CodeMappingComponent codeComponent = new CodeMappingComponent();

            try
            {
                if (!string.IsNullOrWhiteSpace(codeMapData))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    BS_CODEMAPPING codeModel = serializer.Deserialize<BS_CODEMAPPING>(codeMapData);
                    codeModel.Modified = DateTime.Now;

                    if (isUpdate)
                    {
                        result = codeComponent.UpdateCodeMapping(codeModel);
                    }
                    else
                    {
                        codeModel.Created = DateTime.Now;
                        codeModel.MappingID = new Guid();

                        result = codeComponent.SaveCodeMapping(codeModel);
                    }
                }

                if (result)
                {
                    return "success";
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("保存代码映射关系： " + ex.Message + "--------" + ex.StackTrace);
                return "faield";
            }
        }

        public string CheckCodeMappingOnly(string codeMapData, bool isUpdate)
        {
            string result = string.Empty;

            CodeMappingComponent codeComponent = new CodeMappingComponent();

            try
            {
                if (!string.IsNullOrWhiteSpace(codeMapData))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    BS_CODEMAPPING codeModel = serializer.Deserialize<BS_CODEMAPPING>(codeMapData);

                    bool isOnly = codeComponent.CheckCodeMappingOnly(codeModel, isUpdate);

                    result = isOnly ? "yes" : "no";
                }

                return result;
            }
            catch (Exception ex)
            {
                BaseComponent.Error("保存代码映射关系： " + ex.Message + "--------" + ex.StackTrace);
                return "no";
            }
        }

        /// <summary>
        /// 获取所有AD信息
        /// </summary>
        /// <returns></returns>
        public string GetTheAllOrganization()
        {
            string folderInfo = string.Empty;

            try
            {
                if (HttpRuntime.Cache["CMICTOrgnazationADInfo"] != null)
                {
                    folderInfo = Convert.ToString(HttpRuntime.Cache["CMICTOrgnazationADInfo"]);
                    return folderInfo;
                }

                ListEntity listFolderEntity = new ListEntity();

                //获取根节点
                string adPaths = System.Configuration.ConfigurationSettings.AppSettings["CMICTLDAPPath"].ToString();
                List<AdModel> listAD = new List<AdModel>();

                if (!string.IsNullOrWhiteSpace(adPaths))
                {
                    string[] arrPaths = adPaths.Split(';');

                    if (arrPaths.Length > 0)
                    {
                        listAD = ADHelper.GetFirstLevelOUByPath(arrPaths);
                    }
                }

                if (listAD != null)
                {
                    BaseComponent.Info("第一层OU获取数量：" + listAD.Count);
                }
                else
                {
                    BaseComponent.Info("未获取到第一层OU");
                }

                foreach (AdModel model in listAD)
                {
                    FolderEntity folderEntity = new FolderEntity();// The parenet folder entity
                    //Set the value of parent folder
                    folderEntity.TypeId = model.TypeId;
                    folderEntity.LoginName = model.LoginName;
                    folderEntity.LDAPPath = model.LDAPPath;
                    folderEntity.Name = model.Name;
                    //set the value of sub folders
                    folderEntity.SubFolders.Add(GetSubADInfo(model.LDAPPath, folderEntity));
                    listFolderEntity.Folders.Add(folderEntity);
                }

                folderInfo = JsonConvert.SerializeObject(listFolderEntity, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                HttpRuntime.Cache.Add("CMICTOrgnazationADInfo", folderInfo, null,
                                     DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);

                BaseComponent.Info(folderInfo);

            }
            catch (Exception ex)
            {
                BaseComponent.Error("根据Ldap路径获取所有AD信息： " + ex.Message + "--------" + ex.StackTrace);
            }
            return folderInfo;
        }

        public string GetFilterADInfo(string filterName)
        {
            string folderInfo = string.Empty;
            ListEntity listFolderEntity = new ListEntity();

            //获取根节点
            string adPaths = System.Configuration.ConfigurationSettings.AppSettings["CMICTLDAPPath"].ToString();
            List<AdModel> listAD = new List<AdModel>();

            if (!string.IsNullOrWhiteSpace(adPaths))
            {
                string[] arrPaths = adPaths.Split(';');

                if (arrPaths.Length > 0)
                {
                    listAD = ADHelper.GetFirstLevelOUByPath(arrPaths);
                }

                foreach (AdModel model in listAD)
                {
                    FolderEntity folderEntity = new FolderEntity();// The parenet folder entity
                    //Set the value of parent folder
                    folderEntity.TypeId = model.TypeId;
                    folderEntity.LoginName = model.LoginName;
                    folderEntity.LDAPPath = model.LDAPPath;
                    folderEntity.Name = model.Name;
                    //set the value of sub folders
                    folderEntity.SubFolders.Add(GetSubADInfoByFilter(model.LDAPPath, folderEntity, filterName));
                    listFolderEntity.Folders.Add(folderEntity);
                }

                folderInfo = JsonConvert.SerializeObject(listFolderEntity, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            }

            return folderInfo;
        }

        private FolderEntity GetSubADInfoByFilter(string parentOULdapPath, FolderEntity parentFolderEntity, string filterName)
        {
            if (!string.IsNullOrWhiteSpace(parentOULdapPath))
            {
                List<AdModel> listADUnit = ADHelper.GetSubUnitsByPath(parentOULdapPath);
                List<AdModel> listADUser = ADHelper.GetOUUsersByPath(parentOULdapPath);

                if (listADUnit != null)
                {
                    BaseComponent.Info("获取子OU数量：" + listADUnit.Count + " 上级OU路径： " + parentOULdapPath);
                }
                else
                {
                    BaseComponent.Info("未获取到子OU， 上级OU路径： " + parentOULdapPath);
                }

                if (listADUser != null)
                {
                    BaseComponent.Info("获取OU下用户数量：" + listADUser.Count + " 上级OU路径： " + parentOULdapPath);
                }
                else
                {
                    BaseComponent.Info("未获取到OU下用户， 上级OU路径： " + parentOULdapPath);
                }

                List<FolderEntity> listSubFolderEntities = new List<FolderEntity>();

                foreach (AdModel model in listADUser)
                {
                    string displayName = model.Name;
                    if (displayName.ToLower().Contains(filterName.ToLower()))
                    {
                        FolderEntity folderEntity = new FolderEntity();
                        //Set the value of parent folder
                        folderEntity.TypeId = model.TypeId;
                        folderEntity.LoginName = model.LoginName;
                        folderEntity.LDAPPath = model.LDAPPath;
                        folderEntity.Name = model.Name;
                        //set the value of sub folders
                        listSubFolderEntities.Add(folderEntity);
                    }
                }

                foreach (AdModel model in listADUnit)
                {
                    FolderEntity folderEntity = new FolderEntity();// The parenet folder entity
                    //Set the value of parent folder
                    folderEntity.TypeId = model.TypeId;
                    folderEntity.LoginName = model.LoginName;
                    folderEntity.LDAPPath = model.LDAPPath;
                    folderEntity.Name = model.Name;
                    //set the value of sub folders
                    folderEntity.SubFolders.Add(GetSubADInfoByFilter(model.LDAPPath, folderEntity, filterName));
                    listSubFolderEntities.Add(folderEntity);
                }

                parentFolderEntity.SubFolders = listSubFolderEntities;


            }
            return parentFolderEntity;
        }

        private FolderEntity GetSubADInfo(string parentOULdapPath, FolderEntity parentFolderEntity)
        {
            if (!string.IsNullOrWhiteSpace(parentOULdapPath))
            {
                List<AdModel> listADUnit = ADHelper.GetSubUnitsByPath(parentOULdapPath);
                List<AdModel> listADUser = ADHelper.GetOUUsersByPath(parentOULdapPath);

                if (listADUnit != null)
                {
                    BaseComponent.Info("获取子OU数量：" + listADUnit.Count + " 上级OU路径： " + parentOULdapPath);
                }
                else
                {
                    BaseComponent.Info("未获取到子OU， 上级OU路径： " + parentOULdapPath);
                }

                if (listADUser != null)
                {
                    BaseComponent.Info("获取OU下用户数量：" + listADUser.Count + " 上级OU路径： " + parentOULdapPath);
                }
                else
                {
                    BaseComponent.Info("未获取到OU下用户， 上级OU路径： " + parentOULdapPath);
                }

                List<FolderEntity> listSubFolderEntities = new List<FolderEntity>();

                foreach (AdModel model in listADUser)
                {
                    FolderEntity folderEntity = new FolderEntity();
                    //Set the value of parent folder
                    folderEntity.TypeId = model.TypeId;
                    folderEntity.LoginName = model.LoginName;
                    folderEntity.LDAPPath = model.LDAPPath;
                    folderEntity.Name = model.Name;
                    //set the value of sub folders
                    listSubFolderEntities.Add(folderEntity);
                }

                foreach (AdModel model in listADUnit)
                {
                    FolderEntity folderEntity = new FolderEntity();// The parenet folder entity
                    //Set the value of parent folder
                    folderEntity.TypeId = model.TypeId;
                    folderEntity.LoginName = model.LoginName;
                    folderEntity.LDAPPath = model.LDAPPath;
                    folderEntity.Name = model.Name;
                    //set the value of sub folders
                    folderEntity.SubFolders.Add(GetSubADInfo(model.LDAPPath, folderEntity));
                    listSubFolderEntities.Add(folderEntity);
                }

                parentFolderEntity.SubFolders = listSubFolderEntities;


            }
            return parentFolderEntity;
        }

        private void SetPagePermission(string siteUrl, string webUrl, string pageGuid, string userPermission)
        {
            SPPrincipal spPri = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.OpenWeb(webUrl))
                        {
                            web.AllowUnsafeUpdates = true;

                            SPFile file = web.GetFile(new Guid(pageGuid));

                            //断开原来列表项所继承的权限,使其可以设置独立权限
                            if (!file.Item.HasUniqueRoleAssignments)
                            {
                                file.Item.BreakRoleInheritance(true);
                            }

                            if (string.IsNullOrWhiteSpace(userPermission))
                            {
                                //清空该页面的权限
                                SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");
                            }
                            else
                            {
                                string[] arrayPermission = userPermission.Split('?');

                                foreach (string permissionInfo in arrayPermission)
                                {
                                    if (!string.IsNullOrWhiteSpace(permissionInfo))
                                    {
                                        string name = permissionInfo.Split('@')[0];
                                        string right = permissionInfo.Split('@')[1];

                                        string loginName = name.Split('#')[0];
                                        string isGroup = name.Split('#')[1];

                                        if (isGroup.ToLower() == "p")
                                        {
                                            //获取将要设置权限的用户
                                            SPUser authorizationSPUser = web.EnsureUser(loginName);
                                            spPri = (SPPrincipal)authorizationSPUser;
                                        }
                                        else
                                        {
                                            //获取将要设置权限的用户组
                                            SPGroup spGroup = SPHelper.GetSPGroupByName(siteUrl, loginName, web.CurrentUser);

                                            if (spGroup != null)
                                            {
                                                spPri = (SPPrincipal)spGroup;
                                            }
                                        }

                                        if (string.IsNullOrWhiteSpace(right))
                                        {
                                            //删除用户权限
                                            SPHelper.DeleteDelegateFromListItem(spPri, web, file.Item, "");
                                        }
                                        else
                                        {
                                            string[] arrayRights = right.Split(';');

                                            if (right == ConstantClass.MANAGE)
                                            {

                                                SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                                SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.FULLCONTROLCN);
                                            }
                                            else if (right == ConstantClass.VIEW)
                                            {
                                                //若之前有完全控制权限，则移除原有权限，添加只读权限
                                                bool hasManagePermission = CheckListItemPermission(spPri, web, file.Item, ConstantClass.FULLCONTROL);
                                                if (hasManagePermission)
                                                {
                                                    SPRoleAssignment role = file.Item.RoleAssignments.GetAssignmentByPrincipal(spPri);
                                                    if (role != null)
                                                    {
                                                        role.RoleDefinitionBindings.RemoveAll();
                                                        role.Update();
                                                    }
                                                }

                                                SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READ);
                                                SPHelper.DelegateForListItem(spPri, web, file.Item, ConstantClass.READCN);
                                            }
                                        }
                                    }
                                }
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                BaseComponent.Error("页面授权： " + ex.Message + "--------" + ex.StackTrace);
                throw ex;

            }


        }

        /// <summary>
        /// 获取当前用户对根网站页面的权限信息
        /// </summary>
        /// <param name="webCollection"></param>
        /// <param name="siteID"></param>
        /// <param name="query"></param>
        /// <param name="listResult"></param>
        /// <param name="spPri"></param>
        private void GetRootWebPages(SPWeb rootWeb, Guid siteID, string query, List<PagePersonalPermission> listResult, SPPrincipal spPri, SPGroup allUserGroup)
        {

            string userPermission = string.Empty;
            DataTable dtItems = new DataTable();
            SPListItemCollection items = SPHelper.GetAllSPListItems(siteID, rootWeb.ServerRelativeUrl, ConstantClass.PAGESFORM, query, "", 0);

            //检查当前页面该人员拥有的访问权限
            foreach (SPListItem item in items)
            {
                int permissionCount = SPHelper.GetDelegateCount(spPri, rootWeb, item);

                if (permissionCount >= 1)
                {
                    PagePersonalPermission personalPermission = GetUserPermissionForPage(spPri, rootWeb, item, permissionCount);

                    if (personalPermission != null)
                    {
                        listResult.Add(personalPermission);
                    }
                    else
                    {
                        //若没有权限，检查该人员是否在具有权限的group中
                        List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, rootWeb);
                        listGroup.Add(allUserGroup);
                        PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, rootWeb, item);

                        if (lastPermission != null)
                        {
                            listResult.Add(lastPermission);
                        }

                    }

                }
                else
                {
                    //若没有权限，检查该人员是否在具有权限的group中
                    List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, rootWeb);
                    listGroup.Add(allUserGroup);
                    PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, rootWeb, item);

                    if (lastPermission != null)
                    {
                        listResult.Add(lastPermission);
                    }

                }
            }
        }

        /// <summary>
        /// 获取当前用户对子站点页面的权限信息
        /// </summary>
        /// <param name="webCollection"></param>
        /// <param name="siteID"></param>
        /// <param name="query"></param>
        /// <param name="listResult"></param>
        /// <param name="spPri"></param>
        private void GetSubWebPages(SPWebCollection webCollection, Guid siteID, string query, List<PagePersonalPermission> listResult, SPPrincipal spPri, SPGroup allUserGroup)
        {
            foreach (SPWeb subWeb in webCollection)
            {
                string userPermission = string.Empty;
                DataTable dtItems = new DataTable();
                SPListItemCollection items = SPHelper.GetAllSPListItems(siteID, subWeb.ServerRelativeUrl, ConstantClass.PAGESFORM, query, "", 0);

                //检查当前页面该人员拥有的访问权限

                foreach (SPListItem item in items)
                {
                    int permissionCount = SPHelper.GetDelegateCount(spPri, subWeb, item);

                    //如果有权限，则判断是否有管理权限
                    if (permissionCount >= 1)
                    {
                        PagePersonalPermission personalPermission = GetUserPermissionForPage(spPri, subWeb, item, permissionCount);

                        if (personalPermission != null)
                        {
                            listResult.Add(personalPermission);
                        }
                        else
                        {
                            //若没有权限，检查该人员是否在具有权限的group中
                            List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, subWeb);
                            listGroup.Add(allUserGroup);
                            PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, subWeb, item);

                            if (lastPermission != null)
                            {
                                listResult.Add(lastPermission);
                            }
                        }
                    }
                    else
                    {
                        //若没有权限，检查该人员是否在具有权限的group中
                        List<SPGroup> listGroup = CheckUserInGroup((SPUser)spPri, subWeb);
                        listGroup.Add(allUserGroup);
                        PagePersonalPermission lastPermission = GetGroupPermissionForPage(listGroup, subWeb, item);

                        if (lastPermission != null)
                        {
                            listResult.Add(lastPermission);
                        }

                    }
                }

                SPWebCollection subWebCollection = subWeb.GetSubwebsForCurrentUser();

                if (subWebCollection.Count > 0)
                {
                    GetSubWebPages(subWebCollection, siteID, query, listResult, spPri, allUserGroup);
                }
            }
        }

        /// <summary>
        /// 获取Group有权限的页面信息
        /// </summary>
        /// <param name="listGroup"></param>
        /// <param name="web"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private PagePersonalPermission GetGroupPermissionForPage(List<SPGroup> listGroup, SPWeb web, SPListItem item)
        {
            PagePersonalPermission lastPermission = null;

            foreach (SPGroup group in listGroup)
            {
                SPPrincipal spPriGroup = (SPPrincipal)group;

                int groupPermissionCount = SPHelper.GetDelegateCount(spPriGroup, web, item);

                if (groupPermissionCount >= 1)
                {
                    PagePersonalPermission personalPermission = GetUserPermissionForPage(spPriGroup, web, item, groupPermissionCount);

                    if (personalPermission != null)
                    {
                        if (lastPermission != null)
                        {
                            if (lastPermission.PagePermission == ConstantClass.VIEW &&
                                personalPermission.PagePermission == ConstantClass.MANAGE)
                            {
                                lastPermission.PagePermission = lastPermission.PagePermission + "," + ConstantClass.MANAGE;
                            }
                        }
                        else
                        {
                            lastPermission = personalPermission;
                        }
                    }
                }

            }

            return lastPermission;
        }

        /// <summary>
        /// 获取用户有权限的页面信息
        /// </summary>
        /// <param name="spPri"></param>
        /// <param name="web"></param>
        /// <param name="item"></param>
        /// <param name="permissionCount"></param>
        /// <returns></returns>
        private PagePersonalPermission GetUserPermissionForPage(SPPrincipal spPri, SPWeb web, SPListItem item, int permissionCount)
        {
            string userPermission = string.Empty;
            bool hasLimitPermission = false;
            bool hasENLimitPermission = CheckListItemPermission(spPri, web, item, ConstantClass.LIMITEDACCESS);
            bool hasCNLimitPermission = CheckListItemPermission(spPri, web, item, ConstantClass.LIMITEDACCESSCN);

            if (hasENLimitPermission || hasCNLimitPermission)
            {
                hasLimitPermission = true;
            }

            if (permissionCount == 1 && hasLimitPermission)
            {
                return null;
            }

            bool hasManagePermission = false;
            bool hasReadPermission = false;

            bool hasENManagePermission = CheckListItemPermission(spPri, web, item, ConstantClass.FULLCONTROL);
            bool hasENReadPermission = CheckListItemPermission(spPri, web, item, ConstantClass.READ);

            bool hasCNManagePermission = CheckListItemPermission(spPri, web, item, ConstantClass.FULLCONTROLCN);
            bool hasCNReadPermission = CheckListItemPermission(spPri, web, item, ConstantClass.READCN);

            if (hasENManagePermission || hasCNManagePermission)
            {
                hasManagePermission = true;
            }

            if (hasENReadPermission || hasCNReadPermission)
            {
                hasReadPermission = true;
            }

            userPermission = hasManagePermission ? ConstantClass.MANAGE : ConstantClass.VIEW;

            if (hasManagePermission && hasReadPermission)
            {
                userPermission = userPermission + "," + ConstantClass.VIEW;
            }

            string folderNames = System.Configuration.ConfigurationSettings.AppSettings["FilterFolderName"].ToString();
            string[] folderNameList = folderNames.Split(';');

            if (folderNameList.Length > 0)
            {
                foreach (string folderName in folderNameList)
                {
                    if (item.File.ParentFolder.Name.ToLower() == folderName.ToLower())
                    {
                        return null;
                    }
                }
            }

            //将数据添加到集合中
            PagePersonalPermission personalPermission = new PagePersonalPermission();
            personalPermission.Href = web.Url.TrimEnd('/') + "/" + Convert.ToString(item.File.Url);
            personalPermission.PageName = Convert.ToString(item["Title"]);
            personalPermission.FileGuid = item.File.UniqueId;
            personalPermission.PagePermission = userPermission;
            personalPermission.WebUrl = item.File.Web.ServerRelativeUrl;

            return personalPermission;
        }

        /// <summary>
        /// 获取当前用户所在的Group
        /// </summary>
        /// <param name="checkUser">检查的用户</param>
        /// <param name="web">当前网站</param>
        /// <returns>所在Group的集合</returns>
        private List<SPGroup> CheckUserInGroup(SPUser checkUser, SPWeb web)
        {
            List<SPGroup> listGroup = new List<SPGroup>();

            foreach (SPGroup group in web.SiteGroups)
            {
                foreach (SPUser user in group.Users)
                {
                    if (user.ID == checkUser.ID)
                    {
                        listGroup.Add(group);
                        break;
                    }
                }
            }

            return listGroup;
        }

        /// <summary>
        /// 合并DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target">合并后的Table</param>
        private void MergeTable(DataTable source, DataTable target)
        {
            foreach (DataRow row in source.Rows)
            {
                target.ImportRow(row);
            }

        }

        /// <summary>
        /// 获取指定字段的Table
        /// </summary>
        /// <param name="items"></param>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        private DataTable GetDataTable(SPListItemCollection items, string webUrl)
        {
            DataTable dtAllInfo = new DataTable();
            dtAllInfo.Columns.Add("Href");
            dtAllInfo.Columns.Add("RelativeUrl");
            dtAllInfo.Columns.Add("Title");
            dtAllInfo.Columns.Add("SubTitle");
            dtAllInfo.Columns.Add("Author");
            dtAllInfo.Columns.Add("Created");
            dtAllInfo.Columns.Add("PageStatus");
            dtAllInfo.Columns.Add("TemplateName");
            dtAllInfo.Columns.Add("UseUnit");
            dtAllInfo.Columns.Add("FileGuid");
            dtAllInfo.Columns.Add("WebUrl");

            foreach (SPListItem item in items)
            {
                DataRow newRow = dtAllInfo.NewRow();

                newRow["Href"] = webUrl.TrimEnd('/') + "/" + Convert.ToString(item.File.Url);
                newRow["FileGuid"] = Convert.ToString(item.File.UniqueId);
                newRow["WebUrl"] = item.Web.ServerRelativeUrl;
                newRow["RelativeUrl"] = Convert.ToString(item.File.ServerRelativeUrl);
                string title = Convert.ToString(item["Title"]);
                newRow["Title"] = title;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    int len = title.Length;
                    newRow["SubTitle"] = Convert.ToString(item["Title"]).Substring(0, ConstantClass.PAGETITLESUBLEN > len ? len : ConstantClass.PAGETITLESUBLEN);
                }
                else
                {
                    newRow["SubTitle"] = "";
                }

                string author = Convert.ToString(item["Author"]);
                newRow["Author"] = author.Split('#')[1];
                string checkoutUser = Convert.ToString(item["CheckoutUser"]);
                newRow["PageStatus"] = string.IsNullOrWhiteSpace(checkoutUser) ? ConstantClass.CHECKIN : ConstantClass.CHECKEDOUT;
                newRow["Created"] = DateTime.Parse(Convert.ToString(item["Created"])).ToString("yyyy-MM-dd");

                dtAllInfo.Rows.Add(newRow);
            }
            return dtAllInfo;
        }

    }
    //页面权限类
    public class PagePersonalPermission
    {
        public string Href;
        public Guid FileGuid;
        public string PageName;
        public string WebUrl;
        public string PagePermission;
    }


    [Serializable]
    [XmlType("List")]
    public class ListEntity
    {
        private List<FolderEntity> _folders;
        private List<ItemEntity> _items;

        #region Folder Method
        [XmlArray("Folders")]
        public List<FolderEntity> Folders
        {
            get
            {
                EnsureFolders();
                return _folders;
            }
            set { _folders = value; }
        }
        [XmlAttribute]
        public string Permission { get; set; }
        /// <summary>
        /// Make sure the list of Folders has any content
        /// </summary>
        public bool ShouldSerializeFolders()
        {
            return !(Folders == null || Folders.Count <= 0);
        }

        private void EnsureFolders()
        {
            if (_folders == null) _folders = new List<FolderEntity>();
        }
        #endregion

        #region Item Method
        [XmlArray("Items")]
        public List<ItemEntity> Items
        {
            get
            {
                EnsureItems();
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        /// Make sure the list of Folders has any content
        /// </summary>
        public bool ShouldSerializeItems()
        {
            return !(Items == null || Items.Count <= 0);
        }

        private void EnsureItems()
        {
            if (_items == null) _items = new List<ItemEntity>();
        }
        #endregion

        #region Public Methods

        public string Serialize()
        {
            var sb = new StringBuilder();
            var xns = new XmlSerializerNamespaces();

            xns.Add(string.Empty, string.Empty);
            var sw = new StringWriter(sb);
            var serializer = new XmlSerializer(typeof(ListEntity));
            serializer.Serialize(sw, this, xns);

            sw.Flush();
            sw.Close();
            //
            return sb.ToString();
        }

        public static ListEntity Deserialize(string content)
        {
            var serializer = new XmlSerializer(typeof(ListEntity));
            var xns = new XmlSerializerNamespaces();
            var textReader = new StringReader(content);
            var result = (ListEntity)serializer.Deserialize(textReader);
            textReader.Close();
            return result;
        }
        #endregion

    }

    [Serializable]
    [XmlType("Folder")]
    public class FolderEntity
    {
        private List<FolderEntity> _subfolders;
        private List<ItemEntity> _items;


        [XmlAttribute]
        public string LDAPPath { get; set; }

        [XmlAttribute]
        public int TypeId { get; set; }

        [XmlAttribute]
        public string LoginName { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        #region Folder Method
        [XmlArray("Folders")]
        public List<FolderEntity> SubFolders
        {
            get
            {
                EnsureSubFolders();
                return _subfolders;
            }
            set { _subfolders = value; }
        }

        [XmlAttribute]
        public string OrganizationUnitName { get; set; }

        [XmlAttribute]
        public string OrganizationUnitID { get; set; }
        /// <summary>
        /// Make sure the Folder of subfolders has any content
        /// </summary>
        public bool ShouldSerializeSubFolders()
        {
            return !(SubFolders == null || SubFolders.Count <= 0);
        }

        private void EnsureSubFolders()
        {
            if (_subfolders == null) _subfolders = new List<FolderEntity>();
        }
        #endregion

        #region Item Method
        [XmlArray("Items")]
        public List<ItemEntity> Items
        {
            get
            {
                EnsureItems();
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        /// Make sure the list of Folders has any content
        /// </summary>
        public bool ShouldSerializeItems()
        {
            return !(Items == null || Items.Count <= 0);
        }

        private void EnsureItems()
        {
            if (_items == null) _items = new List<ItemEntity>();
        }
        #endregion

    }

    [Serializable]
    [XmlType("Item")]
    public class ItemEntity
    {
        [XmlAttribute]
        public string OrganizationInternalName { get; set; }

        [XmlAttribute]
        public string OrganizationName { get; set; }

        [XmlAttribute]
        public string OrganizationID { get; set; }

        [XmlAttribute]
        public string RoleGroupName { get; set; }

    }
}

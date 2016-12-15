using Microsoft.SharePoint;
using NET.Framework.Common.AD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class SyncADInfoComponent
    {
        /// <summary>
        /// 将AD中的OU和Users添加到Sharepoint Group中
        /// </summary>
        /// <param name="siteUrl"></param>
        public void SyncADInfo(string siteUrl)
        {
            string adPaths = System.Configuration.ConfigurationSettings.AppSettings["CMICTLDAPPath"].ToString();

            if (!string.IsNullOrWhiteSpace(adPaths))
            {
                string[] arrPaths = adPaths.Split(';');

                if (arrPaths.Length > 0)
                {
                    List<AdModel> listFirst = ADHelper.GetFirstLevelOUByPath(arrPaths);

                    List<AdModel> listModel = ADHelper.GetAdInfoByFirstLevel(listFirst);
                    AddGroupByOU(listModel, siteUrl);
                }

            }
        }

        /// <summary>
        /// 添加Group
        /// </summary>
        /// <param name="list"></param>
        /// <param name="siteUrl"></param>
        /// <param name="currentUser"></param>
        private void AddGroupByOU(List<AdModel> list, string siteUrl)
        {
            List<AdModel> listOU = list.FindAll(d => d.TypeId == (int)ADHelper.TypeEnum.OU);

            if (listOU.Count > 0)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(siteUrl))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            foreach (AdModel ou in listOU)
                            {
                                string groupName = string.Empty;
                                try
                                {
                                    groupName = ou.Name;
                                    string ouGuid = ou.Id;

                                    //新增Group
                                    AddGroupRole(web, groupName, ouGuid);

                                    List<AdModel> listUser = list.FindAll(d => d.TypeId == (int)ADHelper.TypeEnum.USER && d.ParentId == ouGuid);

                                    //新增该Group下的人员
                                    AddOUUsers(listUser, web, groupName);
                                }
                                catch (Exception ex)
                                {
                                    BaseComponent.Error("添加Group失败，组名：" + groupName + "  " + ex.Message);
                                    continue;
                                }

                            }
                        }
                    }
                });

            }
        }

        private void AddOUUsers(List<AdModel> list, SPWeb web, string groupName)
        {
            foreach (AdModel user in list)
            {
                string userName = string.Empty;

                try
                {
                    userName = user.LoginName;
                    AddUserToGroup(web, groupName, userName);
                }
                catch (Exception ex)
                {
                    BaseComponent.Error("向" + groupName + "添加用户：" + userName + "失败。" + ex.Message);
                    continue;
                }
            }
        }

        private void AddGroupRole(SPWeb web, string groupNames, string ouGuid)
        {
            SPGroup group = null;

            try
            {
                SPUser user = web.Author;
                foreach (string groupName in groupNames.Split(';'))
                {
                    if (!IsExistGroup(web, groupName))
                    {
                        //web.AllowUnsafeUpdates = true;
                        web.SiteGroups.Add(groupName, user, null, ouGuid);//新建组

                        if (IsExistGroup(web, groupName))
                        {
                            group = web.SiteGroups.GetByName(groupName);
                            //改变站点继承权
                            if (!web.HasUniqueRoleAssignments)
                            {
                                web.BreakRoleInheritance(true);
                            }

                            //组权限分配与定义(New)
                            SPRoleDefinitionCollection roleDefinitions = web.RoleDefinitions;
                            SPRoleAssignmentCollection roleAssignments = web.RoleAssignments;
                            SPMember memCrossSiteGroup = web.SiteGroups[groupName];
                            SPPrincipal myssp = (SPPrincipal)memCrossSiteGroup;
                            SPRoleAssignment myroles = new SPRoleAssignment(myssp);
                            SPRoleDefinitionBindingCollection roleDefBindings = myroles.RoleDefinitionBindings;

                            roleDefBindings.Add(roleDefinitions["Read"]);
                            roleAssignments.Add(myroles);

                        }

                        //web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void DeleteGroup(SPWeb web, string groupNames)
        {
            try
            {
                foreach (string groupName in groupNames.Split(';'))
                {
                    if (IsExistGroup(web, groupName))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPGroupCollection groupCol = web.SiteGroups;

                        SPGroup group = web.SiteGroups.GetByName(groupNames);

                        groupCol.Remove(group.Name);

                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private bool IsExistGroup(SPWeb web, string groupname)
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

        private void AddUserToGroup(SPWeb web, string groupName, string userName)
        {
            if (IsExistGroup(web, groupName))
            {
                //web.AllowUnsafeUpdates = true;

                SPGroup group = web.SiteGroups.GetByName(groupName);
                SPUser user = web.EnsureUser(userName);
                group.AddUser(user);
                group.Update();

                //web.AllowUnsafeUpdates = false;
            }
        }

        private void DeleteUserFromGroup(SPWeb web, string groupName, string userName)
        {
            if (IsExistGroup(web, groupName))
            {
                web.AllowUnsafeUpdates = true;

                SPGroup group = web.SiteGroups.GetByName(groupName);
                SPUser user = web.EnsureUser(userName);
                group.RemoveUser(user);
                group.Update();

                web.AllowUnsafeUpdates = false;
            }
        }

        public bool ChangeUserPassword(string currentName, string newPwd, string oldPwd)
        {
            return ADHelper.ChangePassword(currentName, newPwd, oldPwd);
        }
    }
}

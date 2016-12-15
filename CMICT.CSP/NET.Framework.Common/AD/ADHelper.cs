﻿using NET.Framework.Common.LogHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.AD
{
    public class ADHelper
    {
        ///
        ///域名
        ///
        private static string DomainName = ConfigurationSettings.AppSettings["CMICTDomainName"].ToString(); //"spteam";// ActiveDirectory.GetDomainName;
        ///
        /// LDAP 地址
        ///
        private static string LDAPDomain = ConfigurationSettings.AppSettings["CMICTLDAPDomain"].ToString();//"DC=SPTeam,DC=com";//+ ActiveDirectory.GetDomainName + ",DC=local";
        ///
        /// LDAP绑定路径
        ///
        private static string ADPath = ConfigurationSettings.AppSettings["CMICTLDAPIP"].ToString();//"LDAP://172.19.50.236";// + ActiveDirectory.GetDomainName;
        ///
        ///登录帐号
        ///
        private static string ADUser = ConfigurationSettings.AppSettings["CMICTADUser"].ToString();//"administrator";//ActiveDirectory.GetUserName; //administrator
        ///
        ///登录密码
        ///
        private static string ADPassword = ConfigurationSettings.AppSettings["CMICTADPasseord"].ToString();  //"YZ-it418";//ConstantHelper.PASSWORD;


        ///
        ///扮演类实例
        ///
        private static IdentityImpersonation impersonate = new IdentityImpersonation(ADUser, ADPassword, DomainName);

        private static Log4NetLogger logger = (Log4NetLogger)LoggerManager.GetLogger("CMICT");

        ///
        ///用户登录验证结果
        ///
        public enum LoginResult
        {
            ///
            ///正常登录
            ///
            LOGIN_USER_OK = 0,
            ///
            ///用户不存在
            ///
            LOGIN_USER_DOESNT_EXIST,
            ///
            ///用户帐号被禁用
            ///
            LOGIN_USER_ACCOUNT_INACTIVE,
            ///
            ///用户密码不正确
            ///
            LOGIN_USER_PASSWORD_INCORRECT
        }

        ///
        ///用户属性定义标志
        ///
        public enum ADS_USER_FLAG_ENUM
        {
            ///
            ///登录脚本标志。如果通过 ADSI LDAP 进行读或写操作时，该标志失效。如果通过 ADSI WINNT，该标志为只读。
            ///
            ADS_UF_SCRIPT = 0X0001,
            ///
            ///用户帐号禁用标志
            ///
            ADS_UF_ACCOUNTDISABLE = 0X0002,
            ///
            ///主文件夹标志
            ///
            ADS_UF_HOMEDIR_REQUIRED = 0X0008,
            ///
            ///过期标志
            ///
            ADS_UF_LOCKOUT = 0X0010,
            ///
            ///用户密码不是必须的
            ///
            ADS_UF_PASSWD_NOTREQD = 0X0020,
            ///
            ///密码不能更改标志
            ///
            ADS_UF_PASSWD_CANT_CHANGE = 0X0040,
            ///
            ///使用可逆的加密保存密码
            ///
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,
            ///
            ///本地帐号标志
            ///
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0X0100,
            ///
            ///普通用户的默认帐号类型
            ///
            ADS_UF_NORMAL_ACCOUNT = 0X0200,
            ///
            ///跨域的信任帐号标志
            ///
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0X0800,
            ///
            ///工作站信任帐号标志
            ///
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x1000,
            ///
            ///服务器信任帐号标志
            ///
            ADS_UF_SERVER_TRUST_ACCOUNT = 0X2000,
            ///
            ///密码永不过期标志
            ///
            ADS_UF_DONT_EXPIRE_PASSWD = 0X10000,
            ///
            /// MNS 帐号标志
            ///
            ADS_UF_MNS_LOGON_ACCOUNT = 0X20000,
            ///
            ///交互式登录必须使用智能卡
            ///
            ADS_UF_SMARTCARD_REQUIRED = 0X40000,
            ///
            ///当设置该标志时，服务帐号（用户或计算机帐号）将通过 Kerberos 委托信任
            ///
            ADS_UF_TRUSTED_FOR_DELEGATION = 0X80000,
            ///
            ///当设置该标志时，即使服务帐号是通过 Kerberos 委托信任的，敏感帐号不能被委托
            ///
            ADS_UF_NOT_DELEGATED = 0X100000,
            ///
            ///此帐号需要 DES 加密类型
            ///
            ADS_UF_USE_DES_KEY_ONLY = 0X200000,
            ///
            ///不要进行 Kerberos 预身份验证
            ///
            ADS_UF_DONT_REQUIRE_PREAUTH = 0X4000000,
            ///
            ///用户密码过期标志
            ///
            ADS_UF_PASSWORD_EXPIRED = 0X800000,
            ///
            ///用户帐号可委托标志
            ///
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0X1000000
        }

        public ADHelper()
        {

        }

        #region GetDirectoryObject

        /// <summary>
        /// 获得DirectoryEntry对象实例,以当前用户登陆AD
        /// </summary>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject()
        {
            ((Log4NetLogger)LoggerManager.GetLogger("CMICT")).Info("ADPath:" + ADPath + " ADUser:" + ADUser + " ADPassword:" + ADPassword);
            DirectoryEntry entry = new DirectoryEntry(ADPath, ADUser, ADPassword, AuthenticationTypes.Secure);

            return entry;
        }

        /// <summary>
        /// 根据指定用户名和密码获得相应DirectoryEntry实体
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject(string userName, string password)
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath, userName, password, AuthenticationTypes.None);
            return entry;
        }

        /// <summary>
        /// i.e. /CN=Users,DC=creditsights, DC=cyberelves, DC=Com
        /// </summary>
        /// <param name="domainReference"></param>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject(string domainReference)
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath + domainReference, ADUser, ADPassword, AuthenticationTypes.Secure);
            return entry;
        }

        /// <summary>
        /// 获得以UserName,Password创建的DirectoryEntry
        /// </summary>
        /// <param name="domainReference"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static DirectoryEntry GetDirectoryObject(string domainReference, string userName, string password)
        {
            DirectoryEntry entry = new DirectoryEntry(ADPath + domainReference, userName, password, AuthenticationTypes.Secure);
            return entry;
        }

        #endregion

        #region GetDirectoryEntry


        /// <summary>
        ///根据用户公共名称取得用户的对象
        ///用户公共名称
        ///如果找到该用户，则返回用户的对象；否则返回 null
        /// </summary>
        /// <param name="commonName"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntry(string commonName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
            }
            catch
            {
                de = null;
            }
            return de;
        }

        /// <summary>
        ///根据用户公共名称和密码取得用户的对象。
        ///用户公共名称
        ///用户密码
        ///如果找到该用户，则返回用户的 对象；否则返回null
        /// </summary>
        /// <param name="commonName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntry(string commonName, string password)
        {
            DirectoryEntry de = GetDirectoryObject(commonName, password);
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///根据用户帐号称取得用户的对象
        ///用户帐号名
        ///如果找到该用户，则返回用户的对象；否则返回null
        /// </summary>
        /// <param name="sAMAccountName"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path, ADUser, ADPassword, AuthenticationTypes.Secure);

            }
            catch
            {
                de = null;
            }
            return de;
        }

        /// <summary>
        ///根据用户帐号称取得用户的对象
        ///用户帐号名
        ///如果找到该用户，则返回用户的对象；否则返回null
        /// </summary>
        /// <param name="sAMAccountName"></param>
        /// <returns></returns>
        public static SearchResult GetSearchResultByAccount(string sAMAccountName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            SearchResult result = null;
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                result = deSearch.FindOne();

            }
            catch
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        ///根据用户帐号和密码取得用户的对象
        ///用户帐号名
        ///用户密码
        ///如果找到该用户，则返回用户的 对象；否则返回null
        /// </summary>
        /// <param name="sAMAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName, string password)
        {
            DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);
            if (de != null)
            {
                string commonName = de.Properties["cn"][0].ToString();

                if (GetDirectoryEntry(commonName, password) != null)
                    return GetDirectoryEntry(commonName, password);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///根据组名取得用户组的对象
        ///组名
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntryOfGroup(string groupName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(objectClass=group)(cn=" + groupName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region GetProperty

        /// <summary>
        ///获得指定属性名对应的值
        ///属性名称
        ///属性值
        /// </summary>
        /// <param name="de"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetProperty(DirectoryEntry de, string propertyName)
        {
            if (de.Properties.Contains(propertyName))
            {
                return de.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///获得指定搜索结果中指定属性名对应的值
        ///属性名称
        ///属性值
        /// </summary>
        /// <param name="searchResult"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult.Properties.Contains(propertyName))
            {
                return searchResult.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        ///设置指定的属性值
        ///属性名称
        ///属性值
        /// </summary>
        /// <param name="de"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void SetProperty(DirectoryEntry de, string propertyName, string propertyValue)
        {
            if (propertyValue != string.Empty || propertyValue != "" || propertyValue != null)
            {
                if (de.Properties.Contains(propertyName))
                {
                    de.Properties[propertyName][0] = propertyValue;
                }
                else
                {
                    de.Properties[propertyName].Add(propertyValue);
                }
            }
        }

        /// <summary>
        ///创建新的用户
        ///DN 位置。例如：OU=共享平台 或 CN=Users
        ///公共名称
        ///帐号
        ///密码
        /// </summary>
        /// <param name="ldapDN"></param>
        /// <param name="commonName"></param>
        /// <param name="sAMAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DirectoryEntry CreateNewUser(string ldapDN, string commonName, string sAMAccountName, string password)
        {
            DirectoryEntry entry = GetDirectoryObject();
            DirectoryEntry subEntry = entry.Children.Find(ldapDN);
            DirectoryEntry deUser = subEntry.Children.Add("CN=" + commonName, "user");
            deUser.Properties["sAMAccountName"].Value = sAMAccountName;
            deUser.CommitChanges();
            ADHelper.EnableUser(commonName);
            ADHelper.SetPassword(commonName, password);
            deUser.Close();
            return deUser;
        }

        /// <summary>
        ///创建新的用户。默认创建在Users单元下。
        ///公共名称
        ///帐号
        ///密码
        /// </summary>
        /// <param name="commonName"></param>
        /// <param name="sAMAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DirectoryEntry CreateNewUser(string commonName, string sAMAccountName, string password)
        {
            return CreateNewUser("CN=Users", commonName, sAMAccountName, password);
        }

        /// <summary>
        ///判断指定公共名称的用户是否存在
        ///用户公共名称
        ///如果存在,返回true；否则返回false
        /// </summary>
        /// <param name="commonName"></param>
        /// <returns></returns>
        public static bool IsUserExists(string commonName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";       // LDAP 查询串
            SearchResultCollection results = deSearch.FindAll();

            if (results.Count == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        ///判断用户帐号是否激活
        ///用户帐号属性控制器
        ///如果用户帐号已经激活，返回true；否则返回false
        /// </summary>
        /// <param name="userAccountControl"></param>
        /// <returns></returns>
        public static bool IsAccountActive(int userAccountControl)
        {
            int userAccountControl_Disabled = Convert.ToInt32(ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE);
            int flagExists = userAccountControl & userAccountControl_Disabled;

            if (flagExists > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        ///判断用户与密码是否足够以满足身份验证进而登录
        ///用户公共名称
        ///密码
        ///如能可正常登录，则返回 true；否则返回 false
        /// </summary>
        /// <param name="commonName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginResult Login(string commonName, string password)
        {
            DirectoryEntry de = GetDirectoryEntry(commonName);

            if (de != null)
            {
                // 必须在判断用户密码正确前，对帐号激活属性进行判断；否则将出现异常。
                int userAccountControl = Convert.ToInt32(de.Properties["userAccountControl"][0]);
                de.Close();

                if (!IsAccountActive(userAccountControl))
                    return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE;

                if (GetDirectoryEntry(commonName, password) != null)
                    return LoginResult.LOGIN_USER_OK;
                else
                    return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;
            }
            else
            {
                return LoginResult.LOGIN_USER_DOESNT_EXIST;
            }
        }

        /// <summary>
        ///判断用户帐号与密码是否足够以满足身份验证进而登录
        ///用户帐号
        ///密码
        ///如能可正常登录，则返回true；否则返回false
        /// </summary>
        /// <param name="sAMAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginResult LoginByAccount(string sAMAccountName, string password)
        {
            DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);

            if (de != null)
            {
                // 必须在判断用户密码正确前，对帐号激活属性进行判断；否则将出现异常。
                int userAccountControl = Convert.ToInt32(de.Properties["userAccountControl"][0]);
                de.Close();

                if (!IsAccountActive(userAccountControl))
                    return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE;

                if (GetDirectoryEntryByAccount(sAMAccountName, password) != null)
                    return LoginResult.LOGIN_USER_OK;
                else
                    return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;
            }
            else
            {
                return LoginResult.LOGIN_USER_DOESNT_EXIST;
            }
        }


        /// <summary>
        ///设置用户密码，管理员可以通过它来修改指定用户的密码。
        ///用户公共名称
        ///用户新密码
        /// </summary>
        /// <param name="commonName"></param>
        /// <param name="newPassword"></param>
        public static void SetPassword(string commonName, string newPassword)
        {
            DirectoryEntry de = GetDirectoryEntry(commonName);

            // 模拟超级管理员，以达到有权限修改用户密码
            impersonate.BeginImpersonate();
            de.Invoke("SetPassword", new object[] { newPassword });
            impersonate.StopImpersonate();

            de.Close();
        }


        /// <summary>
        ///设置帐号密码，管理员可以通过它来修改指定帐号的密码。
        ///用户帐号
        ///用户新密码
        /// </summary>
        /// <param name="sAMAccountName"></param>
        /// <param name="newPassword"></param>
        public static void SetPasswordByAccount(string sAMAccountName, string newPassword)
        {
            DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);

            // 模拟超级管理员，以达到有权限修改用户密码
            IdentityImpersonation impersonate = new IdentityImpersonation(ADUser, ADPassword, DomainName);
            impersonate.BeginImpersonate();
            de.Invoke("SetPassword", new object[] { newPassword });
            impersonate.StopImpersonate();

            de.Close();
        }

        /// <summary>
        ///修改用户密码
        ///用户公共名称
        ///旧密码
        ///新密码
        /// </summary>
        /// <param name="commonName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public static void ChangeUserPassword(string commonName, string oldPassword, string newPassword)
        {
            // to-do: 需要解决密码策略问题
            DirectoryEntry oUser = GetDirectoryEntry(commonName);
            oUser.Invoke("ChangePassword", new Object[] { oldPassword, newPassword });
            oUser.Close();
        }

        /// <summary>
        ///启用指定公共名称的用户       
        /// </summary>
        /// <param name="commonName"></param>
        public static void EnableUser(string commonName)
        {
            EnableUser(GetDirectoryEntry(commonName));
        }

        /// <summary>
        /// 启用指定的用户
        /// </summary>
        /// <param name="de"></param>
        public static void EnableUser(DirectoryEntry de)
        {
            impersonate.BeginImpersonate();
            de.Properties["userAccountControl"][0] = ADHelper.ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADHelper.ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD;
            de.CommitChanges();
            impersonate.StopImpersonate();
            de.Close();
        }

        /// <summary>
        /// 禁用指定公共名称的用户
        /// </summary>
        /// <param name="commonName"></param>
        public static void DisableUser(string commonName)
        {
            DisableUser(GetDirectoryEntry(commonName));
        }

        /// <summary>
        /// 禁用指定的用户
        /// </summary>
        /// <param name="de"></param>
        public static void DisableUser(DirectoryEntry de)
        {
            impersonate.BeginImpersonate();
            de.Properties["userAccountControl"][0] = ADHelper.ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADHelper.ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD | ADHelper.ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE;
            de.CommitChanges();
            impersonate.StopImpersonate();
            de.Close();
        }

        /// <summary>
        ///将指定的用户添加到指定的组中。默认为Users下的组和用户。
        ///用户公共名称
        ///组名
        /// </summary>
        /// <param name="userCommonName"></param>
        /// <param name="groupName"></param>
        public static void AddUserToGroup(string userCommonName, string groupName)
        {
            DirectoryEntry oGroup = GetDirectoryEntryOfGroup(groupName);
            DirectoryEntry oUser = GetDirectoryEntry(userCommonName);

            impersonate.BeginImpersonate();
            oGroup.Properties["member"].Add(oUser.Properties["distinguishedName"].Value);
            oGroup.CommitChanges();
            impersonate.StopImpersonate();

            oGroup.Close();
            oUser.Close();
        }

        /// <summary>
        ///将用户从指定组中移除。默认为Users下的组和用户。
        ///用户公共名称
        /// </summary>
        /// <param name="userCommonName"></param>
        /// <param name="groupName"></param>
        public static void RemoveUserFromGroup(string userCommonName, string groupName)
        {
            DirectoryEntry oGroup = GetDirectoryEntryOfGroup(groupName);
            DirectoryEntry oUser = GetDirectoryEntry(userCommonName);

            impersonate.BeginImpersonate();
            oGroup.Properties["member"].Remove(oUser.Properties["distinguishedName"].Value);
            oGroup.CommitChanges();
            impersonate.StopImpersonate();

            oGroup.Close();
            oUser.Close();
        }

        #region## 同步
        /// <summary>
        /// 同步制定OU路径下的所有子OU和Users
        /// </summary>
        /// <param name="adLDAPPath">OULdAp路径</param>
        public static List<AdModel> GetFirstLevelOUByPath(string[] adLDAPPath)
        {
            List<AdModel> listadMOdel = new List<AdModel>();
            foreach (string path in adLDAPPath)
            {
                string LDAPPath = path + "," + LDAPDomain;

                DirectoryEntry searchRoot = new DirectoryEntry(LDAPPath, ADUser, ADPassword, AuthenticationTypes.Secure);

                if (searchRoot.Properties.Contains("ou"))
                {
                    string rootOuName = searchRoot.Properties["ou"][0].ToString();

                    string id = Convert.ToString(searchRoot.Guid);

                    string ldapPath = searchRoot.Path;
                    AdModel adMOdel = new AdModel();
                    adMOdel.Id = id;
                    adMOdel.LDAPPath = ldapPath;
                    adMOdel.LoginName = rootOuName;
                    adMOdel.Name = rootOuName;
                    adMOdel.TypeId = (int)TypeEnum.OU;
                    adMOdel.ParentId = "0";

                    listadMOdel.Add(adMOdel);

                    logger.Info("Ldap地址：" + LDAPPath + "获取到第一层OU信息： " + rootOuName + " 用户：" + ADUser);
                }
                else
                {
                    logger.Info("Ldap地址：" + LDAPPath + "未获取到第一层OU信息 " + " 用户：" + ADUser);
                }

            }

            return listadMOdel;
        }

        /// <summary>
        /// 获取第一层OU下的子OU
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <returns></returns>
        public static List<AdModel> GetSubUnitsByPath(string ldapPath)
        {
            List<AdModel> list = new List<AdModel>();

            DirectoryEntry searchRoot = new DirectoryEntry(ldapPath, ADUser, ADPassword, AuthenticationTypes.Secure);

            foreach (DirectoryEntry subEntry in searchRoot.Children)
            {
                string entrySchemaClsName = subEntry.SchemaClassName;

                string[] arr = subEntry.Name.Split('=');
                string categoryStr = arr[0];
                string nameStr = arr[1];
                string id = Convert.ToString(subEntry.Guid);
                string subLdapPath = subEntry.Path;

                bool isExist = list.Exists(d => d.Id == id);

                if (entrySchemaClsName == "organizationalUnit")
                {
                    if (!isExist)
                    {
                        AdModel adModel = new AdModel();
                        adModel.Id = id;
                        adModel.LDAPPath = subLdapPath;
                        adModel.LoginName = nameStr;
                        adModel.Name = nameStr;
                        adModel.TypeId = (int)TypeEnum.OU;
                        adModel.ParentId = Convert.ToString(searchRoot.Guid);
                        list.Add(adModel);
                    }
                }

            }

            return list;
        }

        /// <summary>
        /// 获取OU下的Users
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <returns></returns>
        public static List<AdModel> GetOUUsersByPath(string ldapPath)
        {
            List<AdModel> list = new List<AdModel>();

            DirectoryEntry searchRoot = new DirectoryEntry(ldapPath, ADUser, ADPassword, AuthenticationTypes.Secure);

            foreach (DirectoryEntry subEntry in searchRoot.Children)
            {
                string entrySchemaClsName = subEntry.SchemaClassName;

                string[] arr = subEntry.Name.Split('=');
                string categoryStr = arr[0];
                string nameStr = arr[1];
                string id = Convert.ToString(subEntry.Guid);

                bool isExist = list.Exists(d => d.Id == id);

                if (entrySchemaClsName == "user")
                {
                    //查看当前用户是否已经禁用
                    bool isDisable = Convert.ToBoolean(Convert.ToInt32(subEntry.Properties["userAccountcontrol"][0]) & 0x0002);

                    if (isDisable)
                    {
                        continue;
                    }

                    string accountName = string.Empty;
                    string displayName = string.Empty;

                    if (subEntry.Properties.Contains("samaccountName"))
                    {
                        accountName = subEntry.Properties["samaccountName"][0].ToString();
                    }
                    if (subEntry.Properties.Contains("displayName"))
                    {
                        displayName = subEntry.Properties["displayName"][0].ToString();
                    }

                    if (!isExist)
                    {
                        AdModel adModel = new AdModel();
                        adModel.Id = id;
                        adModel.LDAPPath = "";
                        adModel.LoginName = accountName;
                        adModel.Name = displayName;
                        adModel.TypeId = (int)TypeEnum.USER;
                        adModel.ParentId = Convert.ToString(searchRoot.Guid);

                        list.Add(adModel);
                    }
                }

            }

            return list;
        }

        /// <summary>
        /// 根据第一层OU获取所有的AD信息
        /// </summary>
        /// <param name="listFirst"></param>
        /// <returns></returns>
        public static List<AdModel> GetAdInfoByFirstLevel(List<AdModel> listFirst)
        {
            List<AdModel> listAll = new List<AdModel>();

            foreach (AdModel model in listFirst)
            {
                //将顶层数据添加到listAll集合
                string LDAPPath = model.LDAPPath;
                string Firstid = Convert.ToString(model.Id);
                string parentName = model.Name;

                bool isExist = listAll.Exists(d => d.Id == Firstid);

                if (!isExist)
                {
                    listAll.Add(model);
                }

                GetADInfoByPath(LDAPPath, listAll, parentName);
            }

            return listAll;
        }

        public static void GetADInfoByPath(string path, List<AdModel> list, string parentName)
        {
            DirectoryEntry searchRoot = new DirectoryEntry(path, ADUser, ADPassword, AuthenticationTypes.Secure);

            //将该层的子集添加到list集合
            foreach (DirectoryEntry subEntry in searchRoot.Children)
            {
                string entrySchemaClsName = subEntry.SchemaClassName;

                string[] arr = subEntry.Name.Split('=');
                string categoryStr = arr[0];
                string nameStr = arr[1];
                string id = Convert.ToString(subEntry.Guid);
                string subLdapPath = subEntry.Path;
                string parentGroupName = string.Empty;

                bool isChildExist = list.Exists(d => d.Id == id);

                switch (entrySchemaClsName)
                {
                    case "user":
                        //查看当前用户是否已经禁用
                        bool isDisable = Convert.ToBoolean(Convert.ToInt32(subEntry.Properties["userAccountcontrol"][0]) & 0x0002);

                        if (isDisable)
                        {
                            continue;
                        }

                        string accountName = string.Empty;
                        string displayName = string.Empty;

                        if (subEntry.Properties.Contains("samaccountName"))
                        {
                            accountName = subEntry.Properties["samaccountName"][0].ToString();
                        }
                        if (subEntry.Properties.Contains("displayName"))
                        {
                            displayName = subEntry.Properties["displayName"][0].ToString();
                        }

                        if (!isChildExist)
                        {
                            AdModel adModel = new AdModel();
                            adModel.Id = id;
                            adModel.LDAPPath = "";
                            adModel.LoginName = accountName;
                            adModel.Name = displayName;
                            adModel.TypeId = (int)TypeEnum.USER;
                            adModel.ParentId = Convert.ToString(searchRoot.Guid);

                            list.Add(adModel);
                        }
                        break;
                    case "organizationalUnit":
                        if (!isChildExist)
                        {
                            parentGroupName = parentName + "_" + nameStr;

                            AdModel adModel = new AdModel();
                            adModel.Id = id;
                            adModel.LDAPPath = subLdapPath;
                            adModel.LoginName = nameStr;
                            adModel.Name = parentGroupName;
                            adModel.TypeId = (int)TypeEnum.OU;
                            adModel.ParentId = Convert.ToString(searchRoot.Guid);

                            list.Add(adModel);
                        }

                        GetADInfoByPath(subEntry.Path, list, parentGroupName);
                        break;
                }

            }
        }

        public static bool ChangePassword(string currentName, string newPwd, string oldPwd)
        {
            bool resetResult = false;

            try
            {
                string container = ConfigurationSettings.AppSettings["CMICTLDAPDomain"].ToString();
                string LDAPPath = ConfigurationSettings.AppSettings["CMICTLDAPIP"].ToString();
                LDAPPath = LDAPPath.TrimEnd('/') + "/" + container;

                int index = currentName.IndexOf("\\");

                string userName = ADUser;
                string pwd = ADPassword;
                string personName = currentName.Substring(index + 1);
                string domain = currentName.Substring(0, index);

                DirectoryEntry searchRoot = new DirectoryEntry(LDAPPath, userName, pwd, AuthenticationTypes.Secure);

                DirectorySearcher deSearch = new DirectorySearcher(searchRoot);

                deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(samaccountName=" + personName + "))";

                SearchResult result = deSearch.FindOne();
                DirectoryEntry userEntry = result.GetDirectoryEntry();

                // 模拟超级管理员，以达到有权限修改用户密码
                IdentityImpersonation impersonate = new IdentityImpersonation(userName, pwd, domain);

                impersonate.BeginImpersonate();
                userEntry.Invoke("ChangePassword", new object[2] { oldPwd, newPwd });
                //userEntry.Invoke("SetPassword", new object[] { newPwd });
                userEntry.CommitChanges();
                userEntry.Close();
                impersonate.StopImpersonate();

                resetResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
                resetResult = false;
            }

            return resetResult;
        }

        /// <summary>
        /// 查找目录项
        /// </summary>
        /// <param name="category">分类 users</param>
        /// <param name="name">用户名</param>
        /// <returns>目录项实体</returns>
        public static DirectoryEntry FindObject(string category, string name)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;
            DirectoryEntry userEntry = null;
            try
            {
                de = new DirectoryEntry(GetDomainPath(), "administrator", "password01!", AuthenticationTypes.Secure);
                ds = new DirectorySearcher(de);
                string queryFilter = string.Format("(&(objectCategory=" + category + ")(sAMAccountName={0}))", name);
                ds.Filter = queryFilter;
                ds.Sort.PropertyName = "cn";
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    userEntry = sr.GetDirectoryEntry();
                }
                return userEntry;
            }
            catch (Exception ex)
            {
                return new DirectoryEntry();
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (de != null)
                {
                    de.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取域路径
        /// </summary>
        /// <returns>路径</returns>
        public static string GetDomainPath()
        {
            using (DirectoryEntry root = new DirectoryEntry("LDAP://172.19.50.161/" + "DC=contoso,DC=com", "administrator", "password01!"))
            {
                return root.Path;
            }
        }

        #endregion

        #region## 类型
        /// <summary>
        /// 类型
        /// </summary>
        public enum TypeEnum : int
        {
            /// <summary>
            /// 组织单位
            /// </summary>
            OU = 1,

            /// <summary>
            /// 用户
            /// </summary>
            USER = 2
        }
        #endregion


    }

    #region## Ad域信息实体
    public class OrgEntity
    {
        public List<OrgEntity> SubOrg;
        public AdModel ParentOrg;

    }


    /// <summary>
    /// Ad域信息实体
    /// </summary>
    public class AdModel
    {
        //public AdModel(string id, string name, int typeId, string parentId, string loginName, string ldapPath)
        //{
        //    Id = id;
        //    Name = name;
        //    TypeId = typeId;
        //    ParentId = parentId;
        //    LoginName = loginName;
        //    LDAPPath = ldapPath;
        //}

        public string Id { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public string ParentId { get; set; }

        public string LoginName { get; set; }
        public string LDAPPath { get; set; }
    }
    #endregion

    /// <summary>
    /// 用户模拟角色类。实现在程序段内进行用户角色模拟。
    /// </summary>
    public class IdentityImpersonation
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        // 要模拟的用户的用户名、密码、域(机器名)
        private String _sImperUsername;
        private String _sImperPassword;
        private String _sImperDomain;
        // 记录模拟上下文
        private WindowsImpersonationContext _imperContext;
        private IntPtr _adminToken;
        private IntPtr _dupeToken;
        // 是否已停止模拟
        private Boolean _bClosed;


        /// <summary>
        ///构造函数
        ///所要模拟的用户的用户名
        ///所要模拟的用户的密码
        ///所要模拟的用户所在的域
        /// </summary>
        /// <param name="impersonationUsername"></param>
        /// <param name="impersonationPassword"></param>
        /// <param name="impersonationDomain"></param>
        public IdentityImpersonation(String impersonationUsername, String impersonationPassword, String impersonationDomain)
        {
            _sImperUsername = impersonationUsername;
            _sImperPassword = impersonationPassword;
            _sImperDomain = impersonationDomain;

            _adminToken = IntPtr.Zero;
            _dupeToken = IntPtr.Zero;
            _bClosed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~IdentityImpersonation()
        {
            if (!_bClosed)
            {
                StopImpersonate();
            }
        }

        /// <summary>
        /// 开始身份角色模拟。
        /// </summary>
        /// <returns></returns>
        public Boolean BeginImpersonate()
        {
            Boolean bLogined = LogonUser(_sImperUsername, _sImperDomain, _sImperPassword, 2, 0, ref _adminToken);

            if (!bLogined)
            {
                return false;
            }

            Boolean bDuped = DuplicateToken(_adminToken, 2, ref _dupeToken);

            if (!bDuped)
            {
                return false;
            }

            WindowsIdentity fakeId = new WindowsIdentity(_dupeToken);
            _imperContext = fakeId.Impersonate();

            _bClosed = false;

            return true;
        }

        /// <summary>
        /// 停止身分角色模拟。
        /// </summary>
        public void StopImpersonate()
        {
            _imperContext.Undo();
            CloseHandle(_dupeToken);
            CloseHandle(_adminToken);
            _bClosed = true;
        }
    }
}
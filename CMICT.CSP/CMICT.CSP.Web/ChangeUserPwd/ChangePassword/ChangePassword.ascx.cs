using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using Microsoft.SharePoint;
using CMICT.CSP.BLL.Components;
using System.Configuration;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace CMICT.CSP.Web.ChangeUserPwd.ChangePassword
{
    [ToolboxItemAttribute(false)]
    public partial class ChangePassword : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ChangePassword()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnSave.Style.Add("display", "none");

                lblUserName.InnerText = SPContext.Current.Web.CurrentUser.Name;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool check = true;
            string msg = string.Empty;

            string newPwd = this.txtNewPwd.Value.Trim();
            string oldPwd = this.txtOldPwd.Value.Trim();
            string confirmNewPwd = this.txtConfirmNewPwd.Value.Trim();

            if (string.IsNullOrWhiteSpace(oldPwd))
            {
                msg = "请输入旧密码。";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetFailed('" + msg + "');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(newPwd))
            {
                msg = "请输入重置密码。";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetFailed('" + msg + "');", true);
                return;
            }

            if (newPwd != confirmNewPwd)
            {
                msg = "两次输入的密码不一致，请重新输入。";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetFailed('" + msg + "');", true);
                return;
            }

            //Regex reg = new Regex(@"(?=.*\d)(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{6}");
            var regex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]{3}).{6}",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            bool isMatch = regex.IsMatch(newPwd);

            if (!isMatch)
            {
                msg = "密码长度至少6位,且由数字,字母,特殊符号组成,特殊符号至少3种。";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetFailed('" + msg + "');", true);
                return;
            }
            else
            {
                //检查通过，重置密码
                bool result = ChangeUserPassword(newPwd, oldPwd);

                if (result)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetSuccess();", true);
                }
                else
                {
                    msg = "密码重置失败。";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ResetFailed('" + msg + "');", true);
                }
            }

        }

        private bool ChangeUserPassword(string NewPwd, string oldPwd)
        {
            Impersonator impersonator = new Impersonator();
            bool result = false;

            try
            {
                impersonator.BeginImpersonation();

                string password = ConfigurationSettings.AppSettings["CMICTADPasseord"].ToString();
                string domain = ConfigurationSettings.AppSettings["CMICTDomainName"].ToString();
                string userName = ConfigurationSettings.AppSettings["CMICTADUser"].ToString();
                string container = ConfigurationSettings.AppSettings["CMICTLDAPDomain"].ToString();

                SyncADInfoComponent sync = new SyncADInfoComponent();
                string loginName = GetLoginName();

                using (PrincipalContext context = GetPContext(oldPwd, domain, loginName, container))
                {
                    using (UserPrincipal principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, loginName))
                    {
                        ////重置用户密码
                        //result = sync.ChangeUserPassword(loginName, NewPwd);

                        //if (!principal.UserCannotChangePassword)
                        //{
                        //    principal.UserCannotChangePassword = true;
                        //}
                        principal.ChangePassword(oldPwd, NewPwd);

                        //principal.SetPassword(NewPwd);
                        principal.Save();

                        result = true;
                    }
                }
                if (impersonator.IsImpersonated)
                {
                    impersonator.StopImpersonation();
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("用户密码重置： " + ex.Message + "--------" + ex.StackTrace);

                if (impersonator.IsImpersonated)
                {
                    impersonator.StopImpersonation();
                }
            }

            return result;
        }
        private PrincipalContext GetPContext(string OldPwd, string domain, string userName, string container)
        {
            return new PrincipalContext(ContextType.Domain, domain, container,
                ContextOptions.Negotiate, userName, OldPwd);
        }

        private string GetDomainContainter(string domain)
        {
            string str = string.Empty;
            string[] strArray = domain.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str2 in strArray)
            {
                str = str + "DC=" + str2 + ",";
            } if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
            } return str;
        }
        private string GetLoginName()
        {
            string username = SPContext.Current.Web.CurrentUser.LoginName.Replace("i:0#.w|", "");
            if (username.EndsWith(@"\system"))
            {
                username = username.Replace("system", "louis1.zhu");
            }
            return username;
        }

    }
    public class Impersonator
    {        // Fields

        private WindowsImpersonationContext ctx = null;        // Methods
        public void BeginImpersonation()
        {
            try
            {
                if (!WindowsIdentity.GetCurrent().IsSystem)
                {
                    this.ctx = WindowsIdentity.Impersonate(WindowsIdentity.GetCurrent().Token);
                    this.IsImpersonated = true;
                }
            }
            catch
            {
                this.IsImpersonated = false;
            }
        }
        public void StopImpersonation()
        {
            if (this.ctx != null)
            {
                this.ctx.Undo();
            }
        }        // Properties
        public bool IsImpersonated
        {
            set;
            get;
        }
    }

}
